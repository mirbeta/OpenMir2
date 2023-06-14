using M2Server;
using NLog;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using SystemModule.Data;
using SystemModule.SocketComponents.AsyncSocketClient;
using SystemModule.SocketComponents.Event;

namespace GameSrv.Services
{
    public class AccountSessionService : IAccountSessionService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IList<PlayerSession> _sessionList;
        private readonly ScoketClient _clientScoket;
        private readonly object UserIDSection;
        private string SocketRecvText = string.Empty;
        private bool SocketConnected = false;

        public AccountSessionService()
        {
            _sessionList = new List<PlayerSession>();
            _clientScoket = new ScoketClient(new IPEndPoint(IPAddress.Parse(SystemShare.Config.sIDSAddr), SystemShare.Config.nIDSPort));
            _clientScoket.OnConnected += IDSocketConnect;
            _clientScoket.OnDisconnected += IDSocketDisconnect;
            _clientScoket.OnError += IDSocketError;
            _clientScoket.OnReceivedData += IdSocketRead;
            UserIDSection = new object();
        }

        public void CheckConnected()
        {
            if (_clientScoket.IsConnected)
            {
                return;
            }
            if (_clientScoket.IsBusy)
            {
                return;
            }
            _clientScoket.Connect();
        }

        private void IdSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            HUtil32.EnterCriticalSection(UserIDSection);
            try
            {
                string recvText = HUtil32.GetString(e.Buff, 0, e.BuffLen);
                SocketRecvText += recvText;
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserIDSection);
            }
        }

        private void IDSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case SocketError.ConnectionRefused:
                    _logger.Error("登录服务器[" + _clientScoket.RemoteEndPoint + "]拒绝链接...");
                    break;
                case SocketError.ConnectionReset:
                    _logger.Error("登录服务器[" + _clientScoket.RemoteEndPoint + "]关闭连接...");
                    break;
                case SocketError.TimedOut:
                    _logger.Error("登录服务器[" + _clientScoket.RemoteEndPoint + "]链接超时...");
                    break;
            }
        }

        public void Initialize()
        {
            CheckConnected();
            _logger.Debug("登录服务器连接初始化完成...");
        }

        public void SendSocket(string sSendMsg)
        {
            if (_clientScoket == null || !_clientScoket.IsConnected) return;
            byte[] data = HUtil32.GetBytes(sSendMsg);
            _clientScoket.Send(data);
        }

        public void SendHumanLogOutMsg(string sUserId, int nId)
        {
            const string sFormatMsg = "({0}/{1}/{2})";
            for (int i = 0; i < _sessionList.Count; i++)
            {
                PlayerSession sessInfo = _sessionList[i];
                if (sessInfo.SessionId == nId && sessInfo.Account == sUserId)
                {
                    break;
                }
            }
            SendSocket(string.Format(sFormatMsg, Messages.SS_SOFTOUTSESSION, sUserId, nId));
        }

        public void SendHumanLogOutMsgA(string sUserID, int nID)
        {
            for (int i = _sessionList.Count - 1; i >= 0; i--)
            {
                PlayerSession sessInfo = _sessionList[i];
                if (sessInfo.SessionId == nID && sessInfo.Account == sUserID)
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
            string sData = string.Empty;
            string sCode = string.Empty;
            const string sExceptionMsg = "[Exception] AccountService:DecodeSocStr";
            HUtil32.EnterCriticalSection(UserIDSection);
            try
            {
                if (string.IsNullOrEmpty(SocketRecvText))
                {
                    return;
                }
                if (SocketRecvText.IndexOf(')') <= 0)
                {
                    return;
                }
                sSocketText = SocketRecvText;
                SocketRecvText = string.Empty;
            }
            finally
            {
                HUtil32.LeaveCriticalSection(UserIDSection);
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
                    string sBody = HUtil32.GetValidStr3(sData, ref sCode, HUtil32.Backslash);
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
                HUtil32.EnterCriticalSection(UserIDSection);
                try
                {
                    SocketRecvText = sSocketText + SocketRecvText;
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(UserIDSection);
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
        }

        private static void QueryAccountExpired(string sData)
        {
            string account = string.Empty;
            string certstr = HUtil32.GetValidStr3(sData, ref account, '/');
            int cert = HUtil32.StrToInt(certstr, 0);
            if (!SystemShare.Config.TestServer)
            {
                int playTime = M2Share.WorldEngine.GetPlayExpireTime(account);
                if (playTime >= 3600 || playTime < 1800) //大于一个小时或者小于半个小时都不处理
                {
                    return;
                }
                if (cert <= 1800)//小于30分钟一分钟查询一次，否则10分钟或者半个小时同步一次都行
                {
                    M2Share.WorldEngine.SetPlayExpireTime(account, cert);
                }
                else
                {
                    M2Share.WorldEngine.SetPlayExpireTime(account, cert);
                }
            }
        }

        private void GetAccountExpired(string sData)
        {
            string account = string.Empty;
            string certstr = HUtil32.GetValidStr3(sData, ref account, '/');
            int cert = HUtil32.StrToInt(certstr, 0);
            if (!SystemShare.Config.TestServer)
            {
                M2Share.WorldEngine.AccountExpired(account);
                DelSession(cert);
            }
        }

        private void GetPasswdSuccess(string sData)
        {
            string sAccount = string.Empty;
            string sSessionID = string.Empty;
            string sPayCost = string.Empty;
            string sIPaddr = string.Empty;
            string sPayMode = string.Empty;
            string sPlayTime = string.Empty;
            const string sExceptionMsg = "[Exception] AccountService:GetPasswdSuccess";
            try
            {
                //todo 这里要获取账号剩余游戏时间
                sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
                sData = HUtil32.GetValidStr3(sData, ref sSessionID, HUtil32.Backslash);
                sData = HUtil32.GetValidStr3(sData, ref sPayCost, HUtil32.Backslash);// boPayCost
                sData = HUtil32.GetValidStr3(sData, ref sPayMode, HUtil32.Backslash);// nPayMode
                sData = HUtil32.GetValidStr3(sData, ref sIPaddr, HUtil32.Backslash);// sIPaddr
                sData = HUtil32.GetValidStr3(sData, ref sPlayTime, HUtil32.Backslash);// playTime
                NewSession(sAccount, sIPaddr, HUtil32.StrToInt(sSessionID, 0), HUtil32.StrToInt(sPayCost, 0), HUtil32.StrToInt(sPayMode, 0), HUtil32.StrToInt(sPlayTime, 0));
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
        }

        private void GetCancelAdmission(string sData)
        {
            string sC = string.Empty;
            const string sExceptionMsg = "[Exception] AccountService:GetCancelAdmission";
            try
            {
                string sSessionID = HUtil32.GetValidStr3(sData, ref sC, HUtil32.Backslash);
                DelSession(HUtil32.StrToInt(sSessionID, 0));
            }
            catch (Exception e)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.Message);
            }
        }

        private void NewSession(string sAccount, string sIPaddr, int nSessionID, int nPayMent, int nPayMode, long playTime)
        {
            PlayerSession sessInfo = new PlayerSession();
            sessInfo.Account = sAccount;
            sessInfo.IPaddr = sIPaddr;
            sessInfo.SessionId = nSessionID;
            sessInfo.PayMent = nPayMent;
            sessInfo.PayMode = nPayMode;
            sessInfo.SessionStatus = 0;
            sessInfo.dwStartTick = HUtil32.GetTickCount();
            sessInfo.ActiveTick = HUtil32.GetTickCount();
            sessInfo.nRefCount = 1;
            sessInfo.PlayTime = playTime;
            _sessionList.Add(sessInfo);
        }

        private void DelSession(int sessionId)
        {
            var sAccount = string.Empty;
            PlayerSession sessInfo = null;
            const string sExceptionMsg = "[Exception] AccountService:DelSession";
            try
            {
                for (int i = 0; i < _sessionList.Count; i++)
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
                    M2Share.SocketMgr.KickUser(sAccount, sessionId, sessInfo?.PayMode ?? 0);
                }
            }
            catch (Exception e)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.Message);
            }
        }

        private void ClearSession()
        {
            for (int i = 0; i < _sessionList.Count; i++)
            {
                _sessionList[i] = null;
            }
            _sessionList.Clear();
        }

        public PlayerSession GetAdmission(string sAccount, string sIPaddr, int nSessionID, ref int nPayMode, ref int nPayMent, ref long playTime)
        {
            PlayerSession result = null;
            bool boFound = false;
            const string sGetFailMsg = "[非法登录] 全局会话验证失败({0}/{1}/{2})";
            nPayMent = 0;
            nPayMode = 0;
            for (var i = 0; i < _sessionList.Count; i++)
            {
                var sessInfo = _sessionList[i];
                if (sessInfo.SessionId == nSessionID && sessInfo.Account == sAccount)
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
                _logger.Error(string.Format(sGetFailMsg, sAccount, sIPaddr, nSessionID));
            }
            return result;
        }

        private static void SetTotalHumanCount(string sData)
        {
            M2Share.TotalHumCount = HUtil32.StrToInt(sData, 0);
        }

        private void GetCancelAdmissionA(string sData)
        {
            string sAccount = string.Empty;
            const string sExceptionMsg = "[Exception] AccountService:GetCancelAdmissionA";
            try
            {
                string sessionId = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
                int sessionID = HUtil32.StrToInt(sessionId, 0);
                if (!SystemShare.Config.TestServer)
                {
                    M2Share.WorldEngine.AccountExpired(sAccount);
                    DelSession(sessionID);
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
        }

        private static void GetServerLoad(string sData)
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

        private void IDSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            SocketConnected = true;
            _logger.Info("登录服务器[" + _clientScoket.RemoteEndPoint + "]连接成功...");
            SendOnlineHumCountMsg(M2Share.WorldEngine.OnlinePlayObject);
        }

        private void IDSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            // if (!Settings.Config.boIDSocketConnected)
            // {
            //     return;
            // }
            ClearSession();
            SocketConnected = false;
            _clientScoket.IsConnected = false;
            _logger.Error("登录服务器[" + _clientScoket.RemoteEndPoint + "]断开连接...");
        }

        public void Close()
        {
            _clientScoket.Disconnect();
        }

        public int GetSessionCount()
        {
            return _sessionList.Count;
        }

        public void GetSessionList(ArrayList List)
        {
            for (int i = 0; i < _sessionList.Count; i++)
            {
                List.Add(_sessionList[i]);
            }
        }
    }
}