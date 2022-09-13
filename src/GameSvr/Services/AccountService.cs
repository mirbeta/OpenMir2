using NLog;
using System.Collections;
using SystemModule;
using SystemModule.Data;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace GameSvr.Services
{
    public class AccountService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int _dwClearEmptySessionTick;
        private readonly IList<TSessInfo> _sessionList;
        private readonly ClientScoket _clientScoket;

        public AccountService()
        {
            _sessionList = new List<TSessInfo>();
            M2Share.Config.boIDSocketConnected = false;
            _clientScoket = new ClientScoket();
            _clientScoket.OnConnected += IDSocketConnect;
            _clientScoket.OnDisconnected += IDSocketDisconnect;
            _clientScoket.OnError += IDSocketError;
            _clientScoket.ReceivedDatagram += IdSocketRead;
            if (M2Share.Config != null)
            {
                _clientScoket.Host = M2Share.Config.sIDSAddr;
                _clientScoket.Port = M2Share.Config.nIDSPort;
            }
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
            _clientScoket.Connect(_clientScoket.Host, _clientScoket.Port);
        }

        private void IdSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            HUtil32.EnterCriticalSection(M2Share.Config.UserIDSection);
            try
            {
                var recvText = HUtil32.GetString(e.Buff, 0, e.BuffLen);
                M2Share.Config.sIDSocketRecvText += recvText;
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.Config.UserIDSection);
            }
        }

        private void IDSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    _logger.Error("登录服务器[" + _clientScoket.Host + ":" + _clientScoket.Port + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    _logger.Error("登录服务器[" + _clientScoket.Host + ":" + _clientScoket.Port + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    _logger.Error("登录服务器[" + _clientScoket.Host + ":" + _clientScoket.Port + "]链接超时...");
                    break;
            }
        }

        public void Initialize()
        {
            CheckConnected();
        }

        private void SendSocket(string sSendMsg)
        {
            if (_clientScoket == null || !_clientScoket.IsConnected) return;
            var data = HUtil32.GetBytes(sSendMsg);
            _clientScoket.Send(data);
        }

        public void SendHumanLogOutMsg(string sUserId, int nId)
        {
            const string sFormatMsg = "({0}/{1}/{2})";
            for (var i = 0; i < _sessionList.Count; i++)
            {
                var sessInfo = _sessionList[i];
                if (sessInfo.nSessionID == nId && sessInfo.sAccount == sUserId)
                {
                    break;
                }
            }
            SendSocket(string.Format(sFormatMsg, Grobal2.SS_SOFTOUTSESSION, sUserId, nId));
        }

        public void SendHumanLogOutMsgA(string sUserID, int nID)
        {
            for (var i = _sessionList.Count - 1; i >= 0; i--)
            {
                var sessInfo = _sessionList[i];
                if (sessInfo.nSessionID == nID && sessInfo.sAccount == sUserID)
                {
                    break;
                }
            }
        }

        public void SendLogonCostMsg(string sAccount, int nTime)
        {
            const string sFormatMsg = "({0}/{1}/{2})";
            SendSocket(string.Format(sFormatMsg, new object[] { Grobal2.SS_LOGINCOST, sAccount, nTime }));
        }

        public void SendOnlineHumCountMsg(int nCount)
        {
            const string sFormatMsg = "({0}/{1}/{2}/{3})";
            SendSocket(string.Format(sFormatMsg, Grobal2.SS_SERVERINFO, M2Share.Config.ServerName, M2Share.ServerIndex, nCount));
        }

        public void Run()
        {
            string sSocketText;
            var sData = string.Empty;
            var sCode = string.Empty;
            const string sExceptionMsg = "[Exception] TFrmIdSoc::DecodeSocStr";
            var Config = M2Share.Config;
            HUtil32.EnterCriticalSection(Config.UserIDSection);
            try
            {
                if (string.IsNullOrEmpty(Config.sIDSocketRecvText))
                {
                    return;
                }
                if (Config.sIDSocketRecvText.IndexOf(')') <= 0)
                {
                    return;
                }
                sSocketText = Config.sIDSocketRecvText;
                Config.sIDSocketRecvText = string.Empty;
            }
            finally
            {
                HUtil32.LeaveCriticalSection(Config.UserIDSection);
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
                    switch (HUtil32.Str_ToInt(sCode, 0))
                    {
                        case Grobal2.SS_OPENSESSION:// 100
                            GetPasswdSuccess(sBody);
                            break;
                        case Grobal2.SS_CLOSESESSION:// 101
                            GetCancelAdmission(sBody);
                            break;
                        case Grobal2.SS_KEEPALIVE:// 104
                            SetTotalHumanCount(sBody);
                            break;
                        case Grobal2.UNKNOWMSG:
                            break;
                        case Grobal2.SS_KICKUSER:// 111
                            GetCancelAdmissionA(sBody);
                            break;
                        case Grobal2.SS_SERVERLOAD:// 113
                            GetServerLoad(sBody);
                            break;
                    }
                    if (sSocketText.IndexOf(')') <= 0)
                    {
                        break;
                    }
                }
                HUtil32.EnterCriticalSection(Config.UserIDSection);
                try
                {
                    Config.sIDSocketRecvText = sSocketText + Config.sIDSocketRecvText;
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(Config.UserIDSection);
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
            if ((HUtil32.GetTickCount() - _dwClearEmptySessionTick) > 10000)
            {
                _dwClearEmptySessionTick = HUtil32.GetTickCount();
            }
        }

        private void GetPasswdSuccess(string sData)
        {
            var sAccount = string.Empty;
            var sSessionID = string.Empty;
            var sPayCost = string.Empty;
            var sIPaddr = string.Empty;
            var sPayMode = string.Empty;
            const string sExceptionMsg = "[Exception] TFrmIdSoc::GetPasswdSuccess";
            try
            {
                sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
                sData = HUtil32.GetValidStr3(sData, ref sSessionID, HUtil32.Backslash);
                sData = HUtil32.GetValidStr3(sData, ref sPayCost, HUtil32.Backslash);// boPayCost
                sData = HUtil32.GetValidStr3(sData, ref sPayMode, HUtil32.Backslash);// nPayMode
                sData = HUtil32.GetValidStr3(sData, ref sIPaddr, HUtil32.Backslash);// sIPaddr
                NewSession(sAccount, sIPaddr, HUtil32.Str_ToInt(sSessionID, 0), HUtil32.Str_ToInt(sPayCost, 0), HUtil32.Str_ToInt(sPayMode, 0));
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
        }

        private void GetCancelAdmission(string sData)
        {
            var sC = string.Empty;
            const string sExceptionMsg = "[Exception] TFrmIdSoc::GetCancelAdmission";
            try
            {
                var sSessionID = HUtil32.GetValidStr3(sData, ref sC, HUtil32.Backslash);
                DelSession(HUtil32.Str_ToInt(sSessionID, 0));
            }
            catch (Exception e)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.Message);
            }
        }

        private void NewSession(string sAccount, string sIPaddr, int nSessionID, int nPayMent, int nPayMode)
        {
            var sessInfo = new TSessInfo();
            sessInfo.sAccount = sAccount;
            sessInfo.sIPaddr = sIPaddr;
            sessInfo.nSessionID = nSessionID;
            sessInfo.PayMent = nPayMent;
            sessInfo.PayMode = nPayMode;
            sessInfo.SessionStatus = 0;
            sessInfo.dwStartTick = HUtil32.GetTickCount();
            sessInfo.ActiveTick = HUtil32.GetTickCount();
            sessInfo.nRefCount = 1;
            _sessionList.Add(sessInfo);
        }

        private void DelSession(int nSessionID)
        {
            var sAccount = string.Empty;
            TSessInfo SessInfo = null;
            const string sExceptionMsg = "[Exception] FrmIdSoc::DelSession";
            try
            {
                for (var i = 0; i < _sessionList.Count; i++)
                {
                    SessInfo = _sessionList[i];
                    if (SessInfo.nSessionID == nSessionID)
                    {
                        sAccount = SessInfo.sAccount;
                        _sessionList.RemoveAt(i);
                        SessInfo = null;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(sAccount))
                {
                    M2Share.GateMgr.KickUser(sAccount, nSessionID, SessInfo == null ? 0 : SessInfo.PayMode);
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
            for (var i = 0; i < _sessionList.Count; i++)
            {
                _sessionList[i] = null;
            }
            _sessionList.Clear();
        }

        public TSessInfo GetAdmission(string sAccount, string sIPaddr, int nSessionID, ref int nPayMode, ref int nPayMent)
        {
            TSessInfo result = null;
            var boFound = false;
            const string sGetFailMsg = "[非法登录] 全局会话验证失败({0}/{1}/{2})";
            nPayMent = 0;
            nPayMode = 0;
            for (var i = 0; i < _sessionList.Count; i++)
            {
                var sessInfo = _sessionList[i];
                if (sessInfo.nSessionID == nSessionID && sessInfo.sAccount == sAccount)
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
                    boFound = true;
                    break;
                }
            }
            if (M2Share.Config.ViewAdmissionFailure && !boFound)
            {
                _logger.Error(string.Format(sGetFailMsg, new object[] { sAccount, sIPaddr, nSessionID }));
            }
            return result;
        }

        private void SetTotalHumanCount(string sData)
        {
            M2Share.g_nTotalHumCount = HUtil32.Str_ToInt(sData, 0);
        }

        private void GetCancelAdmissionA(string sData)
        {
            var sAccount = string.Empty;
            const string sExceptionMsg = "[Exception] FrmIdSoc::GetCancelAdmissionA";
            try
            {
                var sSessionID = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
                var nSessionID = HUtil32.Str_ToInt(sSessionID, 0);
                if (!M2Share.Config.boTestServer)
                {
                    M2Share.WorldEngine.HumanExpire(sAccount);
                    DelSession(nSessionID);
                }
            }
            catch
            {
                _logger.Error(sExceptionMsg);
            }
        }

        private void GetServerLoad(string sData)
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
            M2Share.nCurrentMonthly = HUtil32.Str_ToInt(sC, 0);
            M2Share.nLastMonthlyTotalUsage = HUtil32.Str_ToInt(s10, 0);
            M2Share.nTotalTimeUsage = HUtil32.Str_ToInt(s14, 0);
            M2Share.nGrossTotalCnt = HUtil32.Str_ToInt(s18, 0);
            M2Share.nGrossResetCnt = HUtil32.Str_ToInt(s1C, 0);*/
        }

        private void IDSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            M2Share.Config.boIDSocketConnected = true;
            _logger.Info("登录服务器[" + _clientScoket.Host + ":" + _clientScoket.Port + "]连接成功...");
            SendOnlineHumCountMsg(M2Share.WorldEngine.OnlinePlayObject);
        }

        private void IDSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            // if (!M2Share.g_Config.boIDSocketConnected)
            // {
            //     return;
            // }
            ClearSession();
            M2Share.Config.boIDSocketConnected = false;
            _clientScoket.IsConnected = false;
            _logger.Error("登录服务器[" + _clientScoket.Host + ":" + _clientScoket.Port + "]断开连接...");
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
            for (var i = 0; i < _sessionList.Count; i++)
            {
                List.Add(_sessionList[i]);
            }
        }
    }

    public class IdSrvClient
    {
        private static AccountService instance;

        public static AccountService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountService();
                }
                return instance;
            }
        }
    }
}