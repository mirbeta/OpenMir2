using MakePlayer.Cliens;
using SystemModule;
using SystemModule.Packets.ClientPackets;
using SystemModule.SocketComponents.AsyncSocketClient;
using SystemModule.SocketComponents.Event;

namespace MakePlayer.Scenes.Scene
{
    public class SelectChrScene : SceneBase
    {
        private readonly ScoketClient _clientSocket;
        private readonly PlayClient _play;
        private readonly SelChar[] _chrArr;

        public SelectChrScene(PlayClient playClient)
        {
            _play = playClient;
            _chrArr = new SelChar[2];
            _chrArr[0].UserChr = new UserCharacterInfo();
            _chrArr[1].UserChr = new UserCharacterInfo();
            _clientSocket = new ScoketClient();
            _clientSocket.OnConnected += CSocketConnect;
            _clientSocket.OnDisconnected += CSocketDisconnect;
            _clientSocket.OnReceivedData += CSocketRead;
            _clientSocket.OnError += CSocketError;
        }

        public override void OpenScene()
        {
            _clientSocket.Connect(_play.SelChrAddr, _play.SelChrPort);
        }

        public override void CloseScene()
        {
            SetNotifyEvent(CloseSocket, RandomNumber.GetInstance().Random(1000, 3000));
        }

        internal override void ProcessPacket(CommandMessage command, string sBody)
        {
            switch (command.Ident)
            {
                case Messages.SM_QUERYCHR:
                    ClientGetReceiveChrs(sBody);
                    break;
                case Messages.SM_QUERYCHR_FAIL:
                    ClientQueryChrFail(command.Recog);
                    break;
                case Messages.SM_NEWCHR_SUCCESS:
                    SendQueryChr();
                    break;
                case Messages.SM_NEWCHR_FAIL:
                    ClientNewChrFail(command.Recog);
                    break;
                case Messages.SM_DELCHR_SUCCESS:
                    SendQueryChr();
                    break;
                case Messages.SM_STARTPLAY:
                    ClientGetStartPlay(sBody);
                    break;
                case Messages.SM_STARTFAIL:
                    ClientStartPlayFail();
                    break;
                case Messages.SM_VERSION_FAIL:
                    ClientVersionFail();
                    break;
            }
            base.ProcessPacket(command, sBody);
        }

        private void ClientStartPlayFail()
        {
            MainOutMessage("此服务器满员！");
        }

        private void ClientVersionFail()
        {
            MainOutMessage("游戏程序版本不正确，请下载最新版本游戏程序！");
        }

        public string ClientGetReceiveChrsGetJobName(int nJob)
        {
            string result;
            switch (nJob)
            {
                case 0:
                    result = "武士";
                    break;
                case 1:
                    result = "魔法师";
                    break;
                case 2:
                    result = "道士";
                    break;
                default:
                    result = "未知";
                    break;
            }
            return result;
        }

        public string ClientGetReceiveChrsGetSexName(int nSex)
        {
            string result;
            switch (nSex)
            {
                case 0:
                    result = "男";
                    break;
                case 1:
                    result = "女";
                    break;
                default:
                    result = "未知";
                    break;
            }
            return result;
        }

        private void ClientGetReceiveChrsAddChr(string sName, byte nJob, byte nHair, int nLevel, byte nSex)
        {
            int I;
            if (!_chrArr[0].boValid)
            {
                I = 0;
            }
            else if (!_chrArr[1].boValid)
            {
                I = 1;
            }
            else
            {
                return;
            }
            _chrArr[I].UserChr.sName = sName;
            _chrArr[I].UserChr.btJob = nJob;
            _chrArr[I].UserChr.btHair = nHair;
            _chrArr[I].UserChr.wLevel = (ushort)nLevel;
            _chrArr[I].UserChr.btSex = nSex;
            _chrArr[I].boValid = true;
        }

        private void ClientGetReceiveChrs(string sData)
        {
            if (string.IsNullOrEmpty(sData))
            {
                SetNotifyEvent(NewChr, RandomNumber.GetInstance().Random(1000, 3000));
                return;
            }
            var sName = string.Empty;
            var sJob = string.Empty;
            var sHair = string.Empty;
            var sLevel = string.Empty;
            var sSex = string.Empty;
            var nChrCount = 0;
            var nSelect = 0;
            var sText = EDCode.DeCodeString(sData);
            for (var i = 0; i < _chrArr.Length; i++)
            {
                sText = HUtil32.GetValidStr3(sText, ref sName, '/');
                sText = HUtil32.GetValidStr3(sText, ref sJob, '/');
                sText = HUtil32.GetValidStr3(sText, ref sHair, '/');
                sText = HUtil32.GetValidStr3(sText, ref sLevel, '/');
                sText = HUtil32.GetValidStr3(sText, ref sSex, '/');
                nSelect = 0;
                if (!string.IsNullOrEmpty(sName) && !string.IsNullOrEmpty(sLevel) && !string.IsNullOrEmpty(sSex))
                {
                    if (sName[0] == '*')
                    {
                        nSelect = i;
                        sName = sName.Substring(1, sName.Length - 1);
                    }
                    ClientGetReceiveChrsAddChr(sName, Convert.ToByte(sJob), Convert.ToByte(sHair), Convert.ToInt32(sLevel), Convert.ToByte(sSex));
                    nChrCount++;
                }
                if (nSelect == 0)
                {
                    _chrArr[0].boFreezeState = false;
                    _chrArr[0].boSelected = true;
                    _chrArr[1].boFreezeState = true;
                    _chrArr[1].boSelected = false;
                }
                else
                {
                    _chrArr[0].boFreezeState = true;
                    _chrArr[0].boSelected = false;
                    _chrArr[1].boFreezeState = false;
                    _chrArr[1].boSelected = true;
                }
            }
            if (nChrCount > 0)
            {
                SendSelChr(_chrArr[nSelect].UserChr.sName);
            }
            else
            {
                SetNotifyEvent(NewChr, RandomNumber.GetInstance().Random(1000, 3000));
            }
        }

