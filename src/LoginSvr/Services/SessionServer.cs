using LoginSvr.Conf;
using LoginSvr.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace LoginSvr.Services
{
    /// <summary>
    /// DBSVr、GameSvr会话服务
    /// </summary>
    public class SessionServer
    {
        private readonly MirLogger _logger;
        private readonly IList<ServerSessionInfo> _serverList = null;
        private readonly SocketServer _serverSocket;
        private readonly ConfigManager _configManager;
        private readonly AccountStorage _accountStorage;
        private readonly Config _config;
        private static readonly LimitServerUserInfo[] UserLimit = new LimitServerUserInfo[100];

        public SessionServer(MirLogger logger, ConfigManager configManager, AccountStorage accountStorage)
        {
            _logger = logger;
            _configManager = configManager;
            _accountStorage = accountStorage;
            _serverList = new List<ServerSessionInfo>();
            _serverSocket = new SocketServer(byte.MaxValue, 512);
            _serverSocket.OnClientConnect += SocketClientConnect;
            _serverSocket.OnClientDisconnect += SocketClientDisconnect;
            _serverSocket.OnClientError += SocketClientError;
            _serverSocket.OnClientRead += SocketClientRead;
            _config = _configManager.Config;
        }

        public IList<ServerSessionInfo> ServerList => _serverList;

        public void StartServer()
        {
            LoadServerAddr();
            LoadUserLimit();
            _serverSocket.Init();
            _serverSocket.Start(_config.sServerAddr, _config.nServerPort);
            _logger.LogInformation($"账号数据服务[{_config.sServerAddr}:{_config.nServerPort}]已启动.");
        }

        private void SocketClientConnect(object sender, AsyncUserToken e)
        {
            var sRemoteAddr = e.RemoteIPaddr;
            var boAllowed = false;
            for (var i = 0; i < LsShare.ServerAddr.Length; i++)
            {
                if (sRemoteAddr == LsShare.ServerAddr[i])
                {
                    boAllowed = true;
                    break;
                }
            }
            if (boAllowed)
            {
                var msgServer = new ServerSessionInfo();
                msgServer.ReceiveMsg = string.Empty;
                msgServer.Socket = e.Socket;
                msgServer.EndPoint = e.EndPoint;
                _serverList.Add(msgServer);
                _logger.DebugLog($"{e.EndPoint}链接成功.");
            }
            else
            {
                _logger.LogWarning("非法地址连接:" + sRemoteAddr);
                e.Socket.Close();
            }
        }

        private void SocketClientDisconnect(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < _serverList.Count; i++)
            {
                var MsgServer = _serverList[i];
                if (MsgServer.Socket == e.Socket)
                {
                    if (MsgServer.ServerIndex == 99)
                    {
                        _logger.LogWarning($"[{MsgServer.ServerName}]数据库服务器[{e.RemoteIPaddr}:{e.RemotePort}]断开链接.");
                    }
                    else
                    {
                        _logger.LogWarning($"[{MsgServer.ServerName}]游戏服务器[{e.RemoteIPaddr}:{e.RemotePort}]断开链接.");
                    }
                    MsgServer = null;
                    _serverList.RemoveAt(i);
                    break;
                }
            }
        }

        private void SocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void SocketClientRead(object sender, AsyncUserToken e)
        {
            var sReviceMsg = string.Empty;
            var sMsg = string.Empty;
            var sCode = string.Empty;
            var sAccount = string.Empty;
            var sServerName = string.Empty;
            var sIndex = string.Empty;
            var sOnlineCount = string.Empty;
            for (var i = 0; i < _serverList.Count; i++)
            {
                var msgServer = _serverList[i];
                if (msgServer.Socket == e.Socket)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    sReviceMsg = msgServer.ReceiveMsg + HUtil32.GetString(data, 0, data.Length);
                    while (sReviceMsg.IndexOf(")", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        sReviceMsg = HUtil32.ArrestStringEx(sReviceMsg, "(", ")", ref sMsg);
                        if (string.IsNullOrEmpty(sMsg))
                        {
                            break;
                        }
                        sMsg = HUtil32.GetValidStr3(sMsg, ref sCode, new[] { "/" });
                        var nCode = HUtil32.StrToInt(sCode, -1);
                        switch (nCode)
                        {
                            case Grobal2.SS_SOFTOUTSESSION:
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sAccount, new[] { "/" });
                                CloseUser(sAccount, HUtil32.StrToInt(sMsg, 0));
                                break;
                            case Grobal2.SS_SERVERINFO:
                                //todo 目前的收费/免费模式存在瑕疵，只能同一时间内免费或者收费，应该改成由GameSvr同步过来告诉账号中心计费模式
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sServerName, new[] { "/" });
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sIndex, new[] { "/" });
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sOnlineCount, new[] { "/" });
                                msgServer.ServerName = sServerName;
                                msgServer.ServerIndex = HUtil32.StrToInt(sIndex, 0);
                                msgServer.OnlineCount = HUtil32.StrToInt(sOnlineCount, 0);
                                msgServer.KeepAliveTick = HUtil32.GetTickCount();
                                SortServerList(i);
                                LsShare.OnlineCountMin = GetOnlineHumCount();
                                if (LsShare.OnlineCountMin > LsShare.OnlineCountMax)
                                {
                                    LsShare.OnlineCountMax = LsShare.OnlineCountMin;
                                }
                                SendServerMsgA(Grobal2.SS_KEEPALIVE, LsShare.OnlineCountMin.ToString());
                                RefServerLimit(sServerName);
                                break;
                            case Grobal2.ISM_GAMETIMEOFTIMECARDUSER:
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sServerName, new[] { "/" });
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sAccount, new[] { "/" });
                                ChanggePlayTimeUser(sServerName, sAccount, HUtil32.StrToInt(sMsg, 0));
                                break;
                            case Grobal2.ISM_QUERYACCOUNTEXPIRETIME:
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sAccount, new[] { "/" });
                                QueryPlayTime(sAccount);
                                break;
                            case Grobal2.ISM_CHECKTIMEACCOUNT:
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sAccount, new[] { "/" });
                                CheckTimeAccount(sAccount);
                                break;
                            case Grobal2.UNKNOWMSG:
                                SendServerMsgA(Grobal2.UNKNOWMSG, sMsg);
                                break;
                            default:
                                Console.WriteLine(nCode);
                                break;
                        }
                    }
                }
                msgServer.ReceiveMsg = sReviceMsg;
            }
        }

        /// <summary>
        /// 查询账号剩余游戏时间
        /// </summary>
        private void QueryPlayTime(string account)
        {
            if (string.IsNullOrEmpty(account))
            {
                return;
            }
            if (_config.PayMode == 0)
            {
                return;
            }
            var seconds = _accountStorage.GetAccountPlayTime(account);
            for (var i = 0; i < LsShare.CertList.Count; i++)
            {
                var certUser = LsShare.CertList[i];
                if (string.Compare(LsShare.CertList[i].LoginID, account, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (!LsShare.CertList[i].FreeMode && !LsShare.CertList[i].Closing)
                    {
                        if ((certUser.AvailableType == 2) || ((certUser.AvailableType >= 6) && (certUser.AvailableType <= 10)))
                        {
                            SendServerMsg(Grobal2.ISM_QUERYPLAYTIME, certUser.ServerName, certUser.LoginID + "/" + seconds);
                            _logger.DebugLog($"[GameServer/Send] ISM_QUERYPLAYTIME : {certUser.LoginID} PlayTime: ({seconds})");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 减少或更新账号游戏时间
        /// </summary>
        private void ChanggePlayTimeUser(string serverName, string account, int gameTime)
        {
            if (_config.PayMode == 0)
            {
                return;
            }
            var seconds = _accountStorage.GetAccountPlayTime(account);//获取历史时间
            if (seconds > 0)
            {
                seconds = seconds - 60;//减去一分钟游戏时间
                _accountStorage.UpdateAccountPlayTime(account, seconds);
                _logger.DebugLog($"账号:[{account}] 数据库时间:{seconds} 引擎时间:[{gameTime}]");
                if (seconds < gameTime)
                {
                    _logger.DebugLog($"账号[{account}]游戏时间异常.");
                }
                else
                {
                    seconds = 0;
                }
            }
            if (seconds == 0)
            {
                for (var i = 0; i < LsShare.CertList.Count; i++)
                {
                    var certUser = LsShare.CertList[i];
                    if (string.Compare(LsShare.CertList[i].LoginID, account, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (!LsShare.CertList[i].FreeMode && !LsShare.CertList[i].Closing)
                        {
                            if ((certUser.AvailableType == 2) || ((certUser.AvailableType >= 6) && (certUser.AvailableType <= 10)))
                            {
                                SendCancelAdmissionUser(serverName, certUser);
                            }
                        }
                    }
                }
            }
            else
            {
                SendServerMsg(Grobal2.ISM_QUERYPLAYTIME, serverName, account + "/" + seconds);
                _logger.DebugLog($"[GameServer/Send] ISM_QUERYPLAYTIME : {account} PlayTime: ({seconds})");
            }
        }

        private void SendCancelAdmissionUser(string serverName, CertUser certUser)
        {
            SendServerMsg(Grobal2.SS_CLOSESESSION, serverName, certUser.LoginID + "/" + certUser.Certification);
            _logger.DebugLog($"[GameServer/Send] ISM_CANCELADMISSION : {certUser.LoginID} TO ({certUser.Addr})");
        }

        /// <summary>
        /// 检查账号游戏时间
        /// </summary>
        /// <param name="account"></param>
        private void CheckTimeAccount(string account)
        {
            if (string.IsNullOrEmpty(account))
            {
                return;
            }
            if (_config.PayMode == 0)
            {
                return;
            }
            var seconds = _accountStorage.GetAccountPlayTime(account);
            if (seconds == 0)
            {
                for (var i = 0; i < LsShare.CertList.Count; i++)
                {
                    var certUser = LsShare.CertList[i];
                    if (string.Compare(LsShare.CertList[i].LoginID, account, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (!LsShare.CertList[i].FreeMode && !LsShare.CertList[i].Closing)
                        {
                            if ((certUser.AvailableType == 2) || ((certUser.AvailableType >= 6) && (certUser.AvailableType <= 10)))
                            {
                                SendAccountExpireUser(certUser);
                            }
                        }
                    }
                }
            }
        }

        private void SendAccountExpireUser(CertUser certUser)
        {
            SendServerMsg(Grobal2.ISM_ACCOUNTEXPIRED, certUser.ServerName, certUser.LoginID + "/" + certUser.Certification);
        }

        private void CloseUser(string account, int sessionId)
        {
            var config = _configManager.Config;
            for (var i = config.SessionList.Count - 1; i >= 0; i--)
            {
                var connInfo = config.SessionList[i];
                if ((connInfo.Account == account) || (connInfo.SessionID == sessionId))
                {
                    SendServerMsg(Grobal2.SS_CLOSESESSION, connInfo.ServerName, connInfo.Account + "/" + connInfo.SessionID);
                    connInfo = null;
                    config.SessionList.RemoveAt(i);
                }
            }
        }

        private void RefServerLimit(string serverName)
        {
            var nCount = 0;
            for (var i = 0; i < _serverList.Count; i++)
            {
                var msgServer = _serverList[i];
                if ((msgServer.ServerIndex != 99) && (msgServer.ServerName == serverName))
                {
                    nCount += msgServer.OnlineCount;
                }
            }
            for (var i = 0; i < UserLimit.Length; i++)
            {
                if (UserLimit[i].ServerName == serverName)
                {
                    UserLimit[i].LimitCountMin = nCount;
                    break;
                }
            }
        }

        public bool IsNotUserFull(string serverName)
        {
            var result = true;
            for (var i = 0; i < UserLimit.Length; i++)
            {
                if (UserLimit[i].ServerName == serverName)
                {
                    if (UserLimit[i].LimitCountMin > UserLimit[i].LimitCountMax)
                    {
                        result = false;
                    }
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 清理没有付费的账号
        /// </summary>
        public void SessionClearNoPayMent()
        {
            //todo 判断服务器是否开启收费模式
            for (var i = _config.SessionList.Count - 1; i >= 0; i--)
            {
                var connInfo = _config.SessionList[i];
                if (!connInfo.boKicked && !_config.TestServer && !connInfo.bo11)
                {
                    if (HUtil32.GetTickCount() - connInfo.dwStartTick > 60 * 60 * 1000)
                    {
                        connInfo.dwStartTick = HUtil32.GetTickCount();
                        // if (!IsPayMent(config, connInfo.IPaddr, connInfo.Account))
                        // {
                        //     SendServerMsg(Grobal2.SS_KICKUSER, connInfo.ServerName, connInfo.Account + "/" + connInfo.SessionID);
                        //     _config.SessionList[i] = null;
                        //     _config.SessionList.RemoveAt(i);
                        // }
                    }
                }
            }
        }

        /// <summary>
        /// 服务器排序
        /// </summary>
        /// <param name="nIndex"></param>
        private void SortServerList(int nIndex)
        {
            try
            {
                if (_serverList.Count <= nIndex)
                {
                    return;
                }
                var msgServerSort = _serverList[nIndex];
                _serverList.RemoveAt(nIndex);
                for (var nC = 0; nC < _serverList.Count; nC++)
                {
                    var msgServer = _serverList[nC];
                    if (string.Compare(msgServer.ServerName, msgServerSort.ServerName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (msgServer.ServerIndex < msgServerSort.ServerIndex)
                        {
                            _serverList.Insert(nC, msgServerSort);
                            return;
                        }
                        var nNewIndex = nC + 1;
                        if (nNewIndex < _serverList.Count)
                        {
                            for (var n10 = nNewIndex; n10 < _serverList.Count; n10++)
                            {
                                msgServer = _serverList[n10];
                                if (string.Compare(msgServer.ServerName, msgServerSort.ServerName, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    if (msgServer.ServerIndex < msgServerSort.ServerIndex)
                                    {
                                        _serverList.Insert(n10, msgServerSort);
                                        for (var n14 = n10 + 1; n14 < _serverList.Count; n14++)
                                        {
                                            msgServer = _serverList[n14];
                                            if ((string.Compare(msgServer.ServerName, msgServerSort.ServerName, StringComparison.OrdinalIgnoreCase) == 0) && (msgServer.ServerIndex == msgServerSort.ServerIndex))
                                            {
                                                _serverList.RemoveAt(n14);
                                                return;
                                            }
                                        }
                                        return;
                                    }
                                    nNewIndex = n10 + 1;
                                }
                            }
                            _serverList.Insert(nNewIndex, msgServerSort);
                            return;
                        }
                    }
                }
                _serverList.Add(msgServerSort);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
        }

        public void SendServerMsg(short wIdent, string sServerName, string sMsg)
        {
            const string sFormatMsg = "({0}/{1})";
            try
            {
                var tempName = GetLimitName(sServerName);
                var sSendMsg = string.Format(sFormatMsg, wIdent, sMsg);
                for (var i = 0; i < _serverList.Count; i++)
                {
                    var msgServer = _serverList[i];
                    if (msgServer.Socket.Connected)
                    {
                        if ((string.IsNullOrEmpty(tempName)) || (string.IsNullOrEmpty(msgServer.ServerName)) || (string.Compare(msgServer.ServerName, tempName, StringComparison.OrdinalIgnoreCase) == 0)
                            || (msgServer.ServerIndex == 99))
                        {
                            msgServer.Socket.SendText(sSendMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
        }

        private void LoadServerAddr()
        {
            var nServerIdx = 0;
            var sFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerAddr.txt");
            if (File.Exists(sFileName))
            {
                var LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    var sLineText = LoadList[i].Trim();
                    if ((!string.IsNullOrEmpty(sLineText)) && (sLineText[i] != ';'))
                    {
                        if (HUtil32.TagCount(sLineText, '.') == 3)
                        {
                            LsShare.ServerAddr[nServerIdx] = sLineText;
                            nServerIdx++;
                            if (nServerIdx >= 100)
                            {
                                break;
                            }
                        }
                    }
                }
                LoadList = null;
            }
        }

        private int GetOnlineHumCount()
        {
            var result = 0;
            var nCount = 0;
            try
            {
                for (var i = 0; i < _serverList.Count; i++)
                {
                    var msgServer = _serverList[i];
                    if (msgServer.ServerIndex != 99)
                    {
                        nCount += msgServer.OnlineCount;
                    }
                }
                result = nCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
            return result;
        }

        private void SendServerMsgA(short wIdent, string sMsg)
        {
            const string sFormatMsg = "({0}/{1})";
            try
            {
                var sSendMsg = string.Format(sFormatMsg, wIdent, sMsg);
                for (var i = 0; i < _serverList.Count; i++)
                {
                    if (_serverList[i].Socket.Connected)
                    {
                        _serverList[i].Socket.SendText(sSendMsg);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.StackTrace);
            }
        }

        private string GetLimitName(string sServerName)
        {
            var result = string.Empty;
            for (var i = 0; i < UserLimit.Length; i++)
            {
                if (string.Compare(UserLimit[i].ServerName, sServerName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = UserLimit[i].Name;
                    break;
                }
            }
            return result;
        }

        private void LoadUserLimit()
        {
            var nC = 0;
            var sServerName = string.Empty;
            var s10 = string.Empty;
            var s14 = string.Empty;
            var sFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserLimit.txt");
            if (File.Exists(sFileName))
            {
                var LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    var lineText = LoadList[i];
                    lineText = HUtil32.GetValidStr3(lineText, ref sServerName, new[] { " ", "\09" });
                    lineText = HUtil32.GetValidStr3(lineText, ref s10, new[] { " ", "\09" });
                    lineText = HUtil32.GetValidStr3(lineText, ref s14, new[] { " ", "\09" });
                    if (!string.IsNullOrEmpty(sServerName))
                    {
                        UserLimit[nC] = new LimitServerUserInfo();
                        UserLimit[nC].ServerName = sServerName;
                        UserLimit[nC].Name = s10;
                        UserLimit[nC].LimitCountMax = HUtil32.StrToInt(s14, 3000);
                        UserLimit[nC].LimitCountMin = 0;
                        nC++;
                    }
                }
                LoadList = null;
            }
            else
            {
                _logger.LogError("[Critical Failure] file not found. UserLimit.txt");
            }
        }

        /// <summary>
        /// 获取服务器状态
        /// </summary>
        /// <param name="sServerName"></param>
        /// <returns></returns>
        public ServerStatus GetServerStatus(string sServerName)
        {
            ServerStatus status = 0;
            var boServerOnLine = false;
            try
            {
                for (var i = 0; i < _serverList.Count; i++)
                {
                    var msgServer = _serverList[i];
                    if ((msgServer.ServerIndex != 99) && (msgServer.ServerName == sServerName))
                    {
                        boServerOnLine = true;
                    }
                }
                if (!boServerOnLine)
                {
                    return status;
                }
                for (var i = 0; i < UserLimit.Length; i++)
                {
                    if (UserLimit[i].ServerName == sServerName)
                    {
                        if (UserLimit[i].LimitCountMin <= UserLimit[i].LimitCountMax / 2)
                        {
                            status = ServerStatus.Idle;
                            break;
                        }
                        if (UserLimit[i].LimitCountMin <= UserLimit[i].LimitCountMax - (UserLimit[i].LimitCountMax / 5))
                        {
                            status = ServerStatus.General;
                            break;
                        }
                        if (UserLimit[i].LimitCountMin < UserLimit[i].LimitCountMax)
                        {
                            status = ServerStatus.Busy;
                            break;
                        }
                        if (UserLimit[i].LimitCountMin >= UserLimit[i].LimitCountMax)
                        {
                            status = ServerStatus.Full;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
            }
            return status;
        }
    }

    public class ServerSessionInfo
    {
        public string ReceiveMsg;
        public Socket Socket;
        public IPEndPoint EndPoint;
        public string ServerName;
        public int ServerIndex;
        public int OnlineCount;
        public int SelectID;
        public int KeepAliveTick;
        public string IPaddr;
    }

    public struct LimitServerUserInfo
    {
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string ServerName;
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 最小在线人数
        /// </summary>
        public int LimitCountMin;
        /// <summary>
        /// 最高在线人数
        /// </summary>
        public int LimitCountMax;
    }

    public enum ServerStatus : byte
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle = 1,
        /// <summary>
        /// 良好
        /// </summary>
        General = 2,
        /// <summary>
        /// 繁忙
        /// </summary>
        Busy = 3,
        /// <summary>
        /// 满员
        /// </summary>
        Full = 4
    }
}