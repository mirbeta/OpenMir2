using M2Server;
using NLog;
using System.Net;
using OpenMir2;
using OpenMir2.Data;
using SystemModule;
using SystemModule.Data;
using TouchSocket.Core;
using TouchSocket.Sockets;
using TcpClient = TouchSocket.Sockets.TcpClient;

namespace GameSrv.Services
{
    /// <summary>
    /// 账号会话认证服务
    /// </summary>
    public class AuthenticationService : IAuthentication
    {
        
        private readonly List<AccountSession> _sessionList;
        private readonly TcpClient _tcpClient;
        private readonly object _userIdSection;
        private string _socketRecvText = string.Empty;

        public AuthenticationService()
        {
            _tcpClient = new TcpClient();
            _sessionList = new List<AccountSession>();
            _userIdSection = new object();
        }

        public void Initialize()
        {
            _tcpClient.Setup(new TouchSocketConfig()
                .SetRemoteIPHost(new IPHost(IPAddress.Parse(SystemShare.Config.sIDSAddr), SystemShare.Config.nIDSPort))
                .ConfigureContainer(a =>
                {
                    a.AddConsoleLogger();
                }));
            _tcpClient.Connected = DataSocketConnect; 
            _tcpClient.Disconnected = DataSocketDisconnect;
            _tcpClient.Received = DataSocketRead;
            LogService.Debug("登录服务器连接初始化完成...");
            _tcpClient.Connect();
        }

        private Task DataSocketRead(TcpClient sender, ReceivedDataEventArgs e)
        {
            HUtil32.EnterCriticalSection(_userIdSection);
            try
            {
                _socketRecvText += HUtil32.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(_userIdSection);
            }
            return Task.CompletedTask;
        }

        public void SendSocket(string sSendMsg)
        {
            if (_tcpClient == null || !_tcpClient.Online) return;
            var data = HUtil32.GetBytes(sSendMsg);
            _tcpClient.Send(data);
        }

        public void SendHumanLogOutMsg(string sUserId, int nId)
        {
            const string sFormatMsg = "({0}/{1}/{2})";
            for (var i = 0; i < _sessionList.Count; i++)
            {
                var sessInfo = _sessionList[i];
                if (sessInfo.SessionId == nId && sessInfo.Account == sUserId)
                {
                    break;
                }
            }
            SendSocket(string.Format(sFormatMsg, Messages.SS_SOFTOUTSESSION, sUserId, nId));
        }

        public void SendHumanLogOutMsgA(string userId, int sessionId)
        {
            for (var i = _sessionList.Count - 1; i >= 0; i--)
            {
                var sessInfo = _sessionList[i];
                if (sessInfo.SessionId == sessionId && sessInfo.Account == userId)
                {
                    break;
                }
            }
        }

        public void SendLogonCostMsg(string sAccount, int nTime)
        {
            const string sFormatMsg = "({0}/{1}/{2})";
            SendSocket(string.Format(sFormatMsg, Messages.SS_LOGINCOST, sAccount, nTime));
        }

        public void SendOnlineHumCountMsg(int nCount)
        {
            const string sFormatMsg = "({0}/{1}/{2}/{3}/{4})";
            SendSocket(string.Format(sFormatMsg, Messages.SS_SERVERINFO, SystemShare.Config.ServerName, M2Share.ServerIndex, nCount, SystemShare.Config.PayMentMode));
        }

        public void SendUserPlayTime(string account, long playTime)
        {
            const string sFormatMsg = "({0}/{1}/{2}/{3})";
            SendSocket(string.Format(sFormatMsg, Messages.ISM_GAMETIMEOFTIMECARDUSER, SystemShare.Config.ServerName, account, playTime));
        }

