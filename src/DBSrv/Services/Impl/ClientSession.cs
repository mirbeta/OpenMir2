using DBSrv.Conf;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SystemModule;
using TouchSocket.Core;
using TouchSocket.Sockets;
using TcpClient = TouchSocket.Sockets.TcpClient;

namespace DBSrv.Services.Impl
{
    /// <summary>
    /// 登陆会话同步服务(DBSrv-LoginSrv)
    /// </summary>
    public class ClientSession
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TcpClient _clientScoket;
        private readonly IList<GlobaSessionInfo> _globaSessionList = null;
        private readonly SettingConf _setting;
        private string _sockMsg = string.Empty;

        public ClientSession(SettingConf conf)
        {
            _setting = conf;
            _clientScoket = new TcpClient();
            _clientScoket.Setup(new TouchSocketConfig().SetRemoteIPHost(new IPHost(IPAddress.Parse(_setting.LoginServerAddr), _setting.LoginServerPort)).ConfigureContainer(x => { x.AddConsoleLogger(); }));

            _clientScoket.Received += LoginSocketRead;
            _clientScoket.Connected += LoginSocketConnected;
            _clientScoket.Disconnected += LoginSocketDisconnected;
            _globaSessionList = new List<GlobaSessionInfo>();
        }

        private Task LoginSocketConnected(ITcpClientBase client, ConnectedEventArgs e)
        {
            _logger.Info($"账号服务器[{((TcpClientBase)client).RemoteIPHost.EndPoint}]链接成功.");
            return Task.CompletedTask;
        }

        private Task LoginSocketDisconnected(ITcpClientBase client, DisconnectEventArgs e)
        {
            _logger.Error($"账号服务器[{((TcpClientBase)client).RemoteIPHost.EndPoint}]断开链接.");
            return Task.CompletedTask;
        }

        public void Start()
        {
            _clientScoket.Connect();
        }

        public void Stop()
        {
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                _globaSessionList[i] = null;
            }
        }

        private Task LoginSocketRead(IClient client, ReceivedDataEventArgs e)
        {
            _sockMsg += HUtil32.GetString(e.ByteBlock.Buffer, 0, e.ByteBlock.Len);
            if (_sockMsg.IndexOf(")", StringComparison.OrdinalIgnoreCase) > 0)
            {
                ProcessSocketMsg();
            }
            return Task.CompletedTask;
        }

        private void ProcessSocketMsg()
        {
            var sData = string.Empty;
            var sCode = string.Empty;
            var sScoketText = _sockMsg;
            while (sScoketText.IndexOf(")", StringComparison.OrdinalIgnoreCase) > 0)
            {
                sScoketText = HUtil32.ArrestStringEx(sScoketText, "(", ")", ref sData);
                if (string.IsNullOrEmpty(sData))
                {
                    break;
                }
                var sBody = HUtil32.GetValidStr3(sData, ref sCode, HUtil32.Backslash);
                var nIdent = HUtil32.StrToInt(sCode, 0);
                switch (nIdent)
                {
                    case Messages.SS_OPENSESSION:
                        ProcessAddSession(sBody);
                        break;
                    case Messages.SS_CLOSESESSION:
                        ProcessDelSession(sBody);
                        break;
                    case Messages.SS_KEEPALIVE:
                        ProcessGetOnlineCount(sBody);
                        break;
                }
            }
            _sockMsg = sScoketText;
        }

        public void SendSocketMsg(short wIdent, string sMsg)
        {
            const string sFormatMsg = "({0}/{1})";
            var sSendText = string.Format(sFormatMsg, wIdent, sMsg);
            _clientScoket.Send(sSendText);
        }

        public bool CheckSession(string account, string sIPaddr, int sessionId)
        {
            var result = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.Account == account) && (globaSessionInfo.SessionID == sessionId))
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
            var result = -1;
            boFoundSession = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.Account == sAccount) && (globaSessionInfo.SessionID == nSessionId))
                    {
                        boFoundSession = true;
                        if (!globaSessionInfo.LoadRcd)
                        {
                            globaSessionInfo.LoadRcd = true;
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
            var result = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.Account == sAccount))
                    {
                        globaSessionInfo.LoadRcd = false;
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
                    if ((globaSessionInfo.SessionID == nSessionId))
                    {
                        globaSessionInfo.StartPlay = false;
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
                    if ((globaSessionInfo.SessionID == nSessionId))
                    {
                        globaSessionInfo.StartPlay = true;
                        break;
                    }
                }
            }
        }

        public bool GetGlobaSessionStatus(int nSessionId)
        {
            var result = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.SessionID == nSessionId))
                    {
                        result = globaSessionInfo.StartPlay;
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
                    if ((globaSessionInfo.SessionID == nSessionId))
                    {
                        if (globaSessionInfo.Account == sAccount)
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
            var sAccount = string.Empty;
            var s10 = string.Empty;
            var s14 = string.Empty;
            var s18 = string.Empty;
            var sIPaddr = string.Empty;
            sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref s10, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref s14, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref s18, HUtil32.Backslash);
            sData = HUtil32.GetValidStr3(sData, ref sIPaddr, HUtil32.Backslash);
            var globaSessionInfo = new GlobaSessionInfo();
            globaSessionInfo.Account = sAccount;
            globaSessionInfo.IPaddr = sIPaddr;
            globaSessionInfo.SessionID = HUtil32.StrToInt(s10, 0);
            //GlobaSessionInfo.n24 = HUtil32.StrToInt(s14, 0);
            globaSessionInfo.StartPlay = false;
            globaSessionInfo.LoadRcd = false;
            globaSessionInfo.AddTick = HUtil32.GetTickCount();
            globaSessionInfo.AddDate = DateTime.Now;
            _globaSessionList.Add(globaSessionInfo);
            //_logger.Debug($"同步账号服务[{sAccount}]同步会话消息...");
        }

        private void ProcessDelSession(string sData)
        {
            var sAccount = string.Empty;
            sData = HUtil32.GetValidStr3(sData, ref sAccount, HUtil32.Backslash);
            var nSessionId = HUtil32.StrToInt(sData, 0);
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.SessionID == nSessionId) && (globaSessionInfo.Account == sAccount))
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
            var result = false;
            for (var i = 0; i < _globaSessionList.Count; i++)
            {
                var globaSessionInfo = _globaSessionList[i];
                if (globaSessionInfo != null)
                {
                    if ((globaSessionInfo.Account == sAccount) && (globaSessionInfo.IPaddr == sIPaddr))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private static void ProcessGetOnlineCount(string sData)
        {

        }

        public void SendKeepAlivePacket(int userCount)
        {
            if (_clientScoket.Online)
            {
                _clientScoket.Send(HUtil32.GetBytes("(" + Messages.SS_SERVERINFO + "/" + _setting.ServerName + "/" + "99" + "/" + userCount + ")"));
            }
        }
    }
}