        private void NewChr()
        {
            _play.ConnectionStep = ConnectionStep.NewChr;
            SelectChrCreateNewChr(_play.ChrName);
        }

        private void SelectChrCreateNewChr(string sChrName)
        {
            byte sHair = 0;
            switch (RandomNumber.GetInstance().Random(1))
            {
                case 0:
                    sHair = 2;
                    break;
                case 1:
                    switch (new Random(1).Next())
                    {
                        case 0:
                            sHair = 1;
                            break;
                        case 1:
                            sHair = 3;
                            break;
                    }
                    break;
            }
            var sJob = (byte)RandomNumber.GetInstance().Random(2);
            var sSex = (byte)RandomNumber.GetInstance().Random(1);
            SendNewChr(_play.LoginId, sChrName, sHair, sJob, sSex);
        }

        private void SendSelChr(string sChrName)
        {
            MainOutMessage($"选择人物：{sChrName}");
            _play.ConnectionStep = ConnectionStep.SelChr;
            _play.ChrName = sChrName;
            var defMsg = Messages.MakeMessage(Messages.CM_SELCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(_play.LoginId + "/" + sChrName));
        }

        private void SendQueryChr()
        {
            _play.ConnectionStep = ConnectionStep.QueryChr;
            var defMsg = Messages.MakeMessage(Messages.CM_QUERYCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(_play.LoginId + "/" + _play.Certification));
            MainOutMessage("查询角色.");
        }

        private void SendNewChr(string uid, string uname, byte shair, byte sjob, byte ssex)
        {
            var msg = Messages.MakeMessage(Messages.CM_NEWCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(uid + "/" + uname + "/" + shair + "/" + sjob + "/" + ssex));
            MainOutMessage("创建角色.");
        }

        private void ClientGetStartPlay(string body)
        {
            var addr = string.Empty;
            var str = EDCode.DeCodeString(body);
            var sport = HUtil32.GetValidStr3(str, ref addr, HUtil32.Backslash);
            _play.RunServerPort = HUtil32.StrToInt(sport, 0);
            _play.RunServerAddr = addr;
            _play.ConnectionStep = ConnectionStep.Play;
            MainOutMessage("准备进入游戏");
            _play.DScreen.ChangeScene(SceneType.PlayGame);
        }

        private void ClientNewChrFail(int nFailCode)
        {
            switch (nFailCode)
            {
                case 0:
                    MainOutMessage("[错误信息] 输入的角色名称包含非法字符！ 错误代码 = 0");
                    break;
                case 2:
                    MainOutMessage("[错误信息] 创建角色名称已被其他人使用！ 错误代码 = 2");
                    break;
                case 3:
                    MainOutMessage("[错误信息] 您只能创建二个游戏角色！ 错误代码 = 3");
                    break;
                case 4:
                    MainOutMessage("[错误信息] 创建角色时出现错误！ 错误代码 = 4");
                    break;
                default:
                    MainOutMessage("[错误信息] 创建角色时出现未知错误！");
                    break;
            }
        }

        private void ClientQueryChrFail(int nFailCode)
        {
            MainOutMessage("查询角色失败.");
        }

        private void SendSocket(string sendstr)
        {
            if (_clientSocket.IsConnected)
            {
                _clientSocket.SendText($"#1{sendstr}!");
            }
            else
            {
                MainOutMessage($"Socket Close {_clientSocket.RemoteEndPoint}");
            }
        }

        private void CloseSocket()
        {
            _clientSocket.Disconnect(false);//断开登录网关链接
            MainOutMessage("主动断开");
        }

        #region Socket Events

        private void CSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            if (_play.ConnectionStep == ConnectionStep.SelServer)
            {
                SetNotifyEvent(SendQueryChr, RandomNumber.GetInstance().Random(1000, 3000));
                _play.ConnectionStep = ConnectionStep.SelChr;
            }
            MainOutMessage($"连接角色服务:[{e.RemoteEndPoint}]成功...");
        }

        private void CSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            MainOutMessage($"角色服务[{e.RemoteEndPoint}断开连接...");
        }

        private void CSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            if (e.BuffLen > 0)
            {
                ClientManager.AddPacket(_play.SessionId, e.Buff);
            }
        }

        private void CSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    MainOutMessage($"角色服务[{_clientSocket.RemoteEndPoint}拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    MainOutMessage($"角色服务[{_clientSocket.RemoteEndPoint}关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    MainOutMessage($"角色服务[{_clientSocket.RemoteEndPoint}链接超时...");
                    break;
            }
        }

        #endregion
    }
}