        public void Run()
        {
            string sSocketText;
            var sData = string.Empty;
            var sCode = string.Empty;
            const string sExceptionMsg = "[Exception] AccountService:DecodeSocStr";
            HUtil32.EnterCriticalSection(_userIdSection);
            try
            {
                if (string.IsNullOrEmpty(_socketRecvText))
                {
                    return;
                }
                if (_socketRecvText.IndexOf(')') <= 0)
                {
                    return;
                }
                sSocketText = _socketRecvText;
                _socketRecvText = string.Empty;
            }
            finally
            {
                HUtil32.LeaveCriticalSection(_userIdSection);
            }
            try
            {
                while (true)
                {
                    sSocketText = HUtil32.ArrestStringEx(sSocketText, "(", ")", ref sData);
                    if (string.IsNullOrEmpty(sData))
                    {
                        break;
                    }
                    var sBody = HUtil32.GetValidStr3(sData, ref sCode, HUtil32.Backslash);
                    switch (HUtil32.StrToInt(sCode, 0))
                    {
                        case Messages.SS_OPENSESSION:// 100
                            GetPasswdSuccess(sBody);
                            break;
                        case Messages.SS_CLOSESESSION:// 101
                            GetCancelAdmission(sBody);
                            break;
                        case Messages.SS_KEEPALIVE:// 104
                            SetTotalHumanCount(sBody);
                            break;
                        case Messages.UNKNOWMSG:
                            break;
                        case Messages.SS_KICKUSER:// 111
                            GetCancelAdmissionA(sBody);
                            break;
                        case Messages.SS_SERVERLOAD:// 113
                            GetServerLoad(sBody);
                            break;
                        case Messages.ISM_ACCOUNTEXPIRED:
                            GetAccountExpired(sBody);
                            break;
                        case Messages.ISM_QUERYPLAYTIME:
                            QueryAccountExpired(sBody);
                            break;
                    }
                    if (sSocketText.IndexOf(')') <= 0)
                    {
                        break;
                    }
                }
                HUtil32.EnterCriticalSection(_userIdSection);
                try
                {
                    _socketRecvText = sSocketText + _socketRecvText;
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(_userIdSection);
                }
            }
            catch
            {
                LogService.Error(sExceptionMsg);
            }
        }

        private static void QueryAccountExpired(string sData)
        {
            var account = string.Empty;
            var certstr = HUtil32.GetValidStr3(sData, ref account, '/');
            var cert = HUtil32.StrToInt(certstr, 0);
            if (!SystemShare.Config.TestServer)
            {
                var playTime = SystemShare.WorldEngine.GetPlayExpireTime(account);
                if (playTime >= 3600 || playTime < 1800) //大于一个小时或者小于半个小时都不处理
                {
                    return;
                }
                if (cert <= 1800)//小于30分钟一分钟查询一次，否则10分钟或者半个小时同步一次都行
                {
                    SystemShare.WorldEngine.SetPlayExpireTime(account, cert);
                }
                else
                {
                    SystemShare.WorldEngine.SetPlayExpireTime(account, cert);
                }
            }
        }

        private void GetAccountExpired(string sData)
        {
            var account = string.Empty;
            var certstr = HUtil32.GetValidStr3(sData, ref account, '/');
            var cert = HUtil32.StrToInt(certstr, 0);
            if (!SystemShare.Config.TestServer)
            {
                SystemShare.WorldEngine.AccountExpired(account);
                DelSession(cert);
            }
        }

        private void GetPasswdSuccess(string sData)
        {
            var sAccount = string.Empty;
            var sSessionId = string.Empty;
            var sPayCost = string.Empty;
            var sIPaddr = string.Empty;
            var sPayMode = string.Empty;
            var sPlayTime = string.Empty;
            const string sExceptionMsg = "[Exception] AccountService:GetPasswdSuccess";
            try
            {
                //todo 这里要获取账号剩余游戏时间
                sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
                sData = HUtil32.GetValidStr3(sData, ref sSessionId, HUtil32.Backslash);
                sData = HUtil32.GetValidStr3(sData, ref sPayCost, HUtil32.Backslash);// boPayCost
                sData = HUtil32.GetValidStr3(sData, ref sPayMode, HUtil32.Backslash);// nPayMode
                sData = HUtil32.GetValidStr3(sData, ref sIPaddr, HUtil32.Backslash);// sIPaddr
                sData = HUtil32.GetValidStr3(sData, ref sPlayTime, HUtil32.Backslash);// playTime
                NewSession(sAccount, sIPaddr, HUtil32.StrToInt(sSessionId, 0), HUtil32.StrToInt(sPayCost, 0), HUtil32.StrToInt(sPayMode, 0), HUtil32.StrToInt(sPlayTime, 0));
            }
            catch
            {
                LogService.Error(sExceptionMsg);
            }
        }

        private void GetCancelAdmission(string sData)
        {
            var sC = string.Empty;
            const string sExceptionMsg = "[Exception] AccountService:GetCancelAdmission";
            try
            {
                var sessionId = HUtil32.GetValidStr3(sData, ref sC, HUtil32.Backslash);
                DelSession(HUtil32.StrToInt(sessionId, 0));
            }
            catch (Exception e)
            {
                LogService.Error(sExceptionMsg);
                LogService.Error(e.Message);
            }
        }

        private void NewSession(string account, string sIPaddr, int sessionId, int payMent, int payMode, long playTime)
        {
            var sessInfo = new AccountSession();
            sessInfo.Account = account;
            sessInfo.IPaddr = sIPaddr;
            sessInfo.SessionId = sessionId;
            sessInfo.PayMent = payMent;
            sessInfo.PayMode = payMode;
            sessInfo.SessionStatus = 0;
            sessInfo.StartTick = HUtil32.GetTickCount();
            sessInfo.ActiveTick = HUtil32.GetTickCount();
            sessInfo.RefCount = 1;
            sessInfo.PlayTime = playTime;
            _sessionList.Add(sessInfo);
        }

