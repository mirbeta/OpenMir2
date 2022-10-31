using DBSvr.Conf;
using System;
using System.Collections.Generic;
using SystemModule;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace DBSvr.Services
{
    /// <summary>
    /// 账号中心
    /// 会话同步服务(DBSvr-LoginSvr)
    /// </summary>
    public class LoginSvrService
    {
        private readonly MirLogger _logger;
        private readonly ClientScoket _clientScoket;
        private readonly IList<GlobaSessionInfo> _globaSessionList = null;
        private readonly DBSvrConf _conf;
        private string _sockMsg = string.Empty;

        public LoginSvrService(MirLogger logger, DBSvrConf conf)
        {
            _logger = logger;
            _conf = conf;
            _clientScoket = new ClientScoket();
            _clientScoket.ReceivedDatagram += LoginSocketRead;
            _clientScoket.OnConnected += LoginSocketConnected;
            _clientScoket.OnDisconnected += LoginSocketDisconnected;
            _clientScoket.OnError += LoginSocketError;
            _globaSessionList = new List<GlobaSessionInfo>();
        }

        private void LoginSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    _logger.LogWarning("账号服务器[" + _conf.LoginServerAddr + ":" + _conf.LoginServerPort + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    _logger.LogWarning("账号服务器[" + _conf.LoginServerAddr + ":" + _conf.LoginServerPort + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    _logger.LogWarning("账号服务器[" + _conf.LoginServerAddr + ":" + _conf.LoginServerPort + "]链接超时...");
                    break;
            }
        }

        private void LoginSocketConnected(object sender, DSCClientConnectedEventArgs e)
        {
            _logger.LogInformation($"账号服务器[{e.RemoteEndPoint}]链接成功.");
        }

        private void LoginSocketDisconnected(object sender, DSCClientConnectedEventArgs e)
        {
            _logger.LogError($"账号服务器[{e.RemoteEndPoint}]断开链接.");
        }

        public void Start()
        {
            _clientScoket.Connect(_conf.LoginServerAddr, _conf.LoginServerPort);
        }

        public void Stop()
        {
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                _globaSessionList[i] = null;
            }
        }

        public void CheckConnection()
        {
            if (_clientScoket.IsConnected)
            {
                return;
            }
            if (_clientScoket.IsBusy)
            {
                return;
            }
            _logger.DebugLog($"重新链接账号服务器[{_clientScoket.EndPoint}].");
            _clientScoket.Connect(_conf.LoginServerAddr, _conf.LoginServerPort);
        }

        private void LoginSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            _sockMsg += HUtil32.GetString(e.Buff, 0, e.BuffLen);
            if (_sockMsg.IndexOf(")", StringComparison.OrdinalIgnoreCase) > 0)
            {
                ProcessSocketMsg();
            }
        }

        private void ProcessSocketMsg()
        {
            string sData = string.Empty;
            string sCode = string.Empty;
            string sScoketText = _sockMsg;
            while (sScoketText.IndexOf(")", StringComparison.OrdinalIgnoreCase) > 0)
            {
                sScoketText = HUtil32.ArrestStringEx(sScoketText, "(", ")", ref sData);
                if (string.IsNullOrEmpty(sData))
                {
                    break;
                }
                string sBody = HUtil32.GetValidStr3(sData, ref sCode, HUtil32.Backslash);
                int nIdent = HUtil32.StrToInt(sCode, 0);
                switch (nIdent)
                {
                    case Grobal2.SS_OPENSESSION:
                        ProcessAddSession(sBody);
                        break;
                    case Grobal2.SS_CLOSESESSION:
                        ProcessDelSession(sBody);
                        break;
                    case Grobal2.SS_KEEPALIVE:
                        ProcessGetOnlineCount(sBody);
                        break;
                }
            }
            _sockMsg = sScoketText;
        }

        public void SendSocketMsg(short wIdent, string sMsg)
        {
            const string sFormatMsg = "({0}/{1})";
            string sSendText = string.Format(sFormatMsg, wIdent, sMsg);
            if (_clientScoket.IsConnected)
            {
                _clientScoket.SendText(sSendText);
            }
        }

        public bool CheckSession(string account, string sIPaddr, int sessionId)
        {
            bool result = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.sAccount == account) && (globaSessionInfo.nSessionID == sessionId))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public int CheckSessionLoadRcd(string sAccount, string sIPaddr, int nSessionId, ref bool boFoundSession)
        {
            int result = -1;
            boFoundSession = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.sAccount == sAccount) && (globaSessionInfo.nSessionID == nSessionId))
                    {
                        boFoundSession = true;
                        if (!globaSessionInfo.boLoadRcd)
                        {
                            globaSessionInfo.boLoadRcd = true;
                            result = 1;
                        }
                        break;
                    }
                }
            }
            return result;
        }

        public bool SetSessionSaveRcd(string sAccount)
        {
            bool result = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.sAccount == sAccount))
                    {
                        globaSessionInfo.boLoadRcd = false;
                        result = true;
                    }
                }
            }
            return result;
        }

        public void SetGlobaSessionNoPlay(int nSessionId)
        {
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.nSessionID == nSessionId))
                    {
                        globaSessionInfo.boStartPlay = false;
                        break;
                    }
                }
            }
        }

        public void SetGlobaSessionPlay(int nSessionId)
        {
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.nSessionID == nSessionId))
                    {
                        globaSessionInfo.boStartPlay = true;
                        break;
                    }
                }
            }
        }

        public bool GetGlobaSessionStatus(int nSessionId)
        {
            bool result = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.nSessionID == nSessionId))
                    {
                        result = globaSessionInfo.boStartPlay;
                        break;
                    }
                }
            }
            return result;
        }

        public void CloseSession(string sAccount, int nSessionId)
        {
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.nSessionID == nSessionId))
                    {
                        if (globaSessionInfo.sAccount == sAccount)
                        {
                            globaSessionInfo = null;
                            _globaSessionList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        private void ProcessAddSession(string sData)
        {
            string sAccount = string.Empty;
            string s10 = string.Empty;
            string s14 = string.Empty;
            string s18 = string.Empty;
            string sIPaddr = string.Empty;
            sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref s10, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref s14, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref s18, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref sIPaddr, HUtil32.Backslash);
            GlobaSessionInfo globaSessionInfo = new GlobaSessionInfo();
            globaSessionInfo.sAccount = sAccount;
            globaSessionInfo.sIPaddr = sIPaddr;
            globaSessionInfo.nSessionID = HUtil32.StrToInt(s10, 0);
            //GlobaSessionInfo.n24 = HUtil32.Str_ToInt(s14, 0);
            globaSessionInfo.boStartPlay = false;
            globaSessionInfo.boLoadRcd = false;
            globaSessionInfo.dwAddTick = HUtil32.GetTickCount();
            globaSessionInfo.dAddDate = DateTime.Now;
            _globaSessionList.Add(globaSessionInfo);

            _logger.DebugLog("收到账号中心同步会话消息.");
        }

        private void ProcessDelSession(string sData)
        {
            string sAccount = string.Empty;
            sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
            int nSessionId = HUtil32.StrToInt(sData, 0);
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.nSessionID == nSessionId) && (globaSessionInfo.sAccount == sAccount))
                    {
                        globaSessionInfo = null;
                        _globaSessionList.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public bool GetSession(string sAccount, string sIPaddr)
        {
            bool result = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.sAccount == sAccount) && (globaSessionInfo.sIPaddr == sIPaddr))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private void ProcessGetOnlineCount(string sData)
        {

        }

        public void SendKeepAlivePacket(int userCount)
        {
            if (_clientScoket.IsConnected)
            {
                _clientScoket.SendText("(" + Grobal2.SS_SERVERINFO + "/" + _conf.ServerName + "/" + "99" + "/" + userCount + ")");
            }
        }
    }
}