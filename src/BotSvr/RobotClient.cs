using BotSvr.Maps;
using BotSvr.Objects;
using BotSvr.Scenes;
using BotSvr.Scenes.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using SystemModule;
using SystemModule.Packet.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace BotSvr
{
    public class RobotClient
    {
        public readonly string SessionId;
        public TimerAutoPlay TimerAutoPlay;
        public TimerAutoPlay TimerAutoMove;
        public static int LastestClickTime = 0;
        public static bool g_MoveBusy = false;
        public bool g_PathBusy = false;
        public static int g_MoveStep = 0;
        public static int g_MoveErr = 0;
        public static int g_MoveErrTick = 0;
        public ScreenManager DScreen = null;
        public IntroScene IntroScene = null;
        public LoginScene LoginScene = null;
        public SelectChrScene SelectChrScene = null;
        public PlayScene g_PlayScene = null;
        public TMap Map = null;
        public static TActor ShowMsgActor = null;
        public static long g_dwOverSpaceWarningTick = 0;
        public static char activebuf = '*';
        private readonly TTimerCommand TimerCmd;
        private int ActionLockTime = 0;
        private readonly short ActionKey = 0;
        private ClientMesaagePacket WaitingMsg = null;
        private string WaitingStr = string.Empty;
        private string WhisperName = string.Empty;
        private int m_dwProcUseMagicTick = 0;
        public bool ActionFailLock = false;
        public int ActionFailLockTime = 0;
        public int LastHitTick = 0;
        public string ServerName = string.Empty;
        public bool NewAccount = false;
        public string LoginID = string.Empty;
        public string LoginPasswd = string.Empty;
        public string ChrName = string.Empty;
        public int Certification = 0;
        public int m_nEatRetIdx = 0;
        public bool ActionLock = false;
        public bool m_boSupplyItem = false;
        public int m_dwDuraWarningTick = 0;
        public int dwIPTick = 0;
        public int dwhIPTick = 0;
        private readonly HeroActor heroActor;
        public ClientScoket ClientSocket;
        public int m_dwConnectTick = 0;
        public TConnectionStatus m_ConnectionStatus;
        private readonly ClientManager _clientManager;

        public RobotClient(ClientManager clientManager)
        {
            SessionId = Guid.NewGuid().ToString("N");
            heroActor = new HeroActor(this);
            MShare.InitScreenConfig();
            MShare.g_APPathList = new List<FindMapNode>();
            MShare.g_ShowItemList = new Dictionary<string, string>();
            DScreen = new ScreenManager(this);
            IntroScene = new IntroScene(this);
            LoginScene = new LoginScene(this, clientManager);
            SelectChrScene = new SelectChrScene(this, clientManager);
            g_PlayScene = new PlayScene(this);
            Map = new TMap(this);
            MShare.g_DropedItemList = new List<TDropItem>();
            MShare.g_MagicList = new List<ClientMagic>();
            MShare.g_FreeActorList = new List<TActor>();
            //EventMan = new TClEventManager();
            MShare.g_ChangeFaceReadyList = new ArrayList();
            MShare.g_SendSayList = new ArrayList();
            if (MShare.g_MySelf != null)
            {
                MShare.g_MySelf.m_SlaveObject.Clear();
                MShare.g_MySelf = null;
            }
            MShare.InitClientItems();
            MShare.g_DetectItemMineID = 0;
            MShare.g_nLastMapMusic = -1;
            MShare.g_nTargetX = -1;
            MShare.g_nTargetY = -1;
            MShare.g_TargetCret = null;
            MShare.g_FocusCret = null;
            MShare.g_FocusItem = null;
            MShare.g_MagicTarget = null;
            MShare.g_nTestReceiveCount = 0;
            MShare.g_boServerChanging = false;
            MShare.g_boBagLoaded = false;
            MShare.g_boAutoDig = false;
            MShare.g_boAutoSit = false;
            MShare.g_dwLatestClientTime2 = 0;
            MShare.g_dwFirstClientTime = 0;
            MShare.g_dwFirstServerTime = 0;
            MShare.g_dwFirstClientTimerTime = 0;
            MShare.g_dwLatestClientTimerTime = 0;
            MShare.g_dwFirstClientGetTime = 0;
            MShare.g_dwLatestClientGetTime = 0;
            MShare.g_nTimeFakeDetectCount = 0;
            MShare.g_nTimeFakeDetectTimer = 0;
            MShare.g_nTimeFakeDetectSum = 0;
            MShare.g_nDayBright = 3;
            MShare.g_nAreaStateValue = 0;
            MShare.g_ConnectionStep = TConnectionStep.cnsIntro;
            MShare.g_boSendLogin = false;
            MShare.g_boServerConnected = false;
            MShare.g_SoftClosed = false;
            ActionFailLock = false;
            MShare.g_boMapMoving = false;
            MShare.g_boMapMovingWait = false;
            MShare.g_boCheckBadMapMode = false;
            MShare.g_boCheckSpeedHackDisplay = false;
            MShare.g_nDupSelection = 0;
            MShare.g_dwLastAttackTick = MShare.GetTickCount();
            MShare.g_dwLastMoveTick = MShare.GetTickCount();
            MShare.g_dwLatestSpellTick = MShare.GetTickCount();
            MShare.g_dwAutoPickupTick = MShare.GetTickCount();
            MShare.g_boItemMoving = false;
            MShare.g_boNextTimePowerHit = false;
            MShare.g_boCanLongHit = false;
            MShare.g_boCanWideHit = false;
            MShare.g_boCanCrsHit = false;
            MShare.g_boNextTimeFireHit = false;
            MShare.g_boCanSLonHit = false;
            MShare.g_boNextTimeTwinHit = false;
            MShare.g_boNextTimePursueHit = false;
            MShare.g_boNextTimeRushHit = false;
            MShare.g_boNextTimeSmiteHit = false;
            MShare.g_boNextTimeSmiteLongHit = false;
            MShare.g_boNextTimeSmiteLongHit3 = false;
            MShare.g_boNextTimeSmiteLongHit2 = false;
            MShare.g_boNextTimeSmiteWideHit = false;
            MShare.g_boNextTimeSmiteWideHit2 = false;
            MShare.g_boQueryPrice = false;
            MShare.g_sSellPriceStr = "";
            MShare.g_boAllowGroup = false;
            MShare.g_GroupMembers = new List<string>();
            MShare.LoadItemDesc();
            MShare.LoadItemFilter();
            m_ConnectionStatus = TConnectionStatus.cns_Failure;
            //MaketSystem.Units.MaketSystem.g_Market = new TMarketItemManager();
            for (var i = 0; i < MShare.MAXX * 3; i++)
            {
                for (var j = 0; j < MShare.MAXY * 3; j++)
                {
                    MShare.g_APPassEmpty[i, j] = 0xFF;
                }
            }
            _clientManager = clientManager;
        }

        public void SocketEvents()
        {
            if (ClientSocket == null)
            {
                ClientSocket = new ClientScoket();
            }
            ClientSocket.OnConnected -= CSocketConnect;
            ClientSocket.OnDisconnected -= CSocketDisconnect;
            ClientSocket.ReceivedDatagram -= CSocketRead;
            ClientSocket.OnError -= CSocketError;
            ClientSocket.OnConnected += CSocketConnect;
            ClientSocket.OnDisconnected += CSocketDisconnect;
            ClientSocket.ReceivedDatagram += CSocketRead;
            ClientSocket.OnError += CSocketError;
            ClientSocket.Connect(MShare.g_sRunServerAddr, MShare.g_nRunServerPort);
        }

        #region Socket Events

        private void CSocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.g_boServerConnected = true;
            if (MShare.g_ConnectionStep == TConnectionStep.cnsLogin)
            {
                DScreen.ChangeScene(SceneType.stLogin);
            }
            if (MShare.g_ConnectionStep == TConnectionStep.cnsPlay)
            {
                ClientSocket.IsConnected = true;
                SendRunLogin();
                if (!MShare.g_boServerChanging)
                {
                    ClFunc.ClearBag();
                }
                else
                {
                    ChangeServerClearGameVariables();
                }
            }
        }

        private void CSocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.g_boServerConnected = false;
            if (MShare.g_SoftClosed)
            {
                MShare.g_SoftClosed = false;
                //ActiveCmdTimer(MShare.TTimerCommand.tcReSelConnect);
            }
            else if ((DScreen.CurrentScene == LoginScene) && !MShare.g_boSendLogin)
            {
                MainOutMessage("游戏连接已关闭...");
            }
            if (DScreen.CurrentScene == g_PlayScene)
            {
                LoginOut();
                _clientManager.DelClient(SessionId);
            }
        }

        private void CSocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    Console.WriteLine($"游戏服务器[{ClientSocket.EndPoint}]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    Console.WriteLine($"游戏服务器[{ClientSocket.EndPoint}]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    Console.WriteLine($"游戏服务器[{ClientSocket.EndPoint}]链接超时...");
                    break;
            }
            if (DScreen.CurrentScene == g_PlayScene)
            {
                LoginOut();
                _clientManager.DelClient(SessionId);
            }
        }

        private void CSocketRead(object sender, DSCClientDataInEventArgs e)
        {
            var sData = HUtil32.GetString(e.Buff, 0, e.BuffLen);
            if (!string.IsNullOrEmpty(sData))
            {
                var n = sData.IndexOf("*", StringComparison.Ordinal);
                if (n > 0)
                {
                    var data2 = sData.Substring(0, n - 1);
                    sData = data2 + sData.Substring(n, sData.Length);
                    ClientSocket.SendBuffer(HUtil32.GetBytes(activebuf));
                }
                _clientManager.AddPacket(SessionId, sData);
            }
            Console.WriteLine(sData);
        }

        #endregion

        public void Run()
        {
            if (DScreen.CurrentScene == null)
            {
                DScreen.ChangeScene(SceneType.stLogin);
            }
            else
            {
                if (DScreen.CurrentScene == LoginScene)
                {
                    LoginScene.Login();
                }
                DScreen.CurrentScene.DoNotifyEvent();
                if (DScreen.CurrentScene == g_PlayScene)
                {
                    ProcessActionMessages();
                    if (MShare.g_MySelf != null)
                    {
                        g_PlayScene.BeginScene();
                    }
                }
            }
        }

        private void AppLogout()
        {
            if (MShare.g_boQueryExit)
            {
                return;
            }
            MShare.g_boQueryExit = true;
            try
            {
                SendClientMessage(Grobal2.CM_SOFTCLOSE, 0, 0, 0, 0);
                CloseAllWindows();
                g_PlayScene.ClearActors();
                MShare.g_SoftClosed = true;
                //ActiveCmdTimer(MShare.TTimerCommand.tcSoftClose);
                MShare.SaveItemFilter();
            }
            finally
            {
                MShare.g_boQueryExit = false;
            }
        }

        private void LoginOut()
        {
            MainOutMessage("退出游戏.");
            SendClientMessage(Grobal2.CM_SOFTCLOSE, 0, 0, 0, 0);
        }

        public void AppExit()
        {
            if (MShare.g_boQueryExit)
            {
                return;
            }
            MShare.g_boQueryExit = true;
            try
            {
                SendClientMessage(Grobal2.CM_SOFTCLOSE, 0, 0, 0, 0);
                MShare.SaveItemFilter();
            }
            finally
            {
                MShare.g_boQueryExit = false;
            }
        }

        private void ProcessMagic()
        {
            short nSX;
            short nSY;
            int tdir;
            int targid;
            short targx;
            short targy;
            //TUseMagicInfo pmag;
            if ((g_PlayScene.ProcMagic.NTargetX < 0) || (MShare.g_MySelf == null))
            {
                return;
            }
            if (MShare.GetTickCount() - g_PlayScene.ProcMagic.DwTick > 5000)
            {
                g_PlayScene.ProcMagic.DwTick = MShare.GetTickCount();
                g_PlayScene.ProcMagic.NTargetX = -1;
                return;
            }
            if (MShare.GetTickCount() - m_dwProcUseMagicTick > 28)
            {
                m_dwProcUseMagicTick = MShare.GetTickCount();
                if (g_PlayScene.ProcMagic.FUnLockMagic)
                {
                    targid = 0;
                    targx = g_PlayScene.ProcMagic.NTargetX;
                    targy = g_PlayScene.ProcMagic.NTargetY;
                }
                else if ((g_PlayScene.ProcMagic.XTarget != null) && !g_PlayScene.ProcMagic.XTarget.m_boDeath)
                {
                    targid = g_PlayScene.ProcMagic.XTarget.m_nRecogId;
                    targx = g_PlayScene.ProcMagic.XTarget.m_nCurrX;
                    targy = g_PlayScene.ProcMagic.XTarget.m_nCurrY;
                }
                else
                {
                    g_PlayScene.ProcMagic.NTargetX = -1;
                    return;
                }
                nSX = (short)Math.Abs(MShare.g_MySelf.m_nCurrX - targx);
                nSY = (short)Math.Abs(MShare.g_MySelf.m_nCurrY - targy);
                if ((nSX <= MShare.g_nMagicRange) && (nSY <= MShare.g_nMagicRange))
                {
                    if (g_PlayScene.ProcMagic.FContinue || (CanNextAction() && ServerAcceptNextAction()))
                    {
                        MShare.g_dwLatestSpellTick = MShare.GetTickCount();
                        tdir = ClFunc.GetFlyDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, targx, targy);
                        //pmag = new TUseMagicInfo();
                        //pmag.EffectNumber = g_PlayScene.ProcMagic.XMagic.Def.btEffect;
                        //pmag.MagicSerial = g_PlayScene.ProcMagic.XMagic.Def.wMagicID;
                        //pmag.ServerMagicCode = 0;
                        MShare.g_dwMagicDelayTime = 200 + g_PlayScene.ProcMagic.XMagic.Def.DelayTime;
                        MShare.g_dwMagicPKDelayTime = 0;
                        if (MShare.g_MagicTarget != null)
                        {
                            if (MShare.g_MagicTarget.m_btRace == 0)
                            {
                                MShare.g_dwMagicPKDelayTime = 300 + RandomNumber.GetInstance().Random(1100);
                            }
                        }
                        //MShare.g_MySelf.SendMsg(Grobal2.CM_SPELL, targx, targy, tdir, pmag, targid, "", 0);
                        g_PlayScene.ProcMagic.NTargetX = -1;
                    }
                }
                else
                {
                    MShare.g_ChrAction = TChrAction.caRun;
                    MShare.g_nTargetX = targx;
                    MShare.g_nTargetY = targy;
                }
            }
        }

        private void ProcessKeyMessages()
        {
            if (ActionKey == 0)
            {
                return;
            }
            if ((MShare.g_MySelf != null) && MShare.g_MySelf.m_StallMgr.OnSale)
            {
                return;
            }
            //switch (ActionKey)
            //{
            //    case VK_F1:
            //    case VK_F2:
            //    case VK_F3:
            //    case VK_F4:
            //    case VK_F5:
            //    case VK_F6:
            //    case VK_F7:
            //    case VK_F8:
            //        if (MShare.g_MySelf.m_btHorse == 0)
            //        {
            //            UseMagic(MShare.g_nMouseX, MShare.g_nMouseY, GetMagicByKey((char)(ActionKey - VK_F1) + (byte)"1"));
            //        }
            //        ActionKey = 0;
            //        MShare.g_nTargetX = -1;
            //        return;
            //        break;
            //    // Modify the A .. B: 12 .. 19
            //    case 12:
            //        if (MShare.g_MySelf.m_btHorse == 0)
            //        {
            //            UseMagic(MShare.g_nMouseX, MShare.g_nMouseY, GetMagicByKey((char)(ActionKey - 12) + (byte)"1" + (byte)0x14));
            //        }
            //        ActionKey = 0;
            //        MShare.g_nTargetX = -1;
            //        return;
            //        break;
            //}
        }

        private void ProcessActionMessages()
        {
            if (MShare.g_MySelf == null)
            {
                return;
            }
            if ((MShare.g_nTargetX >= 0) && CanNextAction() && ServerAcceptNextAction())
            {
                if (MShare.g_boOpenAutoPlay && (MShare.g_APMapPath != null) && (MShare.g_APStep >= 0) && (0 < MShare.g_APMapPath.Length))
                {
                    if ((Math.Abs(MShare.g_APMapPath[MShare.g_APStep].X - MShare.g_MySelf.m_nCurrX) <= 3) && (Math.Abs(MShare.g_APMapPath[MShare.g_APStep].X - MShare.g_MySelf.m_nCurrY) <= 3))
                    {
                        if (MShare.g_APMapPath.Length >= 2)// 3点以上
                        {
                            if (MShare.g_APStep >= MShare.g_APMapPath.Length) // 当前点在终点...
                            {
                                // 终点 <-> 起点 距离过远...
                                if ((Math.Abs(MShare.g_APMapPath[MShare.g_APMapPath.Length].X - MShare.g_APMapPath[0].X) >= 36) || (Math.Abs(MShare.g_APMapPath[MShare.g_APMapPath.Length].X - MShare.g_APMapPath[0].X) >= 36))
                                {
                                    MShare.g_APGoBack = true; // 原路返回
                                    MShare.g_APLastPoint = MShare.g_APMapPath[MShare.g_APStep];
                                    MShare.g_APStep -= 1;
                                }
                                else
                                {
                                    MShare.g_APGoBack = false; // 循环到起点...
                                    MShare.g_APLastPoint = MShare.g_APMapPath[MShare.g_APStep];
                                    MShare.g_APStep = 0;
                                }
                            }
                            else
                            {
                                if (MShare.g_APGoBack) // 原路返回
                                {
                                    MShare.g_APLastPoint = MShare.g_APMapPath[MShare.g_APStep];
                                    MShare.g_APStep -= 1;
                                    if (MShare.g_APStep <= 0)// 已回到起点
                                    {
                                        MShare.g_APStep = 0;
                                        MShare.g_APGoBack = false;
                                    }
                                }
                                else
                                {
                                    MShare.g_APLastPoint = MShare.g_APMapPath[MShare.g_APStep]; // 循环...
                                    MShare.g_APStep++;
                                }
                            }
                        }
                        else
                        {
                            // 2点,循环...
                            MShare.g_APLastPoint = MShare.g_APMapPath[MShare.g_APStep];
                            MShare.g_APStep++;
                            if (MShare.g_APStep > MShare.g_APMapPath.Length)
                            {
                                MShare.g_APStep = 0;
                            }
                        }
                    }
                }
                if ((MShare.g_nTargetX != MShare.g_MySelf.m_nCurrX) || (MShare.g_nTargetY != MShare.g_MySelf.m_nCurrY))
                {
                    if ((MShare.g_MySelf.m_nTagX > 0) && (MShare.g_MySelf.m_nTagY > 0))
                    {
                        if (g_MoveBusy)
                        {
                            if (MShare.GetTickCount() - g_MoveErrTick > 60)
                            {
                                g_MoveErrTick = MShare.GetTickCount();
                                g_MoveErr++;
                            }
                        }
                        else
                        {
                            g_MoveErr = 0;
                        }
                        if (g_MoveErr > 10)
                        {
                            g_MoveErr = 0;
                            g_MoveBusy = false;
                            TimerAutoMove.Enabled = false;
                            if ((MShare.g_MySelf.m_nTagX > 0) && (MShare.g_MySelf.m_nTagY > 0))
                            {
                                if (!g_PathBusy)
                                {
                                    g_PathBusy = true;
                                    try
                                    {
                                        Map.ReLoadMapData();
                                        TPathMap.g_MapPath = Map.FindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_MySelf.m_nTagX, MShare.g_MySelf.m_nTagY, 0);
                                        if (TPathMap.g_MapPath != null)
                                        {
                                            g_MoveStep = 1;
                                            TimerAutoMove.Enabled = true;
                                        }
                                        else
                                        {
                                            MShare.g_MySelf.m_nTagX = 0;
                                            MShare.g_MySelf.m_nTagY = 0;
                                            DScreen.AddChatBoardString("自动移动出错，停止移动", GetRGB(5));
                                        }
                                    }
                                    finally
                                    {
                                        g_PathBusy = false;
                                    }
                                }
                            }
                        }
                    }
                TTTT:
                    var mx = MShare.g_MySelf.m_nCurrX;
                    var my = MShare.g_MySelf.m_nCurrY;
                    var dx = MShare.g_nTargetX;
                    var dy = MShare.g_nTargetY;
                    var ndir = ClFunc.GetNextDirection(mx, my, dx, dy);
                    int crun;
                    byte mdir;
                    switch (MShare.g_ChrAction)
                    {
                        case TChrAction.caWalk:
                        LB_WALK:
                            crun = MShare.g_MySelf.CanWalk();
                            if (IsUnLockAction() && (crun > 0))
                            {
                                ClFunc.GetNextPosXY(ndir, ref mx, ref my);
                                var bostop = false;
                                if (!g_PlayScene.CanWalk(mx, my))
                                {
                                    if (MShare.g_boOpenAutoPlay && MShare.g_boAPAutoMove && (MShare.g_APPathList.Count > 0))
                                    {
                                        heroActor.Init_Queue2();
                                        MShare.g_nTargetX = -1;
                                    }
                                    var bowalk = false;
                                    byte adir = 0;
                                    if (!bowalk)
                                    {
                                        mx = MShare.g_MySelf.m_nCurrX;
                                        my = MShare.g_MySelf.m_nCurrY;
                                        ClFunc.GetNextPosXY(ndir, ref mx, ref my);
                                        if (CheckDoorAction(mx, my))
                                        {
                                            bostop = true;
                                        }
                                    }
                                    if (!bostop && (g_PlayScene.CrashMan(mx, my) || !Map.CanMove(mx, my)))
                                    {
                                        mx = MShare.g_MySelf.m_nCurrX;
                                        my = MShare.g_MySelf.m_nCurrY;
                                        adir = ClFunc.PrivDir(ndir);
                                        ClFunc.GetNextPosXY(adir, ref mx, ref my);
                                        if (!Map.CanMove(mx, my))
                                        {
                                            mx = MShare.g_MySelf.m_nCurrX;
                                            my = MShare.g_MySelf.m_nCurrY;
                                            adir = ClFunc.NextDir(ndir);
                                            ClFunc.GetNextPosXY(adir, ref mx, ref my);
                                            if (Map.CanMove(mx, my))
                                            {
                                                bowalk = true;
                                            }
                                        }
                                        else
                                        {
                                            bowalk = true;
                                        }
                                    }
                                    if (bowalk)
                                    {
                                        MShare.g_MySelf.UpdateMsg(Grobal2.CM_WALK, (ushort)mx, (ushort)my, adir, 0, 0, "", 0);
                                        MShare.g_dwLastMoveTick = MShare.GetTickCount();
                                        if (MShare.g_nOverAPZone > 0)
                                        {
                                            MShare.g_nOverAPZone -= 1;
                                        }
                                    }
                                    else
                                    {
                                        mdir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, dx, dy);
                                        if (mdir != MShare.g_MySelf.m_btDir)
                                        {
                                            MShare.g_MySelf.SendMsg(Grobal2.CM_TURN, (ushort)MShare.g_MySelf.m_nCurrX, (ushort)MShare.g_MySelf.m_nCurrY, mdir, 0, 0, "", 0);
                                        }
                                        MShare.g_nTargetX = -1;
                                    }
                                }
                                else
                                {
                                    MShare.g_MySelf.UpdateMsg(Grobal2.CM_WALK, (ushort)mx, (ushort)my, ndir, 0, 0, "", 0);
                                    MShare.g_dwLastMoveTick = MShare.GetTickCount();
                                }
                            }
                            else
                            {
                                MShare.g_nTargetX = -1;
                            }
                            break;
                        case TChrAction.caRun: // 免助跑
                            if (MShare.g_boCanStartRun || (MShare.g_nRunReadyCount >= 1))
                            {
                                crun = MShare.g_MySelf.CanRun();// 骑马开始
                                if ((MShare.g_MySelf.m_btHorse != 0) && (ClFunc.GetDistance(mx, my, dx, dy) >= 3) && (crun > 0) && IsUnLockAction())
                                {
                                    ClFunc.GetNextHorseRunXY(ndir, ref mx, ref my);
                                    if (g_PlayScene.CanRun(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, mx, my))
                                    {
                                        MShare.g_MySelf.UpdateMsg(Grobal2.CM_HORSERUN, (ushort)mx, (ushort)my, ndir, 0, 0, "", 0);
                                        MShare.g_dwLastMoveTick = MShare.GetTickCount();
                                        if (MShare.g_nOverAPZone > 0)
                                        {
                                            MShare.g_nOverAPZone -= 1;
                                        }
                                    }
                                    else
                                    {
                                        // 如果跑失败则跳回去走
                                        MShare.g_ChrAction = TChrAction.caWalk;
                                        goto TTTT;
                                    }
                                }
                                else
                                {
                                    if ((ClFunc.GetDistance(mx, my, dx, dy) >= 2) && (crun > 0))
                                    {
                                        if (IsUnLockAction())
                                        {
                                            ClFunc.GetNextRunXY(ndir, ref mx, ref my);
                                            if (g_PlayScene.CanRun(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, mx, my))
                                            {
                                                MShare.g_MySelf.UpdateMsg(Grobal2.CM_RUN, (ushort)mx, (ushort)my, ndir, 0, 0, "", 0);
                                                MShare.g_dwLastMoveTick = MShare.GetTickCount();
                                                if (MShare.g_nOverAPZone > 0)
                                                {
                                                    MShare.g_nOverAPZone -= 1;
                                                }
                                            }
                                            else
                                            {
                                                // 如果跑失败则跳回去走
                                                MShare.g_ChrAction = TChrAction.caWalk;
                                                goto TTTT;
                                            }
                                        }
                                        else
                                        {
                                            MShare.g_nTargetX = -1;
                                        }
                                    }
                                    else
                                    {
                                        mdir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, dx, dy);
                                        if (mdir != MShare.g_MySelf.m_btDir)
                                        {
                                            MShare.g_MySelf.SendMsg(Grobal2.CM_TURN, (ushort)MShare.g_MySelf.m_nCurrX, (ushort)MShare.g_MySelf.m_nCurrY, mdir, 0, 0, "", 0);
                                        }
                                        MShare.g_nTargetX = -1;
                                        goto LB_WALK;
                                    }
                                }
                            }
                            else
                            {
                                MShare.g_nRunReadyCount++;
                                goto LB_WALK;
                            }
                            break;
                    }
                }
                else if (MShare.g_boOpenAutoPlay && MShare.g_boAPAutoMove && (MShare.g_AutoPicupItem != null))
                {
                    SendPickup();
                    MShare.g_sAPStr = "拾取物品";
                    if (MShare.g_boAPAutoMove && (MShare.g_APPathList.Count > 0))
                    {
                        heroActor.Init_Queue2();
                        MShare.g_nTargetX = -1;
                    }
                }
            }
            MShare.g_nTargetX = -1;
        MMMM:
            if (MShare.g_MySelf.RealActionMsg.Ident > 0)
            {
                if (MShare.g_MySelf.RealActionMsg.Ident == Grobal2.CM_SPELL)
                {
                    SendSpellMsg(MShare.g_MySelf.RealActionMsg.Ident, (ushort)MShare.g_MySelf.RealActionMsg.X, (ushort)MShare.g_MySelf.RealActionMsg.Y, MShare.g_MySelf.RealActionMsg.Dir, MShare.g_MySelf.RealActionMsg.State);
                }
                else
                {
                    SendActMsg(MShare.g_MySelf.RealActionMsg.Ident, (ushort)MShare.g_MySelf.RealActionMsg.X, (ushort)MShare.g_MySelf.RealActionMsg.Y, MShare.g_MySelf.RealActionMsg.Dir);
                }
                MShare.g_MySelf.RealActionMsg.Ident = 0;
                if (MShare.g_nStallX != -1)
                {
                    if ((Math.Abs(MShare.g_nStallX - MShare.g_MySelf.m_nCurrX) >= 8) || (Math.Abs(MShare.g_nStallY - MShare.g_MySelf.m_nCurrY) >= 8))
                    {
                        MShare.g_nStallX = -1;
                    }
                }
            }
        }

        public void OpenAutoPlay()
        {
            if (MShare.g_MySelf == null)
            {
                return;
            }
            MShare.g_gcAss[0] = !MShare.g_gcAss[0];
            if (TimerAutoPlay == null)
            {
                TimerAutoPlay = new TimerAutoPlay();
            }
            TimerAutoPlay.Enabled = MShare.g_gcAss[0];
            if (TimerAutoPlay.Enabled)
            {
                MShare.g_APTagget = null;
                MShare.g_AutoPicupItem = null;
                MShare.g_nAPStatus = -1;
                MShare.g_nTargetX = -1;
                MShare.g_APGoBack = false;
                DScreen.AddChatBoardString("开始自动挂机...", ConsoleColor.Red);
                SaveWayPoint();
                if (MShare.g_APMapPath != null)
                {
                    MShare.g_APStep = 0;
                    MShare.g_APLastPoint.X = -1;
                    GetNearPoint();
                }
            }
            else
            {
                DScreen.AddChatBoardString("停止自动挂机...", ConsoleColor.Red);
            }
            return;
        }

        public ClientMagic GetMagicByKey(char Key)
        {
            ClientMagic result = null;
            for (var i = 0; i < MShare.g_MagicList.Count; i++)
            {
                var pm = MShare.g_MagicList[i];
                if (pm.Key == Key)
                {
                    result = pm;
                    break;
                }
            }
            return result;
        }

        public void UseMagic(int tx, int ty, ClientMagic pcm, bool boReacll = false, bool boContinue = false)
        {
            bool boSeriesSkill;
            int defSpellSpend;
            int tdir;
            var targx = 0;
            var targy = 0;
            var targid = 0;
            //TUseMagicInfo pmag;
            short SpellSpend;
            bool fUnLockMagic;
            if ((MShare.g_MySelf != null))
            {
                return;
            }
            if (pcm == null)
            {
                return;
            }
            if (pcm.Def.MagicId == 0)
            {
                return;
            }
            SpellSpend = (short)(HUtil32.Round(pcm.Def.Spell / (pcm.Def.TrainLv + 1) * (pcm.Level + 1)) + pcm.Def.DefSpell);
            if (pcm.Def.MagicId == 114)
            {
                if (MShare.g_boSkill_114_MP)
                {
                    boSeriesSkill = false;
                }
                else
                {
                    boSeriesSkill = true;
                }
            }
            else if (new ArrayList(new int[] { 68, 78 }).Contains(pcm.Def.MagicId))
            {
                if (MShare.g_boSkill_68_MP)
                {
                    boSeriesSkill = false;
                }
                else
                {
                    boSeriesSkill = true;
                }
            }
            else
            {
                boSeriesSkill = pcm.Def.MagicId >= 100 && pcm.Def.MagicId <= 111;
            }
            if (boSeriesSkill)
            {
                defSpellSpend = MShare.g_MySelf.m_nIPower;
            }
            else
            {
                defSpellSpend = MShare.g_MySelf.m_Abil.MP;
            }
            if (SpellSpend <= defSpellSpend)
            {
                if (pcm.Def.EffectType == 0)
                {
                    if (new ArrayList(new int[] { 68, 78 }).Contains(pcm.Def.MagicId))
                    {
                        boContinue = true;
                        goto labSpell;
                    }
                    switch (pcm.Def.MagicId)
                    {
                        case 26 when MShare.g_boNextTimeFireHit || (MShare.GetTickCount() - MShare.g_dwLatestFireHitTick <= 10 * 1000): // 烈火时间间隔
                        case 66 when MShare.g_boCanSLonHit || (MShare.GetTickCount() - MShare.g_dwLatestSLonHitTick <= 8 * 1000):
                        case 43 when MShare.g_boNextTimeTwinHit || (MShare.GetTickCount() - MShare.g_dwLatestTwinHitTick <= 15 * 1000):
                        case 56 when MShare.g_boNextTimePursueHit || (MShare.GetTickCount() - MShare.g_dwLatestPursueHitTick <= 10 * 1000): // 野蛮时间间隔
                        case 27 when MShare.GetTickCount() - MShare.g_dwLatestRushRushTick <= 3 * 1000:
                            return;
                        case 100 when boContinue || (CanNextAction() && ServerAcceptNextAction() && CanNextHit()):
                            break;
                        case 100:
                        case 101 when MShare.g_boNextTimeSmiteHit || (MShare.GetTickCount() - MShare.g_dwLatestSmiteHitTick <= 1 * 100):
                        case 102 when MShare.g_boNextTimeSmiteLongHit || (MShare.GetTickCount() - MShare.g_dwLatestSmiteLongHitTick <= 1 * 100):
                        case 103 when MShare.g_boNextTimeSmiteWideHit || (MShare.GetTickCount() - MShare.g_dwLatestSmiteWideHitTick <= 1 * 100):
                        case 113 when MShare.g_boNextTimeSmiteLongHit2 || (MShare.GetTickCount() - MShare.g_dwLatestSmiteLongHitTick2 <= 10 * 1000):
                        case 114 when MShare.g_boNextTimeSmiteWideHit2 || (MShare.GetTickCount() - MShare.g_dwLatestSmiteWideHitTick2 <= 2 * 1000):
                        case 115 when MShare.g_boNextTimeSmiteLongHit3 || (MShare.GetTickCount() - MShare.g_dwLatestSmiteLongHitTick3 <= 2 * 1000):
                            return;
                    }
                    if (MShare.g_boSpeedRate)
                    {
                        if (boContinue || (MShare.GetTickCount() - MShare.g_dwLatestSpellTick > MShare.g_dwSpellTime - ((long)MShare.g_MagSpeedRate) * 20))
                        {
                            MShare.g_dwLatestSpellTick = MShare.GetTickCount();
                            MShare.g_dwMagicDelayTime = 0;
                            SendSpellMsg(Grobal2.CM_SPELL, MShare.g_MySelf.m_btDir, 0, pcm.Def.MagicId, 0, false);
                        }
                    }
                    else
                    {
                        if (boContinue || (MShare.GetTickCount() - MShare.g_dwLatestSpellTick > MShare.g_dwSpellTime))
                        {
                            MShare.g_dwLatestSpellTick = MShare.GetTickCount();
                            MShare.g_dwMagicDelayTime = 0;
                            SendSpellMsg(Grobal2.CM_SPELL, MShare.g_MySelf.m_btDir, 0, pcm.Def.MagicId, 0, false);
                        }
                    }
                }
            labSpell:
                fUnLockMagic = new ArrayList(new[] { 2, 9, 10, 14, 21, 33, 37, 41, 46, 50, 58, 70, 72, 75 }).Contains(pcm.Def.MagicId);
                if (fUnLockMagic)
                {
                    MShare.g_MagicTarget = MShare.g_FocusCret;
                }
                else
                {
                    if (MShare.g_boMagicLock && g_PlayScene.IsValidActor(MShare.g_FocusCret) && !MShare.g_FocusCret.m_boDeath)
                    {
                        MShare.g_MagicLockActor = MShare.g_FocusCret;
                    }
                    MShare.g_MagicTarget = MShare.g_MagicLockActor;
                }
                if (MShare.g_MagicTarget != null)
                {
                    if (!MShare.g_boMagicLock || MShare.g_MagicTarget.m_boDeath || (MShare.g_MagicTarget.m_btRace == Grobal2.RCC_MERCHANT) || !g_PlayScene.IsValidActor(MShare.g_MagicTarget))
                    {
                        MShare.g_MagicTarget = null;
                        MShare.g_MagicLockActor = null;
                    }
                }
                if ((MShare.g_MagicTarget != null) && (MShare.g_MagicTarget is THumActor))
                {
                    if (((THumActor)MShare.g_MagicTarget).m_StallMgr.OnSale)
                    {
                        MShare.g_MagicTarget = null;
                        MShare.g_MagicLockActor = null;
                    }
                }
                SmartChangePoison(pcm);
                if (MShare.g_MagicTarget == null)
                {
                    MShare.g_nCurrentMagic = 888;
                    if (boReacll)
                    {
                        targx = tx;
                        targy = ty;
                    }
                    targid = 0;
                }
                else
                {
                    if (!boReacll)
                    {
                        targx = MShare.g_MagicTarget.m_nCurrX;
                        targy = MShare.g_MagicTarget.m_nCurrY;
                    }
                    else
                    {
                        targx = tx;
                        targy = ty;
                    }
                    targid = MShare.g_MagicTarget.m_nRecogId;
                }
                if ((Math.Abs(MShare.g_MySelf.m_nCurrX - targx) > MShare.g_nMagicRange) || (Math.Abs(MShare.g_MySelf.m_nCurrY - targy) > MShare.g_nMagicRange))
                {
                    if (MShare.g_gcTec[14] && (fUnLockMagic || (targid != 0)))
                    {
                        g_PlayScene.ProcMagic.NTargetX = (short)targx;
                        g_PlayScene.ProcMagic.NTargetY = (short)targy;
                        g_PlayScene.ProcMagic.XMagic = pcm;
                        g_PlayScene.ProcMagic.XTarget = MShare.g_MagicLockActor;
                        g_PlayScene.ProcMagic.FReacll = boReacll;
                        g_PlayScene.ProcMagic.FContinue = boContinue;
                        g_PlayScene.ProcMagic.FUnLockMagic = fUnLockMagic;
                        g_PlayScene.ProcMagic.DwTick = MShare.GetTickCount();
                    }
                    else
                    {
                        if (MShare.GetTickCount() - g_dwOverSpaceWarningTick > 1000)
                        {
                            g_dwOverSpaceWarningTick = MShare.GetTickCount();
                            DScreen.AddSysMsg("目标太远了，施展魔法失败！！！");
                        }
                        g_PlayScene.ProcMagic.NTargetX = -1;
                    }
                    return;
                }
                g_PlayScene.ProcMagic.NTargetX = -1;
                if (boContinue || (CanNextAction() && ServerAcceptNextAction()))
                {
                    MShare.g_dwLatestSpellTick = MShare.GetTickCount();
                    tdir = ClFunc.GetFlyDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, targx, targy);
                    var pmag = new TUseMagicInfo();
                    pmag.EffectNumber = pcm.Def.Effect;
                    pmag.MagicSerial = pcm.Def.MagicId;
                    pmag.ServerMagicCode = 0;
                    MShare.g_dwMagicDelayTime = 200 + pcm.Def.DelayTime;
                    MShare.g_dwMagicPKDelayTime = 0;
                    if (MShare.g_MagicTarget != null)
                    {
                        if (MShare.g_MagicTarget.m_btRace == 0)
                        {
                            MShare.g_dwMagicPKDelayTime = 300 + RandomNumber.GetInstance().Random(1100);
                        }
                    }
                    //MShare.g_MySelf.SendMsg(Grobal2.CM_SPELL, targx, targy, tdir, pmag, targid, "", 0);
                }
            }
            else
            {
                if (boSeriesSkill)
                {
                    if (MShare.GetTickCount() - MShare.g_IPointLessHintTick > 5000)
                    {
                        MShare.g_IPointLessHintTick = MShare.GetTickCount();
                        DScreen.AddSysMsg($"需要 {SpellSpend} 内力值才能释放 {pcm.Def.MagicName}");
                    }
                }
                else if (MShare.GetTickCount() - MShare.g_MPLessHintTick > 1000)
                {
                    MShare.g_MPLessHintTick = MShare.GetTickCount();
                    DScreen.AddSysMsg($"需要 {SpellSpend} 魔法值才能释放 {pcm.Def.MagicName}");
                }
            }
        }

        private void UseMagicSpell(int who, int effnum, int targetx, int targety, int magic_id)
        {
            var Actor = g_PlayScene.FindActor(who);
            if (Actor != null)
            {
                var adir = ClFunc.GetFlyDirection(Actor.m_nCurrX, Actor.m_nCurrY, targetx, targety);
                var UseMagic = new TUseMagicInfo();
                UseMagic.EffectNumber = effnum % 255;
                UseMagic.ServerMagicCode = 0;
                UseMagic.MagicSerial = magic_id % 300;
                //Actor.SendMsg(Grobal2.SM_SPELL, effnum / 255, magic_id / 300, adir, UseMagic, 0, "", 0);
                MShare.g_nSpellCount++;
            }
            else
            {
                MShare.g_nSpellFailCount++;
            }
        }

        private void UseMagicFire(int who, int efftype, int effnum, int targetx, int targety, int target, int maglv)
        {
            var Actor = g_PlayScene.FindActor(who);
            if (Actor != null)
            {
                Actor.SendMsg(Grobal2.SM_MAGICFIRE, (ushort)(short)target, (ushort)(short)efftype, effnum, targetx, targety, maglv.ToString(), 0);
                if (MShare.g_nFireCount < MShare.g_nSpellCount)
                {
                    MShare.g_nFireCount++;
                }
            }
            MShare.g_MagicTarget = null;
        }

        private void UseMagicFireFail(int who)
        {
            var Actor = g_PlayScene.FindActor(who);
            if (Actor != null)
            {
                Actor.SendMsg(Grobal2.SM_MAGICFIRE_FAIL, 0, 0, 0, 0, 0, "", 0);
            }
            MShare.g_MagicTarget = null;
        }

        public void ActorAutoEat(THumActor Actor)
        {
            if (!Actor.m_boDeath)
            {
                ActorCheckHealth(false);
                if (MShare.g_EatingItem.Item.Name == "")
                {
                    if (MShare.IsPersentSpc(Actor.m_Abil.HP, Actor.m_Abil.MaxHP))
                    {
                        ActorCheckHealth(true);
                    }
                }
            }
        }

        public void ActorCheckHealth(bool bNeedSP)
        {
            var nCount = 0;
            var hidx = -1;
            var midx = -1;
            var sidx = -1;
            var bidx = -1;
            var uhidx = -1;
            var umidx = -1;
            var usidx = -1;
            var ubidx = -1;
            var MaxHP = int.MaxValue / 2 - 1;
            var MaxMP = int.MaxValue / 2 - 1;
            var MaxSP = int.MaxValue / 2 - 1;
            for (var i = Grobal2.MAXBAGITEM - (1 + 0); i >= 0; i--)
            {
                if ((MShare.g_ItemArr[i].Item.Name != "") && (MShare.g_ItemArr[i].Item.NeedIdentify < 4))
                {
                    switch (MShare.g_ItemArr[i].Item.StdMode)
                    {
                        case 00:
                            switch (MShare.g_ItemArr[i].Item.Shape)
                            {
                                case 0: // 普通药
                                    if (MShare.g_gcProtect[0] && (MShare.g_ItemArr[i].Item.AC > 0) && (MShare.g_ItemArr[i].Item.AC < MaxHP))
                                    {
                                        MaxHP = MShare.g_ItemArr[i].Item.AC;
                                        hidx = i;
                                    }
                                    if (MShare.g_gcProtect[1] && (MShare.g_ItemArr[i].Item.MAC > 0) && (MShare.g_ItemArr[i].Item.MAC < MaxMP))
                                    {
                                        MaxMP = MShare.g_ItemArr[i].Item.MAC;
                                        midx = i;
                                    }
                                    break;
                                case 1: // 速效药
                                    if (MShare.g_gcProtect[3] && (MShare.g_ItemArr[i].Item.AC > 0) && (MShare.g_ItemArr[i].Item.AC < MaxSP))
                                    {
                                        MaxSP = MShare.g_ItemArr[i].Item.AC;
                                        sidx = i;
                                    }
                                    break;
                            }
                            break;
                        case 2:
                        case 3:
                            if (MShare.g_gcProtect[5])
                            {
                                if (String.Compare(MShare.g_ItemArr[i].Item.Name, MShare.g_sRenewBooks[MShare.g_gnProtectPercent[6]], StringComparison.Ordinal) == 0)
                                {
                                    bidx = i;
                                }
                            }
                            break;
                        case 31:
                            switch (MShare.g_ItemArr[i].Item.AniCount)
                            {
                                case 1:
                                    if (MShare.g_gcProtect[0])
                                    {
                                        uhidx = i;
                                    }
                                    break;
                                case 2:
                                    if (MShare.g_gcProtect[1])
                                    {
                                        umidx = i;
                                    }
                                    break;
                                case 3:
                                    if (MShare.g_gcProtect[3])
                                    {
                                        usidx = i;
                                    }
                                    break;
                                default:
                                    if (MShare.g_gcProtect[5] && (string.Compare(MShare.g_ItemArr[i].Item.Name, MShare.g_sRenewBooks[MShare.g_gnProtectPercent[6]] + "包", StringComparison.Ordinal) == 0))
                                    {
                                        ubidx = i;
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    nCount++;
                }
            }
            var bHint = false;
            var bEatSp = false;
            var bEatOK = false;
            if (MShare.GetTickCount() - MShare.g_MySelf.m_dwMsgHint > 15 * 1000)
            {
                MShare.g_MySelf.m_dwMsgHint = MShare.GetTickCount();
                bHint = true;
            }
            if (!bNeedSP)
            {
                if (MShare.g_gcProtect[0] && MShare.IsPersentHP(MShare.g_MySelf.m_Abil.HP, MShare.g_MySelf.m_Abil.MaxHP))
                {
                    if (MShare.GetTickCount() - MShare.g_MySelf.m_dwHealthHP > MShare.g_gnProtectTime[0])
                    {
                        MShare.g_MySelf.m_dwHealthHP = MShare.GetTickCount();
                        if (hidx > -1)
                        {
                            EatItem(hidx);
                            bEatOK = true;
                        }
                        else if ((nCount > 4) && (uhidx > -1))
                        {
                            EatItem(uhidx);
                            bEatOK = true;
                        }
                        else
                        {
                            bEatSp = true;
                            if (bHint)
                            {
                                DScreen.AddChatBoardString("你的金创药已经用完！", ConsoleColor.Green, ConsoleColor.Black);
                            }
                            bEatOK = false;
                        }
                    }
                }
            }
            if (!bNeedSP)
            {
                if (MShare.g_gcProtect[1] && MShare.IsPersentMP(MShare.g_MySelf.m_Abil.MP, MShare.g_MySelf.m_Abil.MaxMP))
                {
                    if (MShare.GetTickCount() - MShare.g_MySelf.m_dwHealthMP > MShare.g_gnProtectTime[1])
                    {
                        MShare.g_MySelf.m_dwHealthMP = MShare.GetTickCount();
                        if (midx > -1)
                        {
                            EatItem(midx);
                            bEatOK = true;
                        }
                        else if ((nCount > 4) && (umidx > -1))
                        {
                            EatItem(umidx);
                            bEatOK = true;
                        }
                        else
                        {
                            if (MShare.g_gcProtect[11])
                            {
                                bEatSp = true;
                            }
                            if (bHint)
                            {
                                DScreen.AddChatBoardString("你的魔法药已经用完！", ConsoleColor.Green, ConsoleColor.Black);
                            }
                            bEatOK = false;
                        }
                    }
                }
            }
            if (!bEatOK)
            {
                if (MShare.g_gcProtect[3] && (bNeedSP || bEatSp || (MShare.g_gcProtect[11] && MShare.IsPersentSpc(MShare.g_MySelf.m_Abil.MP, MShare.g_MySelf.m_Abil.MaxMP))))
                {
                    if (MShare.GetTickCount() - MShare.g_MySelf.m_dwHealthSP > MShare.g_gnProtectTime[3])
                    {
                        MShare.g_MySelf.m_dwHealthSP = MShare.GetTickCount();
                        if (sidx > -1)
                        {
                            EatItem(sidx);
                        }
                        else if ((nCount > 4) && (usidx > -1))
                        {
                            EatItem(usidx);
                        }
                        else if (bHint)
                        {
                            DScreen.AddChatBoardString("你的特殊药品已经用完！", ConsoleColor.Green, ConsoleColor.Black);
                        }
                    }
                }
            }
            if (MShare.g_gcProtect[5] && MShare.IsPersentBook(MShare.g_MySelf.m_Abil.HP, MShare.g_MySelf.m_Abil.MaxHP))
            {
                if (MShare.GetTickCount() - MShare.g_MySelf.m_dwHealthBK > MShare.g_gnProtectTime[5])
                {
                    MShare.g_MySelf.m_dwHealthBK = MShare.GetTickCount();
                    if (bidx > -1)
                    {
                        EatItem(bidx);
                    }
                    else if ((nCount > 4) && (ubidx > -1))
                    {
                        EatItem(ubidx);
                    }
                    else if (bHint)
                    {
                        DScreen.AddChatBoardString("你的" + MShare.g_sRenewBooks[MShare.g_gnProtectPercent[6]] + "已经用完！", ConsoleColor.Green, ConsoleColor.Black);
                    }
                }
            }
        }

        private void AutoSupplyBeltItem(int nType, int idx, string sItem)
        {
            if (idx >= 0 && idx <= 5 && (sItem != ""))
            {
                if (MShare.g_ItemArr[idx].Item.Name == "")
                {
                    for (var i = MShare.MAXBAGITEMCL - 1; i >= 6; i--)
                    {
                        if (MShare.g_ItemArr[i].Item.Name == sItem)
                        {
                            MShare.g_ItemArr[idx] = MShare.g_ItemArr[i];
                            MShare.g_ItemArr[i].Item.Name = "";
                            return;
                        }
                    }
                    AutoUnBindItem(nType, sItem);
                }
            }
        }

        private void AutoSupplyBagItem(int nType, string sItem)
        {
            for (var i = MShare.MAXBAGITEMCL - 1; i >= 6; i--)
            {
                if (MShare.g_ItemArr[i].Item.Name == sItem)
                {
                    return;
                }
            }
            AutoUnBindItem(nType, sItem);
        }

        /// <summary>
        /// 自动解包物品
        /// </summary>
        private void AutoUnBindItem(int nType, string sItem)
        {
            if ((sItem != "") && (nType != 0))
            {
                var boIsUnBindItem = false;
                for (var i = 0; i < MShare.g_UnBindItems.Length; i++)
                {
                    if (sItem == MShare.g_UnBindItems[i])
                    {
                        boIsUnBindItem = true;
                        break;
                    }
                }
                if (!boIsUnBindItem)
                {
                    return;
                }
                var n = 0;
                var boUnBindAble = false;
                for (var i = 0; i < MShare.MAXBAGITEMCL - 1 - 6; i++)
                {
                    if (MShare.g_ItemArr[i].Item.Name == "")
                    {
                        n++;
                        if (n >= 5)
                        {
                            boUnBindAble = true;
                            break;
                        }
                    }
                }
                if (!boUnBindAble)
                {
                    return;
                }
                var idx = -1;
                for (var i = MShare.MAXBAGITEMCL - 1; i >= 6; i--)
                {
                    if (MShare.g_ItemArr[i].Item.StdMode == 31)
                    {
                        if (MShare.g_ItemArr[i].Item.Name != "")
                        {
                            if (MShare.g_ItemArr[i].Item.Shape == nType)
                            {
                                idx = i;
                                break;
                            }
                        }
                    }
                }
                if (idx > -1)
                {
                    SendEat(MShare.g_ItemArr[idx].MakeIndex, "", MShare.g_ItemArr[idx].Item.StdMode);
                    if (MShare.g_ItemArr[idx].Dura > 1)
                    {
                        MShare.g_ItemArr[idx].Dura = (ushort)(MShare.g_ItemArr[idx].Dura - 1);
                        MShare.g_EatingItem = MShare.g_ItemArr[idx];
                        m_nEatRetIdx = -1;
                    }
                    else
                    {
                        MShare.g_ItemArr[idx].Dura = (ushort)(MShare.g_ItemArr[idx].Dura - 1);
                        MShare.g_EatingItem = MShare.g_ItemArr[idx];
                        MShare.g_ItemArr[idx].Item.Name = "";
                        m_nEatRetIdx = -1;
                    }
                }
            }
        }

        private bool EatItemName(string Str)
        {
            var result = false;
            if ((Str == "小退") && (MShare.g_MySelf.m_nHiterCode > 0))
            {
                AppLogout();
                return result;
            }
            if ((Str == "大退") && (MShare.g_MySelf.m_nHiterCode > 0))
            {
                return result;
            }
            for (var i = 0; i < MShare.MAXBAGITEMCL; i++)
            {
                if ((MShare.g_ItemArr[i].Item.Name == Str) && (MShare.g_ItemArr[i].Item.NeedIdentify < 4))
                {
                    EatItem(i);
                    result = true;
                    return result;
                }
            }
            return result;
        }

        private void EatItem(int idx)
        {
            int i;
            var eatable = false;
            var takeon = false;
            var where = -1;
            if (idx >= 0 && idx <= MShare.MAXBAGITEMCL - 1)
            {
                if ((MShare.g_EatingItem.Item.Name != "") && (MShare.GetTickCount() - MShare.g_dwEatTime > 5 * 1000))
                {
                    MShare.g_EatingItem.Item.Name = "";
                }
                if ((MShare.g_EatingItem.Item.Name == "") && (MShare.g_ItemArr[idx].Item.Name != "") && (MShare.g_ItemArr[idx].Item.NeedIdentify < 4))
                {
                    if ((MShare.g_ItemArr[idx].Item.StdMode <= 3) || (MShare.g_ItemArr[idx].Item.StdMode == 31))
                    {
                        if (MShare.g_ItemArr[idx].Dura > 1)
                        {
                            MShare.g_ItemArr[idx].Dura = (ushort)(MShare.g_ItemArr[idx].Dura - 1);
                            MShare.g_EatingItem = MShare.g_ItemArr[idx];
                            MShare.g_ItemArr[idx].Item.Name = "";
                            eatable = true;
                        }
                        else
                        {
                            MShare.g_EatingItem = MShare.g_ItemArr[idx];
                            MShare.g_ItemArr[idx].Item.Name = "";
                            eatable = true;
                        }
                    }
                    else
                    {
                    lab1:
                        if ((MShare.g_ItemArr[idx].Item.StdMode == 46) && MShare.g_ItemArr[idx].Item.Shape >= 2 && MShare.g_ItemArr[idx].Item.Shape <= 6)
                        {
                            //if (!MShare.g_RareBoxWindow.m_boKeyAvail && (MShare.g_OpenBoxItem.Item.Item.Name == "") && !FrmDlg.DWBoxBKGnd.Visible)
                            //{
                            //    MShare.g_OpenBoxItem.Index = idx;
                            //    MShare.g_OpenBoxItem.Item = MShare.g_ItemArr[idx];
                            //    MShare.g_ItemArr[idx].Item.Name = "";
                            //    //FrmDlg.DWBoxBKGnd.Visible = true;
                            //}
                            return;
                        }
                        if ((MShare.g_ItemArr[idx].Item.StdMode == 41) && new ArrayList(new int[] { 10, 30 }).Contains(MShare.g_ItemArr[idx].Item.Shape) && (MShare.g_BuildAcusesStep != 1))
                        {
                            for (i = 0; i < 7; i++)
                            {
                                if (MShare.g_BuildAcuses[i].Item.Item.Name == "")
                                {
                                    if ((MShare.g_ItemArr[idx].Item.Shape >= 30 && MShare.g_ItemArr[idx].Item.Shape <= 34 && i >= 5 && i <= 7) || (MShare.g_ItemArr[idx].Item.Shape >= 10 && MShare.g_ItemArr[idx].Item.Shape <= 14 && i >= 0 && i <= 4))
                                    {
                                        break;
                                    }
                                }
                            }
                            if (i >= 0 && i <= 7)
                            {
                                MShare.g_boItemMoving = true;
                                MShare.g_MovingItem.Index = idx;
                                MShare.g_MovingItem.Item = MShare.g_ItemArr[idx];
                                MShare.g_ItemArr[idx].Item.Name = "";
                            }
                            //switch (i)
                            //{
                            //    case 0:
                            //        FrmDlg.DBAcus1Click(FrmDlg.DBAcus1, 0, 0);
                            //        break;
                            //    case 1:
                            //        FrmDlg.DBAcus1Click(FrmDlg.DBAcus2, 0, 0);
                            //        break;
                            //    case 2:
                            //        FrmDlg.DBAcus1Click(FrmDlg.DBAcus3, 0, 0);
                            //        break;
                            //    case 3:
                            //        FrmDlg.DBAcus1Click(FrmDlg.DBAcus4, 0, 0);
                            //        break;
                            //    case 4:
                            //        FrmDlg.DBAcus1Click(FrmDlg.DBAcus5, 0, 0);
                            //        break;
                            //    case 5:
                            //        FrmDlg.DBAcus1Click(FrmDlg.DBCharm1, 0, 0);
                            //        break;
                            //    case 6:
                            //        FrmDlg.DBAcus1Click(FrmDlg.DBCharm2, 0, 0);
                            //        break;
                            //    case 7:
                            //        FrmDlg.DBAcus1Click(FrmDlg.DBCharm3, 0, 0);
                            //        break;
                            //}
                            return;
                        }
                        where = ClFunc.GetTakeOnPosition(MShare.g_ItemArr[idx], MShare.g_UseItems, true);
                        if (where >= 0 && where <= 13)
                        {
                            takeon = true;
                            MShare.g_EatingItem = MShare.g_ItemArr[idx];
                            MShare.g_ItemArr[idx].Item.Name = "";
                        }
                    }
                }
            }
            else if ((idx == -1) && MShare.g_boItemMoving)
            {
                if ((MShare.g_MovingItem.Item.Item.StdMode <= 4) || (MShare.g_MovingItem.Item.Item.StdMode == 31) && (MShare.g_MovingItem.Item.Item.NeedIdentify < 4))
                {
                    if (((MShare.g_MovingItem.Item.Item.StdMode <= 3) || (MShare.g_MovingItem.Item.Item.StdMode == 31)) && (MShare.g_MovingItem.Item.Dura > 1))
                    {
                        MShare.g_MovingItem.Item.Dura = (ushort)(MShare.g_MovingItem.Item.Dura - 1);
                        MShare.g_boItemMoving = false;
                        MShare.g_EatingItem = MShare.g_MovingItem.Item;
                        MShare.g_MovingItem.Item.Item.Name = "";
                    }
                    else
                    {
                        MShare.g_boItemMoving = false;
                        MShare.g_EatingItem = MShare.g_MovingItem.Item;
                        MShare.g_MovingItem.Item.Item.Name = "";
                    }
                    if ((MShare.g_EatingItem.Item.StdMode == 4) && (MShare.g_EatingItem.Item.Shape < 50))
                    {
                        MainOutMessage($"练习{MShare.g_EatingItem.Item.Name}技能");
                        ClFunc.AddItemBag(MShare.g_EatingItem);
                        return;
                    }
                    idx = m_nEatRetIdx;
                    eatable = true;
                }
                else
                {
                lab2:
                    if ((MShare.g_MovingItem.Item.Item.StdMode == 46) && MShare.g_MovingItem.Item.Item.Shape >= 2 && MShare.g_MovingItem.Item.Item.Shape <= 6)
                    {
                        //if (!MShare.g_RareBoxWindow.m_boKeyAvail && (MShare.g_OpenBoxItem.Item.Item.Name == "") && !FrmDlg.DWBoxBKGnd.Visible)
                        //{
                        //    MShare.g_OpenBoxItem.Index = m_nEatRetIdx;
                        //    MShare.g_OpenBoxItem.Item = MShare.g_MovingItem.Item;
                        //    MShare.g_boItemMoving = false;
                        //    MShare.g_MovingItem.Item.Item.Name = "";
                        //}
                        return;
                    }
                    if ((MShare.g_MovingItem.Item.Item.StdMode == 41) && new ArrayList(new int[] { 10, 30 }).Contains(MShare.g_MovingItem.Item.Item.Shape) && (MShare.g_BuildAcusesStep != 1))
                    {
                        for (i = 0; i < 7; i++)
                        {
                            if (MShare.g_BuildAcuses[i].Item.Item.Name == "")
                            {
                                if ((MShare.g_MovingItem.Item.Item.Shape >= 30 && MShare.g_MovingItem.Item.Item.Shape <= 34 && i >= 5 && i <= 7) || (MShare.g_MovingItem.Item.Item.Shape >= 10 && MShare.g_MovingItem.Item.Item.Shape <= 14 && i >= 0 && i <= 4))
                                {
                                    break;
                                }
                            }
                        }
                        switch (i)
                        {
                            //case 0:
                            //    FrmDlg.DBAcus1Click(FrmDlg.DBAcus1, 0, 0);
                            //    break;
                            //case 1:
                            //    FrmDlg.DBAcus1Click(FrmDlg.DBAcus2, 0, 0);
                            //    break;
                            //case 2:
                            //    FrmDlg.DBAcus1Click(FrmDlg.DBAcus3, 0, 0);
                            //    break;
                            //case 3:
                            //    FrmDlg.DBAcus1Click(FrmDlg.DBAcus4, 0, 0);
                            //    break;
                            //case 4:
                            //    FrmDlg.DBAcus1Click(FrmDlg.DBAcus5, 0, 0);
                            //    break;
                            //case 5:
                            //    FrmDlg.DBAcus1Click(FrmDlg.DBCharm1, 0, 0);
                            //    break;
                            //case 6:
                            //    FrmDlg.DBAcus1Click(FrmDlg.DBCharm2, 0, 0);
                            //    break;
                            //case 7:
                            //    FrmDlg.DBAcus1Click(FrmDlg.DBCharm3, 0, 0);
                            //    break;
                        }
                        return;
                    }

                    where = ClFunc.GetTakeOnPosition(MShare.g_MovingItem.Item, MShare.g_UseItems, true);
                    if (where >= 0 && where <= 13)
                    {
                        takeon = true;
                        MShare.g_boItemMoving = false;
                        MShare.g_EatingItem = MShare.g_MovingItem.Item;
                        MShare.g_MovingItem.Item.Item.Name = "";
                        idx = m_nEatRetIdx;
                    }
                }
            }
            if (eatable)
            {
                m_nEatRetIdx = idx;
                m_boSupplyItem = true;
                MShare.g_dwEatTime = MShare.GetTickCount();
                SendEat(MShare.g_EatingItem.MakeIndex, MShare.g_EatingItem.Item.Name, MShare.g_EatingItem.Item.StdMode);
            }
            else if (takeon)
            {
                m_nEatRetIdx = idx;
                MShare.g_dwEatTime = MShare.GetTickCount();
                MShare.g_WaitingUseItem.Item = MShare.g_EatingItem;
                MShare.g_WaitingUseItem.Index = where;
                SendTakeOnItem(where, MShare.g_EatingItem.MakeIndex, MShare.g_EatingItem.Item.Name);
                MShare.g_EatingItem.Item.Name = "";
            }
        }

        private bool TargetInSwordLongAttackRange(int ndir)
        {
            short nX = 0;
            short nY = 0;
            var result = false;
            if (MShare.g_gcTec[0])
            {
                return true;
            }
            ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, ndir, ref nX, ref nY);
            ClFunc.GetFrontPosition(nX, nY, ndir, ref nX, ref nY);
            if ((Math.Abs(MShare.g_MySelf.m_nCurrX - nX) == 2) || (Math.Abs(MShare.g_MySelf.m_nCurrY - nY) == 2))
            {
                var Actor = g_PlayScene.FindActorXY(nX, nY);
                if (Actor != null)
                {
                    if (!Actor.m_boDeath)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool TargetInSwordLongAttackRange2(int sx, int sy, int dx, int dy)
        {
            var result = false;
            if ((Math.Abs(sx - dx) == 2) && (Math.Abs(sy - dy) == 0))
            {
                result = true;
                return result;
            }
            if ((Math.Abs(sx - dx) == 0) && (Math.Abs(sy - dy) == 2))
            {
                result = true;
                return result;
            }
            if ((Math.Abs(sx - dx) == 2) && (Math.Abs(sy - dy) == 2))
            {
                result = true;
                return result;
            }
            return result;
        }

        private bool TargetInSwordWideAttackRange(int ndir)
        {
            short nX = 0;
            short nY = 0;
            short rx = 0;
            short ry = 0;
            var result = false;
            ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, ndir, ref nX, ref nY);
            var Actor = g_PlayScene.FindActorXY(nX, nY);
            var mdir = (ndir + 1) % 8;
            ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, mdir, ref rx, ref ry);
            var ractor = g_PlayScene.FindActorXY(rx, ry);
            if (ractor == null)
            {
                mdir = (ndir + 2) % 8;
                ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, mdir, ref rx, ref ry);
                ractor = g_PlayScene.FindActorXY(rx, ry);
            }
            if (ractor == null)
            {
                mdir = (ndir + 7) % 8;
                ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, mdir, ref rx, ref ry);
                ractor = g_PlayScene.FindActorXY(rx, ry);
            }
            if ((Actor != null) && (ractor != null))
            {
                if (!Actor.m_boDeath && !ractor.m_boDeath)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool TargetInSwordLongAttackRangeX(int ndir)
        {
            short nX = 0;
            short nY = 0;
            short nC = 1;
            var result = false;
            while (true)
            {
                if (GetNextPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, ndir, nC, ref nX, ref nY))
                {
                    var Actor = g_PlayScene.FindActorXY(nX, nY);
                    if ((Actor != null) && !Actor.m_boDeath)
                    {
                        result = true;
                        break;
                    }
                }
                nC++;
                if (nC >= 5)
                {
                    break;
                }
            }
            return result;
        }

        public bool TargetInSwordLongAttackRangeA(int ndir)
        {
            short nC = 0;
            short nX = 0;
            short nY = 0;
            TActor Actor;
            var result = false;
            nC = 1;
            while (true)
            {
                if (GetNextPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, ndir, nC, ref nX, ref nY))
                {
                    Actor = g_PlayScene.FindActorXY(nX, nY);
                    if ((Actor != null) && !Actor.m_boDeath)
                    {
                        result = true;
                        break;
                    }
                }
                nC++;
                if (nC >= 4)
                {
                    break;
                }
            }
            return result;
        }

        public bool TargetInSwordCrsAttackRange(int ndir)
        {
            short nX = 0;
            short nY = 0;
            short rx = 0;
            short ry = 0;
            var result = false;
            ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, ndir, ref nX, ref nY);
            var Actor = g_PlayScene.FindActorXY(nX, nY);
            var mdir = (ndir + 1) % 8;
            ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, mdir, ref rx, ref ry);
            var ractor = g_PlayScene.FindActorXY(rx, ry);
            if (ractor == null)
            {
                mdir = (ndir + 2) % 8;
                ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, mdir, ref rx, ref ry);
                ractor = g_PlayScene.FindActorXY(rx, ry);
            }
            if (ractor == null)
            {
                mdir = (ndir + 7) % 8;
                ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, mdir, ref rx, ref ry);
                ractor = g_PlayScene.FindActorXY(rx, ry);
            }
            if ((Actor != null) && (ractor != null))
            {
                if (!Actor.m_boDeath && !ractor.m_boDeath)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool AttackTarget(TActor target)
        {
            var result = false;
            var nHitMsg = Grobal2.CM_HIT;
            if (MShare.g_UseItems[Grobal2.U_WEAPON] != null && MShare.g_UseItems[Grobal2.U_WEAPON].Item.StdMode == 6)
            {
                nHitMsg = Grobal2.CM_HEAVYHIT;
            }
            int tdir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, target.m_nCurrX, target.m_nCurrY);
            if ((Math.Abs(MShare.g_MySelf.m_nCurrX - target.m_nCurrX) <= 1) && (Math.Abs(MShare.g_MySelf.m_nCurrY - target.m_nCurrY) <= 1) && (!target.m_boDeath))
            {
                if (TimerAutoPlay.Enabled)
                {
                    MShare.g_boAPAutoMove = false;
                    if (MShare.g_APTagget != null)
                    {
                        MShare.g_sAPStr = $"怪物目标：{MShare.g_APTagget.m_sUserName} ({MShare.g_APTagget.m_nCurrX},{MShare.g_APTagget.m_nCurrY}) 正在使用普通攻击";
                    }
                }
                if (CanNextAction() && ServerAcceptNextAction())
                {
                    if (CanNextHit(false) || MShare.g_NextSeriesSkill)
                    {
                        MShare.g_NextSeriesSkill = false;
                    }
                    if (CanNextHit())
                    {
                        if (MShare.g_boNextTimeFireHit && (MShare.g_MySelf.m_Abil.MP >= 7))
                        {
                            MShare.g_boNextTimeFireHit = false;
                            nHitMsg = Grobal2.CM_FIREHIT;
                        }
                        else if (MShare.g_boNextTimePowerHit)
                        {
                            MShare.g_boNextTimePowerHit = false;
                            nHitMsg = Grobal2.CM_POWERHIT;
                        }
                        else if ((MShare.g_MySelf.m_Abil.MP >= 3) && (MShare.g_boCanWideHit || (MShare.g_gcTec[1] && (GetMagicByID(25) != null) && TargetInSwordWideAttackRange(tdir))))
                        {
                            nHitMsg = Grobal2.CM_WIDEHIT;
                        }
                        else if (MShare.g_boCanCrsHit && (MShare.g_MySelf.m_Abil.MP >= 6))
                        {
                            nHitMsg = Grobal2.CM_CRSHIT;
                        }
                        else if (MShare.g_boCanLongHit && TargetInSwordLongAttackRange(tdir))
                        {
                            nHitMsg = Grobal2.CM_LONGHIT;
                        }
                        MShare.g_MySelf.SendMsg(nHitMsg, (ushort)MShare.g_MySelf.m_nCurrX, (ushort)MShare.g_MySelf.m_nCurrY, tdir, 0, 0, "", 0);
                    }
                }
                result = true;
                MShare.g_dwLastAttackTick = MShare.GetTickCount();
            }
            else
            {
                if (MShare.g_boCanLongHit && (MShare.g_MySelf.m_btJob == 0) && (!target.m_boDeath) && MShare.g_boAutoLongAttack && MShare.g_gcTec[10] && (MShare.g_MagicArr[12] != null) && TargetInSwordLongAttackRange2(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, target.m_nCurrX, target.m_nCurrY))
                {
                    if (CanNextAction() && ServerAcceptNextAction() && CanNextHit())
                    {
                        nHitMsg = Grobal2.CM_LONGHIT;
                        MShare.g_MySelf.SendMsg(nHitMsg, (ushort)MShare.g_MySelf.m_nCurrX, (ushort)MShare.g_MySelf.m_nCurrY, tdir, 0, 0, "", 0);
                        MShare.g_dwLastAttackTick = MShare.GetTickCount();
                    }
                    else if (MShare.g_boAutoLongAttack && MShare.g_gcTec[10] && TimerAutoPlay.Enabled)// 走刺杀位
                    {
                        result = true;
                        return result;
                    }
                }
                else
                {
                    var dx = MShare.g_MySelf.m_nCurrX;
                    var dy = MShare.g_MySelf.m_nCurrY;
                    if ((MShare.g_MySelf.m_btJob == 0) && MShare.g_boAutoLongAttack && MShare.g_gcTec[10] && (MShare.g_MagicArr[12] != null))
                    {
                        ClFunc.GetNextHitPosition(target.m_nCurrX, target.m_nCurrY, ref dx, ref dy);
                        if (!g_PlayScene.CanWalk(dx, dy))
                        {
                            ClFunc.GetBackPosition(target.m_nCurrX, target.m_nCurrY, tdir, ref dx, ref dy);
                        }
                    }
                    else
                    {
                        ClFunc.GetBackPosition(target.m_nCurrX, target.m_nCurrY, tdir, ref dx, ref dy);
                    }
                    MShare.g_nTargetX = dx;
                    MShare.g_nTargetY = dy;
                    MShare.g_ChrAction = TChrAction.caRun;
                }
                if (TimerAutoPlay.Enabled)
                {
                    MShare.g_boAPAutoMove = true;
                    MShare.g_sAPStr = $"怪物目标：{target.m_sUserName} ({target.m_nCurrX},{target.m_nCurrY}) 正在跑向";
                }
            }
            return result;
        }

        private bool CheckDoorAction(int dx, int dy)
        {
            var result = false;
            var door = Map.GetDoor(dx, dy);
            if (door > 0)
            {
                if (!Map.IsDoorOpen(dx, dy))
                {
                    SendClientMessage(Grobal2.CM_OPENDOOR, door, dx, dy, 0);
                    result = true;
                }
            }
            return result;
        }

        public void MouseTimerTimer(object Sender, EventArgs _e1)
        {
            int ii;
            int fixidx;
            TActor target;
            if ((MShare.g_gcGeneral[1] || MShare.g_gcGeneral[9]) && (MShare.GetTickCount() - m_dwDuraWarningTick > 60 * 1000))
            {
                m_dwDuraWarningTick = MShare.GetTickCount();
                if ((MShare.g_MySelf != null) && !MShare.g_MySelf.m_boDeath)
                {
                    for (var i = MShare.g_UseItems.Length; i > 0; i--)
                    {
                        if (MShare.g_UseItems[i].Item.Name != "")
                        {
                            if (MShare.g_UseItems[i].Item.StdMode == 7 || MShare.g_UseItems[i].Item.StdMode == 25)
                            {
                                continue;
                            }
                            if (MShare.g_UseItems[i].Dura < 1500)
                            {
                                if (MShare.g_gcGeneral[1])
                                {
                                    DScreen.AddChatBoardString($"你的[{MShare.g_UseItems[i].Item.Name}]持久已到底限，请及时修理！", ConsoleColor.Green, ConsoleColor.Black);
                                }
                                if (MShare.g_gcGeneral[9])
                                {
                                    fixidx = -1;
                                    for (ii = Grobal2.MAXBAGITEM - (1 + 0); ii >= 0; ii--)
                                    {
                                        if ((MShare.g_ItemArr[ii].Item.NeedIdentify < 4) && (MShare.g_ItemArr[ii].Item.Name != "") && (MShare.g_ItemArr[ii].Item.StdMode == 2) && (MShare.g_ItemArr[ii].Item.Shape == 9) && (MShare.g_ItemArr[ii].Dura > 0))
                                        {
                                            fixidx = ii;
                                            break;
                                        }
                                    }
                                    if (fixidx > -1)
                                    {
                                        EatItem(fixidx);
                                    }
                                    else
                                    {
                                        DScreen.AddChatBoardString($"你的{MShare.g_UseItems[i].Item.Name}已经用完，请及时补充！", ConsoleColor.Green, ConsoleColor.Black);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if ((MShare.g_MySelf != null) && !MShare.g_MySelf.m_boDeath && (MShare.g_MySelf.m_nIPowerLvl > 5) && (MShare.g_MySelf.m_nIPower < 30) && (MShare.GetTickCount() - dwIPTick > 30 * 1000))
            {
                dwIPTick = MShare.GetTickCount();
                fixidx = -1;
                for (ii = Grobal2.MAXBAGITEM - (1 + 0); ii >= 0; ii--)
                {
                    if ((MShare.g_ItemArr[ii].Item.NeedIdentify < 4) && (MShare.g_ItemArr[ii].Item.Name != "") && (MShare.g_ItemArr[ii].Item.StdMode == 2) && (MShare.g_ItemArr[ii].Item.Shape == 13) && (MShare.g_ItemArr[ii].DuraMax > 0))
                    {
                        fixidx = ii;
                        break;
                    }
                }
                if (fixidx > -1)
                {
                    EatItem(fixidx);
                }
            }
            if (MShare.g_TargetCret != null)
            {
                if (ActionKey > 0)
                {
                    ProcessKeyMessages();
                }
                else
                {
                    MShare.g_TargetCret = null;
                }
            }
            if ((MShare.g_MySelf != null) && (MShare.g_boAutoDig || MShare.g_boAutoSit))
            {
                if (CanNextAction() && ServerAcceptNextAction() && (MShare.g_boAutoSit || CanNextHit()))
                {
                    if (MShare.g_boAutoDig)
                    {
                        MShare.g_MySelf.SendMsg(Grobal2.CM_HIT + 1, (ushort)MShare.g_MySelf.m_nCurrX, (ushort)MShare.g_MySelf.m_nCurrY, MShare.g_MySelf.m_btDir, 0, 0, "", 0);
                    }
                    if (MShare.g_boAutoSit)
                    {
                        target = g_PlayScene.ButchAnimal(MShare.g_nMouseCurrX, MShare.g_nMouseCurrY);
                        if (target != null)
                        {
                            ii = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nMouseCurrX, MShare.g_nMouseCurrY);
                            SendButchAnimal(MShare.g_nMouseCurrX, MShare.g_nMouseCurrY, ii, target.m_nRecogId);
                            MShare.g_MySelf.SendMsg(Grobal2.CM_SITDOWN, (ushort)MShare.g_MySelf.m_nCurrX, (ushort)MShare.g_MySelf.m_nCurrY, ii, 0, 0, "", 0);
                        }
                        else
                        {
                            ii = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nMouseCurrX, MShare.g_nMouseCurrY);
                            SendButchAnimal(MShare.g_nMouseCurrX, MShare.g_nMouseCurrY, ii, MShare.g_DetectItemMineID);
                            MShare.g_MySelf.SendMsg(Grobal2.CM_SITDOWN, (ushort)MShare.g_MySelf.m_nCurrX, (ushort)MShare.g_MySelf.m_nCurrY, ii, 0, 0, "", 0);
                        }
                    }
                }
            }
            if (MShare.g_boAutoPickUp && (MShare.g_MySelf != null) && (MShare.GetTickCount() - MShare.g_dwAutoPickupTick) > MShare.g_dwAutoPickupTime)// 动自捡取
            {
                MShare.g_dwAutoPickupTick = MShare.GetTickCount();
                AutoPickUpItem();
            }
        }

        private void AutoPickUpItem()
        {
            if (ServerAcceptNextAction())
            {
                var DropItem = g_PlayScene.GetXyDropItems(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY);
                if (DropItem != null)
                {
                    if (MShare.g_boPickUpAll || DropItem.boPickUp)
                    {
                        SendPickup();
                    }
                }
            }
        }

        public void WaitMsgTimerTimer(object Sender, EventArgs _e1)
        {
            if (MShare.g_MySelf == null)
            {
                return;
            }
            if (MShare.g_MySelf.ActionFinished())
            {
                //WaitMsgTimer.Enabled = false;
                switch (WaitingMsg.Ident)
                {
                    case Grobal2.SM_CHANGEMAP:
                        MShare.g_boMapMovingWait = false;
                        MShare.g_boMapMoving = false;
                        if (MShare.g_nStallX != -1)
                        {
                            MShare.g_nStallX = -1;
                        }
                        ClearDropItems();
                        g_PlayScene.CleanObjects();
                        MShare.g_sMapTitle = "";
                        g_PlayScene.SendMsg(Grobal2.SM_CHANGEMAP, 0, WaitingMsg.Param, WaitingMsg.Tag, (byte)WaitingMsg.Series, 0, 0, WaitingStr);
                        MShare.g_MySelf.CleanCharMapSetting((short)WaitingMsg.Param, (short)WaitingMsg.Tag);
                        MShare.g_nTargetX = -1;
                        MShare.g_TargetCret = null;
                        MShare.g_FocusCret = null;
                        break;
                }
            }
        }

        public void ActiveCmdTimer(TTimerCommand cmd)
        {
            //TimerCmd = cmd;
            //CmdTimer.Enabled = true;
        }

        public void CmdTimerTimer(object Sender, EventArgs _e1)
        {
            //CmdTimer.Enabled = false;
            //CmdTimer.Interval = 500;
            //switch (TimerCmd)
            //{
            //    case MShare.TTimerCommand.tcSoftClose:
            //        CSocket.Socket.Close;
            //        while (true)
            //        {
            //            if (!CSocket.Socket.Connected)
            //            {
            //                CmdTimer.Interval = 100;
            //                ActiveCmdTimer(MShare.TTimerCommand.tcReSelConnect);
            //                break;
            //            }
            //            if (Application.Terminated)
            //            {
            //                break;
            //            }
            //            WaitAndPass(10);
            //        }
            //        break;
            //    case MShare.TTimerCommand.tcReSelConnect:
            //        ResetGameVariables();
            //        this.Active = false;
            //        while (true)
            //        {
            //            if (!CSocket.Socket.Connected)
            //            {
            //                DScreen.ChangeScene(SceneType.stSelectChr);
            //                if (!MShare.g_boDoFadeOut && !MShare.g_boDoFadeIn)
            //                {
            //                    MShare.g_boDoFadeIn = true;
            //                    MShare.g_nFadeIndex = 0;
            //                }
            //                MShare.g_ConnectionStep = TConnectionStep.cnsReSelChr;
            //                MShare.g_boQuerySelChar = true;
            //                CSocket.Address = MShare.g_sSelChrAddr;
            //                CSocket.Port = MShare.g_nSelChrPort;
            //                this.Active = true;
            //                break;
            //            }
            //            if (Application.Terminated)
            //            {
            //                break;
            //            }
            //            WaitAndPass(10);
            //        }
            //        break;
            //    case MShare.TTimerCommand.tcFastQueryChr:
            //        SendQueryChr();
            //        break;
            //}
        }

        private void CloseAllWindows()
        {
            //SaveWayPoint();
            MShare.g_gcAss[0] = false;
            MShare.g_APTagget = null;
            MShare.g_AutoPicupItem = null;
            MShare.g_nAPStatus = -1;
            MShare.g_nTargetX = -1;
            MShare.g_boCanRunSafeZone = true;
            MShare.g_nEatIteminvTime = 200;
            MShare.g_SendSayListIdx = 0;
            MShare.g_SendSayList.Clear();
            MShare.ResetSeriesSkillVar();
            if (MShare.g_nStallX != -1)
            {
                MShare.g_nStallX = -1;
            }
            MShare.g_boItemMoving = false;
        }

        private void ClearDropItems()
        {
            for (var i = 0; i < MShare.g_DropedItemList.Count; i++)
            {
                MShare.g_DropedItemList[i] = null;
            }
            MShare.g_DropedItemList.Clear();
        }

        private void ResetGameVariables()
        {
            CloseAllWindows();
            ClearDropItems();
            //if (MShare.g_RareBoxWindow != null)
            //{
            //    MShare.g_RareBoxWindow.Initialize();
            //}
            //for (i = Low(FrmDlg.m_MissionList); i <= High(FrmDlg.m_MissionList); i++)
            //{
            //    List = FrmDlg.m_MissionList[i];
            //    for (ii = 0; ii < List.Count; ii++)
            //    {
            //        this.Dispose((TClientMission)List[ii]);
            //    }
            //    List.Clear();
            //}
            //for (i = 0; i < MShare.g_MagicList.Count; i++)
            //{
            //    this.Dispose((TClientMagic)MShare.g_MagicList[i]);
            //}
            //MShare.g_MagicList.Clear();
            //for (i = 0; i < MShare.g_IPMagicList.Count; i++)
            //{
            //    this.Dispose((TClientMagic)MShare.g_IPMagicList[i]);
            //}
            //MShare.g_IPMagicList.Clear();
            //for (i = 0; i < MShare.g_HeroMagicList.Count; i++)
            //{
            //    this.Dispose((TClientMagic)MShare.g_HeroMagicList[i]);
            //}
            //MShare.g_HeroMagicList.Clear();
            //for (i = 0; i < MShare.g_HeroIPMagicList.Count; i++)
            //{
            //    this.Dispose((TClientMagic)MShare.g_HeroIPMagicList[i]);
            //}
            //MShare.g_HeroIPMagicList.Clear();
            //for (i = MShare.g_ShopListArr.GetLowerBound(0); i <= MShare.g_ShopListArr..Length; i++)
            //{
            //    List = MShare.g_ShopListArr[i];
            //    for (ii = 0; ii < List.Count; ii++)
            //    {
            //        this.Dispose((TShopItem)List[ii]);
            //    }
            //    List.Clear();
            //}
            MShare.g_boItemMoving = false;
            MShare.g_DetectItem.Item.Name = "";
            MShare.g_WaitingUseItem.Item.Item.Name = "";
            MShare.g_WaitingStallItem.Item.Item.Name = "";
            MShare.g_WaitingDetectItem.Item.Item.Name = "";
            MShare.g_OpenBoxItem.Item.Item.Name = "";
            MShare.g_EatingItem.Item.Name = "";
            MShare.g_nLastMapMusic = -1;
            MShare.g_nTargetX = -1;
            MShare.g_TargetCret = null;
            MShare.g_FocusCret = null;
            MShare.g_MagicTarget = null;
            ActionLock = false;
            m_boSupplyItem = false;
            m_nEatRetIdx = -1;
            MShare.g_GroupMembers.Clear();
            MShare.g_sGuildRankName = "";
            MShare.g_sGuildName = "";
            MShare.g_boMapMoving = false;
            //WaitMsgTimer.Enabled = false;
            MShare.g_boMapMovingWait = false;
            MShare.g_boNextTimePowerHit = false;
            MShare.g_boCanLongHit = false;
            MShare.g_boCanWideHit = false;
            MShare.g_boCanCrsHit = false;
            MShare.g_boCanSquHit = false;
            MShare.g_boNextTimeFireHit = false;
            MShare.g_boCanSLonHit = false;
            MShare.g_boNextTimeTwinHit = false;
            MShare.g_boNextTimePursueHit = false;
            MShare.g_boNextTimeSmiteHit = false;
            MShare.g_boNextTimeRushHit = false;
            MShare.g_boNextTimeSmiteLongHit = false;
            MShare.g_boNextTimeSmiteLongHit3 = false;
            MShare.g_boNextTimeSmiteLongHit2 = false;
            MShare.g_boNextTimeSmiteWideHit = false;
            MShare.g_boNextTimeSmiteWideHit2 = false;
            MShare.InitClientItems();
            MShare.g_DetectItemMineID = 0;
            g_PlayScene.ClearActors();
            ClearDropItems();
            //EventMan.ClearEvents();
            g_PlayScene.CleanObjects();
            //MaketSystem.Units.MaketSystem.g_Market.Clear();
        }

        private void ChangeServerClearGameVariables()
        {
            CloseAllWindows();
            ClearDropItems();
            //for (i = Low(FrmDlg.m_MissionList); i <= High(FrmDlg.m_MissionList); i++)
            //{
            //    List = FrmDlg.m_MissionList[i];
            //    for (ii = 0; ii < List.Count; ii++)
            //    {
            //        this.Dispose((TClientMission)List[ii]);
            //    }
            //    List.Clear();
            //}
            //for (i = 0; i < MShare.g_MagicList.Count; i++)
            //{
            //    this.Dispose((TClientMagic)MShare.g_MagicList[i]);
            //}
            //MShare.g_MagicList.Clear();
            //for (i = 0; i < MShare.g_IPMagicList.Count; i++)
            //{
            //    this.Dispose((TClientMagic)MShare.g_IPMagicList[i]);
            //}
            //MShare.g_IPMagicList.Clear();
            //FillChar(MShare.g_MagicArr, sizeof(MShare.g_MagicArr), 0);
            //for (i = 0; i < MShare.g_HeroIPMagicList.Count; i++)
            //{
            //    this.Dispose((TClientMagic)MShare.g_HeroIPMagicList[i]);
            //}
            //MShare.g_HeroIPMagicList.Clear();
            //for (i = MShare.g_ShopListArr.GetLowerBound(0); i <= MShare.g_ShopListArr..Length; i++)
            //{
            //    List = MShare.g_ShopListArr[i];
            //    for (ii = 0; ii < List.Count; ii++)
            //    {
            //        this.Dispose((TShopItem)List[ii]);
            //    }
            //    List.Clear();
            //}
            MShare.g_boItemMoving = false;
            MShare.g_DetectItem.Item.Name = "";
            MShare.g_WaitingUseItem.Item.Item.Name = "";
            MShare.g_WaitingStallItem.Item.Item.Name = "";
            MShare.g_WaitingDetectItem.Item.Item.Name = "";
            MShare.g_OpenBoxItem.Item.Item.Name = "";
            MShare.g_EatingItem.Item.Name = "";
            MShare.g_nLastMapMusic = -1;
            MShare.g_nTargetX = -1;
            MShare.g_TargetCret = null;
            MShare.g_FocusCret = null;
            MShare.g_MagicTarget = null;
            ActionLock = false;
            m_boSupplyItem = false;
            m_nEatRetIdx = -1;
            MShare.g_GroupMembers.Clear();
            MShare.g_sGuildRankName = "";
            MShare.g_sGuildName = "";
            MShare.g_boMapMoving = false;
            //WaitMsgTimer.Enabled = false;
            MShare.g_boMapMovingWait = false;
            MShare.g_boNextTimePowerHit = false;
            MShare.g_boCanLongHit = false;
            MShare.g_boCanWideHit = false;
            MShare.g_boCanCrsHit = false;
            MShare.g_boCanSquHit = false;
            MShare.g_boNextTimeFireHit = false;
            MShare.g_boCanSLonHit = false;
            MShare.g_boNextTimeTwinHit = false;
            MShare.g_boNextTimePursueHit = false;
            MShare.g_boNextTimeSmiteHit = false;
            MShare.g_boNextTimeRushHit = false;
            MShare.g_boNextTimeSmiteLongHit = false;
            MShare.g_boNextTimeSmiteLongHit3 = false;
            MShare.g_boNextTimeSmiteLongHit2 = false;
            MShare.g_boNextTimeSmiteWideHit = false;
            MShare.g_boNextTimeSmiteWideHit2 = false;
            ClearDropItems();
            //EventMan.ClearEvents();
            g_PlayScene.CleanObjects();
        }

        private void SendSocket(string sendstr)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.SendText($"#1{sendstr}!");
            }
            else
            {
                MainOutMessage($"socket close {ClientSocket.Host}:{ClientSocket.Port}");
            }
        }

        public void CloseSocket()
        {
            if (ClientSocket != null)
            {
                ClientSocket.Disconnect();
            }
        }

        private void SendClientMessage(int msg, int Recog, int param, int tag, int series)
        {
            var dMsg = Grobal2.MakeDefaultMsg(msg, Recog, param, tag, series);
            SendSocket(EDCode.EncodeMessage(dMsg));
        }

        /// <summary>
        /// 发送登录消息
        /// </summary>
        private void SendRunLogin()
        {
            MainOutMessage("进入游戏");
            DScreen.CurrentScene.m_ConnectionStep = TConnectionStep.cnsPlay;
            var sSendMsg = $"**{LoginID}/{ChrName}/{Certification}/{Grobal2.CLIENT_VERSION_NUMBER}/{0}";
            SendSocket(EDCode.EncodeString(sSendMsg));
        }

        public void SendSay(string Str)
        {
            var sx = string.Empty;
            var sy = string.Empty;
            const string sam = "/move";
            if (!string.IsNullOrEmpty(Str))
            {
                if (HUtil32.CompareLStr(Str, sam))
                {
                    var param = Str.Substring(sam.Length, Str.Length - sam.Length);
                    if (param != "")
                    {
                        sy = HUtil32.GetValidStr3(param, ref sx, new string[] { " ", ":", ",", "\09" });
                        if ((sx != "") && (sy != ""))
                        {
                            short X = Convert.ToInt16(sx);
                            short Y = Convert.ToInt16(sy);
                            if ((X > 0) && (Y > 0))
                            {
                                MShare.g_MySelf.m_nTagX = X;
                                MShare.g_MySelf.m_nTagY = Y;
                                if (!g_PathBusy)
                                {
                                    g_PathBusy = true;
                                    try
                                    {
                                        Map.LoadMapData();
                                        TPathMap.g_MapPath = Map.FindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_MySelf.m_nTagX, MShare.g_MySelf.m_nTagY, 0);
                                        if (TPathMap.g_MapPath != null)
                                        {
                                            g_MoveStep = 1;
                                            //TimerAutoMove.Enabled = true;
                                            DScreen.AddChatBoardString(string.Format("自动移动至坐标({0}:{1})，点击鼠标任意键停止……", MShare.g_MySelf.m_nTagX, MShare.g_MySelf.m_nTagY), GetRGB(5));
                                        }
                                        else
                                        {
                                            //TimerAutoMove.Enabled = false;
                                            DScreen.AddChatBoardString(string.Format("自动移动坐标点({0}:{1})不可到达", MShare.g_MySelf.m_nTagX, MShare.g_MySelf.m_nTagY), GetRGB(5));
                                            MShare.g_MySelf.m_nTagX = 0;
                                            MShare.g_MySelf.m_nTagY = 0;
                                        }
                                    }
                                    finally
                                    {
                                        g_PathBusy = false;
                                    }
                                }
                            }
                        }
                    }
                    return;
                }
                var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_SAY, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(Str));
                if (Str[0] == '/')
                {
                    DScreen.AddChatBoardString(Str, GetRGB(180));
                    HUtil32.GetValidStr3(Str.Substring(2 - 1, Str.Length - 1), ref WhisperName, new string[] { " " });
                }
            }
        }

        /// <summary>
        /// 发送角色动作消息（走路 攻击等）
        /// </summary>
        private void SendActMsg(int ident, ushort X, ushort Y, int dir)
        {
            var msg = Grobal2.MakeDefaultMsg(ident, HUtil32.MakeLong(X, Y), 0, dir, 0);
            SendSocket(EDCode.EncodeMessage(msg));
            ActionLock = true;
            ActionLockTime = MShare.GetTickCount();
        }

        private void SendSpellMsg(int ident, ushort X, ushort Y, int dir, int target, bool bLock = false)
        {
            var msg = Grobal2.MakeDefaultMsg(ident, HUtil32.MakeLong(X, Y), HUtil32.LoWord(target), dir, HUtil32.HiWord(target));
            SendSocket(EDCode.EncodeMessage(msg));
            if (!bLock)
            {
                return;
            }
            ActionLock = true;
            ActionLockTime = MShare.GetTickCount();
        }

        public void SendQueryUserName(int targetid, int X, int Y)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_QUERYUSERNAME, targetid, X, Y, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendDropItem(string Name, int itemserverindex, int dropcnt)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_DROPITEM, itemserverindex, dropcnt, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(Name));
        }

        private void SendPickup()
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_PICKUP, 0, MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        private void SendTakeOnItem(int where, int itmindex, string itmname)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_TAKEONITEM, itmindex, where, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itmname));
        }

        public void SendTakeOffItem(byte where, int itmindex, string itmname)
        {
            ClientMesaagePacket msg;
            msg = Grobal2.MakeDefaultMsg(Grobal2.CM_TAKEOFFITEM, itmindex, where, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itmname));
        }

        private void SendEat(int itmindex, string itmname, int nUnBindItem)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_EAT, itmindex, 0, 0, nUnBindItem);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        private void SendButchAnimal(int X, int Y, int dir, int actorid)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_BUTCH, actorid, X, Y, dir);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendMagicKeyChange(int magid, char keych)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_MAGICKEYCHANGE, magid, (byte)keych, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        private void SendMerchantDlgSelect(int merchant, string rstr)
        {
            const string sam = "@_automove ";
            int X;
            int Y;
            ClientMesaagePacket msg;
            var param = string.Empty;
            var sx = string.Empty;
            var sy = string.Empty;
            var sM = string.Empty;
            if (rstr.Length >= 2)
            {
                if (HUtil32.CompareLStr(rstr, sam))
                {
                    param = rstr.Substring(sam.Length + 1 - 1, rstr.Length - sam.Length);
                    if (param != "")
                    {
                        param = HUtil32.GetValidStr3(param, ref sx, new string[] { " ", ":", ",", "\09" });
                        sM = HUtil32.GetValidStr3(param, ref sy, new string[] { " ", ":", ",", "\09" });
                        if ((sx != "") && (sy != ""))
                        {
                            if ((sM != "") && (string.Compare(MShare.g_sMapTitle, sM, StringComparison.OrdinalIgnoreCase) != 0))// 自动移动
                            {
                                DScreen.AddChatBoardString($"到达 {sM} 之后才能使用自动走路", ConsoleColor.Blue);
                                return;
                            }
                            X = Convert.ToInt32(sx);
                            Y = Convert.ToInt32(sy);
                            if ((X > 0) && (Y > 0))
                            {
                                MShare.g_MySelf.m_nTagX = (short)X;
                                MShare.g_MySelf.m_nTagY = (short)Y;
                                if (!g_PathBusy)
                                {
                                    g_PathBusy = true;
                                    try
                                    {
                                        Map.LoadMapData();
                                        TPathMap.g_MapPath = Map.FindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_MySelf.m_nTagX, MShare.g_MySelf.m_nTagY, 0);
                                        if (TPathMap.g_MapPath != null)
                                        {
                                            g_MoveStep = 1;
                                            TimerAutoMove.Enabled = true;
                                            DScreen.AddChatBoardString($"自动移动至坐标({MShare.g_MySelf.m_nTagX}:{MShare.g_MySelf.m_nTagY})，点击鼠标任意键停止……", GetRGB(5));
                                        }
                                        else
                                        {
                                            TimerAutoMove.Enabled = false;
                                            DScreen.AddChatBoardString($"自动移动坐标点({MShare.g_MySelf.m_nTagX}:{MShare.g_MySelf.m_nTagY})不可到达", GetRGB(5));
                                            MShare.g_MySelf.m_nTagX = 0;
                                            MShare.g_MySelf.m_nTagY = 0;
                                        }
                                    }
                                    finally
                                    {
                                        g_PathBusy = false;
                                    }
                                }
                            }
                        }
                    }
                    return;
                }
            }
            msg = Grobal2.MakeDefaultMsg(Grobal2.CM_MERCHANTDLGSELECT, merchant, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(rstr));
        }

        public void SendQueryPrice(int merchant, int itemindex, string itemname)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_MERCHANTQUERYSELLPRICE, merchant, HUtil32.LoWord(itemindex), HUtil32.HiWord(itemindex), 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendQueryRepairCost(int merchant, int itemindex, string itemname)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_MERCHANTQUERYREPAIRCOST, merchant, HUtil32.LoWord(itemindex), HUtil32.HiWord(itemindex), 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendSellItem(int merchant, int itemindex, string itemname, short count)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_USERSELLITEM, merchant, HUtil32.LoWord(itemindex), HUtil32.HiWord(itemindex), count);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendRepairItem(int merchant, int itemindex, string itemname)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_USERREPAIRITEM, merchant, HUtil32.LoWord(itemindex), HUtil32.HiWord(itemindex), 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendStorageItem(int merchant, int itemindex, string itemname, short count)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_USERSTORAGEITEM, merchant, HUtil32.LoWord(itemindex), HUtil32.HiWord(itemindex), count);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendGetDetailItem(int merchant, int menuindex, string itemname)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_USERGETDETAILITEM, merchant, menuindex, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendBuyItem(int merchant, int itemserverindex, string itemname, short conut)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_USERBUYITEM, merchant, HUtil32.LoWord(itemserverindex), HUtil32.HiWord(itemserverindex), conut);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendTakeBackStorageItem(int merchant, int itemserverindex, string itemname, short count)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_USERTAKEBACKSTORAGEITEM, merchant, HUtil32.LoWord(itemserverindex), HUtil32.HiWord(itemserverindex), count);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendMakeDrugItem(int merchant, string itemname)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_USERMAKEDRUGITEM, merchant, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendDropGold(int dropgold)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_DROPGOLD, dropgold, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendGroupMode(bool onoff)
        {
            ClientMesaagePacket msg;
            if (onoff)
            {
                msg = Grobal2.MakeDefaultMsg(Grobal2.CM_GROUPMODE, 0, 1, 0, 0);
            }
            else
            {
                msg = Grobal2.MakeDefaultMsg(Grobal2.CM_GROUPMODE, 0, 0, 0, 0);
            }
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendCreateGroup(string withwho)
        {
            if (withwho != "")
            {
                var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_CREATEGROUP, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(withwho));
            }
        }

        public void SendWantMiniMap()
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_WANTMINIMAP, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendGuildDlg()
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_OPENGUILDDLG, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendDealTry()
        {
            var who = string.Empty;
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_DEALTRY, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(who));
        }

        public void SendCancelDeal()
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_DEALCANCEL, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendAddDealItem(ClientItem ci)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_DEALADDITEM, ci.MakeIndex, 0, 0, ci.Dura);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(ci.Item.Name));
        }

        public void SendDelDealItem(ClientItem ci)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_DEALDELITEM, ci.MakeIndex, 0, 0, ci.Dura);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(ci.Item.Name));
        }

        public void SendChangeDealGold(int gold)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_DEALCHGGOLD, gold, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendDealEnd()
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_DEALEND, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendAddGroupMember(string withwho)
        {
            if (withwho != "")
            {
                var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_ADDGROUPMEMBER, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(withwho));
            }
        }

        public void SendDelGroupMember(string withwho)
        {
            if (withwho != "")
            {
                var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_DELGROUPMEMBER, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(withwho));
            }
        }

        public void SendGuildHome()
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_GUILDHOME, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        private void SendGuildMemberList()
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_GUILDMEMBERLIST, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendGuildAddMem(string who)
        {
            if (who.Trim() != "")
            {
                var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_GUILDADDMEMBER, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(who));
            }
        }

        public void SendGuildDelMem(string who)
        {
            if (who.Trim() != "")
            {
                var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_GUILDDELMEMBER, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(who));
            }
        }

        public void SendGuildUpdateNotice(string notices)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_GUILDUPDATENOTICE, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(notices));
        }

        public void SendGuildUpdateGrade(string rankinfo)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_GUILDUPDATERANKINFO, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(rankinfo));
        }

        public void SendSpeedHackUser()
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_SPEEDHACKUSER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendAdjustBonus(int remain, NakedAbility babil)
        {
            var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_ADJUST_BONUS, remain, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeBuffer(babil));
        }

        public void SendFireSerieSkill()
        {
            if (MShare.g_MySelf == null)
            {
                return;
            }
            if (MShare.g_SeriesSkillFire)
            {
                return;
            }
            if (MShare.g_MySelf.m_boUseCboLib)
            {
                return;
            }
            if (MShare.g_MySelf.m_nIPower < 5)
            {
                if (MShare.GetTickCount() - MShare.g_IPointLessHintTick > 10000)
                {
                    MShare.g_IPointLessHintTick = MShare.GetTickCount();
                    DScreen.AddSysMsg("内力值不足...");
                }
                return;
            }
            if (((MShare.g_MySelf.m_nState & 0x04000000) == 0) && ((MShare.g_MySelf.m_nState & 0x02000000) == 0))
            {
                if (MShare.GetTickCount() - MShare.g_SendFireSerieSkillTick > 1000)
                {
                    MShare.g_SendFireSerieSkillTick = MShare.GetTickCount();
                    //var msg = Grobal2.MakeDefaultMsg(Grobal2.CM_FIRESERIESSKILL, MShare.g_MySelf.m_nRecogId, 0, 0, 0);
                    //SendSocket(EDcode.EncodeMessage(msg));
                }
            }
        }

        public bool ServerAcceptNextAction()
        {
            if ((MShare.g_MySelf != null) && MShare.g_MySelf.m_StallMgr.OnSale)
            {
                return false;
            }
            var result = true;
            if (ActionLock)
            {
                if ((MShare.GetTickCount() - ActionLockTime) > 5 * 1000)
                {
                    ActionLock = false;
                }
                result = false;
            }
            return result;
        }

        public bool CanNextAction()
        {
            if ((MShare.g_MySelf != null) && MShare.g_MySelf.m_StallMgr.OnSale)
            {
                return false;
            }
            if (!MShare.g_MySelf.m_boUseCboLib && MShare.g_MySelf.IsIdle() && ((MShare.g_MySelf.m_nState & 0x04000000) == 0) && ((MShare.g_MySelf.m_nState & 0x02000000) == 0)
                && (MShare.GetTickCount() - MShare.g_dwDizzyDelayStart > MShare.g_dwDizzyDelayTime))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否可以攻击，控制攻击速度
        /// </summary>
        /// <param name="settime"></param>
        /// <returns></returns>
        private bool CanNextHit(bool settime = false)
        {
            bool result;
            int NextHitTime;
            int LevelFastTime;
            if ((MShare.g_MySelf != null) && MShare.g_MySelf.m_StallMgr.OnSale)
            {
                return false;
            }
            LevelFastTime = HUtil32._MIN(370, MShare.g_MySelf.m_Abil.Level * 14);
            LevelFastTime = HUtil32._MIN(800, LevelFastTime + MShare.g_MySelf.m_nHitSpeed * MShare.g_nItemSpeed);
            if (MShare.g_boSpeedRate)
            {
                if (MShare.g_MySelf.m_boAttackSlow)
                {
                    NextHitTime = MShare.g_nHitTime - LevelFastTime + 1500 - MShare.g_HitSpeedRate * 20; // 腕力超过时，减慢攻击速度
                }
                else
                {
                    NextHitTime = MShare.g_nHitTime - LevelFastTime - MShare.g_HitSpeedRate * 20;
                }
            }
            else
            {
                if (MShare.g_MySelf.m_boAttackSlow)
                {
                    NextHitTime = MShare.g_nHitTime - LevelFastTime + 1500;
                }
                else
                {
                    NextHitTime = MShare.g_nHitTime - LevelFastTime;
                }
            }
            if (NextHitTime < 0)
            {
                NextHitTime = 0;
            }
            if (MShare.GetTickCount() - LastHitTick > NextHitTime)
            {
                if (settime)
                {
                    LastHitTick = MShare.GetTickCount();
                }
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        private void ActionFailed()
        {
            MShare.g_nTargetX = -1;
            MShare.g_nTargetY = -1;
            MShare.g_MySelf.m_boUseCboLib = false;
            ActionFailLock = true;
            ActionFailLockTime = MShare.GetTickCount();
            MShare.g_MySelf.MoveFail();
        }

        private bool IsUnLockAction()
        {
            bool result;
            if (ActionFailLock)
            {
                if ((MShare.GetTickCount() - ActionFailLockTime) >= 1000)// 如果操作被锁定，则在指定时间后解锁
                {
                    ActionFailLock = false;
                }
            }
            if (ActionFailLock || MShare.g_boMapMoving || MShare.g_boServerChanging)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        private bool IsGroupMember(string uname)
        {
            return MShare.g_GroupMembers.IndexOf(uname) >= 0; ;
        }

        private void CheckSpeedHack(long rtime)
        {
            return;
        }

        private void RecalcAutoMovePath()
        {
            if ((MShare.g_MySelf.m_nTagX > 0) && (MShare.g_MySelf.m_nTagY > 0))
            {
                if (!g_PathBusy)
                {
                    g_PathBusy = true;
                    try
                    {
                        Map.ReLoadMapData();
                        TPathMap.g_MapPath = Map.FindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_MySelf.m_nTagX, MShare.g_MySelf.m_nTagY, 0);
                        if (TPathMap.g_MapPath != null)
                        {
                            g_MoveStep = 1;
                            TimerAutoMove.Enabled = true;
                        }
                        else
                        {
                            MShare.g_MySelf.m_nTagX = 0;
                            MShare.g_MySelf.m_nTagY = 0;
                            TimerAutoMove.Enabled = false;
                            DScreen.AddChatBoardString($"自动移动目标({MShare.g_MySelf.m_nTagX}:{MShare.g_MySelf.m_nTagY})被占据，不可到达", GetRGB(5));
                        }
                    }
                    finally
                    {
                        g_PathBusy = false;
                    }
                }
            }
        }

        public string DecodeMessagePacket_ExtractUserName(string line)
        {
            var uname = string.Empty;
            HUtil32.GetValidStr3(line, ref line, new string[] { "(", "!", "*", "/", ")" });
            HUtil32.GetValidStr3(line, ref uname, new string[] { " ", "=", ":" });
            if (uname != "")
            {
                if ((uname[0] == '/') || (uname[0] == '(') || (uname[0] == ' ') || (uname[0] == '['))
                {
                    uname = "";
                }
            }
            return uname;
        }

        private void DecodeMessagePacket(string datablock, int btPacket)
        {
            var head = string.Empty;
            var body = string.Empty;
            var body2 = string.Empty;
            var data = string.Empty;
            var Str = string.Empty;
            var Str2 = string.Empty;
            var str3 = string.Empty;
            ClientMesaagePacket msg = null;
            ShortMessage sMsg;
            MessageBodyW mbw;
            CharDesc desc;
            MessageBodyWL wl;
            int i;
            int j;
            var n = 0;
            TActor Actor;
            TActor Actor2;
            //TClEvent __event;
            if ((btPacket == 0) && (datablock[0] == '+'))
            {
                ProcessActMsg(datablock);
                return;
            }
            if (datablock.Length < Grobal2.DEFBLOCKSIZE)
            {
                return;
            }
            if (datablock.Length > Grobal2.DEFBLOCKSIZE)
            {
                body = datablock.Substring(Grobal2.DEFBLOCKSIZE, datablock.Length - Grobal2.DEFBLOCKSIZE);
            }
            if (btPacket == 0)
            {
                head = datablock.Substring(0, Grobal2.DEFBLOCKSIZE);
                msg = EDCode.DecodePacket(head);
                if (msg == null)
                {
                    return;
                }
            }
            else
            {
                body = body2;
            }
            if (MShare.g_MySelf == null)
            {
                switch (msg.Ident)
                {
                    case Grobal2.SM_NEWID_SUCCESS:
                        MainOutMessage("您的帐号创建成功。请妥善保管您的帐号和密码，并且不要因任何原因把帐号和密码告诉任何其他人。如果忘记了密码,你可以通过我们的主页重新找回。");
                        LoginScene.ClientNewIdSuccess();
                        break;
                    case Grobal2.SM_NEWID_FAIL:
                        switch (msg.Recog)
                        {
                            case 0:
                                MainOutMessage($"帐号 [{LoginID}] 已被其他的玩家使用了。请选择其它帐号名注册");
                                break;
                            case -2:
                                MainOutMessage("此帐号名被禁止使用！");
                                break;
                            default:
                                MainOutMessage("帐号创建失败，请确认帐号是否包括空格、及非法字符！Code: " + msg.Recog.ToString());
                                break;
                        }
                        break;
                    case Grobal2.SM_PASSWD_FAIL:
                        switch (msg.Recog)
                        {
                            case -1:
                                MainOutMessage("密码错误！");
                                break;
                            case -2:
                                MainOutMessage("密码输入错误超过3次，此帐号被暂时锁定，请稍候再登录！");
                                break;
                            case -3:
                                MainOutMessage("此帐号已经登录或被异常锁定，请稍候再登录！");
                                break;
                            case -4:
                                MainOutMessage("这个帐号访问失败！\\请使用其他帐号登录，\\或者申请付费注册");
                                break;
                            case -5:
                                MainOutMessage("这个帐号被锁定！");
                                break;
                            case -6:
                                MainOutMessage("请使用专用登陆器登陆游戏！");
                                break;
                            default:
                                MainOutMessage("此帐号不存在，或出现未知错误！");
                                break;
                        }
                        LoginScene.PassWdFail();
                        break;
                    case Grobal2.SM_NEEDUPDATE_ACCOUNT:
                        ClientGetNeedUpdateAccount(body);
                        break;
                    case Grobal2.SM_UPDATEID_SUCCESS:
                        MainOutMessage("您的帐号信息更新成功。请妥善保管您的帐号和密码。并且不要因任何原因把帐号和密码告诉任何其他人。如果忘记了密码，你可以通过我们的主页重新找回。");
                        LoginScene.ClientGetSelectServer();
                        break;
                    case Grobal2.SM_UPDATEID_FAIL:
                        MainOutMessage("更新帐号失败！");
                        LoginScene.ClientGetSelectServer();
                        break;
                    case Grobal2.SM_PASSOK_SELECTSERVER:
                        LoginScene.ClientGetPasswordOk(msg, body);
                        break;
                    case Grobal2.SM_SELECTSERVER_OK:
                        LoginScene.ClientGetPasswdSuccess(body);
                        DScreen.ChangeScene(SceneType.stSelectChr);
                        break;
                    case Grobal2.SM_QUERYCHR:
                        SelectChrScene.ClientGetReceiveChrs(body);
                        break;
                    case Grobal2.SM_QUERYCHR_FAIL:
                        MainOutMessage("服务器认证失败！");
                        break;
                    case Grobal2.SM_NEWCHR_SUCCESS:
                        SelectChrScene.SendQueryChr();
                        break;
                    case Grobal2.SM_NEWCHR_FAIL:
                        switch (msg.Recog)
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
                        break;
                    case Grobal2.SM_CHGPASSWD_SUCCESS:
                        MainOutMessage("密码修改成功");
                        break;
                    case Grobal2.SM_CHGPASSWD_FAIL:
                        switch (msg.Recog)
                        {
                            case -1:
                                MainOutMessage("输入的原始密码不正确！");
                                break;
                            case -2:
                                MainOutMessage("此帐号被锁定！");
                                break;
                            default:
                                MainOutMessage("输入的新密码长度小于四位！");
                                break;
                        }
                        break;
                    case Grobal2.SM_DELCHR_SUCCESS:
                        SelectChrScene.SendQueryChr();
                        break;
                    case Grobal2.SM_DELCHR_FAIL:
                        MainOutMessage("[错误信息] 删除游戏角色时出现错误！");
                        break;
                    case Grobal2.SM_STARTPLAY:
                        SelectChrScene.ClientGetStartPlay(body);
                        DScreen.ChangeScene(SceneType.stPlayGame);
                        break;
                    case Grobal2.SM_STARTFAIL:
                        MainOutMessage("此服务器满员！");
                        LoginScene.ClientGetSelectServer();
                        break;
                    case Grobal2.SM_VERSION_FAIL:
                        MainOutMessage("游戏程序版本不正确，请下载最新版本游戏程序！");
                        break;
                    //case Grobal2.SM_OVERCLIENTCOUNT:
                    //    MShare.g_boDoFastFadeOut = false;
                    //    DebugOutStr("客户端开启数量过多，连接被断开！！！");
                    //    break;
                    case Grobal2.SM_OUTOFCONNECTION:
                    case Grobal2.SM_NEWMAP:
                    case Grobal2.SM_LOGON:
                    case Grobal2.SM_RECONNECT:
                    case Grobal2.SM_SENDNOTICE:
                    case Grobal2.SM_DLGMSG:
                        break;
                }
            }
            if (MShare.g_boMapMoving)
            {
                if (msg.Ident == Grobal2.SM_CHANGEMAP)
                {
                    WaitingMsg = msg;
                    WaitingStr = EDCode.DeCodeString(body);
                    MShare.g_boMapMovingWait = true;
                    //WaitMsgTimer.Enabled = true;
                }
                return;
            }
            switch (msg.Ident)
            {
                case Grobal2.SM_PLAYERCONFIG:
                    switch (msg.Recog)
                    {
                        case -1:
                            DScreen.AddChatBoardString("切换时装外显操作太快了！", ConsoleColor.Red);
                            break;
                    }
                    break;
                case Grobal2.SM_NEWMAP:
                    MShare.g_sMapTitle = "";
                    Str = EDCode.DeCodeString(body);
                    g_PlayScene.SendMsg(Grobal2.SM_NEWMAP, 0, msg.Param, msg.Tag, (byte)msg.Series, 0, 0, Str);
                    break;
                case Grobal2.SM_LOGON:
                    MShare.g_dwFirstServerTime = 0;
                    MShare.g_dwFirstClientTime = 0;
                    wl = EDCode.DecodeBuffer<MessageBodyWL>(body);
                    if (msg.Series > 8)
                    {
                        msg.Series = (byte)RandomNumber.GetInstance().Random(8);
                    }
                    g_PlayScene.SendMsg(Grobal2.SM_LOGON, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, wl.Param1, wl.Param2, "");
                    SendClientMessage(Grobal2.CM_QUERYBAGITEMS, 1, 0, 0, 0);
                    if (HUtil32.LoByte(HUtil32.LoWord(wl.Tag1)) == 1)
                    {
                        MShare.g_boAllowGroup = true;
                    }
                    else
                    {
                        MShare.g_boAllowGroup = false;
                    }
                    MShare.g_boServerChanging = false;
                    if (MShare.g_wAvailIDDay > 0)
                    {
                        DScreen.AddChatBoardString("您当前通过包月帐号充值", GetRGB(219));
                    }
                    else if (MShare.g_wAvailIPDay > 0)
                    {
                        DScreen.AddChatBoardString("您当前通过包月IP 充值", GetRGB(219));
                    }
                    else if (MShare.g_wAvailIPHour > 0)
                    {
                        DScreen.AddChatBoardString("您当前通过计时IP 充值", GetRGB(219));
                    }
                    else if (MShare.g_wAvailIDHour > 0)
                    {
                        DScreen.AddChatBoardString("您当前通过计时帐号充值", GetRGB(219));
                    }
                    MShare.LoadUserConfig(ChrName);
                    MShare.LoadItemFilter2();
                    //SendClientMessage(Grobal2.CM_HIDEDEATHBODY, MShare.g_MySelf.m_nRecogId, (int)MShare.g_gcGeneral[8], 0, 0);
                    MainOutMessage("成功进入游戏");
                    MainOutMessage("-----------------------------------------------");
                    break;
                case Grobal2.SM_SERVERCONFIG:
                    ClientGetServerConfig(msg, body);
                    break;
                case Grobal2.SM_RECONNECT:
                    SelectChrScene.ClientGetReconnect(body);
                    break;
                case Grobal2.SM_TIMECHECK_MSG:
                    CheckSpeedHack(msg.Recog);
                    break;
                case Grobal2.SM_AREASTATE:
                    MShare.g_nAreaStateValue = msg.Recog;
                    break;
                case Grobal2.SM_MAPDESCRIPTION:
                    ClientGetMapDescription(msg, body);
                    break;
                case Grobal2.SM_GAMEGOLDNAME:
                    ClientGetGameGoldName(msg, body);
                    break;
                case Grobal2.SM_ADJUST_BONUS:
                    ClientGetAdjustBonus(msg.Recog, body);
                    break;
                case Grobal2.SM_MYSTATUS:
                    MShare.g_nMyHungryState = msg.Param;
                    break;
                case Grobal2.SM_TURN:
                    //n = HUtil32.GetCodeMsgSize(8 * 4 / 3);
                    if (body.Length > n)
                    {
                        body2 = body.Substring(n, body.Length - n);
                        data = EDCode.DeCodeString(body2);
                        body2 = body.Substring(0, n);
                        Str = HUtil32.GetValidStr3(data, ref data, HUtil32.Backslash);
                    }
                    else
                    {
                        body2 = body;
                        data = "";
                    }
                    desc = EDCode.DecodeBuffer<CharDesc>(body2);
                    g_PlayScene.SendMsg(Grobal2.SM_TURN, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    if (data != "")
                    {
                        Actor = g_PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.m_sDescUserName = HUtil32.GetValidStr3(data, ref Actor.m_sUserName, new string[] { "\\" });
                            if (Actor.m_sUserName.IndexOf("(") != 0)
                            {
                                HUtil32.ArrestStringEx(Actor.m_sUserName, "(", ")", ref data);
                                if (data == MShare.g_MySelf.m_sUserName)
                                {
                                    j = 0;
                                    for (i = 0; i < MShare.g_MySelf.m_SlaveObject.Count; i++)
                                    {
                                        if (MShare.g_MySelf.m_SlaveObject[i] == Actor)
                                        {
                                            j = 1;
                                            break;
                                        }
                                    }
                                    if (j == 0)
                                    {
                                        MShare.g_MySelf.m_SlaveObject.Add(Actor);
                                    }
                                }
                            }
                            Actor.m_btNameColor = (byte)HUtil32.StrToInt(Str, 0);
                            if (Actor.m_btRace == Grobal2.RCC_MERCHANT)
                            {
                                Actor.m_nNameColor = Color.Lime.ToArgb();
                            }
                            else
                            {
                                Actor.m_nNameColor = GetRGB(Actor.m_btNameColor);
                            }
                        }
                    }
                    break;
                case Grobal2.SM_BACKSTEP:
                    //n = HUtil32.GetCodeMsgSize(sizeof(TCharDesc) * 4 / 3);
                    if (body.Length > n)
                    {
                        body2 = body.Substring(n + 1 - 1, body.Length);
                        data = EDCode.DeCodeString(body2);
                        body2 = body.Substring(1 - 1, n);
                        Str = HUtil32.GetValidStr3(data, ref data, HUtil32.Backslash);
                    }
                    else
                    {
                        body2 = body;
                        data = "";
                    }
                    desc = EDCode.DecodeBuffer<CharDesc>(body2);
                    g_PlayScene.SendMsg(Grobal2.SM_BACKSTEP, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    if (data != "")
                    {
                        Actor = g_PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.m_sDescUserName = HUtil32.GetValidStr3(data, ref Actor.m_sUserName, new string[] { "\\" });
                            Actor.m_btNameColor = (byte)HUtil32.StrToInt(Str, 0);
                            if (Actor.m_btRace == Grobal2.RCC_MERCHANT)
                            {
                                Actor.m_nNameColor = (byte)Color.Lime.ToArgb();
                            }
                            else
                            {
                                Actor.m_nNameColor = GetRGB(Actor.m_btNameColor);
                            }
                        }
                    }
                    break;
                case Grobal2.SM_SPACEMOVE_HIDE:
                case Grobal2.SM_SPACEMOVE_HIDE2:
                    if (msg.Recog != MShare.g_MySelf.m_nRecogId)
                    {
                        g_PlayScene.SendMsg(msg.Ident, msg.Recog, msg.Param, msg.Tag, 0, 0, 0, "");
                    }
                    break;
                case Grobal2.SM_SPACEMOVE_SHOW:
                case Grobal2.SM_SPACEMOVE_SHOW2:
                    //n = HUtil32.GetCodeMsgSize(sizeof(TCharDesc) * 4 / 3);
                    if (body.Length > n)
                    {
                        body2 = body.Substring(n + 1 - 1, body.Length);
                        data = EDCode.DeCodeString(body2);
                        body2 = body.Substring(1 - 1, n);
                        Str = HUtil32.GetValidStr3(data, ref data, HUtil32.Backslash);
                    }
                    else
                    {
                        body2 = body;
                        data = "";
                    }
                    desc = EDCode.DecodeBuffer<CharDesc>(body2);
                    if (msg.Recog != MShare.g_MySelf.m_nRecogId)
                    {
                        g_PlayScene.NewActor(msg.Recog, msg.Param, msg.Tag, msg.Series, desc.Feature, desc.Status);
                    }
                    g_PlayScene.SendMsg(msg.Ident, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    if (data != "")
                    {
                        Actor = g_PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.m_sDescUserName = HUtil32.GetValidStr3(data, ref Actor.m_sUserName, new string[] { "\\" });
                            Actor.m_btNameColor = (byte)HUtil32.StrToInt(Str, 0);
                            if (Actor.m_btRace == Grobal2.RCC_MERCHANT)
                            {
                                Actor.m_nNameColor = Color.Lime.ToArgb();
                            }
                            else
                            {
                                Actor.m_nNameColor = GetRGB(Actor.m_btNameColor);
                            }
                        }
                    }
                    break;
                case Grobal2.SM_RUSH:
                case Grobal2.SM_RUSHKUNG:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    if (msg.Recog == MShare.g_MySelf.m_nRecogId)
                    {
                        g_PlayScene.SendMsg(msg.Ident, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    }
                    else
                    {
                        g_PlayScene.SendMsg(msg.Ident, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    }
                    if (msg.Ident == Grobal2.SM_RUSH)
                    {
                        MShare.g_dwLatestRushRushTick = MShare.GetTickCount();
                    }
                    break;
                case Grobal2.SM_WALK:
                case Grobal2.SM_RUN:
                case Grobal2.SM_HORSERUN:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    if (msg.Recog != MShare.g_MySelf.m_nRecogId)
                    {
                        g_PlayScene.SendMsg(msg.Ident, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    }
                    break;
                case Grobal2.SM_CHANGELIGHT:
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.m_nChrLight = msg.Param;
                    }
                    break;
                case Grobal2.SM_LAMPCHANGEDURA:
                    if (MShare.g_UseItems[Grobal2.U_RIGHTHAND].Item.Name != "")
                    {
                        MShare.g_UseItems[Grobal2.U_RIGHTHAND].Dura = (ushort)msg.Recog;
                    }
                    break;
                case Grobal2.SM_MOVEFAIL:
                    ActionFailed();
                    ActionLock = false;
                    RecalcAutoMovePath();
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    ActionFailLock = false;
                    g_PlayScene.SendMsg(Grobal2.SM_TURN, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    break;
                case Grobal2.SM_BUTCH:// 挖肉动作封包
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    if (msg.Recog != MShare.g_MySelf.m_nRecogId)
                    {
                        Actor = g_PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.SendMsg(Grobal2.SM_SITDOWN, msg.Param, msg.Tag, msg.Series, 0, 0, "", 0);
                        }
                    }
                    break;
                case Grobal2.SM_SITDOWN:// 蹲下动作封包
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    if (msg.Recog != MShare.g_MySelf.m_nRecogId)
                    {
                        Actor = g_PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.SendMsg(Grobal2.SM_SITDOWN, msg.Param, msg.Tag, msg.Series, 0, 0, "", 0);
                        }
                    }
                    break;
                case Grobal2.SM_HIT:
                case Grobal2.SM_HEAVYHIT:
                case Grobal2.SM_POWERHIT:
                case Grobal2.SM_LONGHIT:
                case Grobal2.SM_CRSHIT:
                case Grobal2.SM_WIDEHIT:
                case Grobal2.SM_BIGHIT:
                case Grobal2.SM_FIREHIT:
                    if (msg.Recog != MShare.g_MySelf.m_nRecogId)
                    {
                        Actor = g_PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.SendMsg(msg.Ident, msg.Param, msg.Tag, msg.Series, 0, 0, body, 0);
                            if (msg.Ident == Grobal2.SM_HEAVYHIT)
                            {
                                if (body != "")
                                {
                                    Actor.m_boDigFragment = true;
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.SM_FLYAXE:
                    mbw = EDCode.DecodeBuffer<MessageBodyW>(body);
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.SendMsg(msg.Ident, msg.Param, msg.Tag, msg.Series, 0, 0, "", 0);
                        Actor.m_nTargetX = mbw.Param1;
                        Actor.m_nTargetY = mbw.Param2;
                        Actor.m_nTargetRecog = HUtil32.MakeLong(mbw.Tag1, mbw.Tag2);
                    }
                    break;
                // Modify the A .. B: Grobal2.SM_LIGHTING, Grobal2.SM_LIGHTING_1 .. Grobal2.SM_LIGHTING_3
                case Grobal2.SM_LIGHTING:
                    wl = EDCode.DecodeBuffer<MessageBodyWL>(body);
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.SendMsg(msg.Ident, msg.Param, msg.Tag, msg.Series, 0, 0, "", 0);
                        Actor.m_nTargetX = wl.Param1;
                        Actor.m_nTargetY = wl.Param2;
                        Actor.m_nTargetRecog = wl.Tag1;
                        Actor.m_nMagicNum = wl.Tag2;
                    }
                    break;
                case Grobal2.SM_SPELL:
                    UseMagicSpell(msg.Recog, msg.Series, msg.Param, msg.Tag, HUtil32.StrToInt(body, 0));
                    break;
                case Grobal2.SM_MAGICFIRE:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    UseMagicFire(msg.Recog, HUtil32.LoByte(msg.Series), HUtil32.HiByte(msg.Series), msg.Param, msg.Tag, desc.Feature, desc.Status);
                    break;
                case Grobal2.SM_MAGICFIRE_FAIL:
                    UseMagicFireFail(msg.Recog);
                    break;
                case Grobal2.SM_OUTOFCONNECTION:
                    MainOutMessage("服务器连接被强行中断。连接时间可能超过限制");
                    LoginOut();
                    break;
                case Grobal2.SM_DEATH:
                case Grobal2.SM_NOWDEATH:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.SendMsg(msg.Ident, msg.Param, msg.Tag, msg.Series, desc.Feature, desc.Status, "", 0);
                        Actor.m_Abil.HP = 0;
                        Actor.m_nIPower = -1;
                    }
                    else
                    {
                        g_PlayScene.SendMsg(Grobal2.SM_DEATH, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    }
                    break;
                case Grobal2.SM_SKELETON:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    g_PlayScene.SendMsg(Grobal2.SM_SKELETON, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    break;
                case Grobal2.SM_ALIVE:
                    desc = EDCode.DecodeBuffer<CharDesc>(body);
                    g_PlayScene.SendMsg(Grobal2.SM_ALIVE, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, desc.Feature, desc.Status, "", 0);
                    break;
                case Grobal2.SM_ABILITY:
                    MShare.g_MySelf.m_nGold = msg.Recog;
                    MShare.g_MySelf.m_btJob = HUtil32.LoByte(msg.Param);
                    MShare.g_MySelf.m_nIPowerLvl = HUtil32.HiByte(msg.Param);
                    MShare.g_MySelf.m_nGameGold = HUtil32.MakeLong(msg.Tag, msg.Series);
                    MShare.g_MySelf.m_Abil = EDCode.DecodeBuffer<Ability>(body);
                    break;
                case Grobal2.SM_SUBABILITY:
                    MShare.g_nMyHitPoint = HUtil32.LoByte(msg.Param);
                    MShare.g_nMySpeedPoint = HUtil32.HiByte(msg.Param);
                    MShare.g_nMyAntiPoison = HUtil32.LoByte(msg.Tag);
                    MShare.g_nMyPoisonRecover = HUtil32.HiByte(msg.Tag);
                    MShare.g_nMyHealthRecover = HUtil32.LoByte(msg.Series);
                    MShare.g_nMySpellRecover = HUtil32.HiByte(msg.Series);
                    MShare.g_nMyAntiMagic = HUtil32.LoByte(HUtil32.LoWord(msg.Recog));
                    MShare.g_nMyIPowerRecover = HUtil32.HiByte(HUtil32.LoWord(msg.Recog));
                    MShare.g_nMyAddDamage = HUtil32.LoByte(HUtil32.HiWord(msg.Recog));
                    MShare.g_nMyDecDamage = HUtil32.HiByte(HUtil32.HiWord(msg.Recog));
                    break;
                case Grobal2.SM_DAYCHANGING:
                    MShare.g_nDayBright = msg.Param;
                    break;
                case Grobal2.SM_WINEXP:
                    MShare.g_MySelf.m_Abil.Exp = msg.Recog;
                    if (!MShare.g_gcGeneral[3] || (HUtil32.MakeLong(msg.Param, msg.Tag) > MShare.g_MaxExpFilter))
                    {
                        DScreen.AddSysMsg($"经验值 +{HUtil32.MakeLong(msg.Param, msg.Tag)}");
                    }
                    break;
                case Grobal2.SM_LEVELUP:
                    MShare.g_MySelf.m_Abil.Level = (byte)msg.Param;
                    DScreen.AddSysMsg("您的等级已升级！");
                    break;
                case Grobal2.SM_HEALTHSPELLCHANGED:
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.m_Abil.HP = msg.Param;
                        Actor.m_Abil.MP = msg.Tag;
                        Actor.m_Abil.MaxHP = msg.Series;
                    }
                    break;
                case Grobal2.SM_STRUCK:
                    wl = EDCode.DecodeBuffer<MessageBodyWL>(body);
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        if (MShare.g_gcGeneral[13] && (msg.Series > 0))
                        {
                            Actor.GetMoveHPShow(msg.Series);
                        }
                        if (Actor != MShare.g_MySelf)
                        {
                            if (Actor.CanCancelAction())
                            {
                                Actor.CancelAction();
                            }
                        }
                        if ((Actor != MShare.g_MySelf))
                        {
                            if ((Actor.m_btRace != 0) || !MShare.g_gcGeneral[15])
                            {
                                Actor.UpdateMsg(Grobal2.SM_STRUCK, (ushort)(short)wl.Tag2, 0, msg.Series, wl.Param1, wl.Param2, "", wl.Tag1);
                            }
                        }
                        Actor.m_Abil.HP = msg.Param;
                        Actor.m_Abil.MaxHP = msg.Tag;
                        if (MShare.g_boOpenAutoPlay && TimerAutoPlay.Enabled) //  自己受人攻击,小退
                        {
                            Actor2 = g_PlayScene.FindActor(wl.Tag1);
                            if ((Actor2 == null) || ((Actor2.m_btRace != 0) && (Actor2.m_btIsHero != 1)))
                            {
                                return;
                            }
                            if (MShare.g_MySelf != null)
                            {
                                if (Actor == MShare.g_MySelf) // 自己受人攻击,小退
                                {
                                    MShare.g_nAPReLogon = 1;
                                    MShare.g_nOverAPZone2 = MShare.g_nOverAPZone;
                                    MShare.g_APGoBack2 = MShare.g_APGoBack;
                                    if (MShare.g_APMapPath != null)
                                    {
                                        MShare.g_APMapPath2 = new Point[MShare.g_APMapPath.Length + 1];
                                        for (i = 0; i < MShare.g_APMapPath.Length; i++)
                                        {
                                            MShare.g_APMapPath2[i] = MShare.g_APMapPath[i];
                                        }
                                    }
                                    MShare.g_APLastPoint2 = MShare.g_APLastPoint;
                                    MShare.g_APStep2 = MShare.g_APStep;
                                    AppLogout();
                                    // SaveBagsData();
                                }
                            }
                        }
                    }
                    break;
                case Grobal2.SM_CHANGEFACE:
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        desc = EDCode.DecodeBuffer<CharDesc>(body);
                        Actor.m_nWaitForRecogId = HUtil32.MakeLong(msg.Param, msg.Tag);
                        Actor.m_nWaitForFeature = desc.Feature;
                        Actor.m_nWaitForStatus = desc.Status;
                        ClFunc.AddChangeFace(Actor.m_nWaitForRecogId);
                    }
                    break;
                case Grobal2.SM_PASSWORD:
                    break;
                case Grobal2.SM_OPENHEALTH:
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        if (Actor != MShare.g_MySelf)
                        {
                            Actor.m_Abil.HP = msg.Param;
                            Actor.m_Abil.MaxHP = msg.Tag;
                        }
                        Actor.m_boOpenHealth = true;
                    }
                    break;
                case Grobal2.SM_CLOSEHEALTH:
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.m_boOpenHealth = false;
                    }
                    break;
                case Grobal2.SM_INSTANCEHEALGUAGE:
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.m_Abil.HP = msg.Param;
                        Actor.m_Abil.MaxHP = msg.Tag;
                        Actor.m_noInstanceOpenHealth = true;
                        Actor.m_dwOpenHealthTime = 2 * 1000;
                        Actor.m_dwOpenHealthStart = MShare.GetTickCount();
                    }
                    break;
                case Grobal2.SM_BREAKWEAPON:
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        if (Actor is THumActor)
                        {
                            ((THumActor)Actor).DoWeaponBreakEffect();
                        }
                    }
                    break;
                case Grobal2.SM_HEAR:
                case Grobal2.SM_CRY:
                case Grobal2.SM_GROUPMESSAGE:
                case Grobal2.SM_GUILDMESSAGE:
                case Grobal2.SM_WHISPER:
                case Grobal2.SM_SYSMESSAGE:
                    Str = EDCode.DeCodeString(body);
                    if (msg.Tag > 0)
                    {
                        DScreen.AddChatBoardString(Str, GetRGB(HUtil32.LoByte(msg.Param)), GetRGB(HUtil32.HiByte(msg.Param)));
                        return;
                    }
                    if (msg.Ident == Grobal2.SM_WHISPER)
                    {
                        HUtil32.GetValidStr3(Str, ref str3, new string[] { " ", "=", ">" });
                        DScreen.AddChatBoardString(Str, GetRGB(HUtil32.LoByte(msg.Param)), GetRGB(HUtil32.HiByte(msg.Param)));
                    }
                    else
                    {
                        DScreen.AddChatBoardString(Str, GetRGB(HUtil32.LoByte(msg.Param)), GetRGB(HUtil32.HiByte(msg.Param)));
                    }
                    if (msg.Ident == Grobal2.SM_HEAR)
                    {
                        Actor = g_PlayScene.FindActor(msg.Recog);
                        if (Actor != null)
                        {
                            Actor.Say(Str);
                        }
                    }
                    break;
                case Grobal2.SM_USERNAME:
                    Str = EDCode.DeCodeString(body);
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.m_sDescUserName = HUtil32.GetValidStr3(Str, ref Actor.m_sUserName, new string[] { "\\" });
                        Actor.m_btNameColor = (byte)msg.Param;
                        if (Actor.m_btRace == Grobal2.RCC_MERCHANT)
                        {
                            Actor.m_nNameColor = (byte)Color.Lime.ToArgb();
                        }
                        else
                        {
                            Actor.m_nNameColor = GetRGB((byte)msg.Param);
                        }
                        if (msg.Tag >= 1 && msg.Tag <= 5)
                        {
                            Actor.m_btAttribute = (byte)msg.Tag;
                        }
                    }
                    break;
                case Grobal2.SM_CHANGENAMECOLOR:
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor != null)
                    {
                        Actor.m_btNameColor = (byte)msg.Param;
                        if (Actor.m_btRace == Grobal2.RCC_MERCHANT)
                        {
                            Actor.m_nNameColor = (byte)Color.Lime.ToArgb();
                        }
                        else
                        {
                            Actor.m_nNameColor = GetRGB((byte)msg.Param);
                        }
                    }
                    break;
                case Grobal2.SM_HIDE:
                case Grobal2.SM_GHOST:
                case Grobal2.SM_DISAPPEAR:
                    if (MShare.g_MySelf.m_nRecogId != msg.Recog)
                    {
                        g_PlayScene.SendMsg(Grobal2.SM_HIDE, msg.Recog, msg.Param, msg.Tag, (byte)msg.Series, 0, 0, "");
                    }
                    break;
                case Grobal2.SM_DIGUP:
                    wl = EDCode.DecodeBuffer<MessageBodyWL>(body);
                    Actor = g_PlayScene.FindActor(msg.Recog);
                    if (Actor == null)
                    {
                        Actor = g_PlayScene.NewActor(msg.Recog, msg.Param, msg.Tag, msg.Series, wl.Param1, wl.Param2);
                    }
                    Actor.m_nCurrentEvent = wl.Tag1;
                    Actor.SendMsg(Grobal2.SM_DIGUP, msg.Param, msg.Tag, msg.Series, wl.Param1, wl.Param2, "", 0);
                    break;
                case Grobal2.SM_DIGDOWN:
                    g_PlayScene.SendMsg(Grobal2.SM_DIGDOWN, msg.Recog, msg.Param, msg.Tag, 0, 0, 0, "");
                    break;
                case Grobal2.SM_SHOWEVENT:
                    sMsg = EDCode.DecodeBuffer<ShortMessage>(body);
                    //__event = new TClEvent(msg.Recog, HUtil32.LoWord(msg.Tag), msg.Series, msg.Param);
                    //__event.m_nDir = 0;
                    //__event.m_nEventParam = sMsg.Ident;
                    //__event.m_nEventLevel = sMsg.wMsg;
                    //EventMan.AddEvent(__event);
                    break;
                case Grobal2.SM_HIDEEVENT:
                    //EventMan.DelEventById(msg.Recog);
                    break;
                case Grobal2.SM_ADDITEM:
                    ClientGetAddItem(msg.Series, body);
                    break;
                case Grobal2.SM_BAGITEMS:
                    ClientGetBagItmes(body);
                    break;
                case Grobal2.SM_UPDATEITEM:
                    ClientGetUpdateItem(body);
                    break;
                case Grobal2.SM_DELITEM:
                    ClientGetDelItem(body);
                    break;
                case Grobal2.SM_DELITEMS:
                    ClientGetDelItems(body, msg.Param);
                    break;
                case Grobal2.SM_DROPITEM_SUCCESS:
                    ClFunc.DelDropItem(EDCode.DeCodeString(body), msg.Recog);
                    break;
                case Grobal2.SM_DROPITEM_FAIL:
                    ClientGetDropItemFail(EDCode.DeCodeString(body), msg.Recog);
                    break;
                case Grobal2.SM_ITEMSHOW:
                    ClientGetShowItem(msg.Recog, (short)msg.Param, (short)msg.Tag, msg.Series, EDCode.DeCodeString(body));
                    break;
                case Grobal2.SM_ITEMHIDE:
                    ClientGetHideItem(msg.Recog, msg.Param, msg.Tag);
                    break;
                case Grobal2.SM_OPENDOOR_OK:
                    Map.OpenDoor(msg.Param, msg.Tag);
                    break;
                case Grobal2.SM_OPENDOOR_LOCK:
                    DScreen.AddSysMsg("此门被锁定");
                    break;
                case Grobal2.SM_CLOSEDOOR:
                    Map.CloseDoor(msg.Param, msg.Tag);
                    break;
                case Grobal2.SM_TAKEON_OK:
                    MShare.g_MySelf.m_nFeature = msg.Recog;
                    MShare.g_MySelf.FeatureChanged();
                    if (MShare.g_WaitingUseItem.Item.Item.Name != "")
                    {
                        if (MShare.g_WaitingUseItem.Index >= 0 && MShare.g_WaitingUseItem.Index <= 13)
                        {
                            MShare.g_UseItems[MShare.g_WaitingUseItem.Index] = MShare.g_WaitingUseItem.Item;
                        }
                        MShare.g_WaitingUseItem.Item.Item.Name = "";
                    }
                    break;
                case Grobal2.SM_TAKEON_FAIL:
                    if (MShare.g_WaitingUseItem.Item.Item.Name != "")
                    {
                        ClFunc.AddItemBag(MShare.g_WaitingUseItem.Item);
                        MShare.g_WaitingUseItem.Item.Item.Name = "";
                    }
                    break;
                case Grobal2.SM_TAKEOFF_OK:
                    MShare.g_MySelf.m_nFeature = msg.Recog;
                    MShare.g_MySelf.FeatureChanged();
                    MShare.g_WaitingUseItem.Item.Item.Name = "";
                    break;
                case Grobal2.SM_TAKEOFF_FAIL:
                    if (MShare.g_WaitingUseItem.Item.Item.Name != "")
                    {
                        if (MShare.g_WaitingUseItem.Index < 0)
                        {
                            n = -(MShare.g_WaitingUseItem.Index + 1);
                            MShare.g_UseItems[n] = MShare.g_WaitingUseItem.Item;
                        }
                        MShare.g_WaitingUseItem.Item.Item.Name = "";
                    }
                    break;
                case Grobal2.SM_SENDUSEITEMS:
                    ClientGetSendUseItems(body);
                    break;
                case Grobal2.SM_WEIGHTCHANGED:
                    MShare.g_MySelf.m_Abil.Weight = (ushort)msg.Recog;
                    MShare.g_MySelf.m_Abil.WearWeight = (byte)msg.Param;
                    MShare.g_MySelf.m_Abil.HandWeight = (byte)msg.Tag;
                    break;
                case Grobal2.SM_GOLDCHANGED:
                    if (msg.Recog > MShare.g_MySelf.m_nGold)
                    {
                        DScreen.AddSysMsg("获得 " + (msg.Recog - MShare.g_MySelf.m_nGold).ToString() + MShare.g_sGoldName);
                    }
                    MShare.g_MySelf.m_nGold = msg.Recog;
                    MShare.g_MySelf.m_nGameGold = HUtil32.MakeLong(msg.Param, msg.Tag);
                    break;
                case Grobal2.SM_FEATURECHANGED:
                    g_PlayScene.SendMsg(msg.Ident, msg.Recog, 0, 0, 0, HUtil32.MakeLong(msg.Param, msg.Tag), HUtil32.MakeLong(msg.Series, 0), body);
                    break;
                case Grobal2.SM_CHARSTATUSCHANGED:
                    if (body != "")
                    {
                        g_PlayScene.SendMsg(msg.Ident, msg.Recog, 0, 0, 0, HUtil32.MakeLong(msg.Param, msg.Tag), msg.Series, EDCode.DeCodeString(body));
                    }
                    else
                    {
                        g_PlayScene.SendMsg(msg.Ident, msg.Recog, 0, 0, 0, HUtil32.MakeLong(msg.Param, msg.Tag), msg.Series, "");
                    }
                    break;
                case Grobal2.SM_CLEAROBJECTS:
                    MShare.g_boMapMoving = true;
                    break;
                case Grobal2.SM_EAT_OK:
                    if (msg.Recog != 0)
                    {
                        Str = "";
                        if (msg.Recog != MShare.g_EatingItem.MakeIndex)
                        {
                            for (i = MShare.MAXBAGITEMCL - 1; i >= 0; i--)
                            {
                                if (MShare.g_ItemArr[i].Item.Name != "")
                                {
                                    if (MShare.g_ItemArr[i].MakeIndex == MShare.g_EatingItem.MakeIndex)
                                    {
                                        ClFunc.DelStallItem(MShare.g_ItemArr[i]);
                                        Str = MShare.g_ItemArr[i].Item.Name;
                                        MShare.g_ItemArr[i].Item.Name = "";
                                        break;
                                    }
                                }
                            }
                        }
                        if (Str == "")
                        {
                            Str = MShare.g_EatingItem.Item.Name;
                            if (m_boSupplyItem)
                            {
                                if (m_nEatRetIdx >= 0 && m_nEatRetIdx <= 5)
                                {
                                    AutoSupplyBeltItem(MShare.g_EatingItem.Item.AniCount, m_nEatRetIdx, Str);
                                }
                                else
                                {
                                    AutoSupplyBagItem(MShare.g_EatingItem.Item.AniCount, Str);
                                }
                                m_boSupplyItem = false;
                            }
                        }
                        MShare.g_EatingItem.Item.Name = "";
                        ClFunc.ArrangeItembag();
                        m_nEatRetIdx = -1;
                    }
                    break;
                case Grobal2.SM_EAT_FAIL:
                    if (msg.Recog == MShare.g_EatingItem.MakeIndex)
                    {
                        // DScreen.AddChatBoardString(g_EatingItem.Item.Name + ' ' + IntToStr(msg.tag), clRed, clWhite);
                        if (msg.Tag > 0)
                        {
                            MShare.g_EatingItem.Dura = msg.Tag;
                        }
                        ClFunc.AddItemBag(MShare.g_EatingItem, m_nEatRetIdx);
                        MShare.g_EatingItem.Item.Name = "";
                        m_nEatRetIdx = -1;
                    }
                    m_boSupplyItem = false;
                    switch (msg.Series)
                    {
                        case 1:
                            DScreen.AddChatBoardString("[失败] 你的金币不足，不能释放积灵珠！", ConsoleColor.Red);
                            break;
                        case 2:
                            DScreen.AddChatBoardString("[失败] 你的元宝不足，不能释放积灵珠！", ConsoleColor.Red);
                            break;
                        case 3:
                            DScreen.AddChatBoardString("[失败] 你的金刚石不足，不能释放积灵珠！", ConsoleColor.Red);
                            break;
                        case 4:
                            DScreen.AddChatBoardString("[失败] 你的灵符不足，不能释放积灵珠！", ConsoleColor.Red);
                            break;
                    }
                    break;
                case Grobal2.SM_ADDMAGIC:
                    if (body != "")
                    {
                        ClientGetAddMagic(body);
                    }
                    break;
                case Grobal2.SM_SENDMYMAGIC:
                    if (body != "")
                    {
                        ClientGetMyMagics(body);
                    }
                    break;
                case Grobal2.SM_DELMAGIC:
                    ClientGetDelMagic(msg.Recog, msg.Param);
                    break;
                case Grobal2.SM_MAGIC_LVEXP:
                    ClientGetMagicLvExp(msg.Recog, msg.Param, HUtil32.MakeLong(msg.Tag, msg.Series));
                    break;
                case Grobal2.SM_DURACHANGE:
                    ClientGetDuraChange(msg.Param, msg.Recog, HUtil32.MakeLong(msg.Tag, msg.Series));
                    break;
                case Grobal2.SM_MERCHANTSAY:
                    ClientGetMerchantSay(msg.Recog, msg.Param, EDCode.DeCodeString(body));
                    break;
                case Grobal2.SM_SENDGOODSLIST:
                    ClientGetSendGoodsList(msg.Recog, msg.Param, body);
                    break;
                case Grobal2.SM_SENDUSERMAKEDRUGITEMLIST:
                    ClientGetSendMakeDrugList(msg.Recog, body);
                    break;
                case Grobal2.SM_SENDUSERSELL:
                    ClientGetSendUserSell(msg.Recog);
                    break;
                case Grobal2.SM_SENDUSERREPAIR:
                    ClientGetSendUserRepair(msg.Recog);
                    break;
                case Grobal2.SM_SENDBUYPRICE:
                    if (MShare.g_SellDlgItem.Item.Name != "")
                    {
                        if (msg.Recog > 0)
                        {
                            MShare.g_sSellPriceStr = msg.Recog.ToString() + MShare.g_sGoldName;
                        }
                        else
                        {
                            MShare.g_sSellPriceStr = "???? " + MShare.g_sGoldName;
                        }
                    }
                    break;
                case Grobal2.SM_USERSELLITEM_OK:
                    LastestClickTime = MShare.GetTickCount();
                    MShare.g_MySelf.m_nGold = msg.Recog;
                    MShare.g_SellDlgItemSellWait.Item.Item.Name = "";
                    break;
                case Grobal2.SM_USERSELLITEM_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    ClFunc.AddItemBag(MShare.g_SellDlgItemSellWait.Item);
                    MShare.g_SellDlgItemSellWait.Item.Item.Name = "";
                    MainOutMessage("此物品不能出售");
                    break;
                case Grobal2.SM_SENDREPAIRCOST:
                    if (MShare.g_SellDlgItem.Item.Name != "")
                    {
                        if (msg.Recog >= 0)
                        {
                            // 金币
                            MShare.g_sSellPriceStr = msg.Recog.ToString() + " " + MShare.g_sGoldName;
                        }
                        else
                        {
                            // 金币
                            MShare.g_sSellPriceStr = "???? " + MShare.g_sGoldName;
                        }
                    }
                    break;
                case Grobal2.SM_USERREPAIRITEM_OK:
                    if (MShare.g_SellDlgItemSellWait.Item.Item.Name != "")
                    {
                        LastestClickTime = MShare.GetTickCount();
                        MShare.g_MySelf.m_nGold = msg.Recog;
                        MShare.g_SellDlgItemSellWait.Item.Dura = msg.Param;
                        MShare.g_SellDlgItemSellWait.Item.DuraMax = msg.Tag;
                        ClFunc.AddItemBag(MShare.g_SellDlgItemSellWait.Item);
                        MShare.g_SellDlgItemSellWait.Item.Item.Name = "";
                    }
                    break;
                case Grobal2.SM_USERREPAIRITEM_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    ClFunc.AddItemBag(MShare.g_SellDlgItemSellWait.Item);
                    MShare.g_SellDlgItemSellWait.Item.Item.Name = "";
                    MainOutMessage("您不能修理此物品");
                    break;
                case Grobal2.SM_STORAGE_OK:
                case Grobal2.SM_STORAGE_FULL:
                case Grobal2.SM_STORAGE_FAIL:
                    //LastestClickTime = MShare.GetTickCount();
                    //if (msg.Ident != Grobal2.SM_STORAGE_OK)
                    //{
                    //    if (msg.Ident == Grobal2.SM_STORAGE_FULL)
                    //    {
                    //        DebugOutStr("您的个人仓库已经满了，不能再保管任何东西了");
                    //    }
                    //    else
                    //    {
                    //        if (msg.Recog == 2)
                    //        {
                    //            DebugOutStr("寄存物品失败,同类单个物品最高重叠数量是 " + Grobal2.MAX_OVERLAPITEM.ToString());
                    //        }
                    //        else if (msg.Recog == 3)
                    //        {
                    //            MShare.g_SellDlgItemSellWait.Item.Dura = MShare.g_SellDlgItemSellWait.Item.Dura - msg.Param;
                    //            DScreen.AddChatBoardString(string.Format("成功寄存 %s %d个", MShare.g_SellDlgItemSellWait.Item.Item.Name, msg.Param), Color.Blue);
                    //        }
                    //        else
                    //        {
                    //            DebugOutStr("您不能寄存物品");
                    //        }
                    //    }
                    //    ClFunc.AddItemBag(MShare.g_SellDlgItemSellWait.Item);
                    //}
                    MShare.g_SellDlgItemSellWait.Item.Item.Name = "";
                    break;
                case Grobal2.SM_SAVEITEMLIST:
                    ClientGetSaveItemList(msg.Recog, body);
                    break;
                case Grobal2.SM_TAKEBACKSTORAGEITEM_OK:
                case Grobal2.SM_TAKEBACKSTORAGEITEM_FAIL:
                case Grobal2.SM_TAKEBACKSTORAGEITEM_FULLBAG:
                    LastestClickTime = MShare.GetTickCount();
                    if (msg.Ident != Grobal2.SM_TAKEBACKSTORAGEITEM_OK)
                    {
                        if (msg.Ident == Grobal2.SM_TAKEBACKSTORAGEITEM_FULLBAG)
                        {
                            MainOutMessage("您无法携带更多物品了");
                        }
                        else
                        {
                            MainOutMessage("您无法取回物品");
                        }
                    }
                    break;
                case Grobal2.SM_BUYITEM_SUCCESS:
                    LastestClickTime = MShare.GetTickCount();
                    MShare.g_MySelf.m_nGold = msg.Recog;
                    break;
                case Grobal2.SM_BUYITEM_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case 1:
                            MainOutMessage("此物品被卖出");
                            break;
                        case 2:
                            MainOutMessage("您无法携带更多物品了");
                            break;
                        case 3:
                            MainOutMessage("您没有足够的钱来购买此物品");
                            break;
                    }
                    break;
                case Grobal2.SM_MAKEDRUG_SUCCESS:
                    LastestClickTime = MShare.GetTickCount();
                    MShare.g_MySelf.m_nGold = msg.Recog;
                    MainOutMessage("您要的物品已经搞定了");
                    break;
                case Grobal2.SM_MAKEDRUG_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case 1:
                            MainOutMessage("未知错误");
                            break;
                        case 2:
                            MainOutMessage("发生了错误");
                            break;
                        case 3:
                            MainOutMessage(MShare.g_sGoldName + "不足");
                            break;
                        case 4:
                            MainOutMessage("你缺乏所必需的物品");
                            break;
                    }
                    break;
                case Grobal2.SM_SENDDETAILGOODSLIST:
                    ClientGetSendDetailGoodsList(msg.Recog, msg.Param, msg.Tag, body);
                    break;
                case Grobal2.SM_TEST:
                    MShare.g_nTestReceiveCount++;
                    break;
                case Grobal2.SM_SENDNOTICE:
                    ClientGetSendNotice(body);
                    break;
                case Grobal2.SM_GROUPMODECHANGED:
                    if (msg.Param > 0)
                    {
                        MShare.g_boAllowGroup = true;
                        DScreen.AddChatBoardString("[开启组队开关]", GetRGB(219));
                    }
                    else
                    {
                        MShare.g_boAllowGroup = false;
                        DScreen.AddChatBoardString("[关闭组队开关]", GetRGB(219));
                    }
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    break;
                case Grobal2.SM_CREATEGROUP_OK:
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    MShare.g_boAllowGroup = true;
                    break;
                case Grobal2.SM_CREATEGROUP_FAIL:
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("编组还未成立或者你还不够等级创建！");
                            break;
                        case -2:
                            MainOutMessage("输入的人物名称不正确！");
                            break;
                        case -3:
                            MainOutMessage("您想邀请加入编组的人已经加入了其它组！");
                            break;
                        case -4:
                            MainOutMessage("对方不允许编组！");
                            break;
                    }
                    break;
                case Grobal2.SM_GROUPADDMEM_OK:
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    break;
                case Grobal2.SM_GROUPADDMEM_FAIL:
                    // GroupMembers.Add (DeCodeString(body));
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("编组还未成立或者你还不够等级创建！");
                            break;
                        case -2:
                            MainOutMessage("输入的人物名称不正确！");
                            break;
                        case -3:
                            MainOutMessage("已经加入编组！");
                            break;
                        case -4:
                            MainOutMessage("对方不允许编组！");
                            break;
                        case -5:
                            MainOutMessage("您想邀请加入编组的人已经加入了其它组！");
                            break;
                    }
                    break;
                case Grobal2.SM_GROUPDELMEM_OK:
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    break;
                case Grobal2.SM_GROUPDELMEM_FAIL:
                    MShare.g_dwChangeGroupModeTick = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("编组还未成立或者您还不够等级创建");
                            break;
                        case -2:
                            MainOutMessage("输入的人物名称不正确！");
                            break;
                        case -3:
                            MainOutMessage("此人不在本组中！");
                            break;
                    }
                    break;
                case Grobal2.SM_GROUPCANCEL:
                    MShare.g_GroupMembers.Clear();
                    break;
                case Grobal2.SM_GROUPMEMBERS:
                    ClientGetGroupMembers(EDCode.DeCodeString(body));
                    break;
                case Grobal2.SM_OPENGUILDDLG:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    break;
                case Grobal2.SM_SENDGUILDMEMBERLIST:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    break;
                case Grobal2.SM_OPENGUILDDLG_FAIL:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    MainOutMessage("您还没有加入行会！");
                    break;
                case Grobal2.SM_DEALTRY_FAIL:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    MainOutMessage("只有二人面对面才能进行交易");
                    break;
                case Grobal2.SM_DEALMENU:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    MShare.g_sDealWho = EDCode.DeCodeString(body);
                    break;
                case Grobal2.SM_DEALCANCEL:
                    ClFunc.MoveDealItemToBag();
                    if (MShare.g_DealDlgItem.Item.Name != "")
                    {
                        ClFunc.AddItemBag(MShare.g_DealDlgItem);
                        MShare.g_DealDlgItem.Item.Name = "";
                    }
                    if (MShare.g_nDealGold > 0)
                    {
                        MShare.g_MySelf.m_nGold = MShare.g_MySelf.m_nGold + MShare.g_nDealGold;
                        MShare.g_nDealGold = 0;
                    }
                    break;
                case Grobal2.SM_DEALADDITEM_OK:
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    if (MShare.g_DealDlgItem.Item.Name != "")
                    {
                        ClFunc.ResultDealItem(MShare.g_DealDlgItem, msg.Recog, msg.Param);
                        MShare.g_DealDlgItem.Item.Name = "";
                    }
                    break;
                case Grobal2.SM_DEALADDITEM_FAIL:
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    if (MShare.g_DealDlgItem.Item.Name != "")
                    {
                        ClFunc.AddItemBag(MShare.g_DealDlgItem);
                        MShare.g_DealDlgItem.Item.Name = "";
                    }
                    if (msg.Recog != 0)
                    {
                        // DScreen.AddChatBoardString("重叠失败,物品最高数量是 " + Grobal2.MAX_OVERLAPITEM.ToString(), Color.Red);
                    }
                    break;
                case Grobal2.SM_DEALDELITEM_OK:
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    if (MShare.g_DealDlgItem.Item.Name != "")
                    {
                        MShare.g_DealDlgItem.Item.Name = "";
                    }
                    break;
                case Grobal2.SM_DEALDELITEM_FAIL:
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    if (MShare.g_DealDlgItem.Item.Name != "")
                    {
                        ClFunc.AddDealItem(MShare.g_DealDlgItem);
                        MShare.g_DealDlgItem.Item.Name = "";
                    }
                    break;
                case Grobal2.SM_DEALREMOTEADDITEM:
                    ClientGetDealRemoteAddItem(body);
                    break;
                case Grobal2.SM_DEALREMOTEDELITEM:
                    ClientGetDealRemoteDelItem(body);
                    break;
                case Grobal2.SM_DEALCHGGOLD_OK:
                    MShare.g_nDealGold = msg.Recog;
                    MShare.g_MySelf.m_nGold = HUtil32.MakeLong(msg.Param, msg.Tag);
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    break;
                case Grobal2.SM_DEALCHGGOLD_FAIL:
                    MShare.g_nDealGold = msg.Recog;
                    MShare.g_MySelf.m_nGold = HUtil32.MakeLong(msg.Param, msg.Tag);
                    MShare.g_dwDealActionTick = MShare.GetTickCount();
                    break;
                case Grobal2.SM_DEALREMOTECHGGOLD:
                    MShare.g_nDealRemoteGold = msg.Recog;
                    break;
                case Grobal2.SM_SENDUSERSTORAGEITEM:
                    ClientGetSendUserStorage(msg.Recog);
                    break;
                case Grobal2.SM_READMINIMAP_OK:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    break;
                case Grobal2.SM_READMINIMAP_FAIL:
                    MShare.g_dwQueryMsgTick = MShare.GetTickCount();
                    DScreen.AddChatBoardString("没有小地图", ConsoleColor.Red);
                    break;
                case Grobal2.SM_CHANGEGUILDNAME:
                    ClientGetChangeGuildName(EDCode.DeCodeString(body));
                    break;
                case Grobal2.SM_SENDUSERSTATE:
                    ClientGetSendUserState(body);
                    break;
                case Grobal2.SM_GUILDADDMEMBER_OK:
                    SendGuildMemberList();
                    break;
                case Grobal2.SM_GUILDADDMEMBER_FAIL:
                    switch (msg.Recog)
                    {
                        case 1:
                            MainOutMessage("你没有权利使用这个命令");
                            break;
                        case 2:
                            MainOutMessage("想加入进来的成员应该来面对掌门人");
                            break;
                        case 3:
                            MainOutMessage("对方已经加入我们的行会");
                            break;
                        case 4:
                            MainOutMessage("对方已经加入其他行会");
                            break;
                        case 5:
                            MainOutMessage("对方不允许加入行会");
                            break;
                    }
                    break;
                case Grobal2.SM_GUILDDELMEMBER_OK:
                    SendGuildMemberList();
                    break;
                case Grobal2.SM_GUILDDELMEMBER_FAIL:
                    switch (msg.Recog)
                    {
                        case 1:
                            MainOutMessage("不能使用命令！");
                            break;
                        case 2:
                            MainOutMessage("此人非本行会成员！");
                            break;
                        case 3:
                            MainOutMessage("行会掌门人不能开除自己！");
                            break;
                        case 4:
                            MainOutMessage("不能使用命令！");
                            break;
                    }
                    break;
                case Grobal2.SM_GUILDRANKUPDATE_FAIL:
                    switch (msg.Recog)
                    {
                        case -2:
                            MainOutMessage("[提示信息] 掌门人位置不能为空");
                            break;
                        case -3:
                            MainOutMessage("[提示信息] 新的行会掌门人已经被传位");
                            break;
                        case -4:
                            MainOutMessage("[提示信息] 一个行会最多只能有二个掌门人");
                            break;
                        case -5:
                            MainOutMessage("[提示信息] 掌门人位置不能为空");
                            break;
                        case -6:
                            MainOutMessage("[提示信息] 不能添加成员/删除成员");
                            break;
                        case -7:
                            MainOutMessage("[提示信息] 职位重复或者出错");
                            break;
                    }
                    break;
                case Grobal2.SM_GUILDMAKEALLY_OK:
                case Grobal2.SM_GUILDMAKEALLY_FAIL:
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("您无此权限！");
                            break;
                        case -2:
                            MainOutMessage("结盟失败！");
                            break;
                        case -3:
                            MainOutMessage("行会结盟必须双方掌门人面对面！");
                            break;
                        case -4:
                            MainOutMessage("对方行会掌门人不允许结盟！");
                            break;
                    }
                    break;
                case Grobal2.SM_GUILDBREAKALLY_OK:
                case Grobal2.SM_GUILDBREAKALLY_FAIL:
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("解除结盟！");
                            break;
                        case -2:
                            MainOutMessage("此行会不是您行会的结盟行会！");
                            break;
                        case -3:
                            MainOutMessage("没有此行会！");
                            break;
                    }
                    break;
                case Grobal2.SM_BUILDGUILD_OK:
                    LastestClickTime = MShare.GetTickCount();
                    MainOutMessage("行会建立成功");
                    break;
                case Grobal2.SM_BUILDGUILD_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    switch (msg.Recog)
                    {
                        case -1:
                            MainOutMessage("您已经加入其它行会");
                            break;
                        case -2:
                            MainOutMessage("缺少创建费用");
                            break;
                        case -3:
                            MainOutMessage("你没有准备好需要的全部物品");
                            break;
                        default:
                            MainOutMessage("创建行会失败！！！");
                            break;
                    }
                    break;
                case Grobal2.SM_MENU_OK:
                    LastestClickTime = MShare.GetTickCount();
                    if (body != "")
                    {
                        MainOutMessage(EDCode.DeCodeString(body));
                    }
                    break;
                case Grobal2.SM_DLGMSG:
                    if (body != "")
                    {
                        MainOutMessage(EDCode.DeCodeString(body));
                    }
                    break;
                case Grobal2.SM_DONATE_OK:
                    LastestClickTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_DONATE_FAIL:
                    LastestClickTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_PLAYDICE:
                    //n = HUtil32.GetCodeMsgSize(sizeof(TMessageBodyWL) * 4 / 3);
                    body2 = body.Substring(n + 1 - 1, body.Length);
                    data = EDCode.DeCodeString(body2);
                    body2 = body.Substring(1 - 1, n);
                    wl = EDCode.DecodeBuffer<MessageBodyWL>(body2);
                    //FrmDlg.m_nDiceCount = msg.Param;
                    //FrmDlg.m_Dice[0].nDicePoint = HUtil32.LoByte(HUtil32.LoWord(wl.lParam1));
                    //FrmDlg.m_Dice[1].nDicePoint = HUtil32.HiByte(HUtil32.LoWord(wl.lParam1));
                    //FrmDlg.m_Dice[2].nDicePoint = HUtil32.LoByte(HUtil32.HiWord(wl.lParam1));
                    //FrmDlg.m_Dice[3].nDicePoint = HUtil32.HiByte(HUtil32.HiWord(wl.lParam1));
                    //FrmDlg.m_Dice[4].nDicePoint = HUtil32.LoByte(HUtil32.LoWord(wl.lParam2));
                    //FrmDlg.m_Dice[5].nDicePoint = HUtil32.HiByte(HUtil32.LoWord(wl.lParam2));
                    //FrmDlg.m_Dice[6].nDicePoint = HUtil32.LoByte(HUtil32.HiWord(wl.lParam2));
                    //FrmDlg.m_Dice[7].nDicePoint = HUtil32.HiByte(HUtil32.HiWord(wl.lParam2));
                    //FrmDlg.m_Dice[8].nDicePoint = HUtil32.LoByte(HUtil32.LoWord(wl.lTag1));
                    //FrmDlg.m_Dice[9].nDicePoint = HUtil32.HiByte(HUtil32.LoWord(wl.lTag1));
                    //FrmDlg.DialogSize = 0;
                    SendMerchantDlgSelect(msg.Recog, data);
                    break;
                case Grobal2.SM_PASSWORDSTATUS:
                    ClientGetPasswordStatus(msg, body);
                    break;
                default:
                    break;
                    // if g_MySelf = nil then Exit;
                    // g_PlayScene.MemoLog.Lines.Add('Ident: ' + IntToStr(Msg.ident));
                    // g_PlayScene.MemoLog.Lines.Add('Recog: ' + IntToStr(Msg.Recog));
                    // g_PlayScene.MemoLog.Lines.Add('Param: ' + IntToStr(Msg.param));
                    // g_PlayScene.MemoLog.Lines.Add('Tag: ' + IntToStr(Msg.tag));
                    // g_PlayScene.MemoLog.Lines.Add('Series: ' + IntToStr(Msg.series));
            }
        }

        private void ClientGetNeedUpdateAccount(string body)
        {
            //TUserEntry ue = EDcode.DecodeBuffer<TUserEntry>(body);
            //LoginScene.UpdateAccountInfos(ue);
        }

        private void ClientGetMapDescription(ClientMesaagePacket msg, string sBody)
        {
            sBody = EDCode.DeCodeString(sBody);
            var sTitle = sBody;
            MShare.g_sMapTitle = sTitle;
            LoadWayPoint();
            if (!MShare.g_gcGeneral[11])
            {
                MShare.g_nLastMapMusic = msg.Recog;
            }
            else
            {
                if (msg.Recog == -1)
                {
                    MShare.g_nLastMapMusic = -1;
                }
                if (MShare.g_nLastMapMusic != msg.Recog)
                {
                    MShare.g_nLastMapMusic = msg.Recog;
                }
            }
        }

        private void ClientGetGameGoldName(ClientMesaagePacket msg, string sBody)
        {
            var sData = string.Empty;
            if (sBody != "")
            {
                sBody = EDCode.DeCodeString(sBody);
                sBody = HUtil32.GetValidStr3(sBody, ref sData, new char[] { '\r' });
                MShare.g_sGameGoldName = sData;
                MShare.g_sGamePointName = sBody;
            }
            MShare.g_MySelf.m_nGameGold = msg.Recog;
            MShare.g_MySelf.m_nGamePoint = HUtil32.MakeLong(msg.Param, msg.Tag);
        }

        private void ClientGetAdjustBonus(int bonus, string body)
        {
            //string str1;
            //string Str2;
            //string str3;
            //MShare.g_nBonusPoint = bonus;
            //body = HUtil32.GetValidStr3(body, ref str1, HUtil32.Backslash);
            //str3 = HUtil32.GetValidStr3(body, ref Str2, HUtil32.Backslash);
            //EDcode.DecodeBuffer(str1, MShare.g_BonusTick);
            //EDcode.DecodeBuffer(Str2, MShare.g_BonusAbil);
            //EDcode.DecodeBuffer(str3, MShare.g_NakedAbil);
        }

        private void ClientGetAddItem(int Hint, string body)
        {
            if (!string.IsNullOrEmpty(body))
            {
                var cu = EDCode.DecodeBuffer<ClientItem>(body);
                ClFunc.AddItemBag(cu);
                if (Hint != 0)
                {
                    DScreen.AddSysMsg(cu.Item.Name + " 被发现");
                }
            }
        }

        private void ClientGetUpdateItem(string body)
        {
            if (!string.IsNullOrEmpty(body))
            {
                var cu = EDCode.DecodeBuffer<ClientItem>(body);
                ClFunc.UpdateItemBag(cu);
                for (var i = 0; i < MShare.g_UseItems.Length; i++)
                {
                    if ((MShare.g_UseItems[i].Item.Name == cu.Item.Name) && (MShare.g_UseItems[i].MakeIndex == cu.MakeIndex))
                    {
                        MShare.g_UseItems[i] = cu;
                    }
                }
                if ((MShare.g_SellDlgItem.Item.Name != "") && (MShare.g_SellDlgItem.MakeIndex == cu.MakeIndex))
                {
                    MShare.g_SellDlgItem = cu;
                }
                for (var i = 0; i < 1; i++)
                {
                    if ((MShare.g_TIItems[i].Item.MakeIndex == cu.MakeIndex) && (MShare.g_TIItems[i].Item.Item.Name != ""))
                    {
                        MShare.g_TIItems[i].Item = cu;
                        if (i == 0)
                        {
                            MShare.GetTIHintString1(1, MShare.g_TIItems[0].Item);
                        }
                    }
                }
                MShare.AutoPutOntiBooks();
                for (var i = 0; i < 1; i++)
                {
                    if ((MShare.g_spItems[i].Item.MakeIndex == cu.MakeIndex) && (MShare.g_spItems[i].Item.Item.Name != ""))
                    {
                        MShare.g_spItems[i].Item = cu;
                    }
                }
            }
        }

        private void ClientGetDelItem(string body)
        {
            if (body != "")
            {
                var cu = EDCode.DecodeBuffer<ClientItem>(body);
                ClFunc.DelItemBag(cu.Item.Name, cu.MakeIndex);
                for (var i = 0; i < MShare.g_UseItems.Length; i++)
                {
                    if ((MShare.g_UseItems[i].Item.Name == cu.Item.Name) && (MShare.g_UseItems[i].MakeIndex == cu.MakeIndex))
                    {
                        MShare.g_UseItems[i].Item.Name = "";
                    }
                }
                for (var i = 0; i < 1; i++)
                {
                    if (MShare.g_TIItems[i].Item.MakeIndex == cu.MakeIndex)
                    {
                        MShare.g_TIItems[i].Item.Item.Name = "";
                        if (i == 0)
                        {
                            MShare.GetTIHintString1(0);
                        }
                    }
                }
                for (var i = 0; i < 1; i++)
                {
                    if (MShare.g_spItems[i].Item.MakeIndex == cu.MakeIndex)
                    {
                        MShare.g_spItems[i].Item.Item.Name = "";
                    }
                }
            }
        }

        private void ClientGetDelItems(string body, ushort wOnlyBag)
        {
            int iindex;
            var Str = string.Empty;
            var iname = string.Empty;
            var cu = new ClientItem();
            body = EDCode.DeCodeString(body);
            while (body != "")
            {
                body = HUtil32.GetValidStr3(body, ref iname, HUtil32.Backslash);
                body = HUtil32.GetValidStr3(body, ref Str, HUtil32.Backslash);
                if ((iname != "") && (!string.IsNullOrEmpty(Str)))
                {
                    iindex = HUtil32.StrToInt(Str, 0);
                    ClFunc.DelItemBag(iname, iindex);
                    if (wOnlyBag == 0)
                    {
                        for (var i = 0; i < MShare.g_UseItems.Length; i++)
                        {
                            if ((MShare.g_UseItems[i].Item.Name == iname) && (MShare.g_UseItems[i].MakeIndex == iindex))
                            {
                                MShare.g_UseItems[i].Item.Name = "";
                                break;
                            }
                        }
                    }
                    for (var i = 0; i < 1; i++)
                    {
                        if (MShare.g_TIItems[i].Item.MakeIndex == cu.MakeIndex)
                        {
                            MShare.g_TIItems[i].Item.Item.Name = "";
                            if (i == 0)
                            {
                                MShare.GetTIHintString1(0);
                            }
                        }
                    }
                    for (var i = 0; i < 1; i++)
                    {
                        if (MShare.g_spItems[i].Item.MakeIndex == cu.MakeIndex)
                        {
                            MShare.g_spItems[i].Item.Item.Name = "";
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public bool ClientGetBagItmes_CompareItemArr(ClientItem[] ItemSaveArr)
        {
            var flag = true;
            for (var i = 0; i < MShare.MAXBAGITEMCL; i++)
            {
                if (ItemSaveArr[i].Item.Name != "")
                {
                    flag = false;
                    for (var j = 0; j < MShare.MAXBAGITEMCL; j++)
                    {
                        if ((MShare.g_ItemArr[j].Item.Name == ItemSaveArr[i].Item.Name) && (MShare.g_ItemArr[j].MakeIndex == ItemSaveArr[i].MakeIndex))
                        {
                            if ((MShare.g_ItemArr[j].Dura == ItemSaveArr[i].Dura) && (MShare.g_ItemArr[j].DuraMax == ItemSaveArr[i].DuraMax))
                            {
                                flag = true;
                            }
                            break;
                        }
                    }
                    if (!flag)
                    {
                        break;
                    }
                }
            }
            if (flag)
            {
                for (var i = 0; i < MShare.MAXBAGITEMCL; i++)
                {
                    if (MShare.g_ItemArr[i].Item.Name != "")
                    {
                        flag = false;
                        for (var j = 0; j < MShare.MAXBAGITEMCL; j++)
                        {
                            if ((MShare.g_ItemArr[i].Item.Name == ItemSaveArr[j].Item.Name) && (MShare.g_ItemArr[i].MakeIndex == ItemSaveArr[j].MakeIndex))
                            {
                                if ((MShare.g_ItemArr[i].Dura == ItemSaveArr[j].Dura) && (MShare.g_ItemArr[i].DuraMax == ItemSaveArr[j].DuraMax))
                                {
                                    flag = true;
                                }
                                break;
                            }
                        }
                        if (!flag)
                        {
                            break;
                        }
                    }
                }
            }
            var result = flag;
            return result;
        }

        private void ClientGetBagItmes(string body)
        {
            int k;
            var Str = string.Empty;
            ClientItem cu;
            var ItemSaveArr = new ClientItem[MShare.MAXBAGITEMCL - 1 + 1];
            //MShare.g_SellDlgItem.Item.Name = "";
            //FillChar(MShare.g_RefineItems, sizeof(TMovingItem) * 3, '\0');
            //FillChar(MShare.g_BuildAcuses, sizeof(MShare.g_BuildAcuses), '\0');
            //FillChar(MShare.g_ItemArr * MShare.MAXBAGITEMCL, '\0');
            //FillChar(MShare.g_TIItems, sizeof(MShare.g_TIItems), '\0');
            //FillChar(MShare.g_spItems, sizeof(MShare.g_spItems), '\0');
            if (MShare.g_MovingItem != null)
            {
                if ((MShare.g_MovingItem.Item.Item.Name != "") && MShare.IsBagItem(MShare.g_MovingItem.Index))
                {
                    MShare.g_MovingItem.Item.Item.Name = "";
                    MShare.g_boItemMoving = false;
                }
            }
            while (true)
            {
                if (string.IsNullOrEmpty(body))
                {
                    break;
                }
                body = HUtil32.GetValidStr3(body, ref Str, HUtil32.Backslash);
                cu = EDCode.DecodeBuffer<ClientItem>(Str);
                ClFunc.AddItemBag(cu);
            }
            //ClFunc.Loadbagsdat(".\\Config\\" + MShare.g_sServerName + "." + m_sChrName + ".itm-plus", ItemSaveArr);
            //if (ClientGetBagItmes_CompareItemArr())
            //{
            //    Move(ItemSaveArr, MShare.g_ItemArr * MShare.MAXBAGITEMCL);
            //}
            ClFunc.ArrangeItembag();
            MShare.g_boBagLoaded = true;
            if (MShare.g_MySelf != null)
            {
                if (!MShare.g_MySelf.m_StallMgr.OnSale)
                {
                    for (var i = 0; i < 9; i++)
                    {
                        if (MShare.g_MySelf.m_StallMgr.mBlock.Items[i] == null)
                        {
                            continue;
                        }
                        if (MShare.g_MySelf.m_StallMgr.mBlock.Items[i].Item.Name != "")
                        {
                            ClFunc.UpdateBagStallItem(MShare.g_MySelf.m_StallMgr.mBlock.Items[i], 4);
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < 9; i++)
                    {
                        if (MShare.g_MySelf.m_StallMgr.mBlock.Items[i] == null)
                        {
                            continue;
                        }
                        if (MShare.g_MySelf.m_StallMgr.mBlock.Items[i].Item.Name != "")
                        {
                            ClFunc.UpdateBagStallItem(MShare.g_MySelf.m_StallMgr.mBlock.Items[i], 5);
                        }
                    }
                }
            }
            if (MShare.g_boOpenAutoPlay && (MShare.g_nAPReLogon == 4))
            {
                MShare.g_nAPReLogon = 0;
                MShare.g_nOverAPZone = MShare.g_nOverAPZone2;
                MShare.g_APGoBack = MShare.g_APGoBack2;
                if (MShare.g_APMapPath2 != null)
                {
                    MShare.g_APMapPath = new Point[MShare.g_APMapPath2.Length + 1];
                    for (k = 0; k <= MShare.g_APMapPath2.Length; k++)
                    {
                        MShare.g_APMapPath[k] = MShare.g_APMapPath2[k];
                    }
                }
                MShare.g_APLastPoint = MShare.g_APLastPoint2;
                MShare.g_APStep = MShare.g_APStep2;
                MShare.g_gcAss[0] = true;
                MShare.g_APTagget = null;
                MShare.g_AutoPicupItem = null;
                MShare.g_nAPStatus = -1;
                MShare.g_nTargetX = -1;
                MShare.g_APGoBack2 = false;
                MShare.g_APMapPath2 = null;
                GetNearPoint();
                TimerAutoPlay.Enabled = MShare.g_gcAss[0];
                DScreen.AddChatBoardString("开始自动挂机...", ConsoleColor.Red);
                SaveWayPoint();
            }
        }

        private void ClientGetDropItemFail(string iname, int sindex)
        {
            var pc = ClFunc.GetDropItem(iname, sindex);
            if (pc != null)
            {
                ClFunc.AddItemBag(pc);
                ClFunc.DelDropItem(iname, sindex);
            }
        }

        private void ClientGetShowItem(int itemid, short X, short Y, int looks, string itmname)
        {
            TDropItem DropItem;
            for (var i = 0; i < MShare.g_DropedItemList.Count; i++)
            {
                if (MShare.g_DropedItemList[i].id == itemid)
                {
                    return;
                }
            }
            DropItem = new TDropItem();
            DropItem.id = itemid;
            DropItem.X = X;
            DropItem.Y = Y;
            DropItem.looks = looks;
            DropItem.Name = itmname;
            DropItem.Width = itmname.Length;
            DropItem.Height = itmname.Length;
            HUtil32.GetValidStr3(DropItem.Name, ref itmname, new string[] { "\\" });
            DropItem.FlashTime = MShare.GetTickCount() - RandomNumber.GetInstance().Random(3000);
            DropItem.BoFlash = false;
            DropItem.boNonSuch = false;
            DropItem.boShowName = MShare.g_ShowItemList.ContainsKey(itmname);
            DropItem.boPickUp = DropItem.boShowName;
            if (MShare.g_gcAss[5])
            {
                DropItem.boNonSuch = false;
                DropItem.boPickUp = false;
                DropItem.boShowName = false;
                //i = MShare.g_APPickUpList.IndexOf(itmname);
                //if (i >= 0)
                //{
                //    DropItem.boNonSuch = ((int)MShare.g_APPickUpList.Values[i]) != 0;
                //    DropItem.boPickUp = true;
                //    if (!DropItem.boNonSuch)
                //    {
                //        DropItem.boShowName = true;
                //    }
                //}
            }
            else
            {
                //P = (TCItemRule)MShare.g_ItemsFilter_All.GetValues(itmname);
                //if (P != null)
                //{
                //    DropItem.boNonSuch = P.rare;
                //    DropItem.boPickUp = P.pick;
                //    DropItem.boShowName = P.Show;
                //}
            }
            MShare.g_DropedItemList.Add(DropItem);
        }

        private void ClientGetHideItem(int itemid, int X, int Y)
        {
            TDropItem DropItem;
            for (var i = 0; i < MShare.g_DropedItemList.Count; i++)
            {
                DropItem = MShare.g_DropedItemList[i];
                if (DropItem.id == itemid)
                {
                    DropItem = null;
                    MShare.g_DropedItemList.RemoveAt(i);
                    break;
                }
            }
        }

        private void ClientGetSendUseItems(string body)
        {
            int Index;
            var Str = string.Empty;
            var data = string.Empty;
            ClientItem cu;
            while (true)
            {
                if (string.IsNullOrEmpty(body))
                {
                    break;
                }
                body = HUtil32.GetValidStr3(body, ref Str, HUtil32.Backslash);
                body = HUtil32.GetValidStr3(body, ref data, HUtil32.Backslash);
                Index = HUtil32.StrToInt(Str, -1);
                if (Index >= 0 && Index <= 13)
                {
                    cu = EDCode.DecodeBuffer<ClientItem>(data);
                    MShare.g_UseItems[Index] = cu;
                }
            }
        }

        public int ClientGetAddMagic_ListSortCompareLevel(object Item1, object Item2)
        {
            var result = 1;
            if (((ClientMagic)Item1).Def.TrainLevel[0] < ((ClientMagic)Item2).Def.TrainLevel[0])
            {
                result = -1;
            }
            else if (((ClientMagic)Item1).Def.TrainLevel[0] == ((ClientMagic)Item2).Def.TrainLevel[0])
            {
                result = 0;
            }
            return result;
        }

        private void ClientGetAddMagic(string body)
        {
            var pcm = EDCode.DecodeBuffer<ClientMagic>(body);
            MShare.g_MagicArr[pcm.Def.MagicId] = pcm;
            MShare.g_MagicList.Add(pcm);
            for (var i = 0; i < MShare.g_MagicList.Count; i++)
            {
                if (MShare.g_MagicList[i].Def.MagicId == 67)
                {
                    //MShare.g_MagicList.Move(i, 0);
                    break;
                }
            }
        }

        private void ClientGetDelMagic(int magid, int btclass)
        {
            for (var i = MShare.g_MagicList.Count - 1; i >= 0; i--)
            {
                if (MShare.g_MagicList[i].Def.MagicId == magid)
                {
                    MShare.g_MagicList[i] = null;
                    MShare.g_MagicList.RemoveAt(i);
                    break;
                }
            }
            MShare.g_MagicArr[magid] = null;
        }

        public int ClientConvertMagic_ListSortCompareLevel(object Item1, object Item2)
        {
            var result = 1;
            if (((ClientMagic)Item1).Def.TrainLevel[0] < ((ClientMagic)Item2).Def.TrainLevel[0])
            {
                result = -1;
            }
            else if (((ClientMagic)Item1).Def.TrainLevel[0] == ((ClientMagic)Item2).Def.TrainLevel[0])
            {
                result = 0;
            }
            return result;
        }

        private void ClientConvertMagic(int t1, int t2, int id1, int id2, string S)
        {
            //int i;
            //TClientMagic cm;
            //TClientMagic pcm;
            //EDcode.DecodeBuffer(S, cm);
            //if (t1 == 0)
            //{
            //    for (i = MShare.g_MagicList2.Count - 1; i >= 0; i--)
            //    {
            //        pcm = (TClientMagic)MShare.g_MagicList2[i];
            //        if (pcm.Def.wMagicID == id1)
            //        {
            //            pcm = cm;
            //            if (t1 == t2)
            //            {
            //                MShare.g_MagicArr[t1][id1] = null;
            //                MShare.g_MagicArr[t1][id2] = pcm;
            //            }
            //            else
            //            {
            //                MShare.g_MagicList2.RemoveAt(i);
            //                MShare.g_MagicList2.Sort(ClientConvertMagic_ListSortCompareLevel);
            //                MShare.g_IPMagicList.Add(pcm);
            //                MShare.g_MagicArr[t1][id1] = null;
            //                MShare.g_MagicArr[t2][id2] = pcm;
            //            }
            //            break;
            //        }
            //    }
            //    for (i = MShare.g_MagicList.Count - 1; i >= 0; i--)
            //    {
            //        pcm = (TClientMagic)MShare.g_MagicList[i];
            //        if (pcm.Def.wMagicID == id1)
            //        {
            //            pcm = cm;
            //            if (t1 == t2)
            //            {
            //                MShare.g_MagicArr[t1][id1] = null;
            //                MShare.g_MagicArr[t1][id2] = pcm;
            //            }
            //            else
            //            {
            //                MShare.g_MagicList.RemoveAt(i);
            //                MShare.g_IPMagicList.Add(pcm);
            //                MShare.g_MagicArr[t1][id1] = null;
            //                MShare.g_MagicArr[t2][id2] = pcm;
            //            }
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    for (i = MShare.g_IPMagicList.Count - 1; i >= 0; i--)
            //    {
            //        pcm = (TClientMagic)MShare.g_IPMagicList[i];
            //        if (pcm.Def.wMagicID == id1)
            //        {
            //            pcm = cm;
            //            if (t1 == t2)
            //            {
            //                MShare.g_MagicArr[t1][id1] = null;
            //                MShare.g_MagicArr[t1][id2] = pcm;
            //            }
            //            else
            //            {
            //                MShare.g_IPMagicList.RemoveAt(i);
            //                MShare.g_MagicList.Add(pcm);
            //            }
            //            MShare.g_MagicArr[t1][id1] = null;
            //            MShare.g_MagicArr[t2][id2] = pcm;
            //        }
            //        break;
            //    }
            //}
        }

        public int hClientConvertMagic_ListSortCompareLevel(object Item1, object Item2)
        {
            var result = 1;
            if (((ClientMagic)Item1).Def.TrainLevel[0] < ((ClientMagic)Item2).Def.TrainLevel[0])
            {
                result = -1;
            }
            else if (((ClientMagic)Item1).Def.TrainLevel[0] == ((ClientMagic)Item2).Def.TrainLevel[0])
            {
                result = 0;
            }
            return result;
        }

        public int ClientGetMyMagics_ListSortCompareLevel(object Item1, object Item2)
        {
            var result = 1;
            if (((ClientMagic)Item1).Def.TrainLevel[0] < ((ClientMagic)Item2).Def.TrainLevel[0])
            {
                result = -1;
            }
            else if (((ClientMagic)Item1).Def.TrainLevel[0] == ((ClientMagic)Item2).Def.TrainLevel[0])
            {
                result = 0;
            }
            return result;
        }

        private void ClientGetMyMagics(string body)
        {
            var data = string.Empty;
            for (var i = 0; i < MShare.g_MagicList.Count; i++)
            {
                MShare.g_MagicList[i] = null;
            }
            MShare.g_MagicList.Clear();
            while (true)
            {
                if (string.IsNullOrEmpty(body))
                {
                    break;
                }
                body = HUtil32.GetValidStr3(body, ref data, HUtil32.Backslash);
                if (data != "")
                {
                    var pcm = EDCode.DecodeBuffer<ClientMagic>(data);
                    MShare.g_MagicList.Add(pcm);
                    MShare.g_MagicArr[pcm.Def.MagicId] = pcm;
                }
                else
                {
                    break;
                }
            }
            for (var i = 0; i < MShare.g_MagicList.Count; i++)
            {
                if (MShare.g_MagicList[i].Def.MagicId == 67)
                {
                    //MShare.g_MagicList.Move(i, 0);
                    break;
                }
            }
        }

        private void ClientGetMagicLvExp(int magid, int maglv, int magtrain)
        {
            magid = HUtil32.LoWord(magid);
            var pcm = MShare.g_MagicArr[magid];
            if (pcm != null)
            {
                pcm.Level = (byte)maglv;
                pcm.CurTrain = magtrain;
            }
        }

        private void ClientGetMagicMaxLv(int magid, int magMaxlv, int hero)
        {
            magid = HUtil32.LoWord(magid);
            if (hero == 0)
            {
                var pcm = MShare.g_MagicArr[magid];
                if ((magid <= 0) || (magid >= 255))
                {
                    return;
                }
                if (pcm != null)
                {
                    pcm.Def.TrainLv = (byte)magMaxlv;
                }
            }
        }

        private void ClientGetDuraChange(int uidx, int newdura, int newduramax)
        {
            //if (uidx >= 0 && uidx <= Grobal2.U_FASHION)
            //{
            //    if (MShare.g_UseItems[uidx].Item.Name != "")
            //    {
            //        MShare.g_UseItems[uidx].Dura = newdura;
            //        MShare.g_UseItems[uidx].DuraMax = newduramax;
            //    }
            //}
        }

        private void ClientGetMerchantSay(int merchant, int face, string saying)
        {
            //string npcname;
            //MShare.g_nMDlgX = MShare.g_MySelf.m_nCurrX;
            //MShare.g_nMDlgY = MShare.g_MySelf.m_nCurrY;
            //if (MShare.g_nCurMerchant != merchant)
            //{
            //    MShare.g_nCurMerchant = merchant;
            //    FrmDlg.ResetMenuDlg;
            //    FrmDlg.CloseMDlg;
            //}
            //saying = HUtil32.GetValidStr3(saying, ref npcname, HUtil32.Backslash);
            //FrmDlg.ShowMDlg(face, npcname, saying);
        }

        private void ClientGetSendGoodsList(int merchant, int count, string body)
        {
            //string gname;
            //string gsub;
            //string gprice;
            //string gstock;
            //TClientGoods pcg;
            //FrmDlg.ResetMenuDlg;
            //MShare.g_nCurMerchant = merchant;
            //body = EDcode.DeCodeString(body);
            //while (body != "")
            //{
            //    body = HUtil32.GetValidStr3(body, ref gname, HUtil32.Backslash);
            //    body = HUtil32.GetValidStr3(body, ref gsub, HUtil32.Backslash);
            //    body = HUtil32.GetValidStr3(body, ref gprice, HUtil32.Backslash);
            //    body = HUtil32.GetValidStr3(body, ref gstock, HUtil32.Backslash);
            //    if ((gname != "") && (gprice != "") && (gstock != ""))
            //    {
            //        pcg = new TClientGoods();
            //        pcg.Name = gname;
            //        pcg.SubMenu = HUtil32.Str_ToInt(gsub, 0);
            //        pcg.Price = HUtil32.Str_ToInt(gprice, 0);
            //        pcg.Stock = HUtil32.Str_ToInt(gstock, 0);
            //        pcg.Grade = -1;
            //        FrmDlg.MenuList.Add(pcg);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //FrmDlg.FrmDlg.ShowShopMenuDlg(FrmDlg.dmBuy);
            //FrmDlg.FrmDlg.CurDetailItem = "";
        }

        private void ClientGetDelCharList(int count, string body)
        {
            //string gname;
            //string gjob;
            //string gsex;
            //string glevel;
            //TDelChar pcg;
            //FrmDlg.ResetDelCharMenuDlg;
            //body = EDcode.DeCodeString(body);
            //while (body != "")
            //{
            //    body = HUtil32.GetValidStr3(body, ref gname, HUtil32.Backslash);
            //    body = HUtil32.GetValidStr3(body, ref gjob, HUtil32.Backslash);
            //    body = HUtil32.GetValidStr3(body, ref gsex, HUtil32.Backslash);
            //    body = HUtil32.GetValidStr3(body, ref glevel, HUtil32.Backslash);
            //    body = HUtil32.GetValidStr3(body, ref gsex, HUtil32.Backslash);
            //    if ((gname != "") && (glevel != "") && (gsex != ""))
            //    {
            //        pcg = new TDelChar();
            //        pcg.sChrName = gname;
            //        pcg.nLevel = HUtil32.Str_ToInt(glevel, 1);
            //        pcg.btJob = HUtil32.Str_ToInt(gjob, 0);
            //        pcg.btSex = HUtil32.Str_ToInt(gsex, 0);
            //        FrmDlg.m_DelCharList.Add(pcg);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //FrmDlg.FrmDlg.ShowDelCharInfoDlg;
        }

        private void ClientGetSendMakeDrugList(int merchant, string body)
        {
            //string gname;
            //string gsub;
            //string gprice;
            //string gstock;
            //TClientGoods pcg;
            //FrmDlg.ResetMenuDlg;
            //MShare.g_nCurMerchant = merchant;
            //body = EDcode.DeCodeString(body);
            //while (body != "")
            //{
            //    body = HUtil32.GetValidStr3(body, ref gname, HUtil32.Backslash);
            //    body = HUtil32.GetValidStr3(body, ref gsub, HUtil32.Backslash);
            //    body = HUtil32.GetValidStr3(body, ref gprice, HUtil32.Backslash);
            //    body = HUtil32.GetValidStr3(body, ref gstock, HUtil32.Backslash);
            //    if ((gname != "") && (gprice != "") && (gstock != ""))
            //    {
            //        pcg = new TClientGoods();
            //        pcg.Name = gname;
            //        pcg.SubMenu = HUtil32.Str_ToInt(gsub, 0);
            //        pcg.Price = HUtil32.Str_ToInt(gprice, 0);
            //        pcg.Stock = HUtil32.Str_ToInt(gstock, 0);
            //        pcg.Grade = -1;
            //        FrmDlg.MenuList.Add(pcg);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //FrmDlg.FrmDlg.ShowShopMenuDlg(FrmDlg.dmMakeDrug);
            //FrmDlg.FrmDlg.CurDetailItem = "";
            //FrmDlg.FrmDlg.BoMakeDrugMenu = true;
        }

        private void ClientGetSendUserSell(int merchant)
        {
            //FrmDlg.CloseDSellDlg;
            //MShare.g_nCurMerchant = merchant;
            //FrmDlg.SpotDlgMode = dmSell;
            //FrmDlg.ShowShopSellDlg;
        }

        private void ClientGetSendUserExchgBook(int merchant)
        {
            //FrmDlg.CloseDSellDlg;
            //MShare.g_nCurMerchant = merchant;
            //FrmDlg.SpotDlgMode = dmExchangeBook;
            //FrmDlg.ShowShopSellDlg;
        }

        private void ClientGetSendItemDlg(int merchant, string Str)
        {
            //FrmDlg.CloseDSellDlg;
            //MShare.g_nCurMerchant = merchant;
            //FrmDlg.SpotDlgStr = Str;
            //FrmDlg.SpotDlgMode = dmItemDlg;
            //FrmDlg.ShowShopSellDlg;
        }

        private void ClientGetSendBindItem(int merchant)
        {
            //FrmDlg.CloseDSellDlg;
            //MShare.g_nCurMerchant = merchant;
            //FrmDlg.SpotDlgMode = dmBindItem;
            //FrmDlg.ShowShopSellDlg;
        }

        private void ClientGetSendUnBindItem(int merchant)
        {
            //FrmDlg.CloseDSellDlg;
            //MShare.g_nCurMerchant = merchant;
            //FrmDlg.SpotDlgMode = dmUnBindItem;
            //FrmDlg.ShowShopSellDlg;
        }

        private void ClientGetSendUserRepair(int merchant)
        {
            //FrmDlg.CloseDSellDlg;
            //MShare.g_nCurMerchant = merchant;
            //FrmDlg.SpotDlgMode = dmRepair;
            //FrmDlg.ShowShopSellDlg;
        }

        private void ClientGetSendUserStorage(int merchant)
        {
            //FrmDlg.CloseDSellDlg;
            //MShare.g_nCurMerchant = merchant;
            //FrmDlg.SpotDlgMode = dmStorage;
            //FrmDlg.ShowShopSellDlg;
        }

        private void ClientGetSendUserMaketSell(int merchant)
        {
            //FrmDlg.CloseDSellDlg;
            //MShare.g_nCurMerchant = merchant;
            //FrmDlg.SpotDlgMode = dmMaketSell;
            //FrmDlg.ShowShopSellDlg;
            //FrmDlg.DItemMarketCloseClick(null, 0, 0);
        }

        private void ClientGetSaveItemList(int merchant, string bodystr)
        {
            //int i;
            //string data;
            //TClientItem pc;
            //TClientGoods pcg;
            //FrmDlg.ResetMenuDlg;
            //for (i = 0; i < MShare.g_SaveItemList.Count; i++)
            //{
            //    this.Dispose((TClientItem)MShare.g_SaveItemList[i]);
            //}
            //MShare.g_SaveItemList.Clear();
            //while (true)
            //{
            //    if (bodystr == "")
            //    {
            //        break;
            //    }
            //    bodystr = HUtil32.GetValidStr3(bodystr, ref data, HUtil32.Backslash);
            //    if (data != "")
            //    {
            //        pc = new TClientItem();
            //        EDcode.DecodeBuffer(data, pc);
            //        MShare.g_SaveItemList.Add(pc);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //MShare.g_nCurMerchant = merchant;
            //for (i = 0; i < MShare.g_SaveItemList.Count; i++)
            //{
            //    pcg = new TClientGoods();
            //    pcg.Name = ((TClientItem)MShare.g_SaveItemList[i]).Item.Name;
            //    pcg.SubMenu = 0;
            //    pcg.Price = ((TClientItem)MShare.g_SaveItemList[i]).MakeIndex;
            //    pcg.Stock = HUtil32.Round(((TClientItem)MShare.g_SaveItemList[i]).Dura / 1000);
            //    pcg.Grade = HUtil32.Round(((TClientItem)MShare.g_SaveItemList[i]).DuraMax / 1000);
            //    FrmDlg.MenuList.Add(pcg);
            //}
            //FrmDlg.FrmDlg.ShowShopMenuDlg(FrmDlg.dmGetSave);
            //FrmDlg.FrmDlg.BoStorageMenu = true;
        }

        private void ClientGetSendDetailGoodsList(int merchant, int count, int topline, string bodystr)
        {
            //int i;
            //string data;
            //TClientGoods pcg;
            //TClientItem pc;
            //FrmDlg.ResetMenuDlg;
            //MShare.g_nCurMerchant = merchant;
            //bodystr = EDcode.DeCodeString(bodystr);
            //while (true)
            //{
            //    if (bodystr == "")
            //    {
            //        break;
            //    }
            //    bodystr = HUtil32.GetValidStr3(bodystr, ref data, HUtil32.Backslash);
            //    if (data != "")
            //    {
            //        pc = new TClientItem();
            //        EDcode.DecodeBuffer(data, pc);
            //        MShare.g_MenuItemList.Add(pc);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            //// clear shop menu list
            //for (i = 0; i < MShare.g_MenuItemList.Count; i++)
            //{
            //    pcg = new TClientGoods();
            //    pcg.Name = ((TClientItem)MShare.g_MenuItemList[i]).Item.Name;
            //    pcg.SubMenu = 0;
            //    pcg.Price = ((TClientItem)MShare.g_MenuItemList[i]).DuraMax;
            //    pcg.Stock = ((TClientItem)MShare.g_MenuItemList[i]).MakeIndex;
            //    pcg.Grade = HUtil32.Round(((TClientItem)MShare.g_MenuItemList[i]).Dura / 1000);
            //    FrmDlg.MenuList.Add(pcg);
            //}
            //FrmDlg.FrmDlg.ShowShopMenuDlg(FrmDlg.dmDetailMenu);
            //FrmDlg.FrmDlg.BoDetailMenu = true;
            //FrmDlg.FrmDlg.MenuTopLine = topline;
        }

        private void ClientGetSendNotice(string body)
        {
            if (MShare.g_boOpenAutoPlay && (MShare.g_nAPReLogon == 3))
            {
                MShare.g_nAPReLogon = 4;
                SendClientMessage(Grobal2.CM_LOGINNOTICEOK, 0, 0, 0, MShare.CLIENTTYPE);
                return;
            }
            MainOutMessage("发送游戏公告");
            SendClientMessage(Grobal2.CM_LOGINNOTICEOK, HUtil32.GetTickCount(), 0, 0, 0);
        }

        private void ClientGetGroupMembers(string bodystr)
        {
            var memb = string.Empty;
            MShare.g_GroupMembers.Clear();
            while (true)
            {
                if (bodystr == "")
                {
                    break;
                }
                bodystr = HUtil32.GetValidStr3(bodystr, ref memb, HUtil32.Backslash);
                if (memb != "")
                {
                    MShare.g_GroupMembers.Add(memb);
                }
                else
                {
                    break;
                }
            }
        }

        public void MinTimerTimer(object Sender, EventArgs _e1)
        {
            for (var i = 0; i < g_PlayScene.m_ActorList.Count; i++)
            {
                if (IsGroupMember(g_PlayScene.m_ActorList[i].m_sUserName))
                {
                    g_PlayScene.m_ActorList[i].m_boGrouped = true;
                }
                else
                {
                    g_PlayScene.m_ActorList[i].m_boGrouped = false;
                }
            }
            for (var i = MShare.g_FreeActorList.Count - 1; i >= 0; i--)
            {
                if (MShare.GetTickCount() - MShare.g_FreeActorList[i].m_dwDeleteTime > 60 * 1000)
                {
                    MShare.g_FreeActorList[i] = null;
                    MShare.g_FreeActorList.RemoveAt(i);
                }
            }
        }

        public void CheckHackTimerTimer(object Sender)
        {

        }

        private void ClientGetDealRemoteAddItem(string body)
        {
            if (body != "")
            {
                var ci = EDCode.DecodeBuffer<ClientItem>(body);
                ClFunc.AddDealRemoteItem(ci);
            }
        }

        private void ClientGetDealRemoteDelItem(string body)
        {
            if (body != "")
            {
                var ci = EDCode.DecodeBuffer<ClientItem>(body);
                ClFunc.DelDealRemoteItem(ci);
            }
        }

        private void ClientGetChangeGuildName(string body)
        {
            var Str = HUtil32.GetValidStr3(body, ref MShare.g_sGuildName, HUtil32.Backslash);
            MShare.g_sGuildRankName = Str.Trim();
        }

        private void ClientGetSendUserState(string body)
        {
            //TUserStateInfo UserState;
            //THumTitle[] Titles;
            //FillChar(UserState, sizeof(TUserStateInfo), '\0');
            //EDcode.DecodeBuffer(body, UserState, sizeof(TUserStateInfo));
            //UserState.NameColor = GetRGB(UserState.NameColor);
            //ii = 0;
            //FillChar(Titles, sizeof(Titles), 0);
            //for (var i = Titles.GetLowerBound(0); i <= Titles.Length; i++)
            //{
            //    if (UserState.Titles[i].Index > 0)
            //    {
            //        Titles[ii] = UserState.Titles[i];
            //        ii++;
            //    }
            //}
        }

        private void ClientGetPasswordStatus(ClientMesaagePacket msg, string body)
        {

        }

        public void SendPassword(string sPassword, int nIdent)
        {
            var DefMsg = Grobal2.MakeDefaultMsg(Grobal2.CM_PASSWORD, 0, nIdent, 0, 0);
            SendSocket(EDCode.EncodeMessage(DefMsg) + EDCode.EncodeString(sPassword));
        }

        private void ClientGetServerConfig(ClientMesaagePacket msg, string sBody)
        {
            TimerAutoMove = new TimerAutoPlay();
            TimerAutoMove.Enabled = true;
            TimerAutoPlay = new TimerAutoPlay();
            TimerAutoPlay.Enabled = true;
            MShare.g_boOpenAutoPlay = true; //HUtil32.LoByte(HUtil32.LoWord(msg.Recog)) == 1;
            MShare.g_boSpeedRate = msg.Series != 0;
            MShare.g_boSpeedRateShow = MShare.g_boSpeedRate;
            MShare.g_boCanRunMon = HUtil32.HiByte(HUtil32.LoWord(msg.Recog)) == 1;
            MShare.g_boCanRunNpc = HUtil32.LoByte(HUtil32.HiWord(msg.Recog)) == 1;
            MShare.g_boCanRunAllInWarZone = HUtil32.HiByte(HUtil32.HiWord(msg.Recog)) == 1;
            sBody = EDCode.DeCodeString(sBody);
            var ClientConf = EDCode.DecodeBuffer<ClientConf>(sBody);
            //MShare.g_boCanRunHuman = ClientConf.boRunHuman;
            //MShare.g_boCanRunMon = ClientConf.boRunMon;
            //MShare.g_boCanRunNpc = ClientConf.boRunNpc;
            //MShare.g_boCanRunAllInWarZone = ClientConf.boWarRunAll;
            //MShare.g_boCanStartRun = ClientConf.boCanStartRun;
            //MShare.g_boParalyCanRun = ClientConf.boParalyCanRun;
            //MShare.g_boParalyCanWalk = ClientConf.boParalyCanWalk;
            //MShare.g_boParalyCanHit = ClientConf.boParalyCanHit;
            //MShare.g_boParalyCanSpell = ClientConf.boParalyCanSpell;
            //MShare.g_boShowRedHPLable = ClientConf.boShowRedHPLable;
            //MShare.g_boShowHPNumber = ClientConf.boShowHPNumber;
            //MShare.g_boShowJobLevel = ClientConf.boShowJobLevel;
            //MShare.g_boDuraAlert = ClientConf.boDuraAlert;
            //MShare.g_boMagicLock = ClientConf.boMagicLock;
        }

        public ClientMagic GetMagicByID(int magid)
        {
            if ((magid <= 0) || (magid >= 255))
            {
                return null;
            }
            return MShare.g_MagicArr[magid];
        }

        public void ProcessPacket(string str)
        {
            var data = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                while (str.Length >= 2)
                {
                    if (MShare.g_boMapMovingWait)
                    {
                        break;
                    }
                    if (str.IndexOf("!", StringComparison.Ordinal) <= 0)
                    {
                        break;
                    }
                    str = HUtil32.ArrestStringEx(str, "#", "!", ref data);
                    if (!string.IsNullOrEmpty(data))
                    {
                        DecodeMessagePacket(data, 0);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (MShare.g_SeriesSkillFire_100 && (MShare.g_MySelf != null) && MShare.g_MySelf.ActionFinished())
            {
                MShare.g_SeriesSkillFire_100 = false;
                MShare.g_nCurrentMagic2 = 1;
                MShare.g_nCurrentMagic = 888;
                UseMagic(MShare.g_nMouseX, MShare.g_nMouseY, MShare.g_MagicArr[MShare.g_SeriesSkillArr[0]], false, true);
            }
        }

        private void SmartChangePoison(ClientMagic pcm)
        {
            string Str;
            string cStr;
            if (MShare.g_MySelf == null)
            {
                return;
            }
            MShare.g_MySelf.m_btPoisonDecHealth = 0;
            if (new ArrayList(new int[] { 13, 30, 43, 55, 57 }).Contains(pcm.Def.MagicId))
            {
                Str = "符";
                cStr = "符";
            }
            else if (new ArrayList(new int[] { 6, 38 }).Contains(pcm.Def.MagicId))
            {
                if (MShare.g_MagicTarget != null)
                {
                    Str = "药";
                    MShare.g_boExchgPoison = !MShare.g_boExchgPoison;
                    if (MShare.g_boExchgPoison)
                    {
                        MShare.g_MySelf.m_btPoisonDecHealth = 1;
                        cStr = "灰";
                    }
                    else
                    {
                        MShare.g_MySelf.m_btPoisonDecHealth = 2;
                        cStr = "黄";
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
            if ((MShare.g_UseItems[Grobal2.U_BUJUK].Item.StdMode == 25) && (MShare.g_UseItems[Grobal2.U_BUJUK].Item.Shape != 6) && (MShare.g_UseItems[Grobal2.U_BUJUK].Item.Name.IndexOf(cStr) > 0))
            {
                return;
            }
            MShare.g_boCheckTakeOffPoison = false;
            MShare.g_WaitingUseItem.Index = Grobal2.U_BUJUK;
            for (var i = 6; i < MShare.MAXBAGITEMCL; i++)
            {
                if ((MShare.g_ItemArr[i].Item.NeedIdentify < 4) && (MShare.g_ItemArr[i].Item.StdMode == 25) && (MShare.g_ItemArr[i].Item.Shape != 6) && (MShare.g_ItemArr[i].Item.Name.IndexOf(Str) > 0) && (MShare.g_ItemArr[i].Item.Name.IndexOf(cStr) > 0))
                {
                    MShare.g_WaitingUseItem.Item = MShare.g_ItemArr[i];
                    MShare.g_ItemArr[i].Item.Name = "";
                    MShare.g_boCheckTakeOffPoison = true;
                    SendTakeOnItem(MShare.g_WaitingUseItem.Index, MShare.g_WaitingUseItem.Item.MakeIndex, MShare.g_WaitingUseItem.Item.Item.Name);
                    ClFunc.ArrangeItembag();
                    return;
                }
            }
            if (Str == "符")
            {
                DScreen.AddChatBoardString("你的[护身符]已经用完", ConsoleColor.Blue);
            }
            else if (MShare.g_boExchgPoison)
            {
                DScreen.AddChatBoardString("你的[灰色药粉]已经用完", ConsoleColor.Blue);
            }
            else
            {
                DScreen.AddChatBoardString("你的[黄色药粉]已经用完", ConsoleColor.Blue);
            }
        }

        public void TimerAutoMagicTimer(object Sender, EventArgs _e1)
        {
            ClientMagic pcm;
            if ((MShare.g_MySelf != null) && MShare.g_MySelf.m_StallMgr.OnSale)
            {
                return;
            }
            if ((MShare.g_MySelf != null) && MShare.g_boAutoSay && (MShare.g_MySelf.m_sAutoSayMsg != ""))
            {
                //if (MShare.GetTickCount() - FrmDlg.m_sAutoSayMsgTick > 30 * 1000)
                //{
                //    FrmDlg.m_sAutoSayMsgTick = MShare.GetTickCount();
                //    SendSay(MShare.g_MySelf.m_sAutoSayMsg);
                //}
            }
            if ((MShare.g_MySelf != null) && IsUnLockAction())
            {
                if (CanNextAction() && ServerAcceptNextAction())
                {
                    var nspeed = 0;
                    if (MShare.g_boSpeedRate)
                    {
                        nspeed = MShare.g_MagSpeedRate * 20;
                    }
                    if (MShare.GetTickCount() - MShare.g_dwLatestSpellTick > (MShare.g_dwSpellTime + MShare.g_dwMagicDelayTime - nspeed))
                    {
                        if (MShare.g_gcTec[4] && ((MShare.g_MySelf.m_nState & 0x00100000) == 0))
                        {
                            if (MShare.g_MagicArr[31] != null)
                            {
                                UseMagic(MShare.SCREENWIDTH / 2, MShare.SCREENHEIGHT / 2, MShare.g_MagicArr[31]);
                                return;
                            }
                        }
                        switch (MShare.g_MySelf.m_btJob)
                        {
                            case 0:
                                if (MShare.g_gcTec[3] && !MShare.g_boNextTimePursueHit)
                                {
                                    pcm = GetMagicByID(56);
                                    if (pcm != null)
                                    {
                                        UseMagic(MShare.SCREENWIDTH / 2, MShare.SCREENHEIGHT / 2, pcm);
                                        return;
                                    }
                                }
                                if (MShare.g_gcTec[11] && !MShare.g_boNextTimeSmiteLongHit2)
                                {
                                    pcm = GetMagicByID(113);
                                    if (pcm != null)
                                    {
                                        UseMagic(MShare.SCREENWIDTH / 2, MShare.SCREENHEIGHT / 2, pcm);
                                        return;
                                    }
                                }
                                if (MShare.g_gcTec[2] && !MShare.g_boNextTimeFireHit)
                                {
                                    pcm = GetMagicByID(26);
                                    if (pcm != null)
                                    {
                                        UseMagic(MShare.SCREENWIDTH / 2, MShare.SCREENHEIGHT / 2, pcm);
                                        return;
                                    }
                                }
                                if (MShare.g_gcTec[13] && !MShare.g_boCanSLonHit)
                                {
                                    pcm = GetMagicByID(66);
                                    if (pcm != null)
                                    {
                                        UseMagic(MShare.SCREENWIDTH / 2, MShare.SCREENHEIGHT / 2, pcm);
                                        return;
                                    }
                                }
                                if (MShare.g_gcTec[9] && !MShare.g_boNextTimeTwinHit)
                                {
                                    pcm = GetMagicByID(43);
                                    if (pcm != null)
                                    {
                                        UseMagic(MShare.SCREENWIDTH / 2, MShare.SCREENHEIGHT / 2, pcm);
                                        return;
                                    }
                                }
                                break;
                            case 2:
                                if (MShare.g_gcTec[6] && ((MShare.g_MySelf.m_nState & 0x00800000) == 0))
                                {
                                    pcm = GetMagicByID(18);
                                    if (pcm != null)
                                    {
                                        UseMagic(MShare.SCREENWIDTH / 2, MShare.SCREENHEIGHT / 2, pcm);
                                    }
                                }
                                break;
                        }
                        if (MShare.g_gcTec[7] && (MShare.GetTickCount() - MShare.g_MySelf.m_dwPracticeTick > HUtil32._MAX(500, MShare.g_gnTecTime[8])))
                        {
                            MShare.g_MySelf.m_dwPracticeTick = MShare.GetTickCount();
                            pcm = GetMagicByID(MShare.g_gnTecPracticeKey);
                            if (pcm != null)
                            {
                                UseMagic(MShare.g_nMouseX, MShare.g_nMouseY, pcm);
                            }
                        }
                    }
                }
            }
        }

        public int DirToDX(int Direction, int tdir)
        {
            int result;
            if (Direction == -1)
            {
                Direction = 7;
            }
            switch (Direction)
            {
                case 0:
                case 4:
                    result = 0;
                    break;
                // Modify the A .. B: 1 .. 3
                case 1:
                    result = 1 * tdir;
                    break;
                default:
                    result = -1 * tdir;
                    break;
            }
            return result;
        }

        public int DirToDY(int Direction, int tdir)
        {
            int result;
            if (Direction == -1)
            {
                Direction = 7;
            }
            switch (Direction)
            {
                case 2:
                case 6:
                    result = 0;
                    break;
                // Modify the A .. B: 3 .. 5
                case 3:
                    result = 1 * tdir;
                    break;
                default:
                    result = -1 * tdir;
                    break;
            }
            return result;
        }

        public void TimerAutoMoveTimer(object Sender, EventArgs _e1)
        {
            var ndir = 0;
            short X1 = 0;
            short Y1 = 0;
            short X2 = 0;
            short Y2 = 0;
            short X3 = 0;
            short Y3 = 0;
            bool boCanRun;
            //if ((MShare.g_MySelf == null) || (Map.m_MapBuf == null) || (!CSocket.Active))
            //{
            //    return;
            //}
            if (TPathMap.g_MapPath != null)
            {
                if ((MShare.g_MySelf.m_nCurrX == MShare.g_MySelf.m_nTagX) && (MShare.g_MySelf.m_nCurrY == MShare.g_MySelf.m_nTagY))
                {
                    TimerAutoMove.Enabled = false;
                    DScreen.AddChatBoardString("已经到达终点", GetRGB(5));
                    TPathMap.g_MapPath = Array.Empty<Point>();
                    TPathMap.g_MapPath = null;
                    MShare.g_MySelf.m_nTagX = 0;
                    MShare.g_MySelf.m_nTagY = 0;
                }
                if (CanNextAction() && ServerAcceptNextAction() && IsUnLockAction())
                {
                    if (g_MoveStep <= TPathMap.g_MapPath.Length)
                    {
                        MShare.g_nTargetX = (short)TPathMap.g_MapPath[g_MoveStep].X;
                        MShare.g_nTargetY = (short)TPathMap.g_MapPath[g_MoveStep].X;
                        while ((Math.Abs(MShare.g_MySelf.m_nCurrX - MShare.g_nTargetX) <= 1) && (Math.Abs(MShare.g_MySelf.m_nCurrY - MShare.g_nTargetY) <= 1))
                        {
                            boCanRun = false;
                            if (g_MoveStep + 1 <= TPathMap.g_MapPath.Length)
                            {
                                X1 = MShare.g_MySelf.m_nCurrX;
                                Y1 = MShare.g_MySelf.m_nCurrY;
                                X2 = (short)TPathMap.g_MapPath[g_MoveStep + 1].X;
                                Y2 = (short)TPathMap.g_MapPath[g_MoveStep + 1].X;
                                ndir = ClFunc.GetNextDirection(X1, Y1, X2, Y2);
                                ClFunc.GetNextPosXY((byte)ndir, ref X1, ref Y1);
                                X3 = MShare.g_MySelf.m_nCurrX;
                                Y3 = MShare.g_MySelf.m_nCurrY;
                                ClFunc.GetNextRunXY(ndir, ref X3, ref Y3);
                                if ((TPathMap.g_MapPath[g_MoveStep + 1].X == X3) && (TPathMap.g_MapPath[g_MoveStep + 1].X == Y3))
                                {
                                    boCanRun = true;
                                }
                            }
                            if (boCanRun && Map.CanMove(X1, Y1) && !g_PlayScene.CrashMan(X1, Y1))
                            {
                                g_MoveStep++;
                                MShare.g_nTargetX = (short)TPathMap.g_MapPath[g_MoveStep].X;
                                MShare.g_nTargetY = (short)TPathMap.g_MapPath[g_MoveStep].X;
                                if (g_MoveStep >= TPathMap.g_MapPath.Length)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                MShare.g_nTargetX = (short)TPathMap.g_MapPath[g_MoveStep].X;
                                MShare.g_nTargetY = (short)TPathMap.g_MapPath[g_MoveStep].X;
                                break;
                            }
                        }
                        if ((Math.Abs(MShare.g_MySelf.m_nCurrX - MShare.g_MySelf.m_nTagX) <= 1) && (Math.Abs(MShare.g_MySelf.m_nCurrY - MShare.g_MySelf.m_nTagY) <= 1))
                        {
                            MShare.g_nTargetX = MShare.g_MySelf.m_nTagX;
                            MShare.g_nTargetY = MShare.g_MySelf.m_nTagY;
                        }
                        if ((Math.Abs(MShare.g_MySelf.m_nCurrX - MShare.g_nTargetX) <= 1) && (Math.Abs(MShare.g_MySelf.m_nCurrY - MShare.g_nTargetY) <= 1))
                        {
                            MShare.g_ChrAction = TChrAction.caWalk;// 目标座标
                            g_MoveBusy = true;
                        }
                        else
                        {
                            if (MShare.g_MySelf.CanRun() > 0)
                            {
                                MShare.g_ChrAction = TChrAction.caRun;
                                g_MoveBusy = true;
                            }
                            else
                            {
                                MShare.g_ChrAction = TChrAction.caWalk;
                                g_MoveBusy = true;
                            }
                        }
                    }
                }
            }
        }

        private void RunAutoPlayRandomTag(ref bool success, ref byte ndir)
        {
            var i = 0;
            success = false;
            ndir = MShare.g_MySelf.m_btDir;
            if (RandomNumber.GetInstance().Random(28) == 0)
            {
                ndir = (byte)RandomNumber.GetInstance().Random(8);
            }
            while (i < 16)
            {
                if (!GetNextPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, ndir, 2, ref MShare.g_nTargetX, ref MShare.g_nTargetY))
                {
                    ClFunc.GetFrontPosition(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, ndir, ref MShare.g_nTargetX, ref MShare.g_nTargetY);
                    if (!g_PlayScene.CanWalk(MShare.g_nTargetX, MShare.g_nTargetY))
                    {
                        MShare.g_MySelf.SendMsg(Grobal2.CM_TURN, (ushort)MShare.g_MySelf.m_nCurrX, (ushort)MShare.g_MySelf.m_nCurrY, RandomNumber.GetInstance().Random(8), 0, 0, "", 0);
                        i++;
                    }
                    else
                    {
                        success = true;
                        break;
                    }
                }
                else
                {
                    if (g_PlayScene.CanWalk(MShare.g_nTargetX, MShare.g_nTargetY))
                    {
                        success = true;
                        break;
                    }
                    ndir = (byte)RandomNumber.GetInstance().Random(8);
                    i++;
                }
            }
        }

        public void RunAutoPlay()
        {
            FindMapNode T;
            byte ndir = 0;
            short X1 = 0;
            short Y1 = 0;
            var success = false;
            MShare.g_sAPStr = "";
            MShare.g_boAPAutoMove = false;
            if (MShare.g_MySelf == null)
            {
                return;
            }
            if (!MShare.g_boOpenAutoPlay)
            {
                return;
            }
            if (MShare.g_MySelf.m_boDeath)
            {
                MShare.g_gcAss[0] = false;
                MShare.g_APTagget = null;
                MShare.g_AutoPicupItem = null;
                MShare.g_nAPStatus = -1;
                MShare.g_nTargetX = -1;
                TimerAutoPlay.Enabled = MShare.g_gcAss[0];
                return;
            }
            MShare.g_AutoPicupItem = null;
            switch (heroActor.GetAutoPalyStation())
            {
                case 0:
                    if (!EatItemName("回城卷") && !EatItemName("回城卷包") && !EatItemName("盟重传送石") && !EatItemName("比奇传送石"))
                    {
                        DScreen.AddChatBoardString("你的回城卷已用完,已挂机停止!!!", ConsoleColor.Red);
                    }
                    else
                    {
                        DScreen.AddChatBoardString("回城并挂机停止!!!", ConsoleColor.Red);
                    }
                    MShare.g_gcAss[0] = false;
                    MShare.g_APTagget = null;
                    MShare.g_AutoPicupItem = null;
                    MShare.g_nAPStatus = -1;
                    MShare.g_nTargetX = -1;
                    TimerAutoPlay.Enabled = MShare.g_gcAss[0];
                    MShare.g_boAPAutoMove = true;
                    break;
                case 1:// 此时为该怪物首次被发现，自动寻找路径
                    if (heroActor.AttackTagget(MShare.g_APTagget))
                    {
                        return;
                    }
                    if (MShare.g_APTagget != null)
                    {
                        MShare.g_nTargetX = MShare.g_APTagget.m_nCurrX;
                        MShare.g_nTargetY = MShare.g_APTagget.m_nCurrY;
                        heroActor.AutoFindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_APTagget.m_nCurrX, MShare.g_APTagget.m_nCurrY);
                    }
                    MShare.g_nTargetX = -1;
                    MShare.g_nAPStatus = 1;
                    MShare.g_boAPAutoMove = true;
                    break;
                case 2:// 此时该物品为首次发现，自动寻找路径
                    if ((MShare.g_AutoPicupItem != null) && ((MShare.g_nAPStatus != 2) || (MShare.g_APPathList.Count == 0)))
                    {
                        MShare.g_nTargetX = MShare.g_AutoPicupItem.X;
                        MShare.g_nTargetY = MShare.g_AutoPicupItem.Y;
                        heroActor.AutoFindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nTargetX, MShare.g_nTargetY);
                        MShare.g_nTargetX = -1;
                        MShare.g_sAPStr = $"物品目标：{MShare.g_AutoPicupItem.Name}({MShare.g_AutoPicupItem.X},{MShare.g_AutoPicupItem.Y}) 正在去拾取";
                    }
                    else if (MShare.g_AutoPicupItem != null)
                    {
                        MShare.g_nTargetX = MShare.g_AutoPicupItem.X;
                        MShare.g_nTargetY = MShare.g_AutoPicupItem.Y;
                        heroActor.AutoFindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nTargetX, MShare.g_nTargetY);
                        MShare.g_nTargetX = -1;
                        MShare.g_sAPStr = $"物品目标：{MShare.g_AutoPicupItem.Name}({MShare.g_AutoPicupItem.X},{MShare.g_AutoPicupItem.Y}) 正在去拾取";
                    }
                    MShare.g_nAPStatus = 2;
                    MShare.g_boAPAutoMove = true;
                    break;
                case 3:
                    if ((MShare.g_APMapPath != null) && (MShare.g_APStep >= 0) && (MShare.g_APStep <= MShare.g_APMapPath.Length))
                    {
                        if (MShare.g_APMapPath.Length > 0)
                        {
                            MShare.g_nTargetX = (short)MShare.g_APMapPath[MShare.g_APStep].X;
                            MShare.g_nTargetY = (short)MShare.g_APMapPath[MShare.g_APStep].X;
                            heroActor.AutoFindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nTargetX, MShare.g_nTargetY);
                            MShare.g_sAPStr = $"循路搜寻目标({MShare.g_nTargetX},{MShare.g_nTargetY})";
                            MShare.g_nTargetX = -1;
                        }
                        else
                        {
                            if ((MShare.g_nTargetX == -1) || (MShare.g_APPathList.Count == 0))
                            {
                                if (success)
                                {
                                    heroActor.AutoFindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nTargetX, MShare.g_nTargetY);
                                }
                                MShare.g_sAPStr = $"定点随机搜寻目标({MShare.g_APMapPath[MShare.g_APStep].X},{MShare.g_APMapPath[MShare.g_APStep].X})";
                                MShare.g_nTargetX = -1;
                            }
                        }
                    }
                    else if ((MShare.g_nTargetX == -1) || (MShare.g_APPathList.Count == 0))
                    {
                        RunAutoPlayRandomTag(ref success, ref ndir);
                        if (success)
                        {
                            heroActor.AutoFindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nTargetX, MShare.g_nTargetY);
                        }
                        MShare.g_sAPStr = "随机搜寻目标...";
                        MShare.g_nTargetX = -1;
                    }
                    MShare.g_nAPStatus = 3;
                    MShare.g_boAPAutoMove = true;
                    break;
                case 4:
                    if ((MShare.g_APMapPath != null) && (MShare.g_APStep >= 0) && (MShare.g_APStep <= MShare.g_APMapPath.Length))
                    {
                        if (MShare.g_APLastPoint.X >= 0)
                        {
                            MShare.g_nTargetX = (short)MShare.g_APLastPoint.X;
                            MShare.g_nTargetY = (short)MShare.g_APLastPoint.X;
                            heroActor.AutoFindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nTargetX, MShare.g_nTargetY);
                        }
                        else
                        {
                            MShare.g_nTargetX = (short)MShare.g_APMapPath[MShare.g_APStep].X;
                            MShare.g_nTargetY = (short)MShare.g_APMapPath[MShare.g_APStep].X;
                            heroActor.AutoFindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nTargetX, MShare.g_nTargetY);
                        }
                        MShare.g_sAPStr = $"超出搜寻范围,返回({MShare.g_nTargetX},{MShare.g_nTargetY})";
                        MShare.g_nTargetX = -1;
                    }
                    else if ((MShare.g_nTargetX == -1) || (MShare.g_APPathList.Count == 0))
                    {
                        RunAutoPlayRandomTag(ref success, ref ndir);
                        if (success)
                        {
                            heroActor.AutoFindPath(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nTargetX, MShare.g_nTargetY);
                        }
                        MShare.g_sAPStr = $"超出搜寻范围,随机搜寻目标({MShare.g_nTargetX},{MShare.g_nTargetY})";
                        MShare.g_nTargetX = -1;
                    }
                    MShare.g_nAPStatus = 3;
                    MShare.g_boAPAutoMove = true;
                    break;
            }
            if ((MShare.g_APPathList.Count > 0) && ((MShare.g_nTargetX == -1) || ((MShare.g_nTargetX == MShare.g_MySelf.m_nCurrX) && (MShare.g_nTargetY == MShare.g_MySelf.m_nCurrY))))
            {
                T = MShare.g_APPathList[0];
                MShare.g_nTargetX = (short)T.X;
                MShare.g_nTargetY = (short)T.Y;
                if (MShare.g_nAPStatus >= 1 && MShare.g_nAPStatus <= 4)
                {
                    if ((Math.Abs(MShare.g_MySelf.m_nCurrX - MShare.g_nTargetX) <= 1) && (Math.Abs(MShare.g_MySelf.m_nCurrY - MShare.g_nTargetY) <= 1))
                    {
                        if (g_PlayScene.CanWalk(MShare.g_nTargetX, MShare.g_nTargetY))
                        {
                            if ((MShare.g_nAPStatus == 2) && (MShare.g_AutoPicupItem != null))
                            {
                                if ((Math.Abs(MShare.g_MySelf.m_nCurrX - MShare.g_AutoPicupItem.X) > 1) || (Math.Abs(MShare.g_MySelf.m_nCurrY - MShare.g_AutoPicupItem.Y) > 1))
                                {
                                    goto AAAA;
                                }
                                else
                                {
                                    T = null;
                                    MShare.g_APPathList.RemoveAt(0);
                                    return;
                                }
                            }
                        AAAA:
                            if (MShare.g_APPathList.Count > 2)
                            {
                                ndir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_APPathList[2].X, MShare.g_APPathList[2].Y);
                            }
                            else
                            {
                                ndir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, MShare.g_nTargetX, MShare.g_nTargetY);
                            }
                            X1 = MShare.g_MySelf.m_nCurrX;
                            Y1 = MShare.g_MySelf.m_nCurrY;
                            ClFunc.GetNextRunXY(ndir, ref X1, ref Y1);
                            if (Map.CanMove(X1, Y1))
                            {
                                if (g_PlayScene.CrashMan(X1, Y1))
                                {
                                    MShare.g_nTargetX = (short)T.X;
                                    MShare.g_nTargetY = (short)T.Y;
                                    MShare.g_ChrAction = TChrAction.caWalk;
                                }
                                else
                                {
                                    MShare.g_nTargetX = X1;
                                    MShare.g_nTargetY = Y1;
                                    MShare.g_ChrAction = TChrAction.caRun;
                                }
                            }
                        }
                    }
                }
                T = null;
                MShare.g_APPathList.RemoveAt(0);
            }
            if (MShare.g_boAPAutoMove && (MShare.g_APPathList.Count > 0))
            {
                heroActor.Init_Queue2();
            }
            //Console.WriteLine(MShare.g_sAPStr);
        }

        private void ProcessActMsg(string datablock)
        {
            var data = string.Empty;
            var tagstr = string.Empty;
            if ((datablock[1] == 'G') && (datablock[2] == 'D') && (datablock[3] == '/'))
            {
                data = datablock.Substring(1, datablock.Length - 1);
                data = HUtil32.GetValidStr3(data, ref tagstr, HUtil32.Backslash);
                if (data != "")
                {
                    var rtime = HUtil32.StrToInt(data, 0);
                    if (rtime <= 0)
                    {
                        return;
                    }
                    if (MShare.g_rtime == rtime)
                    {
                        return;
                    }
                    MShare.g_rtime = rtime;
                    ActionLock = false;
                    g_MoveBusy = false;
                    g_MoveErr = 0;
                    if (TimerAutoMove.Enabled)
                    {
                        g_MoveStep++;
                    }
                    if (MShare.g_dwFirstServerTime > 0)
                    {
                        if ((MShare.GetTickCount() - MShare.g_dwFirstClientTime) > 10 * 60 * 1000)
                        {
                            MShare.g_dwFirstServerTime = rtime;
                            MShare.g_dwFirstClientTime = MShare.GetTickCount();
                        }
                        var cltime = MShare.GetTickCount() - MShare.g_dwFirstClientTime;
                        var svtime = rtime - MShare.g_dwFirstServerTime;
                        // DScreen.AddChatBoardString('[速度检测] 时间差：' + IntToStr(cltime - svtime), GetRGB(219), clWhite);
                        if (cltime > svtime + 4500)
                        {
                            MShare.g_nTimeFakeDetectCount++;
                            if (MShare.g_nTimeFakeDetectCount >= 3)
                            {
                                MainOutMessage("系统不稳定或网络状态极差，游戏被中止！如有问题请联系游戏管理员！");
                                //DScreen.Finalize();
                                //g_PlayScene.Finalize();
                                //LoginNoticeScene.Finalize();
                            }
                        }
                        else
                        {
                            if (Math.Abs(cltime - svtime) < 20)
                            {
                                MShare.g_nTimeFakeDetectCount = 0;
                            }
                            else if (Math.Abs(cltime - svtime) < 40)
                            {
                                if (MShare.g_nTimeFakeDetectCount > 1)
                                {
                                    MShare.g_nTimeFakeDetectCount -= 2;
                                }
                            }
                            else if (Math.Abs(cltime - svtime) < 80)
                            {
                                if (MShare.g_nTimeFakeDetectCount > 0)
                                {
                                    MShare.g_nTimeFakeDetectCount -= 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        MShare.g_dwFirstServerTime = rtime;
                        MShare.g_dwFirstClientTime = MShare.GetTickCount();
                    }
                }
                return;
            }
            else
            {
                tagstr = datablock.Substring(2 - 1, datablock.Length - 1);
            }
            switch (tagstr)
            {
                case "DIG":
                    MShare.g_MySelf.m_boDigFragment = true;
                    break;
                case "PWR":
                    MShare.g_boNextTimePowerHit = true;
                    break;
                case "LNG":
                    MShare.g_boCanLongHit = true;
                    break;
                case "ULNG":
                    MShare.g_boCanLongHit = false;
                    break;
                case "WID":
                    MShare.g_boCanWideHit = true;
                    break;
                case "UWID":
                    MShare.g_boCanWideHit = false;
                    break;
                case "STN":
                    MShare.g_boCanStnHit = true;
                    break;
                case "USTN":
                    MShare.g_boCanStnHit = false;
                    break;
                case "CRS":
                    MShare.g_boCanCrsHit = true;
                    DScreen.AddChatBoardString("双龙斩开启", GetRGB(219));
                    break;
                case "UCRS":
                    MShare.g_boCanCrsHit = false;
                    DScreen.AddChatBoardString("双龙斩关闭", GetRGB(219));
                    break;
                case "TWN":
                    MShare.g_boNextTimeTwinHit = true;
                    MShare.g_dwLatestTwinHitTick = MShare.GetTickCount();
                    DScreen.AddChatBoardString("召集雷电力量成功", GetRGB(219));
                    break;
                case "UTWN":
                    MShare.g_boNextTimeTwinHit = false;
                    DScreen.AddChatBoardString("雷电力量消失", GetRGB(219));
                    break;
                case "SQU":
                    MShare.g_boCanSquHit = true;
                    DScreen.AddChatBoardString("[龙影剑法] 开启", GetRGB(219));
                    break;
                case "FIR":
                    MShare.g_boNextTimeFireHit = true;
                    MShare.g_dwLatestFireHitTick = MShare.GetTickCount();
                    break;
                case "PUR":
                    MShare.g_boNextTimePursueHit = true;
                    MShare.g_dwLatestPursueHitTick = MShare.GetTickCount();
                    break;
                case "RSH":
                    MShare.g_boNextTimeRushHit = true;
                    MShare.g_dwLatestRushHitTick = MShare.GetTickCount();
                    break;
                case "SMI":
                    MShare.g_boNextTimeSmiteHit = true;
                    MShare.g_dwLatestSmiteHitTick = MShare.GetTickCount();
                    break;
                case "SMIL3":
                    MShare.g_boNextTimeSmiteLongHit3 = true;
                    MShare.g_dwLatestSmiteLongHitTick3 = MShare.GetTickCount();
                    DScreen.AddChatBoardString("[血魂一击] 已准备...", GetRGB(219));
                    break;
                case "SMIL":
                    MShare.g_boNextTimeSmiteLongHit = true;
                    MShare.g_dwLatestSmiteLongHitTick = MShare.GetTickCount();
                    break;
                case "SMIL2":
                    MShare.g_boNextTimeSmiteLongHit2 = true;
                    MShare.g_dwLatestSmiteLongHitTick2 = MShare.GetTickCount();
                    DScreen.AddChatBoardString("[断空斩] 已准备...", GetRGB(219));
                    break;
                case "SMIW":
                    MShare.g_boNextTimeSmiteWideHit = true;
                    MShare.g_dwLatestSmiteWideHitTick = MShare.GetTickCount();
                    break;
                case "SMIW2":
                    MShare.g_boNextTimeSmiteWideHit2 = true;
                    MShare.g_dwLatestSmiteWideHitTick2 = MShare.GetTickCount();
                    DScreen.AddChatBoardString("[倚天辟地] 已准备", ConsoleColor.Blue);
                    break;
                case "MDS":
                    DScreen.AddChatBoardString("[美杜莎之瞳] 技能可施展", ConsoleColor.Blue);
                    break;
                case "UFIR":
                    MShare.g_boNextTimeFireHit = false;
                    break;
                case "UPUR":
                    MShare.g_boNextTimePursueHit = false;
                    break;
                case "USMI":
                    MShare.g_boNextTimeSmiteHit = false;
                    break;
                case "URSH":
                    MShare.g_boNextTimeRushHit = false;
                    break;
                case "USMIL":
                    MShare.g_boNextTimeSmiteLongHit = false;
                    break;
                case "USML3":
                    MShare.g_boNextTimeSmiteLongHit3 = false;
                    break;
                case "USML2":
                    MShare.g_boNextTimeSmiteLongHit2 = false;
                    // DScreen.AddChatBoardString('[断空斩] 力量消失...', clWhite, clRed);
                    break;
                case "USMIW":
                    MShare.g_boNextTimeSmiteWideHit = false;
                    break;
                case "USMIW2":
                    MShare.g_boNextTimeSmiteWideHit2 = false;
                    break;
                case "USQU":
                    MShare.g_boCanSquHit = false;
                    DScreen.AddChatBoardString("[龙影剑法] 关闭", GetRGB(219));
                    break;
                case "SLON":
                    MShare.g_boCanSLonHit = true;
                    MShare.g_dwLatestSLonHitTick = MShare.GetTickCount();
                    DScreen.AddChatBoardString("[开天斩] 力量凝聚...", GetRGB(219));
                    break;
                case "USLON":
                    MShare.g_boCanSLonHit = false;
                    DScreen.AddChatBoardString("[开天斩] 力量消失", ConsoleColor.Red);
                    break;
            }
        }

        public int GetRGB(byte c256)
        {
            return 255;
        }

        private static void GetNearPoint()
        {
            if ((MShare.g_APMapPath != null) && (MShare.g_APMapPath.Length > 0))
            {
                var n14 = 0;
                MShare.g_APLastPoint.X = -1;
                var n10 = 999;
                for (var i = 0; i < MShare.g_APMapPath.Length; i++)
                {
                    var nC = Math.Abs(MShare.g_APMapPath[i].X - MShare.g_MySelf.m_nCurrX) + Math.Abs(MShare.g_APMapPath[i].X - MShare.g_MySelf.m_nCurrY);
                    if (nC < n10)
                    {
                        n10 = nC;
                        n14 = i;
                    }
                }
                MShare.g_APStep = n14;
            }
        }

        public bool GetNextPosition(short sx, short sy, int ndir, short nFlag, ref short snx, ref short sny)
        {
            bool result;
            snx = sx;
            sny = sy;
            switch (ndir)
            {
                case Grobal2.DR_UP:
                    if (sny > nFlag - 1)
                    {
                        sny -= nFlag;
                    }
                    break;
                case Grobal2.DR_DOWN:
                    if (sny < (Map.m_MapHeader.wHeight - nFlag))
                    {
                        sny += nFlag;
                    }
                    break;
                case Grobal2.DR_LEFT:
                    if (snx > nFlag - 1)
                    {
                        snx -= nFlag;
                    }
                    break;
                case Grobal2.DR_RIGHT:
                    if (snx < (Map.m_MapHeader.wWidth - nFlag))
                    {
                        snx += nFlag;
                    }
                    break;
                case Grobal2.DR_UPLEFT:
                    if ((snx > nFlag - 1) && (sny > nFlag - 1))
                    {
                        snx -= nFlag;
                        sny -= nFlag;
                    }
                    break;
                case Grobal2.DR_UPRIGHT:
                    if ((snx > nFlag - 1) && (sny < (Map.m_MapHeader.wHeight - nFlag)))
                    {
                        snx += nFlag;
                        sny -= nFlag;
                    }
                    break;
                case Grobal2.DR_DOWNLEFT:
                    if ((snx < (Map.m_MapHeader.wWidth - nFlag)) && (sny > nFlag - 1))
                    {
                        snx -= nFlag;
                        sny += nFlag;
                    }
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if ((snx < (Map.m_MapHeader.wWidth - nFlag)) && (sny < (Map.m_MapHeader.wHeight - nFlag)))
                    {
                        snx += nFlag;
                        sny += nFlag;
                    }
                    break;
            }
            if ((snx == sx) && (sny == sy))
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public int CheckMagPassThrough(short sx, short sy, short tx, short ty, int ndir)
        {
            var tCount = 0;
            for (var i = 0; i < 12; i++)
            {
                TActor Actor = g_PlayScene.FindActorXY(sx, sy);
                if (Actor != null)
                {
                    if (heroActor.IsProperTarget(Actor))
                    {
                        tCount++;
                    }
                }
                if (!((Math.Abs(sx - tx) <= 0) && (Math.Abs(sy - ty) <= 0)))
                {
                    ndir = ClFunc.GetNextDirection(sx, sy, tx, ty);
                    if (!GetNextPosition(sx, sy, ndir, 1, ref sx, ref sy))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return tCount;
        }

        public bool GetAdvPosition(TActor TargetCret, ref short nX, ref short nY)
        {
            var result = false;
            nX = MShare.g_MySelf.m_nCurrX;
            nY = MShare.g_MySelf.m_nCurrY;
            var btDir = ClFunc.GetNextDirection(MShare.g_MySelf.m_nCurrX, MShare.g_MySelf.m_nCurrY, TargetCret.m_nCurrX, TargetCret.m_nCurrY);
            var _wvar1 = MShare.g_MySelf;
            switch (btDir)
            {
                case Grobal2.DR_UP:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nY += 2;
                        nX += 2;
                    }
                    else
                    {
                        nY += 2;
                        nX -= 2;
                    }
                    if (!g_PlayScene.CanWalk(nX, nY))
                    {
                        nY = (short)(_wvar1.m_nCurrY + 2);
                    }
                    break;
                case Grobal2.DR_DOWN:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nY -= 2;
                        nX += 2;
                    }
                    else
                    {
                        nY -= 2;
                        nX -= 2;
                    }
                    if (!g_PlayScene.CanWalk(nX, nY))
                    {
                        nY = (short)(_wvar1.m_nCurrY - 2);
                    }
                    break;
                case Grobal2.DR_LEFT:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nX += 2;
                        nY += 2;
                    }
                    else
                    {
                        nX += 2;
                        nY -= 2;
                    }
                    if (!g_PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(_wvar1.m_nCurrX + 2);
                    }
                    break;
                case Grobal2.DR_RIGHT:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nX -= 2;
                        nY += 2;
                    }
                    else
                    {
                        nX -= 2;
                        nY -= 2;
                    }
                    if (!g_PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(_wvar1.m_nCurrX - 2);
                    }
                    break;
                case Grobal2.DR_UPLEFT:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nX += 2;
                    }
                    else
                    {
                        nY += 2;
                        nY -= 1;
                    }
                    if (!g_PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(_wvar1.m_nCurrX + 2);
                        nY = (short)(_wvar1.m_nCurrY + 2);
                    }
                    break;
                case Grobal2.DR_UPRIGHT:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nY += 2;
                        nY -= 1;
                    }
                    else
                    {
                        nX -= 2;
                        nX++;
                    }
                    if (!g_PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(_wvar1.m_nCurrX - 2);
                        nY = (short)(_wvar1.m_nCurrY + 2);
                    }
                    break;
                case Grobal2.DR_DOWNLEFT:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nX += 2;
                    }
                    else
                    {
                        nY -= 2;
                    }
                    if (!g_PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(_wvar1.m_nCurrX + 2);
                        nY = (short)(_wvar1.m_nCurrY - 2);
                    }
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nX -= 2;
                    }
                    else
                    {
                        nY -= 2;
                    }
                    if (!g_PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(_wvar1.m_nCurrX - 2);
                        nY = (short)(_wvar1.m_nCurrY - 2);
                    }
                    break;
            }
            return result;
        }

        public static void SaveWayPoint()
        {
            //int i;
            //string S;
            //FileStream ini;
            //if (MShare.g_APMapPath != null)
            //{
            //    try
            //    {
            //        ini = new FileStream(".\\Config\\" + MShare.g_sServerName + "." + MShare.g_MySelf.m_sUserName + ".WayPoint.txt");
            //        S = "";
            //        for (i = MShare.g_APMapPath.GetLowerBound(0); i <= MShare.g_APMapPath.Length; i++)
            //        {
            //            S = S + string.Format("%d,%d ", new int[] { MShare.g_APMapPath[i].X, MShare.g_APMapPath[i].X });
            //        }
            //        ini.WriteString(MShare.g_sMapTitle, "WayPoint", S);
            //        ini.Free;
            //    }
            //    catch
            //    {
            //    }
            //}
            //else
            //{
            //    if (MShare.g_MySelf != null)
            //    {
            //        try
            //        {
            //            if (!Directory.Exists(".\\Config\\"))
            //            {
            //                Directory.CreateDirectory(".\\Config\\");
            //            }
            //            ini = new FileStream(".\\Config\\" + MShare.g_sServerName + "." + MShare.g_MySelf.m_sUserName + ".WayPoint.txt");
            //            ini.WriteString(MShare.g_sMapTitle, "WayPoint", "");
            //            ini.Free;
            //        }
            //        catch
            //        {
            //        }
            //    }
            //}
        }

        public static void LoadWayPoint()
        {
            //string X;
            //string Y;
            //string S;
            //string ss;
            //FileStream ini;
            //MShare.g_APMapPath = null;
            //ini = new FileStream(".\\Config\\" + MShare.g_sServerName + "." + MShare.g_MySelf.m_sUserName + ".WayPoint.txt");
            //S = ini.ReadString(MShare.g_sMapTitle, "WayPoint", "");
            //while (true)
            //{
            //    if (S == "")
            //    {
            //        break;
            //    }
            //    S = HUtil32.GetValidStr3(S, ref ss, new string[] { " " });
            //    if (ss != "")
            //    {
            //        Y = HUtil32.GetValidStr3(ss, ref X, new string[] { "," });
            //        if (MShare.g_APMapPath == null)
            //        {
            //            MShare.g_APMapPath = new Point[1];
            //            MShare.g_APMapPath[0].X = Convert.ToInt32(X);
            //            MShare.g_APMapPath[0].X = Convert.ToInt32(Y);
            //        }
            //        else
            //        {
            //            MShare.g_APMapPath = new Point[MShare.g_APMapPath.Length + 2];
            //            MShare.g_APMapPath[MShare.g_APMapPath.Length].X = Convert.ToInt32(X);
            //            MShare.g_APMapPath[MShare.g_APMapPath.Length].X = Convert.ToInt32(Y);
            //        }
            //    }
            //}
        }

        public int GetMagicLv(TActor Actor, int magid)
        {
            if (Actor == null)
            {
                return 0;
            }
            if ((magid <= 0) || (magid >= 255))
            {
                return 0;
            }
            return MShare.g_MagicArr[magid] != null ? MShare.g_MagicArr[magid].Level : 0;
        }

        private void MainOutMessage(string msg)
        {
            Console.WriteLine($"账号:[{LoginID}] {msg}");
        }
    }

    public class TimerAutoPlay
    {
        public bool Enabled;
    }
}