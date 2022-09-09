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
    /// 登陆会话服务
    /// </summary>
    public class LoginSvrService
    {
        private readonly MirLog _logger;
        private readonly ClientScoket _loginSocket;
        private readonly IList<TGlobaSessionInfo> _globaSessionList = null;
        private readonly SvrConf _config;
        private string _sockMsg = string.Empty;

        public LoginSvrService(MirLog logger, SvrConf conf)
        {
            _logger = logger;
            _config = conf;
            _loginSocket = new ClientScoket();
            _loginSocket.ReceivedDatagram += LoginSocketRead;
            _loginSocket.OnConnected += LoginSocketConnected;
            _loginSocket.OnDisconnected += LoginSocketDisconnected;
            _loginSocket.OnError += LoginSocketError;
            _globaSessionList = new List<TGlobaSessionInfo>();
        }

        private void LoginSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    _logger.LogWarning("账号服务器[" + _config.LoginServerAddr + ":" + _config.LoginServerPort + "]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    _logger.LogWarning("账号服务器[" + _config.LoginServerAddr + ":" + _config.LoginServerPort + "]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    _logger.LogWarning("账号服务器[" + _config.LoginServerAddr + ":" + _config.LoginServerPort + "]链接超时...");
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
            _loginSocket.Connect(_config.LoginServerAddr, _config.LoginServerPort);
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
            if (_loginSocket.IsConnected)
            {
                return;
            }
            if (_loginSocket.IsBusy)
            {
                return;
            }
            _logger.DebugLog($"重新链接账号服务器[{_loginSocket.EndPoint}].");
            _loginSocket.Connect(_config.LoginServerAddr, _config.LoginServerPort);
        }

        private void LoginSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            _sockMsg += HUtil32.GetString(e.Buff, 0, e.BuffLen);
            if (_sockMsg.IndexOf(")", StringComparison.Ordinal) > 0)
            {
                ProcessSocketMsg();
            }
        }

        private void ProcessSocketMsg()
        {
            string sData = string.Empty;
            string sCode = string.Empty;
            string sScoketText = _sockMsg;
            while (sScoketText.IndexOf(")", StringComparison.Ordinal) > 0)
            {
                sScoketText = HUtil32.ArrestStringEx(sScoketText, "(", ")", ref sData);
                if (string.IsNullOrEmpty(sData))
                {
                    break;
                }
                string sBody = HUtil32.GetValidStr3(sData, ref sCode, HUtil32.Backslash);
                int nIdent = HUtil32.Str_ToInt(sCode, 0);
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
            if (_loginSocket.IsConnected)
            {
                _loginSocket.SendText(sSendText);
            }
        }

        public bool CheckSession(string sAccount, string sIPaddr, int nSessionID)
        {
            bool result = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.sAccount == sAccount) && (globaSessionInfo.nSessionID == nSessionID))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public int CheckSessionLoadRcd(string sAccount, string sIPaddr, int nSessionID, ref bool boFoundSession)
        {
            int result = -1;
            boFoundSession = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var GlobaSessionInfo = _globaSessionList[i];
                if (GlobaSessionInfo != null)
                {
                    if ((GlobaSessionInfo.sAccount == sAccount) && (GlobaSessionInfo.nSessionID == nSessionID))
                    {
                        boFoundSession = true;
                        if (!GlobaSessionInfo.boLoadRcd)
                        {
                            GlobaSessionInfo.boLoadRcd = true;
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
                var GlobaSessionInfo = _globaSessionList[i];
                if (GlobaSessionInfo != null)
                {
                    if ((GlobaSessionInfo.sAccount == sAccount))
                    {
                        GlobaSessionInfo.boLoadRcd = false;
                        result = true;
                    }
                }
            }
            return result;
        }

        public void SetGlobaSessionNoPlay(int nSessionID)
        {
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var GlobaSessionInfo = _globaSessionList[i];
                if (GlobaSessionInfo != null)
                {
                    if ((GlobaSessionInfo.nSessionID == nSessionID))
                    {
                        GlobaSessionInfo.boStartPlay = false;
                        break;
                    }
                }
            }
        }

        public void SetGlobaSessionPlay(int nSessionID)
        {
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var GlobaSessionInfo = _globaSessionList[i];
                if (GlobaSessionInfo != null)
                {
                    if ((GlobaSessionInfo.nSessionID == nSessionID))
                    {
                        GlobaSessionInfo.boStartPlay = true;
                        break;
                    }
                }
            }
        }

        public bool GetGlobaSessionStatus(int nSessionID)
        {
            bool result = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var GlobaSessionInfo = _globaSessionList[i];
                if (GlobaSessionInfo != null)
                {
                    if ((GlobaSessionInfo.nSessionID == nSessionID))
                    {
                        result = GlobaSessionInfo.boStartPlay;
                        break;
                    }
                }
            }
            return result;
        }

        public void CloseSession(string sAccount, int nSessionID)
        {
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var GlobaSessionInfo = _globaSessionList[i];
                if (GlobaSessionInfo != null)
                {
                    if ((GlobaSessionInfo.nSessionID == nSessionID))
                    {
                        if (GlobaSessionInfo.sAccount == sAccount)
                        {
                            GlobaSessionInfo = null;
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
            TGlobaSessionInfo GlobaSessionInfo = new TGlobaSessionInfo();
            GlobaSessionInfo.sAccount = sAccount;
            GlobaSessionInfo.sIPaddr = sIPaddr;
            GlobaSessionInfo.nSessionID = HUtil32.Str_ToInt(s10, 0);
            //GlobaSessionInfo.n24 = HUtil32.Str_ToInt(s14, 0);
            GlobaSessionInfo.boStartPlay = false;
            GlobaSessionInfo.boLoadRcd = false;
            GlobaSessionInfo.dwAddTick = HUtil32.GetTickCount();
            GlobaSessionInfo.dAddDate = DateTime.Now;
            _globaSessionList.Add(GlobaSessionInfo);

            _logger.DebugLog("收到账号中心同步会话消息.");
        }

        private void ProcessDelSession(string sData)
        {
            string sAccount = string.Empty;
            TGlobaSessionInfo GlobaSessionInfo;
            sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
            int nSessionID = HUtil32.Str_ToInt(sData, 0);
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                GlobaSessionInfo = _globaSessionList[i];
                if (GlobaSessionInfo != null)
                {
                    if ((GlobaSessionInfo.nSessionID == nSessionID) && (GlobaSessionInfo.sAccount == sAccount))
                    {
                        GlobaSessionInfo = null;
                        _globaSessionList.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public bool GetSession(string sAccount, string sIPaddr)
        {
            bool result = false;
            TGlobaSessionInfo GlobaSessionInfo;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                GlobaSessionInfo = _globaSessionList[i];
                if (GlobaSessionInfo != null)
                {
                    if ((GlobaSessionInfo.sAccount == sAccount) && (GlobaSessionInfo.sIPaddr == sIPaddr))
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
            if (_loginSocket.IsConnected)
            {
                _loginSocket.SendText("(" + Grobal2.SS_SERVERINFO + "/" + _config.ServerName + "/" + "99" + "/" + userCount + ")");
            }
        }
    }
}