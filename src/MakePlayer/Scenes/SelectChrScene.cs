using NLog;
using SystemModule;
using SystemModule.Packets.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace MakePlayer.Scenes
{
    public class SelectChrScene : SceneBase
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly ScoketClient ClientSocket;
        private int NewIndex = 0;
        private readonly SelChar[] ChrArr;
        private string SelChrAddr;
        private int SelChrPort;
        private int Certification;
        private string ChrName;

        public SelectChrScene() 
        {
            ChrArr = new SelChar[2];
            ChrArr[0].UserChr = new UserCharacterInfo();
            ChrArr[1].UserChr = new UserCharacterInfo();
            NewIndex = 0;
            ClientSocket = new ScoketClient();
            ClientSocket.OnConnected += CSocketConnect;
            ClientSocket.OnDisconnected += CSocketDisconnect;
            ClientSocket.OnReceivedData += CSocketRead;
            ClientSocket.OnError += CSocketError;
        }
        
        public override void OpenScene()
        {
            ConnectionStep = ConnectionStep.QueryChr;
            ClientSocket.Connect(SelChrAddr, SelChrPort);
        }

        protected override void ProcessPacket(CommandMessage command, string sBody)
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

        public string ClientGetReceiveChrs_GetJobName(int nJob)
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

        public string ClientGetReceiveChrs_GetSexName(int nSex)
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
        
        private void ClientGetReceiveChrs_AddChr(string sName, byte nJob, byte nHair, int nLevel, byte nSex)
        {
            int I;
            if (!ChrArr[0].boValid)
            {
                I = 0;
            }
            else if (!ChrArr[1].boValid)
            {
                I = 1;
            }
            else
            {
                return;
            }
            ChrArr[I].UserChr.sName = sName;
            ChrArr[I].UserChr.btJob = nJob;
            ChrArr[I].UserChr.btHair = nHair;
            ChrArr[I].UserChr.wLevel = (ushort)nLevel;
            ChrArr[I].UserChr.btSex = nSex;
            ChrArr[I].boValid = true;
        }

        private void ClientGetReceiveChrs(string sData)
        {
            if (string.IsNullOrEmpty(sData))
            {
                SetNotifyEvent(NewChr, 3000);
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
            for (var i = 0; i < ChrArr.Length; i++)
            {
                sText = HUtil32.GetValidStr3(sText, ref sName, '/');
                sText = HUtil32.GetValidStr3(sText, ref sJob, '/');
                sText = HUtil32.GetValidStr3(sText, ref sHair, '/');
                sText = HUtil32.GetValidStr3(sText, ref sLevel, '/');
                sText = HUtil32.GetValidStr3(sText, ref sSex, '/');
                nSelect = 0;
                if ((!string.IsNullOrEmpty(sName)) && (!string.IsNullOrEmpty(sLevel)) && (!string.IsNullOrEmpty(sSex)))
                {
                    if (sName[0] == '*')
                    {
                        nSelect = i;
                        sName = sName.Substring(1, sName.Length - 1);
                    }
                    ClientGetReceiveChrs_AddChr(sName, Convert.ToByte(sJob), Convert.ToByte(sHair), Convert.ToInt32(sLevel), Convert.ToByte(sSex));
                    nChrCount++;
                }
                if (nSelect == 0)
                {
                    ChrArr[0].boFreezeState = false;
                    ChrArr[0].boSelected = true;
                    ChrArr[1].boFreezeState = true;
                    ChrArr[1].boSelected = false;
                }
                else
                {
                    ChrArr[0].boFreezeState = true;
                    ChrArr[0].boSelected = false;
                    ChrArr[1].boFreezeState = false;
                    ChrArr[1].boSelected = true;
                }
            }
            if (nChrCount > 0)
            {
                SendSelChr(ChrArr[nSelect].UserChr.sName);
            }
            else
            {
                SetNotifyEvent(NewChr, 3000);
            }
        }
        
        private void NewChr()
        {
            ConnectionStep = ConnectionStep.NewChr;
            SelectChrCreateNewChr(ChrName);
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
            SendNewChr(LoginId, sChrName, sHair, sJob, sSex);
        }
        
        private void SendSelChr(string sChrName)
        {
            MainOutMessage($"选择人物：{sChrName}");
            ConnectionStep = ConnectionStep.SelChr;
            ChrName = sChrName;
            var defMsg = Messages.MakeMessage(Messages.CM_SELCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(LoginId + "/" + sChrName));
        }

        public void SendQueryChr()
        {
            ConnectionStep = ConnectionStep.QueryChr;
            var DefMsg = Messages.MakeMessage(Messages.CM_QUERYCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(DefMsg) + EDCode.EncodeString(LoginId + "/" + Certification));
            logger.Info("查询角色.");
        }
        
        private void SendNewChr(string uid, string uname, byte shair, byte sjob, byte ssex)
        {
            var msg = Messages.MakeMessage(Messages.CM_NEWCHR, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(uid + "/" + uname + "/" + shair + "/" + sjob + "/" + ssex));
            logger.Info("创建角色.");
        }
        
        public void ClientGetStartPlay(string body)
        {
            logger.Info("准备进入游戏");
            string addr = string.Empty;
            string Str = EDCode.DeCodeString(body);
            string sport = HUtil32.GetValidStr3(Str, ref addr, HUtil32.Backslash);
            RunServerPort = HUtil32.StrToInt(sport, 0);
            RunServerAddr = addr;
            ConnectionStep = ConnectionStep.Play;
        }
        
        private void ClientNewChrFail(int nFailCode)
        {
            //ConnectionStatus = ConnectionStatus.Failure;
            //Close();
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
            //ConnectionStatus = ConnectionStatus.Failure;
        }

        private void SendSocket(string sendstr)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.SendText($"#1{sendstr}!");
            }
            else
            {
                logger.Warn($"Socket Close {ClientSocket.RemoteEndPoint}");
            }
        }
        
        #region Socket Events

        private void CSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            if (ConnectionStep == ConnectionStep.QueryChr)
            {
                SetNotifyEvent(SendQueryChr, 1000);
                ConnectionStep = ConnectionStep.SelChr;
            }
            logger.Info($"连接角色网关:[{SelChrAddr}:{SelChrPort}]");
        }

        private void CSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            logger.Warn($"角色服务器[{ClientSocket.RemoteEndPoint}断开连接...");
        }

        private void CSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    logger.Warn($"角色服务器[{ClientSocket.RemoteEndPoint}拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    logger.Warn($"角色服务器[{ClientSocket.RemoteEndPoint}关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    logger.Warn($"角色服务器[{ClientSocket.RemoteEndPoint}链接超时...");
                    break;
            }
        }

        private void CSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            var sData = HUtil32.GetString(e.Buff, 0, e.BuffLen);
            if (!string.IsNullOrEmpty(sData))
            {
                ClientManager.AddPacket(SessionId, e.Buff);
            }
        }

        #endregion
    }
}