        private void DelSession(int sessionId)
        {
            var sAccount = string.Empty;
            AccountSession sessInfo = null;
            const string sExceptionMsg = "[Exception] AccountService:DelSession";
            try
            {
                for (var i = 0; i < _sessionList.Count; i++)
                {
                    sessInfo = _sessionList[i];
                    if (sessInfo.SessionId == sessionId)
                    {
                        sAccount = sessInfo.Account;
                        _sessionList.RemoveAt(i);
                        sessInfo = null;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(sAccount))
                {
                    M2Share.NetChannel.KickUser(sAccount, sessionId, sessInfo?.PayMode ?? 0);
                }
            }
            catch (Exception e)
            {
                LogService.Error(sExceptionMsg);
                LogService.Error(e.Message);
            }
        }

        private void ClearSession()
        {
            for (var i = 0; i < _sessionList.Count; i++)
            {
                _sessionList[i] = null;
            }
            _sessionList.Clear();
        }

        public AccountSession GetAdmission(string sAccount, string sIPaddr, int sessionId, ref int nPayMode, ref int nPayMent, ref long playTime)
        {
            AccountSession result = null;
            var boFound = false;
            const string sGetFailMsg = "[非法登录] 全局会话验证失败({0}/{1}/{2})";
            nPayMent = 0;
            nPayMode = 0;
            for (var i = 0; i < _sessionList.Count; i++)
            {
                var sessInfo = _sessionList[i];
                if (sessInfo.SessionId == sessionId && sessInfo.Account == sAccount)
                {
                    switch (sessInfo.PayMent)
                    {
                        case 2:
                            nPayMent = 3;
                            break;
                        case 1:
                            nPayMent = 2;
                            break;
                        case 0:
                            nPayMent = 1;
                            break;
                    }
                    result = sessInfo;
                    nPayMode = sessInfo.PayMode;
                    playTime = sessInfo.PlayTime;
                    boFound = true;
                    break;
                }
            }
            if (SystemShare.Config.ViewAdmissionFailure && !boFound)
            {
                LogService.Error(string.Format(sGetFailMsg, sAccount, sIPaddr, sessionId));
            }
            return result;
        }

        private static void SetTotalHumanCount(string sData)
        {
            M2Share.TotalHumCount = HUtil32.StrToInt(sData, 0);
        }

        private void GetCancelAdmissionA(string sData)
        {
            var sAccount = string.Empty;
            const string sExceptionMsg = "[Exception] AccountService:GetCancelAdmissionA";
            try
            {
                var sessionData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
                var sessionId = HUtil32.StrToInt(sessionData, 0);
                if (!SystemShare.Config.TestServer)
                {
                    SystemShare.WorldEngine.AccountExpired(sAccount);
                    DelSession(sessionId);
                }
            }
            catch
            {
                LogService.Error(sExceptionMsg);
            }
        }

        private static void GetServerLoad(string data)
        {
            /*var sC = string.Empty;
            var s10 = string.Empty;
            var s14 = string.Empty;
            var s18 = string.Empty;
            var s1C = string.Empty;
            sData = HUtil32.GetValidStr3(sData, ref sC, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref s10, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref s14, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref s18, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref s1C, HUtil32.Backslash);
            M2Share.nCurrentMonthly = HUtil32.StrToInt(sC, 0);
            M2Share.nLastMonthlyTotalUsage = HUtil32.StrToInt(s10, 0);
            M2Share.nTotalTimeUsage = HUtil32.StrToInt(s14, 0);
            M2Share.nGrossTotalCnt = HUtil32.StrToInt(s18, 0);
            M2Share.nGrossResetCnt = HUtil32.StrToInt(s1C, 0);*/
        }

        private Task DataSocketConnect(ITcpClient client, ConnectedEventArgs e)
        {
            LogService.Info("登录服务器[" + client.RemoteIPHost + "]连接成功...");
            SendOnlineHumCountMsg(SystemShare.WorldEngine.OnlinePlayObject);
            return Task.CompletedTask;
        }

        private Task DataSocketDisconnect(ITcpClientBase sender, DisconnectEventArgs e)
        {
            // if (!Settings.Config.boIDSocketConnected)
            // {
            //     return;
            // }
            ClearSession();
            LogService.Error("登录服务器[" + sender.IP + "]断开连接...");
            return Task.CompletedTask;
        }

        public void Close()
        {
            _tcpClient.Close();
        }

        public int GetSessionCount()
        {
            return _sessionList.Count;
        }

        public void GetSessionList(IList<AccountSession> sessions)
        {
            for (var i = 0; i < _sessionList.Count; i++)
            {
                sessions.Add(_sessionList[i]);
            }
        }
    }
}