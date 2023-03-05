using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BotSrv.Maps;
using BotSrv.Objects;
using BotSrv.Scenes;
using BotSrv.Scenes.Scene;
using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.Event;

namespace BotSrv.Player
{
    public partial class RobotPlayer
    {
        public readonly string SessionId;
        public TimerAutoPlay TimerAutoPlay;
        public TimerAutoPlay TimerAutoMove;
        public static int LastestClickTime = 0;
        public static bool GMoveBusy = false;
        public bool GPathBusy = false;
        public static int GMoveStep = 0;
        public static int GMoveErr = 0;
        public static int GMoveErrTick = 0;
        public ScreenManager DScreen = null;
        public IntroScene IntroScene = null;
        public LoginScene LoginScene = null;
        public SelectChrScene SelectChrScene = null;
        public PlayScene PlayScene = null;
        public TMap Map = null;
        public static Actor ShowMsgActor = null;
        public static long GDwOverSpaceWarningTick = 0;
        private const char Activebuf = '*';
        private readonly TTimerCommand _timerCmd;
        private int _actionLockTime = 0;
        private readonly short _actionKey = 0;
        private CommandMessage _waitingMsg;
        private string _waitingStr = string.Empty;
        private string _whisperName = string.Empty;
        private int _mDwProcUseMagicTick = 0;
        public static long g_dwOverSpaceWarningTick = 0;
        private readonly TTimerCommand TimerCmd;
        private int ActionLockTime = 0;
        private readonly short ActionKey = 0;
        private CommandMessage WaitingMsg;
        private string WaitingStr = string.Empty;
        private string WhisperName = string.Empty;
        private int m_dwProcUseMagicTick = 0;
        public bool ActionFailLock = false;
        public int ActionFailLockTime = 0;
        public int LastHitTick = 0;
        public bool NewAccount = false;
        public string LoginId = string.Empty;
        public string LoginPasswd = string.Empty;
        public string ChrName = string.Empty;
        public int Certification = 0;
        public int MNEatRetIdx = 0;
        public bool ActionLock = false;
        public bool MBoSupplyItem = false;
        public int MDwDuraWarningTick = 0;
        public int DwIpTick = 0;
        public int DwhIpTick = 0;
        private readonly HeroActor _heroActor;
        public ScoketClient ClientSocket;
        public int ConnectTick = 0;
        public ConnectionStatus ConnectionStatus;

        public RobotPlayer()
        {
            SessionId = Guid.NewGuid().ToString("N");
            _heroActor = new HeroActor(this);
            MShare.AutoPathList = new List<FindMapNode>();
            DScreen = new ScreenManager(this);
            IntroScene = new IntroScene(this);
            LoginScene = new LoginScene(this);
            SelectChrScene = new SelectChrScene(this);
            PlayScene = new PlayScene(this);
            Map = new TMap(this);
            MShare.g_DropedItemList = new List<TDropItem>();
            MShare.g_MagicList = new List<ClientMagic>();
            MShare.g_FreeActorList = new List<Actor>();
            //EventMan = new TClEventManager();
            MShare.g_ChangeFaceReadyList = new ArrayList();
            MShare.g_SendSayList = new ArrayList();
            if (MShare.MySelf != null)
            {
                MShare.MySelf.SlaveObject.Clear();
                MShare.MySelf = null;
            }
            MShare.InitClientItems();
            MShare.g_DetectItemMineID = 0;
            MShare.LastMapMusic = -1;
            MShare.TargetX = -1;
            MShare.TargetY = -1;
            MShare.TargetCret = null;
            MShare.FocusCret = null;
            MShare.g_FocusItem = null;
            MShare.MagicTarget = null;
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
            MShare.g_nAreaStateValue = 0;
            MShare.ConnectionStep = ConnectionStep.Intro;
            MShare.SendLogin = false;
            MShare.ServerConnected = false;
            MShare.g_SoftClosed = false;
            ActionFailLock = false;
            MShare.g_boMapMoving = false;
            MShare.MapMovingWait = false;
            MShare.g_boCheckBadMapMode = false;
            MShare.g_boCheckSpeedHackDisplay = false;
            MShare.g_nDupSelection = 0;
            MShare.LastAttackTick = MShare.GetTickCount();
            MShare.LastMoveTick = MShare.GetTickCount();
            MShare.LatestSpellTick = MShare.GetTickCount();
            MShare.AutoPickupTick = MShare.GetTickCount();
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
            ConnectionStatus = ConnectionStatus.Failure;
            for (var i = 0; i < BotConst.MAXX * 3; i++)
            {
                for (var j = 0; j < BotConst.MAXY * 3; j++)
                {
                    MShare.g_APPassEmpty[i, j] = 0xFF;
                }
            }
        }

        public void SocketEvents()
        {
            if (ClientSocket == null)
            {
                ClientSocket = new ScoketClient();
            }
            ClientSocket.OnConnected -= SocketConnect;
            ClientSocket.OnDisconnected -= SocketDisconnect;
            ClientSocket.OnReceivedData -= ReceivedData;
            ClientSocket.OnError -= SocketError;
            ClientSocket.OnConnected += SocketConnect;
            ClientSocket.OnDisconnected += SocketDisconnect;
            ClientSocket.OnReceivedData += ReceivedData;
            ClientSocket.OnError += SocketError;
            ClientSocket.Connect(MShare.RunServerAddr, MShare.RunServerPort);
        }

        #region Socket Events

        private void SocketConnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.ServerConnected = true;
            if (MShare.ConnectionStep == ConnectionStep.Login)
            {
                DScreen.ChangeScene(SceneType.Login);
            }
            if (MShare.ConnectionStep == ConnectionStep.Play)
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

        private void SocketDisconnect(object sender, DSCClientConnectedEventArgs e)
        {
            MShare.ServerConnected = false;
            if (MShare.g_SoftClosed)
            {
                MShare.g_SoftClosed = false;
                //ActiveCmdTimer(MShare.TTimerCommand.tcReSelConnect);
            }
            else if ((DScreen.CurrentScene == LoginScene) && !MShare.SendLogin)
            {
                MainOutMessage("游戏连接已关闭...");
            }
            if (DScreen.CurrentScene == PlayScene)
            {
                LoginOut();
                BotShare.ClientMgr.DelClient(SessionId);
            }
        }

        private void SocketError(object sender, DSCClientErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case System.Net.Sockets.SocketError.ConnectionRefused:
                    MainOutWarnMessage($"游戏服务器[{ClientSocket.RemoteEndPoint}]拒绝链接...");
                    break;
                case System.Net.Sockets.SocketError.ConnectionReset:
                    MainOutWarnMessage($"游戏服务器[{ClientSocket.RemoteEndPoint}]关闭连接...");
                    break;
                case System.Net.Sockets.SocketError.TimedOut:
                    MainOutWarnMessage($"游戏服务器[{ClientSocket.RemoteEndPoint}]链接超时...");
                    break;
            }
            if (DScreen.CurrentScene == PlayScene)
            {
                LoginOut();
                BotShare.ClientMgr.DelClient(SessionId);
            }
        }

        private void ReceivedData(object sender, DSCClientDataInEventArgs e)
        {
            var sData = HUtil32.GetString(e.Buff, 0, e.BuffLen);
            if (!string.IsNullOrEmpty(sData))
            {
                var n = sData.IndexOf("*", StringComparison.OrdinalIgnoreCase);
                if (n > 0)
                {
                    var data2 = sData[..(n - 1)];
                    sData = data2 + sData.Substring(n, sData.Length);
                    ClientSocket.SendBuffer(HUtil32.GetBytes(BotConst.Activebuf));
                }
                BotShare.ClientMgr.AddPacket(SessionId, sData);
            }
        }

        #endregion

        public void Run()
        {
            if (DScreen.CurrentScene == null)
            {
                DScreen.ChangeScene(SceneType.Login);
            }
            else
            {
                if (DScreen.CurrentScene == LoginScene)
                {
                    LoginScene.Login();
                }
                DScreen.CurrentScene.DoNotifyEvent();
                if (DScreen.CurrentScene == PlayScene)
                {
                    ProcessActionMessages();
                    if (MShare.MySelf != null)
                    {
                        PlayScene.BeginScene();
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
                SendClientMessage(Messages.CM_SOFTCLOSE, 0, 0, 0, 0);
                CloseAllWindows();
                PlayScene.ClearActors();
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
            SendClientMessage(Messages.CM_SOFTCLOSE, 0, 0, 0, 0);
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
                SendClientMessage(Messages.CM_SOFTCLOSE, 0, 0, 0, 0);
                MShare.SaveItemFilter();
            }
            finally
            {
                MShare.g_boQueryExit = false;
            }
        }

        private void ProcessMagic()
        {
            short nSx;
            short nSy;
            int tdir;
            int targid;
            short targx;
            short targy;
            //TUseMagicInfo pmag;
            if ((PlayScene.ProcMagic.NTargetX < 0) || (MShare.MySelf == null))
            {
                return;
            }
            if (MShare.GetTickCount() - PlayScene.ProcMagic.DwTick > 5000)
            {
                PlayScene.ProcMagic.DwTick = MShare.GetTickCount();
                PlayScene.ProcMagic.NTargetX = -1;
                return;
            }
            if (MShare.GetTickCount() - _mDwProcUseMagicTick > 28)
            {
                _mDwProcUseMagicTick = MShare.GetTickCount();
                if (PlayScene.ProcMagic.FUnLockMagic)
                {
                    targx = PlayScene.ProcMagic.NTargetX;
                    targy = PlayScene.ProcMagic.NTargetY;
                }
                else if ((PlayScene.ProcMagic.XTarget != null) && !PlayScene.ProcMagic.XTarget.Death)
                {
                    targid = PlayScene.ProcMagic.XTarget.RecogId;
                    targx = PlayScene.ProcMagic.XTarget.CurrX;
                    targy = PlayScene.ProcMagic.XTarget.CurrY;
                }
                else
                {
                    PlayScene.ProcMagic.NTargetX = -1;
                    return;
                }
                nSx = (short)Math.Abs(MShare.MySelf.CurrX - targx);
                nSy = (short)Math.Abs(MShare.MySelf.CurrY - targy);
                if ((nSx <= BotConst.MagicRange) && (nSy <= BotConst.MagicRange))
                {
                    if (PlayScene.ProcMagic.FContinue || (CanNextAction() && ServerAcceptNextAction()))
                    {
                        MShare.LatestSpellTick = MShare.GetTickCount();
                        tdir = ClFunc.GetFlyDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, targx, targy);
                        //pmag = new TUseMagicInfo();
                        //pmag.EffectNumber = g_PlayScene.ProcMagic.XMagic.Def.btEffect;
                        //pmag.MagicSerial = g_PlayScene.ProcMagic.XMagic.Def.wMagicID;
                        //pmag.ServerMagicCode = 0;
                        MShare.g_dwMagicDelayTime = 200 + PlayScene.ProcMagic.XMagic.Def.DelayTime;
                        MShare.g_dwMagicPKDelayTime = 0;
                        if (MShare.MagicTarget != null)
                        {
                            if (MShare.MagicTarget.Race == 0)
                            {
                                MShare.g_dwMagicPKDelayTime = 300 + RandomNumber.GetInstance().Random(1100);
                            }
                        }
                        //MShare.g_MySelf.SendMsg(Messages.CM_SPELL, targx, targy, tdir, pmag, targid, "", 0);
                        PlayScene.ProcMagic.NTargetX = -1;
                    }
                }
                else
                {
                    MShare.PlayerAction = PlayerAction.Run;
                    MShare.TargetX = targx;
                    MShare.TargetY = targy;
                }
            }
        }

        private void ProcessKeyMessages()
        {
            if (_actionKey == 0)
            {
                return;
            }
            if ((MShare.MySelf != null) && MShare.MySelf.StallMgr.OnSale)
            {
                return;
            }
        }

        private void ProcessActionMessages()
        {
            if (MShare.MySelf == null)
            {
                return;
            }
            if ((MShare.TargetX >= 0) && CanNextAction() && ServerAcceptNextAction())
            {
                if (MShare.OpenAutoPlay && (MShare.MapPath != null) && (MShare.AutoStep >= 0) && (0 < MShare.MapPath.Length))
                {
                    if ((Math.Abs(MShare.MapPath[MShare.AutoStep].X - MShare.MySelf.CurrX) <= 3) && (Math.Abs(MShare.MapPath[MShare.AutoStep].X - MShare.MySelf.CurrY) <= 3))
                    {
                        if (MShare.MapPath.Length >= 2)// 3点以上
                        {
                            if (MShare.AutoStep >= MShare.MapPath.Length) // 当前点在终点...
                            {
                                // 终点 <-> 起点 距离过远...
                                if ((Math.Abs(MShare.MapPath[MShare.MapPath.Length].X - MShare.MapPath[0].X) >= 36) || (Math.Abs(MShare.MapPath[MShare.MapPath.Length].X - MShare.MapPath[0].X) >= 36))
                                {
                                    MShare.g_APGoBack = true; // 原路返回
                                    MShare.AutoLastPoint = MShare.MapPath[MShare.AutoStep];
                                    MShare.AutoStep -= 1;
                                }
                                else
                                {
                                    MShare.g_APGoBack = false; // 循环到起点...
                                    MShare.AutoLastPoint = MShare.MapPath[MShare.AutoStep];
                                    MShare.AutoStep = 0;
                                }
                            }
                            else
                            {
                                if (MShare.g_APGoBack) // 原路返回
                                {
                                    MShare.AutoLastPoint = MShare.MapPath[MShare.AutoStep];
                                    MShare.AutoStep -= 1;
                                    if (MShare.AutoStep <= 0)// 已回到起点
                                    {
                                        MShare.AutoStep = 0;
                                        MShare.g_APGoBack = false;
                                    }
                                }
                                else
                                {
                                    MShare.AutoLastPoint = MShare.MapPath[MShare.AutoStep]; // 循环...
                                    MShare.AutoStep++;
                                }
                            }
                        }
                        else
                        {
                            // 2点,循环...
                            MShare.AutoLastPoint = MShare.MapPath[MShare.AutoStep];
                            MShare.AutoStep++;
                            if (MShare.AutoStep > MShare.MapPath.Length)
                            {
                                MShare.AutoStep = 0;
                            }
                        }
                    }
                }
                if ((MShare.TargetX != MShare.MySelf.CurrX) || (MShare.TargetY != MShare.MySelf.CurrY))
                {
                    if ((MShare.MySelf.m_nTagX > 0) && (MShare.MySelf.m_nTagY > 0))
                    {
                        if (GMoveBusy)
                        {
                            if (MShare.GetTickCount() - GMoveErrTick > 60)
                            {
                                GMoveErrTick = MShare.GetTickCount();
                                GMoveErr++;
                            }
                        }
                        else
                        {
                            GMoveErr = 0;
                        }
                        if (GMoveErr > 10)
                        {
                            GMoveErr = 0;
                            GMoveBusy = false;
                            TimerAutoMove.Enabled = false;
                            if ((MShare.MySelf.m_nTagX > 0) && (MShare.MySelf.m_nTagY > 0))
                            {
                                if (!GPathBusy)
                                {
                                    GPathBusy = true;
                                    try
                                    {
                                        Map.ReLoadMapData();
                                        PathMap.g_MapPath = Map.FindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.MySelf.m_nTagX, MShare.MySelf.m_nTagY, 0);
                                        if (PathMap.g_MapPath != null)
                                        {
                                            GMoveStep = 1;
                                            TimerAutoMove.Enabled = true;
                                        }
                                        else
                                        {
                                            MShare.MySelf.m_nTagX = 0;
                                            MShare.MySelf.m_nTagY = 0;
                                            ScreenManager.AddChatBoardString("自动移动出错，停止移动");
                                        }
                                    }
                                    finally
                                    {
                                        GPathBusy = false;
                                    }
                                }
                            }
                        }
                    }
                TTTT:
                    var mx = MShare.MySelf.CurrX;
                    var my = MShare.MySelf.CurrY;
                    var dx = MShare.TargetX;
                    var dy = MShare.TargetY;
                    var ndir = ClFunc.GetNextDirection(mx, my, dx, dy);
                    int crun;
                    byte mdir;
                    switch (MShare.PlayerAction)
                    {
                        case PlayerAction.Walk:
                        LB_WALK:
                            crun = MShare.MySelf.CanWalk();
                            if (IsUnLockAction() && (crun > 0))
                            {
                                ClFunc.GetNextPosXY(ndir, ref mx, ref my);
                                var bostop = false;
                                if (!PlayScene.CanWalk(mx, my))
                                {
                                    if (MShare.OpenAutoPlay && MShare.AutoMove && (MShare.AutoPathList.Count > 0))
                                    {
                                        _heroActor.InitQueue2();
                                        MShare.TargetX = -1;
                                    }
                                    var bowalk = false;
                                    byte adir = 0;
                                    if (!bowalk)
                                    {
                                        mx = MShare.MySelf.CurrX;
                                        my = MShare.MySelf.CurrY;
                                        ClFunc.GetNextPosXY(ndir, ref mx, ref my);
                                        if (CheckDoorAction(mx, my))
                                        {
                                            bostop = true;
                                        }
                                    }
                                    if (!bostop && (PlayScene.CrashMan(mx, my) || !Map.CanMove(mx, my)))
                                    {
                                        mx = MShare.MySelf.CurrX;
                                        my = MShare.MySelf.CurrY;
                                        adir = ClFunc.PrivDir(ndir);
                                        ClFunc.GetNextPosXY(adir, ref mx, ref my);
                                        if (!Map.CanMove(mx, my))
                                        {
                                            mx = MShare.MySelf.CurrX;
                                            my = MShare.MySelf.CurrY;
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
                                        MShare.MySelf.UpdateMsg(Messages.CM_WALK, (ushort)mx, (ushort)my, adir, 0, 0, "", 0);
                                        MShare.LastMoveTick = MShare.GetTickCount();
                                        if (MShare.g_nOverAPZone > 0)
                                        {
                                            MShare.g_nOverAPZone -= 1;
                                        }
                                    }
                                    else
                                    {
                                        mdir = ClFunc.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, dx, dy);
                                        if (mdir != MShare.MySelf.m_btDir)
                                        {
                                            MShare.MySelf.SendMsg(Messages.CM_TURN, (ushort)MShare.MySelf.CurrX, (ushort)MShare.MySelf.CurrY, mdir, 0, 0, "", 0);
                                        }
                                        MShare.TargetX = -1;
                                    }
                                }
                                else
                                {
                                    MShare.MySelf.UpdateMsg(Messages.CM_WALK, (ushort)mx, (ushort)my, ndir, 0, 0, "", 0);
                                    MShare.LastMoveTick = MShare.GetTickCount();
                                }
                            }
                            else
                            {
                                MShare.TargetX = -1;
                            }
                            break;
                        case PlayerAction.Run: // 免助跑
                            if (MShare.g_boCanStartRun || (MShare.g_nRunReadyCount >= 1))
                            {
                                crun = MShare.MySelf.CanRun();// 骑马开始
                                if ((MShare.MySelf.m_btHorse != 0) && (ClFunc.GetDistance(mx, my, dx, dy) >= 3) && (crun > 0) && IsUnLockAction())
                                {
                                    ClFunc.GetNextHorseRunXY(ndir, ref mx, ref my);
                                    if (PlayScene.CanRun(MShare.MySelf.CurrX, MShare.MySelf.CurrY, mx, my))
                                    {
                                        MShare.MySelf.UpdateMsg(Messages.CM_HORSERUN, (ushort)mx, (ushort)my, ndir, 0, 0, "", 0);
                                        MShare.LastMoveTick = MShare.GetTickCount();
                                        if (MShare.g_nOverAPZone > 0)
                                        {
                                            MShare.g_nOverAPZone -= 1;
                                        }
                                    }
                                    else
                                    {
                                        // 如果跑失败则跳回去走
                                        MShare.PlayerAction = PlayerAction.Walk;
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
                                            if (PlayScene.CanRun(MShare.MySelf.CurrX, MShare.MySelf.CurrY, mx, my))
                                            {
                                                MShare.MySelf.UpdateMsg(Messages.CM_RUN, (ushort)mx, (ushort)my, ndir, 0, 0, "", 0);
                                                MShare.LastMoveTick = MShare.GetTickCount();
                                                if (MShare.g_nOverAPZone > 0)
                                                {
                                                    MShare.g_nOverAPZone -= 1;
                                                }
                                            }
                                            else
                                            {
                                                // 如果跑失败则跳回去走
                                                MShare.PlayerAction = PlayerAction.Walk;
                                                goto TTTT;
                                            }
                                        }
                                        else
                                        {
                                            MShare.TargetX = -1;
                                        }
                                    }
                                    else
                                    {
                                        mdir = ClFunc.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, dx, dy);
                                        if (mdir != MShare.MySelf.m_btDir)
                                        {
                                            MShare.MySelf.SendMsg(Messages.CM_TURN, (ushort)MShare.MySelf.CurrX, (ushort)MShare.MySelf.CurrY, mdir, 0, 0, "", 0);
                                        }
                                        MShare.TargetX = -1;
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
                else if (MShare.OpenAutoPlay && MShare.AutoMove && (MShare.AutoPicupItem != null))
                {
                    SendPickup();
                    MainOutMessage("拾取物品");
                    if (MShare.AutoMove && (MShare.AutoPathList.Count > 0))
                    {
                        _heroActor.InitQueue2();
                        MShare.TargetX = -1;
                    }
                }
            }
            MShare.TargetX = -1;
            if (MShare.MySelf.RealActionMsg.Ident > 0)
            {
                if (MShare.MySelf.RealActionMsg.Ident == Messages.CM_SPELL)
                {
                    SendSpellMsg(MShare.MySelf.RealActionMsg.Ident, (ushort)MShare.MySelf.RealActionMsg.X, (ushort)MShare.MySelf.RealActionMsg.Y, MShare.MySelf.RealActionMsg.Dir, MShare.MySelf.RealActionMsg.State);
                }
                else
                {
                    SendActMsg(MShare.MySelf.RealActionMsg.Ident, (ushort)MShare.MySelf.RealActionMsg.X, (ushort)MShare.MySelf.RealActionMsg.Y, MShare.MySelf.RealActionMsg.Dir);
                }
                MShare.MySelf.RealActionMsg.Ident = 0;
                if (MShare.g_nStallX != -1)
                {
                    if ((Math.Abs(MShare.g_nStallX - MShare.MySelf.CurrX) >= 8) || (Math.Abs(MShare.g_nStallY - MShare.MySelf.CurrY) >= 8))
                    {
                        MShare.g_nStallX = -1;
                    }
                }
            }
        }

        public void OpenAutoPlay()
        {
            if (MShare.MySelf == null)
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
                MShare.AutoTagget = null;
                MShare.AutoPicupItem = null;
                MShare.g_nAPStatus = -1;
                MShare.TargetX = -1;
                MShare.g_APGoBack = false;
                ScreenManager.AddChatBoardString("开始自动挂机...");
                SaveWayPoint();
                if (MShare.MapPath != null)
                {
                    MShare.AutoStep = 0;
                    MShare.AutoLastPoint.X = -1;
                    GetNearPoint();
                }
            }
            else
            {
                ScreenManager.AddChatBoardString("停止自动挂机...");
            }
            return;
        }

        public ClientMagic GetMagicByKey(char key)
        {
            ClientMagic result = null;
            for (var i = 0; i < MShare.g_MagicList.Count; i++)
            {
                var pm = MShare.g_MagicList[i];
                if (pm.Key == key)
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
            //TUseMagicInfo pmag;
            bool fUnLockMagic;
            if ((MShare.MySelf != null))
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
            var spellSpend = (short)(HUtil32.Round(pcm.Def.Spell / (pcm.Def.TrainLv + 1) * (pcm.Level + 1)) + pcm.Def.DefSpell);
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
                defSpellSpend = MShare.MySelf.m_nIPower;
            }
            else
            {
                defSpellSpend = MShare.MySelf.Abil.MP;
            }
            if (spellSpend <= defSpellSpend)
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
                        case 26 when MShare.g_boNextTimeFireHit || (MShare.GetTickCount() - MShare.LatestFireHitTick <= 10 * 1000): // 烈火时间间隔
                        case 66 when MShare.g_boCanSLonHit || (MShare.GetTickCount() - MShare.LatestSLonHitTick <= 8 * 1000):
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
                    if (MShare.SpeedRate)
                    {
                        if (boContinue || (MShare.GetTickCount() - MShare.LatestSpellTick > MShare.SpellTime - ((long)MShare.g_MagSpeedRate) * 20))
                        {
                            MShare.LatestSpellTick = MShare.GetTickCount();
                            MShare.g_dwMagicDelayTime = 0;
                            SendSpellMsg(Messages.CM_SPELL, MShare.MySelf.m_btDir, 0, pcm.Def.MagicId, 0, false);
                        }
                    }
                    else
                    {
                        if (boContinue || (MShare.GetTickCount() - MShare.LatestSpellTick > MShare.SpellTime))
                        {
                            MShare.LatestSpellTick = MShare.GetTickCount();
                            MShare.g_dwMagicDelayTime = 0;
                            SendSpellMsg(Messages.CM_SPELL, MShare.MySelf.m_btDir, 0, pcm.Def.MagicId, 0, false);
                        }
                    }
                }
            labSpell:
                fUnLockMagic = new ArrayList(new[] { 2, 9, 10, 14, 21, 33, 37, 41, 46, 50, 58, 70, 72, 75 }).Contains(pcm.Def.MagicId);
                if (fUnLockMagic)
                {
                    MShare.MagicTarget = MShare.FocusCret;
                }
                else
                {
                    if (MShare.MagicLock && PlayScene.IsValidActor(MShare.FocusCret) && !MShare.FocusCret.Death)
                    {
                        MShare.MagicLockActor = MShare.FocusCret;
                    }
                    MShare.MagicTarget = MShare.MagicLockActor;
                }
                if (MShare.MagicTarget != null)
                {
                    if (!MShare.MagicLock || MShare.MagicTarget.Death || (MShare.MagicTarget.Race == ActorRace.Merchant) || !PlayScene.IsValidActor(MShare.MagicTarget))
                    {
                        MShare.MagicTarget = null;
                        MShare.MagicLockActor = null;
                    }
                }
                if ((MShare.MagicTarget != null) && (MShare.MagicTarget is THumActor))
                {
                    if (((THumActor)MShare.MagicTarget).StallMgr.OnSale)
                    {
                        MShare.MagicTarget = null;
                        MShare.MagicLockActor = null;
                    }
                }
                SmartChangePoison(pcm);
                int targid;
                if (MShare.MagicTarget == null)
                {
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
                        targx = MShare.MagicTarget.CurrX;
                        targy = MShare.MagicTarget.CurrY;
                    }
                    else
                    {
                        targx = tx;
                        targy = ty;
                    }
                    targid = MShare.MagicTarget.RecogId;
                }
                if ((Math.Abs(MShare.MySelf.CurrX - targx) > BotConst.MagicRange) || (Math.Abs(MShare.MySelf.CurrY - targy) > BotConst.MagicRange))
                {
                    if (MShare.g_gcTec[14] && (fUnLockMagic || (targid != 0)))
                    {
                        PlayScene.ProcMagic.NTargetX = (short)targx;
                        PlayScene.ProcMagic.NTargetY = (short)targy;
                        PlayScene.ProcMagic.XMagic = pcm;
                        PlayScene.ProcMagic.XTarget = MShare.MagicLockActor;
                        PlayScene.ProcMagic.FReacll = boReacll;
                        PlayScene.ProcMagic.FContinue = boContinue;
                        PlayScene.ProcMagic.FUnLockMagic = fUnLockMagic;
                        PlayScene.ProcMagic.DwTick = MShare.GetTickCount();
                    }
                    else
                    {
                        if (MShare.GetTickCount() - GDwOverSpaceWarningTick > 1000)
                        {
                            GDwOverSpaceWarningTick = MShare.GetTickCount();
                            DScreen.AddSysMsg("目标太远了，施展魔法失败！！！");
                        }
                        PlayScene.ProcMagic.NTargetX = -1;
                    }
                    return;
                }
                PlayScene.ProcMagic.NTargetX = -1;
                if (boContinue || (CanNextAction() && ServerAcceptNextAction()))
                {
                    MShare.LatestSpellTick = MShare.GetTickCount();
                    tdir = ClFunc.GetFlyDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, targx, targy);
                    var pmag = new TUseMagicInfo();
                    pmag.EffectNumber = pcm.Def.Effect;
                    pmag.MagicSerial = pcm.Def.MagicId;
                    pmag.ServerMagicCode = 0;
                    MShare.g_dwMagicDelayTime = 200 + pcm.Def.DelayTime;
                    MShare.g_dwMagicPKDelayTime = 0;
                    if (MShare.MagicTarget != null)
                    {
                        if (MShare.MagicTarget.Race == 0)
                        {
                            MShare.g_dwMagicPKDelayTime = 300 + RandomNumber.GetInstance().Random(1100);
                        }
                    }
                    //MShare.g_MySelf.SendMsg(Messages.CM_SPELL, targx, targy, tdir, pmag, targid, "", 0);
                }
            }
            else
            {
                if (boSeriesSkill)
                {
                    if (MShare.GetTickCount() - MShare.g_IPointLessHintTick > 5000)
                    {
                        MShare.g_IPointLessHintTick = MShare.GetTickCount();
                        DScreen.AddSysMsg($"需要 {spellSpend} 内力值才能释放 {pcm.Def.MagicName}");
                    }
                }
                else if (MShare.GetTickCount() - MShare.g_MPLessHintTick > 1000)
                {
                    MShare.g_MPLessHintTick = MShare.GetTickCount();
                    DScreen.AddSysMsg($"需要 {spellSpend} 魔法值才能释放 {pcm.Def.MagicName}");
                }
            }
        }

        private void UseMagicSpell(int who, int effnum, int targetx, int targety, int magicId)
        {
            var actor = PlayScene.FindActor(who);
            if (actor != null)
            {
                var adir = ClFunc.GetFlyDirection(actor.CurrX, actor.CurrY, targetx, targety);
                var useMagic = new TUseMagicInfo();
                useMagic.EffectNumber = effnum % 255;
                useMagic.ServerMagicCode = 0;
                useMagic.MagicSerial = magicId % 300;
                //Actor.SendMsg(Messages.SM_SPELL, effnum / 255, magic_id / 300, adir, UseMagic, 0, "", 0);
                MShare.g_nSpellCount++;
            }
            else
            {
                MShare.g_nSpellFailCount++;
            }
        }

        private void UseMagicFire(int who, int efftype, int effnum, int targetx, int targety, int target, int maglv)
        {
            var actor = PlayScene.FindActor(who);
            if (actor != null)
            {
                actor.SendMsg(Messages.SM_MAGICFIRE, (ushort)(short)target, (ushort)(short)efftype, effnum, targetx, targety, maglv.ToString(), 0);
                if (MShare.g_nFireCount < MShare.g_nSpellCount)
                {
                    MShare.g_nFireCount++;
                }
            }
            MShare.MagicTarget = null;
        }

        private void UseMagicFireFail(int who)
        {
            var actor = PlayScene.FindActor(who);
            if (actor != null)
            {
                actor.SendMsg(Messages.SM_MAGICFIRE_FAIL, 0, 0, 0, 0, 0, "", 0);
            }
            MShare.MagicTarget = null;
        }

        public void ActorAutoEat(THumActor actor)
        {
            if (!actor.Death)
            {
                ActorCheckHealth(false);
                if (MShare.g_EatingItem.Item.Name == "")
                {
                    if (MShare.IsPersentSpc(actor.Abil.HP, actor.Abil.MaxHP))
                    {
                        ActorCheckHealth(true);
                    }
                }
            }
        }

        public void ActorCheckHealth(bool bNeedSp)
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
            var maxHp = int.MaxValue / 2 - 1;
            var maxMp = int.MaxValue / 2 - 1;
            var maxSp = int.MaxValue / 2 - 1;
            for (var i = Grobal2.MaxBagItem - (1 + 0); i >= 0; i--)
            {
                if ((MShare.ItemArr[i].Item.Name != "") && (MShare.ItemArr[i].Item.NeedIdentify < 4))
                {
                    switch (MShare.ItemArr[i].Item.StdMode)
                    {
                        case 00:
                            switch (MShare.ItemArr[i].Item.Shape)
                            {
                                case 0: // 普通药
                                    if (MShare.g_gcProtect[0] && (MShare.ItemArr[i].Item.AC > 0) && (MShare.ItemArr[i].Item.AC < maxHp))
                                    {
                                        maxHp = MShare.ItemArr[i].Item.AC;
                                        hidx = i;
                                    }
                                    if (MShare.g_gcProtect[1] && (MShare.ItemArr[i].Item.MAC > 0) && (MShare.ItemArr[i].Item.MAC < maxMp))
                                    {
                                        maxMp = MShare.ItemArr[i].Item.MAC;
                                        midx = i;
                                    }
                                    break;
                                case 1: // 速效药
                                    if (MShare.g_gcProtect[3] && (MShare.ItemArr[i].Item.AC > 0) && (MShare.ItemArr[i].Item.AC < maxSp))
                                    {
                                        maxSp = MShare.ItemArr[i].Item.AC;
                                        sidx = i;
                                    }
                                    break;
                            }
                            break;
                        case 2:
                        case 3:
                            if (MShare.g_gcProtect[5])
                            {
                                if (String.Compare(MShare.ItemArr[i].Item.Name, BotConst.g_sRenewBooks[MShare.g_gnProtectPercent[6]], StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    bidx = i;
                                }
                            }
                            break;
                        case 31:
                            switch (MShare.ItemArr[i].Item.AniCount)
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
                                    if (MShare.g_gcProtect[5] && (string.Compare(MShare.ItemArr[i].Item.Name, BotConst.g_sRenewBooks[MShare.g_gnProtectPercent[6]] + "包", StringComparison.OrdinalIgnoreCase) == 0))
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
            var bEatOk = false;
            if (MShare.GetTickCount() - MShare.MySelf.m_dwMsgHint > 15 * 1000)
            {
                MShare.MySelf.m_dwMsgHint = MShare.GetTickCount();
                bHint = true;
            }
            if (!bNeedSp)
            {
                if (MShare.g_gcProtect[0] && MShare.IsPersentHP(MShare.MySelf.Abil.HP, MShare.MySelf.Abil.MaxHP))
                {
                    if (MShare.GetTickCount() - MShare.MySelf.m_dwHealthHP > MShare.g_gnProtectTime[0])
                    {
                        MShare.MySelf.m_dwHealthHP = MShare.GetTickCount();
                        if (hidx > -1)
                        {
                            EatItem(hidx);
                            bEatOk = true;
                        }
                        else if ((nCount > 4) && (uhidx > -1))
                        {
                            EatItem(uhidx);
                            bEatOk = true;
                        }
                        else
                        {
                            bEatSp = true;
                            if (bHint)
                            {
                                ScreenManager.AddChatBoardString("你的金创药已经用完！");
                            }
                            bEatOk = false;
                        }
                    }
                }
            }
            if (!bNeedSp)
            {
                if (MShare.g_gcProtect[1] && MShare.IsPersentMP(MShare.MySelf.Abil.MP, MShare.MySelf.Abil.MaxMP))
                {
                    if (MShare.GetTickCount() - MShare.MySelf.m_dwHealthMP > MShare.g_gnProtectTime[1])
                    {
                        MShare.MySelf.m_dwHealthMP = MShare.GetTickCount();
                        if (midx > -1)
                        {
                            EatItem(midx);
                            bEatOk = true;
                        }
                        else if ((nCount > 4) && (umidx > -1))
                        {
                            EatItem(umidx);
                            bEatOk = true;
                        }
                        else
                        {
                            if (MShare.g_gcProtect[11])
                            {
                                bEatSp = true;
                            }
                            if (bHint)
                            {
                                ScreenManager.AddChatBoardString("你的魔法药已经用完！");
                            }
                            bEatOk = false;
                        }
                    }
                }
            }
            if (!bEatOk)
            {
                if (MShare.g_gcProtect[3] && (bNeedSp || bEatSp || (MShare.g_gcProtect[11] && MShare.IsPersentSpc(MShare.MySelf.Abil.MP, MShare.MySelf.Abil.MaxMP))))
                {
                    if (MShare.GetTickCount() - MShare.MySelf.m_dwHealthSP > MShare.g_gnProtectTime[3])
                    {
                        MShare.MySelf.m_dwHealthSP = MShare.GetTickCount();
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
                            ScreenManager.AddChatBoardString("你的特殊药品已经用完！");
                        }
                    }
                }
            }
            if (MShare.g_gcProtect[5] && MShare.IsPersentBook(MShare.MySelf.Abil.HP, MShare.MySelf.Abil.MaxHP))
            {
                if (MShare.GetTickCount() - MShare.MySelf.m_dwHealthBK > MShare.g_gnProtectTime[5])
                {
                    MShare.MySelf.m_dwHealthBK = MShare.GetTickCount();
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
                        ScreenManager.AddChatBoardString("你的" + BotConst.g_sRenewBooks[MShare.g_gnProtectPercent[6]] + "已经用完！");
                    }
                }
            }
        }

        private void AutoSupplyBeltItem(int nType, int idx, string sItem)
        {
            if (idx >= 0 && idx <= 5 && (sItem != ""))
            {
                if (MShare.ItemArr[idx].Item.Name == "")
                {
                    for (var i = BotConst.MaxBagItemcl - 1; i >= 6; i--)
                    {
                        if (MShare.ItemArr[i].Item.Name == sItem)
                        {
                            MShare.ItemArr[idx] = MShare.ItemArr[i];
                            MShare.ItemArr[i].Item.Name = "";
                            return;
                        }
                    }
                    AutoUnBindItem(nType, sItem);
                }
            }
        }

        private void AutoSupplyBagItem(int nType, string sItem)
        {
            for (var i = BotConst.MaxBagItemcl - 1; i >= 6; i--)
            {
                if (MShare.ItemArr[i].Item.Name == sItem)
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
            if ((!string.IsNullOrEmpty(sItem)) && (nType != 0))
            {
                var boIsUnBindItem = false;
                for (var i = 0; i < BotConst.g_UnBindItems.Length; i++)
                {
                    if (sItem == BotConst.g_UnBindItems[i])
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
                for (var i = 0; i < BotConst.MaxBagItemcl - 1 - 6; i++)
                {
                    if (MShare.ItemArr[i].Item.Name == "")
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
                for (var i = BotConst.MaxBagItemcl - 1; i >= 6; i--)
                {
                    if (MShare.ItemArr[i].Item.StdMode == 31)
                    {
                        if (MShare.ItemArr[i].Item.Name != "")
                        {
                            if (MShare.ItemArr[i].Item.Shape == nType)
                            {
                                idx = i;
                                break;
                            }
                        }
                    }
                }
                if (idx > -1)
                {
                    SendEat(MShare.ItemArr[idx].MakeIndex, "", MShare.ItemArr[idx].Item.StdMode);
                    if (MShare.ItemArr[idx].Dura > 1)
                    {
                        MShare.ItemArr[idx].Dura = (ushort)(MShare.ItemArr[idx].Dura - 1);
                        MShare.g_EatingItem = MShare.ItemArr[idx];
                        MNEatRetIdx = -1;
                    }
                    else
                    {
                        MShare.ItemArr[idx].Dura = (ushort)(MShare.ItemArr[idx].Dura - 1);
                        MShare.g_EatingItem = MShare.ItemArr[idx];
                        MShare.ItemArr[idx].Item.Name = "";
                        MNEatRetIdx = -1;
                    }
                }
            }
        }

        private bool EatItemName(string str)
        {
            if ((str == "小退") && (MShare.MySelf.HiterCode > 0))
            {
                AppLogout();
                return false;
            }
            if ((str == "大退") && (MShare.MySelf.HiterCode > 0))
            {
                return false;
            }
            for (var i = 0; i < BotConst.MaxBagItemcl; i++)
            {
                if ((MShare.ItemArr[i].Item.Name == str) && (MShare.ItemArr[i].Item.NeedIdentify < 4))
                {
                    EatItem(i);
                    return true;
                }
            }
            return false;
        }

        private void EatItem(int idx)
        {
            int i;
            var eatable = false;
            var takeon = false;
            var where = -1;
            if (idx >= 0 && idx <= BotConst.MaxBagItemcl - 1)
            {
                if ((MShare.g_EatingItem.Item.Name != "") && (MShare.GetTickCount() - MShare.g_dwEatTime > 5 * 1000))
                {
                    MShare.g_EatingItem.Item.Name = "";
                }
                if ((MShare.g_EatingItem.Item.Name == "") && (MShare.ItemArr[idx].Item.Name != "") && (MShare.ItemArr[idx].Item.NeedIdentify < 4))
                {
                    if ((MShare.ItemArr[idx].Item.StdMode <= 3) || (MShare.ItemArr[idx].Item.StdMode == 31))
                    {
                        if (MShare.ItemArr[idx].Dura > 1)
                        {
                            MShare.ItemArr[idx].Dura = (ushort)(MShare.ItemArr[idx].Dura - 1);
                            MShare.g_EatingItem = MShare.ItemArr[idx];
                            MShare.ItemArr[idx].Item.Name = "";
                            eatable = true;
                        }
                        else
                        {
                            MShare.g_EatingItem = MShare.ItemArr[idx];
                            MShare.ItemArr[idx].Item.Name = "";
                            eatable = true;
                        }
                    }
                    else
                    {
                    lab1:
                        if ((MShare.ItemArr[idx].Item.StdMode == 46) && MShare.ItemArr[idx].Item.Shape >= 2 && MShare.ItemArr[idx].Item.Shape <= 6)
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
                        if ((MShare.ItemArr[idx].Item.StdMode == 41) && new ArrayList(new int[] { 10, 30 }).Contains(MShare.ItemArr[idx].Item.Shape) && (MShare.g_BuildAcusesStep != 1))
                        {
                            for (i = 0; i < 7; i++)
                            {
                                if (MShare.g_BuildAcuses[i].Item.Item.Name == "")
                                {
                                    if ((MShare.ItemArr[idx].Item.Shape >= 30 && MShare.ItemArr[idx].Item.Shape <= 34 && i >= 5 && i <= 7) || (MShare.ItemArr[idx].Item.Shape >= 10 && MShare.ItemArr[idx].Item.Shape <= 14 && i >= 0 && i <= 4))
                                    {
                                        break;
                                    }
                                }
                            }
                            if (i >= 0 && i <= 7)
                            {
                                MShare.g_boItemMoving = true;
                                MShare.MovingItem.Index = idx;
                                MShare.MovingItem.Item = MShare.ItemArr[idx];
                                MShare.ItemArr[idx].Item.Name = "";
                            }
                            return;
                        }
                        where = ClFunc.GetTakeOnPosition(MShare.ItemArr[idx], MShare.UseItems, true);
                        if (where >= 0 && where <= 13)
                        {
                            takeon = true;
                            MShare.g_EatingItem = MShare.ItemArr[idx];
                            MShare.ItemArr[idx].Item.Name = "";
                        }
                    }
                }
            }
            else if ((idx == -1) && MShare.g_boItemMoving)
            {
                if ((MShare.MovingItem.Item.Item.StdMode <= 4) || (MShare.MovingItem.Item.Item.StdMode == 31) && (MShare.MovingItem.Item.Item.NeedIdentify < 4))
                {
                    if (((MShare.MovingItem.Item.Item.StdMode <= 3) || (MShare.MovingItem.Item.Item.StdMode == 31)) && (MShare.MovingItem.Item.Dura > 1))
                    {
                        MShare.MovingItem.Item.Dura = (ushort)(MShare.MovingItem.Item.Dura - 1);
                        MShare.g_boItemMoving = false;
                        MShare.g_EatingItem = MShare.MovingItem.Item;
                        MShare.MovingItem.Item.Item.Name = "";
                    }
                    else
                    {
                        MShare.g_boItemMoving = false;
                        MShare.g_EatingItem = MShare.MovingItem.Item;
                        MShare.MovingItem.Item.Item.Name = "";
                    }
                    if ((MShare.g_EatingItem.Item.StdMode == 4) && (MShare.g_EatingItem.Item.Shape < 50))
                    {
                        MainOutMessage($"练习{MShare.g_EatingItem.Item.Name}技能");
                        ClFunc.AddItemBag(MShare.g_EatingItem);
                        return;
                    }
                    idx = MNEatRetIdx;
                    eatable = true;
                }
                else
                {
                lab2:
                    if ((MShare.MovingItem.Item.Item.StdMode == 46) && MShare.MovingItem.Item.Item.Shape >= 2 && MShare.MovingItem.Item.Item.Shape <= 6)
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
                    if ((MShare.MovingItem.Item.Item.StdMode == 41) && new ArrayList(new int[] { 10, 30 }).Contains(MShare.MovingItem.Item.Item.Shape) && (MShare.g_BuildAcusesStep != 1))
                    {
                        for (i = 0; i < 7; i++)
                        {
                            if (MShare.g_BuildAcuses[i].Item.Item.Name == "")
                            {
                                if ((MShare.MovingItem.Item.Item.Shape >= 30 && MShare.MovingItem.Item.Item.Shape <= 34 && i >= 5 && i <= 7) || (MShare.MovingItem.Item.Item.Shape >= 10 && MShare.MovingItem.Item.Item.Shape <= 14 && i >= 0 && i <= 4))
                                {
                                    break;
                                }
                            }
                        }
                        return;
                    }
                    where = ClFunc.GetTakeOnPosition(MShare.MovingItem.Item, MShare.UseItems, true);
                    if (where >= 0 && where <= 13)
                    {
                        takeon = true;
                        MShare.g_boItemMoving = false;
                        MShare.g_EatingItem = MShare.MovingItem.Item;
                        MShare.MovingItem.Item.Item.Name = "";
                        idx = MNEatRetIdx;
                    }
                }
            }
            if (eatable)
            {
                MNEatRetIdx = idx;
                MBoSupplyItem = true;
                MShare.g_dwEatTime = MShare.GetTickCount();
                SendEat(MShare.g_EatingItem.MakeIndex, MShare.g_EatingItem.Item.Name, MShare.g_EatingItem.Item.StdMode);
            }
            else if (takeon)
            {
                MNEatRetIdx = idx;
                MShare.g_dwEatTime = MShare.GetTickCount();
                MShare.g_WaitingUseItem.Item = MShare.g_EatingItem;
                MShare.g_WaitingUseItem.Index = where;
                SendTakeOnItem(where, MShare.g_EatingItem.MakeIndex, MShare.g_EatingItem.Item.Name);
                MShare.g_EatingItem.Item.Name = "";
            }
        }

        private bool TargetInSwordLongAttackRange(int ndir)
        {
            if (MShare.g_gcTec[0])
            {
                return true;
            }
            short nX = 0;
            short nY = 0;
            ClFunc.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, ndir, ref nX, ref nY);
            ClFunc.GetFrontPosition(nX, nY, ndir, ref nX, ref nY);
            if ((Math.Abs(MShare.MySelf.CurrX - nX) == 2) || (Math.Abs(MShare.MySelf.CurrY - nY) == 2))
            {
                var actor = PlayScene.FindActorXY(nX, nY);
                if (actor != null)
                {
                    if (!actor.Death)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool TargetInSwordLongAttackRange2(int sx, int sy, int dx, int dy)
        {
            if ((Math.Abs(sx - dx) == 2) && (Math.Abs(sy - dy) == 0))
            {
                return true;
            }
            if ((Math.Abs(sx - dx) == 0) && (Math.Abs(sy - dy) == 2))
            {
                return true;
            }
            if ((Math.Abs(sx - dx) == 2) && (Math.Abs(sy - dy) == 2))
            {
                return true;
            }
            return false;
        }

        private bool TargetInSwordWideAttackRange(int ndir)
        {
            short nX = 0;
            short nY = 0;
            short rx = 0;
            short ry = 0;
            var result = false;
            ClFunc.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, ndir, ref nX, ref nY);
            var actor = PlayScene.FindActorXY(nX, nY);
            var mdir = (ndir + 1) % 8;
            ClFunc.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, mdir, ref rx, ref ry);
            var ractor = PlayScene.FindActorXY(rx, ry);
            if (ractor == null)
            {
                mdir = (ndir + 2) % 8;
                ClFunc.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, mdir, ref rx, ref ry);
                ractor = PlayScene.FindActorXY(rx, ry);
            }
            if (ractor == null)
            {
                mdir = (ndir + 7) % 8;
                ClFunc.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, mdir, ref rx, ref ry);
                ractor = PlayScene.FindActorXY(rx, ry);
            }
            if ((actor != null) && (ractor != null))
            {
                if (!actor.Death && !ractor.Death)
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
                if (GetNextPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, ndir, nC, ref nX, ref nY))
                {
                    var actor = PlayScene.FindActorXY(nX, nY);
                    if ((actor != null) && !actor.Death)
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
            short nX = 0;
            short nY = 0;
            Actor actor;
            var result = false;
            short nC = 1;
            while (true)
            {
                if (GetNextPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, ndir, nC, ref nX, ref nY))
                {
                    actor = PlayScene.FindActorXY(nX, nY);
                    if ((actor != null) && !actor.Death)
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
            ClFunc.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, ndir, ref nX, ref nY);
            var actor = PlayScene.FindActorXY(nX, nY);
            var mdir = (ndir + 1) % 8;
            ClFunc.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, mdir, ref rx, ref ry);
            var ractor = PlayScene.FindActorXY(rx, ry);
            if (ractor == null)
            {
                mdir = (ndir + 2) % 8;
                ClFunc.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, mdir, ref rx, ref ry);
                ractor = PlayScene.FindActorXY(rx, ry);
            }
            if (ractor == null)
            {
                mdir = (ndir + 7) % 8;
                ClFunc.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, mdir, ref rx, ref ry);
                ractor = PlayScene.FindActorXY(rx, ry);
            }
            if ((actor != null) && (ractor != null))
            {
                if (!actor.Death && !ractor.Death)
                {
                    result = true;
                }
            }
            return result;
        }

        public bool AttackTarget(Actor target)
        {
            var result = false;
            var nHitMsg = Messages.CM_HIT;
            if (MShare.UseItems[ItemLocation.Weapon] != null && MShare.UseItems[ItemLocation.Weapon].Item.StdMode == 6)
            {
                nHitMsg = Messages.CM_HEAVYHIT;
            }
            int tdir = ClFunc.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, target.CurrX, target.CurrY);
            if ((Math.Abs(MShare.MySelf.CurrX - target.CurrX) <= 1) && (Math.Abs(MShare.MySelf.CurrY - target.CurrY) <= 1) && (!target.Death))
            {
                if (TimerAutoPlay.Enabled)
                {
                    MShare.AutoMove = false;
                    if (MShare.AutoTagget != null)
                    {
                        MainOutMessage($"怪物目标：{MShare.AutoTagget.UserName} ({MShare.AutoTagget.CurrX},{MShare.AutoTagget.CurrY}) 正在使用普通攻击");
                    }
                }
                if (CanNextAction() && ServerAcceptNextAction())
                {
                    if (CanNextHit())
                    {
                        if (MShare.g_boNextTimeFireHit && (MShare.MySelf.Abil.MP >= 7))
                        {
                            MShare.g_boNextTimeFireHit = false;
                            nHitMsg = Messages.CM_FIREHIT;
                        }
                        else if (MShare.g_boNextTimePowerHit)
                        {
                            MShare.g_boNextTimePowerHit = false;
                            nHitMsg = Messages.CM_POWERHIT;
                        }
                        else if ((MShare.MySelf.Abil.MP >= 3) && (MShare.g_boCanWideHit || (MShare.g_gcTec[1] && (GetMagicById(25) != null) && TargetInSwordWideAttackRange(tdir))))
                        {
                            nHitMsg = Messages.CM_WIDEHIT;
                        }
                        else if (MShare.g_boCanCrsHit && (MShare.MySelf.Abil.MP >= 6))
                        {
                            nHitMsg = Messages.CM_CRSHIT;
                        }
                        else if (MShare.g_boCanLongHit && TargetInSwordLongAttackRange(tdir))
                        {
                            nHitMsg = Messages.CM_LONGHIT;
                        }
                        MShare.MySelf.SendMsg(nHitMsg, (ushort)MShare.MySelf.CurrX, (ushort)MShare.MySelf.CurrY, tdir, 0, 0, "", 0);
                    }
                }
                result = true;
                MShare.LastAttackTick = MShare.GetTickCount();
            }
            else
            {
                if (MShare.g_boCanLongHit && (MShare.MySelf.Job == 0) && (!target.Death) && MShare.g_boAutoLongAttack && MShare.g_gcTec[10] && (MShare.MagicArr[12] != null) && TargetInSwordLongAttackRange2(MShare.MySelf.CurrX, MShare.MySelf.CurrY, target.CurrX, target.CurrY))
                {
                    if (CanNextAction() && ServerAcceptNextAction() && CanNextHit())
                    {
                        nHitMsg = Messages.CM_LONGHIT;
                        MShare.MySelf.SendMsg(nHitMsg, (ushort)MShare.MySelf.CurrX, (ushort)MShare.MySelf.CurrY, tdir, 0, 0, "", 0);
                        MShare.LastAttackTick = MShare.GetTickCount();
                    }
                    else if (MShare.g_boAutoLongAttack && MShare.g_gcTec[10] && TimerAutoPlay.Enabled)// 走刺杀位
                    {
                        return true;
                    }
                }
                else
                {
                    var dx = MShare.MySelf.CurrX;
                    var dy = MShare.MySelf.CurrY;
                    if ((MShare.MySelf.Job == 0) && MShare.g_boAutoLongAttack && MShare.g_gcTec[10] && (MShare.MagicArr[12] != null))
                    {
                        ClFunc.GetNextHitPosition(target.CurrX, target.CurrY, ref dx, ref dy);
                        if (!PlayScene.CanWalk(dx, dy))
                        {
                            ClFunc.GetBackPosition(target.CurrX, target.CurrY, tdir, ref dx, ref dy);
                        }
                    }
                    else
                    {
                        ClFunc.GetBackPosition(target.CurrX, target.CurrY, tdir, ref dx, ref dy);
                    }
                    MShare.TargetX = dx;
                    MShare.TargetY = dy;
                    MShare.PlayerAction = PlayerAction.Run;
                }
                if (TimerAutoPlay.Enabled)
                {
                    MShare.AutoMove = true;
                    MainOutMessage($"跑向目标怪物：{target.UserName} ({target.CurrX},{target.CurrY}) ");
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
                    SendClientMessage(Messages.CM_OPENDOOR, door, dx, dy, 0);
                    result = true;
                }
            }
            return result;
        }

        public void MouseTimerTimer(object sender, EventArgs e1)
        {
            int ii;
            int fixidx;
            Actor target;
            if ((MShare.g_gcGeneral[1] || MShare.g_gcGeneral[9]) && (MShare.GetTickCount() - MDwDuraWarningTick > 60 * 1000))
            {
                MDwDuraWarningTick = MShare.GetTickCount();
                if ((MShare.MySelf != null) && !MShare.MySelf.Death)
                {
                    for (var i = MShare.UseItems.Length; i > 0; i--)
                    {
                        if (MShare.UseItems[i].Item.Name != "")
                        {
                            if (MShare.UseItems[i].Item.StdMode == 7 || MShare.UseItems[i].Item.StdMode == 25)
                            {
                                continue;
                            }
                            if (MShare.UseItems[i].Dura < 1500)
                            {
                                if (MShare.g_gcGeneral[1])
                                {
                                    ScreenManager.AddChatBoardString($"你的[{MShare.UseItems[i].Item.Name}]持久已到底限，请及时修理！");
                                }
                                if (MShare.g_gcGeneral[9])
                                {
                                    fixidx = -1;
                                    for (ii = Grobal2.MaxBagItem - (1 + 0); ii >= 0; ii--)
                                    {
                                        if ((MShare.ItemArr[ii].Item.NeedIdentify < 4) && (MShare.ItemArr[ii].Item.Name != "") && (MShare.ItemArr[ii].Item.StdMode == 2) && (MShare.ItemArr[ii].Item.Shape == 9) && (MShare.ItemArr[ii].Dura > 0))
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
                                        ScreenManager.AddChatBoardString($"你的{MShare.UseItems[i].Item.Name}已经用完，请及时补充！");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if ((MShare.MySelf != null) && !MShare.MySelf.Death && (MShare.MySelf.m_nIPowerLvl > 5) && (MShare.MySelf.m_nIPower < 30) && (MShare.GetTickCount() - DwIpTick > 30 * 1000))
            {
                DwIpTick = MShare.GetTickCount();
                fixidx = -1;
                for (ii = Grobal2.MaxBagItem - (1 + 0); ii >= 0; ii--)
                {
                    if ((MShare.ItemArr[ii].Item.NeedIdentify < 4) && (MShare.ItemArr[ii].Item.Name != "") && (MShare.ItemArr[ii].Item.StdMode == 2) && (MShare.ItemArr[ii].Item.Shape == 13) && (MShare.ItemArr[ii].DuraMax > 0))
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
            if (MShare.TargetCret != null)
            {
                if (_actionKey > 0)
                {
                    ProcessKeyMessages();
                }
                else
                {
                    MShare.TargetCret = null;
                }
            }
            if ((MShare.MySelf != null) && (MShare.g_boAutoDig || MShare.g_boAutoSit))
            {
                if (CanNextAction() && ServerAcceptNextAction() && (MShare.g_boAutoSit || CanNextHit()))
                {
                    if (MShare.g_boAutoDig)
                    {
                        MShare.MySelf.SendMsg(Messages.CM_HIT + 1, (ushort)MShare.MySelf.CurrX, (ushort)MShare.MySelf.CurrY, MShare.MySelf.m_btDir, 0, 0, "", 0);
                    }
                    if (MShare.g_boAutoSit)
                    {
                        target = PlayScene.ButchAnimal(MShare.MouseCurrX, MShare.MouseCurrY);
                        if (target != null)
                        {
                            ii = ClFunc.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.MouseCurrX, MShare.MouseCurrY);
                            SendButchAnimal(MShare.MouseCurrX, MShare.MouseCurrY, ii, target.RecogId);
                            MShare.MySelf.SendMsg(Messages.CM_SITDOWN, (ushort)MShare.MySelf.CurrX, (ushort)MShare.MySelf.CurrY, ii, 0, 0, "", 0);
                        }
                        else
                        {
                            ii = ClFunc.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.MouseCurrX, MShare.MouseCurrY);
                            SendButchAnimal(MShare.MouseCurrX, MShare.MouseCurrY, ii, MShare.g_DetectItemMineID);
                            MShare.MySelf.SendMsg(Messages.CM_SITDOWN, (ushort)MShare.MySelf.CurrX, (ushort)MShare.MySelf.CurrY, ii, 0, 0, "", 0);
                        }
                    }
                }
            }
            if (MShare.AutoPickUp && (MShare.MySelf != null) && (MShare.GetTickCount() - MShare.AutoPickupTick) > MShare.AutoPickupTime)// 动自捡取
            {
                MShare.AutoPickupTick = MShare.GetTickCount();
                AutoPickUpItem();
            }
        }

        private void AutoPickUpItem()
        {
            if (ServerAcceptNextAction())
            {
                var dropItem = PlayScene.GetXyDropItems(MShare.MySelf.CurrX, MShare.MySelf.CurrY);
                if (dropItem != null)
                {
                    if (MShare.PickUpAll || dropItem.boPickUp)
                    {
                        SendPickup();
                    }
                }
            }
        }

        public void WaitMsgTimerTimer(object sender, EventArgs e1)
        {
            if (MShare.MySelf == null)
            {
                return;
            }
            if (MShare.MySelf.ActionFinished())
            {
                //WaitMsgTimer.Enabled = false;
                switch (_waitingMsg.Ident)
                {
                    case Messages.SM_CHANGEMAP:
                        MShare.MapMovingWait = false;
                        MShare.g_boMapMoving = false;
                        if (MShare.g_nStallX != -1)
                        {
                            MShare.g_nStallX = -1;
                        }
                        ClearDropItems();
                        PlayScene.CleanObjects();
                        MShare.MapTitle = "";
                        PlayScene.SendMsg(Messages.SM_CHANGEMAP, 0, _waitingMsg.Param, _waitingMsg.Tag, (byte)_waitingMsg.Series, 0, 0, _waitingStr);
                        MShare.MySelf.CleanCharMapSetting((short)_waitingMsg.Param, (short)_waitingMsg.Tag);
                        MShare.TargetX = -1;
                        MShare.TargetCret = null;
                        MShare.FocusCret = null;
                        break;
                }
            }
        }

        public void ActiveCmdTimer(TTimerCommand cmd)
        {
            //TimerCmd = cmd;
            //CmdTimer.Enabled = true;
        }

        public void CmdTimerTimer(object sender, EventArgs e1)
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
            MShare.AutoTagget = null;
            MShare.AutoPicupItem = null;
            MShare.g_nAPStatus = -1;
            MShare.TargetX = -1;
            MShare.g_boCanRunSafeZone = true;
            MShare.g_nEatIteminvTime = 200;
            MShare.g_SendSayListIdx = 0;
            MShare.g_SendSayList.Clear();
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
            MShare.OpenBoxItem.Item.Item.Name = "";
            MShare.g_EatingItem.Item.Name = "";
            MShare.LastMapMusic = -1;
            MShare.TargetX = -1;
            MShare.TargetCret = null;
            MShare.FocusCret = null;
            MShare.MagicTarget = null;
            ActionLock = false;
            MBoSupplyItem = false;
            MNEatRetIdx = -1;
            MShare.g_GroupMembers.Clear();
            MShare.g_sGuildRankName = "";
            MShare.g_sGuildName = "";
            MShare.g_boMapMoving = false;
            //WaitMsgTimer.Enabled = false;
            MShare.MapMovingWait = false;
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
            PlayScene.ClearActors();
            ClearDropItems();
            //EventMan.ClearEvents();
            PlayScene.CleanObjects();
            //MaketSystem.Units.MaketSystem.g_Market.Clear();
        }

        private void ChangeServerClearGameVariables()
        {
            CloseAllWindows();
            ClearDropItems();
            MShare.g_boItemMoving = false;
            MShare.g_DetectItem.Item.Name = "";
            MShare.g_WaitingUseItem.Item.Item.Name = "";
            MShare.g_WaitingStallItem.Item.Item.Name = "";
            MShare.g_WaitingDetectItem.Item.Item.Name = "";
            MShare.OpenBoxItem.Item.Item.Name = "";
            MShare.g_EatingItem.Item.Name = "";
            MShare.LastMapMusic = -1;
            MShare.TargetX = -1;
            MShare.TargetCret = null;
            MShare.FocusCret = null;
            MShare.MagicTarget = null;
            ActionLock = false;
            MBoSupplyItem = false;
            MNEatRetIdx = -1;
            MShare.g_GroupMembers.Clear();
            MShare.g_sGuildRankName = "";
            MShare.g_sGuildName = "";
            MShare.g_boMapMoving = false;
            //WaitMsgTimer.Enabled = false;
            MShare.MapMovingWait = false;
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
            PlayScene.CleanObjects();
        }

        private void SendSocket(string sendstr)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.SendText($"#1{sendstr}!");
            }
            else
            {
                MainOutMessage($"socket close {ClientSocket.RemoteEndPoint}");
            }
        }

        public void CloseSocket()
        {
            if (ClientSocket != null)
            {
                ClientSocket.Disconnect();
            }
        }

        private void SendClientMessage(int msg, int recog, int param, int tag, int series)
        {
            var dMsg = Messages.MakeMessage(msg, recog, param, tag, series);
            SendSocket(EDCode.EncodeMessage(dMsg));
        }

        /// <summary>
        /// 发送登录消息
        /// </summary>
        private void SendRunLogin()
        {
            MainOutMessage("进入游戏");
            DScreen.CurrentScene.ConnectionStep = ConnectionStep.Play;
            var sSendMsg = $"**{LoginId}/{ChrName}/{Certification}/{Grobal2.CLIENT_VERSION_NUMBER}/{Grobal2.CLIENT_VERSION_NUMBER}";
            SendSocket(EDCode.EncodeString(sSendMsg));
        }

        public void SendSay(string str)
        {
            var sx = string.Empty;
            const string sam = "/move";
            if (!string.IsNullOrEmpty(str))
            {
                if (HUtil32.CompareLStr(str, sam))
                {
                    var param = str[sam.Length..];
                    if (!string.IsNullOrEmpty(param))
                    {
                        string sy = HUtil32.GetValidStr3(param, ref sx, new string[] { " ", ":", ",", "\09" });
                        if ((sx != "") && (sy != ""))
                        {
                            short x = Convert.ToInt16(sx);
                            short y = Convert.ToInt16(sy);
                            if ((x > 0) && (y > 0))
                            {
                                MShare.MySelf.m_nTagX = x;
                                MShare.MySelf.m_nTagY = y;
                                if (!GPathBusy)
                                {
                                    GPathBusy = true;
                                    try
                                    {
                                        Map.LoadMapData();
                                        PathMap.g_MapPath = Map.FindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.MySelf.m_nTagX, MShare.MySelf.m_nTagY, 0);
                                        if (PathMap.g_MapPath != null)
                                        {
                                            GMoveStep = 1;
                                            //TimerAutoMove.Enabled = true;
                                            ScreenManager.AddChatBoardString($"自动移动至坐标({MShare.MySelf.m_nTagX}:{MShare.MySelf.m_nTagY})，点击鼠标任意键停止……");
                                        }
                                        else
                                        {
                                            //TimerAutoMove.Enabled = false;
                                            ScreenManager.AddChatBoardString($"自动移动坐标点({MShare.MySelf.m_nTagX}:{MShare.MySelf.m_nTagY})不可到达");
                                            MShare.MySelf.m_nTagX = 0;
                                            MShare.MySelf.m_nTagY = 0;
                                        }
                                    }
                                    finally
                                    {
                                        GPathBusy = false;
                                    }
                                }
                            }
                        }
                    }
                    return;
                }
                var msg = Messages.MakeMessage(Messages.CM_SAY, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(str));
                if (str[0] == '/')
                {
                    ScreenManager.AddChatBoardString(str);
                    HUtil32.GetValidStr3(str[1..], ref WhisperName, " ");
                }
            }
        }

        /// <summary>
        /// 发送角色动作消息（走路 攻击等）
        /// </summary>
        private void SendActMsg(int ident, ushort x, ushort y, int dir)
        {
            var msg = Messages.MakeMessage(ident, HUtil32.MakeLong(x, y), 0, dir, 0);
            SendSocket(EDCode.EncodeMessage(msg));
            ActionLock = true;
            _actionLockTime = MShare.GetTickCount();
        }

        private void SendSpellMsg(int ident, ushort x, ushort y, int dir, int target, bool bLock = false)
        {
            var msg = Messages.MakeMessage(ident, HUtil32.MakeLong(x, y), HUtil32.LoWord(target), dir, HUtil32.HiWord(target));
            SendSocket(EDCode.EncodeMessage(msg));
            if (!bLock)
            {
                return;
            }
            ActionLock = true;
            _actionLockTime = MShare.GetTickCount();
        }

        public void SendQueryUserName(int targetid, int x, int y)
        {
            var msg = Messages.MakeMessage(Messages.CM_QUERYUSERNAME, targetid, x, y, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendDropItem(string name, int itemserverindex, int dropcnt)
        {
            var msg = Messages.MakeMessage(Messages.CM_DROPITEM, itemserverindex, dropcnt, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(name));
        }

        private void SendPickup()
        {
            var msg = Messages.MakeMessage(Messages.CM_PICKUP, 0, MShare.MySelf.CurrX, MShare.MySelf.CurrY, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        private void SendTakeOnItem(int where, int itmindex, string itmname)
        {
            var msg = Messages.MakeMessage(Messages.CM_TAKEONITEM, itmindex, where, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itmname));
        }

        public void SendTakeOffItem(byte where, int itmindex, string itmname)
        {
            var msg = Messages.MakeMessage(Messages.CM_TAKEOFFITEM, itmindex, where, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itmname));
        }

        private void SendEat(int itmindex, string itmname, int nUnBindItem)
        {
            var msg = Messages.MakeMessage(Messages.CM_EAT, itmindex, 0, 0, nUnBindItem);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        private void SendButchAnimal(int x, int y, int dir, int actorid)
        {
            var msg = Messages.MakeMessage(Messages.CM_BUTCH, actorid, x, y, dir);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendMagicKeyChange(int magid, char keych)
        {
            var msg = Messages.MakeMessage(Messages.CM_MAGICKEYCHANGE, magid, (byte)keych, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        private void SendMerchantDlgSelect(int merchant, string rstr)
        {
            const string sam = "@_automove ";
            int x;
            int y;
            var sx = string.Empty;
            var sy = string.Empty;
            if (rstr.Length >= 2)
            {
                if (HUtil32.CompareLStr(rstr, sam))
                {
                    string param = rstr.Substring(sam.Length + 1 - 1, rstr.Length - sam.Length);
                    if (param != "")
                    {
                        param = HUtil32.GetValidStr3(param, ref sx, new string[] { " ", ":", ",", "\09" });
                        string sM = HUtil32.GetValidStr3(param, ref sy, new string[] { " ", ":", ",", "\09" });
                        if ((sx != "") && (sy != ""))
                        {
                            if ((sM != "") && (string.Compare(MShare.MapTitle, sM, StringComparison.OrdinalIgnoreCase) != 0))// 自动移动
                            {
                                ScreenManager.AddChatBoardString($"到达 {sM} 之后才能使用自动走路");
                                return;
                            }
                            x = Convert.ToInt32(sx);
                            y = Convert.ToInt32(sy);
                            if ((x > 0) && (y > 0))
                            {
                                MShare.MySelf.m_nTagX = (short)x;
                                MShare.MySelf.m_nTagY = (short)y;
                                if (!GPathBusy)
                                {
                                    GPathBusy = true;
                                    try
                                    {
                                        Map.LoadMapData();
                                        PathMap.g_MapPath = Map.FindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.MySelf.m_nTagX, MShare.MySelf.m_nTagY, 0);
                                        if (PathMap.g_MapPath != null)
                                        {
                                            GMoveStep = 1;
                                            TimerAutoMove.Enabled = true;
                                            ScreenManager.AddChatBoardString($"自动移动至坐标({MShare.MySelf.m_nTagX}:{MShare.MySelf.m_nTagY})，点击鼠标任意键停止……");
                                        }
                                        else
                                        {
                                            TimerAutoMove.Enabled = false;
                                            ScreenManager.AddChatBoardString($"自动移动坐标点({MShare.MySelf.m_nTagX}:{MShare.MySelf.m_nTagY})不可到达");
                                            MShare.MySelf.m_nTagX = 0;
                                            MShare.MySelf.m_nTagY = 0;
                                        }
                                    }
                                    finally
                                    {
                                        GPathBusy = false;
                                    }
                                }
                            }
                        }
                    }
                    return;
                }
            }
            var msg = Messages.MakeMessage(Messages.CM_MERCHANTDLGSELECT, merchant, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(rstr));
        }

        public void SendQueryPrice(int merchant, int itemindex, string itemname)
        {
            var msg = Messages.MakeMessage(Messages.CM_MERCHANTQUERYSELLPRICE, merchant, HUtil32.LoWord(itemindex), HUtil32.HiWord(itemindex), 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendQueryRepairCost(int merchant, int itemindex, string itemname)
        {
            var msg = Messages.MakeMessage(Messages.CM_MERCHANTQUERYREPAIRCOST, merchant, HUtil32.LoWord(itemindex), HUtil32.HiWord(itemindex), 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendSellItem(int merchant, int itemindex, string itemname, short count)
        {
            var msg = Messages.MakeMessage(Messages.CM_USERSELLITEM, merchant, HUtil32.LoWord(itemindex), HUtil32.HiWord(itemindex), count);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendRepairItem(int merchant, int itemindex, string itemname)
        {
            var msg = Messages.MakeMessage(Messages.CM_USERREPAIRITEM, merchant, HUtil32.LoWord(itemindex), HUtil32.HiWord(itemindex), 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendStorageItem(int merchant, int itemindex, string itemname, short count)
        {
            var msg = Messages.MakeMessage(Messages.CM_USERSTORAGEITEM, merchant, HUtil32.LoWord(itemindex), HUtil32.HiWord(itemindex), count);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendGetDetailItem(int merchant, int menuindex, string itemname)
        {
            var msg = Messages.MakeMessage(Messages.CM_USERGETDETAILITEM, merchant, menuindex, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendBuyItem(int merchant, int itemserverindex, string itemname, short conut)
        {
            var msg = Messages.MakeMessage(Messages.CM_USERBUYITEM, merchant, HUtil32.LoWord(itemserverindex), HUtil32.HiWord(itemserverindex), conut);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendTakeBackStorageItem(int merchant, int itemserverindex, string itemname, short count)
        {
            var msg = Messages.MakeMessage(Messages.CM_USERTAKEBACKSTORAGEITEM, merchant, HUtil32.LoWord(itemserverindex), HUtil32.HiWord(itemserverindex), count);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendMakeDrugItem(int merchant, string itemname)
        {
            var msg = Messages.MakeMessage(Messages.CM_USERMAKEDRUGITEM, merchant, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(itemname));
        }

        public void SendDropGold(int dropgold)
        {
            var msg = Messages.MakeMessage(Messages.CM_DROPGOLD, dropgold, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendGroupMode(bool onoff)
        {
            CommandMessage msg;
            if (onoff)
            {
                msg = Messages.MakeMessage(Messages.CM_GROUPMODE, 0, 1, 0, 0);
            }
            else
            {
                msg = Messages.MakeMessage(Messages.CM_GROUPMODE, 0, 0, 0, 0);
            }
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendCreateGroup(string withwho)
        {
            if (withwho != "")
            {
                var msg = Messages.MakeMessage(Messages.CM_CREATEGROUP, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(withwho));
            }
        }

        public void SendWantMiniMap()
        {
            var msg = Messages.MakeMessage(Messages.CM_WANTMINIMAP, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendGuildDlg()
        {
            var msg = Messages.MakeMessage(Messages.CM_OPENGUILDDLG, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendDealTry()
        {
            var who = string.Empty;
            var msg = Messages.MakeMessage(Messages.CM_DEALTRY, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(who));
        }

        public void SendCancelDeal()
        {
            var msg = Messages.MakeMessage(Messages.CM_DEALCANCEL, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendAddDealItem(ClientItem ci)
        {
            var msg = Messages.MakeMessage(Messages.CM_DEALADDITEM, ci.MakeIndex, 0, 0, ci.Dura);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(ci.Item.Name));
        }

        public void SendDelDealItem(ClientItem ci)
        {
            var msg = Messages.MakeMessage(Messages.CM_DEALDELITEM, ci.MakeIndex, 0, 0, ci.Dura);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(ci.Item.Name));
        }

        public void SendChangeDealGold(int gold)
        {
            var msg = Messages.MakeMessage(Messages.CM_DEALCHGGOLD, gold, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendDealEnd()
        {
            var msg = Messages.MakeMessage(Messages.CM_DEALEND, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendAddGroupMember(string withwho)
        {
            if (withwho != "")
            {
                var msg = Messages.MakeMessage(Messages.CM_ADDGROUPMEMBER, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(withwho));
            }
        }

        public void SendDelGroupMember(string withwho)
        {
            if (withwho != "")
            {
                var msg = Messages.MakeMessage(Messages.CM_DELGROUPMEMBER, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(withwho));
            }
        }

        public void SendGuildHome()
        {
            var msg = Messages.MakeMessage(Messages.CM_GUILDHOME, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        private void SendGuildMemberList()
        {
            var msg = Messages.MakeMessage(Messages.CM_GUILDMEMBERLIST, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendGuildAddMem(string who)
        {
            if (who.Trim() != "")
            {
                var msg = Messages.MakeMessage(Messages.CM_GUILDADDMEMBER, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(who));
            }
        }

        public void SendGuildDelMem(string who)
        {
            if (who.Trim() != "")
            {
                var msg = Messages.MakeMessage(Messages.CM_GUILDDELMEMBER, 0, 0, 0, 0);
                SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(who));
            }
        }

        public void SendGuildUpdateNotice(string notices)
        {
            var msg = Messages.MakeMessage(Messages.CM_GUILDUPDATENOTICE, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(notices));
        }

        public void SendGuildUpdateGrade(string rankinfo)
        {
            var msg = Messages.MakeMessage(Messages.CM_GUILDUPDATERANKINFO, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodeString(rankinfo));
        }

        public void SendSpeedHackUser()
        {
            var msg = Messages.MakeMessage(Messages.CM_SPEEDHACKUSER, 0, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg));
        }

        public void SendAdjustBonus(int remain, NakedAbility babil)
        {
            var msg = Messages.MakeMessage(Messages.CM_ADJUST_BONUS, remain, 0, 0, 0);
            SendSocket(EDCode.EncodeMessage(msg) + EDCode.EncodePacket(babil));
        }

        public bool ServerAcceptNextAction()
        {
            if ((MShare.MySelf != null) && MShare.MySelf.StallMgr.OnSale)
            {
                return false;
            }
            var result = true;
            if (ActionLock)
            {
                if ((MShare.GetTickCount() - _actionLockTime) > 5 * 1000)
                {
                    ActionLock = false;
                }
                result = false;
            }
            return result;
        }

        public bool CanNextAction()
        {
            if ((MShare.MySelf != null) && MShare.MySelf.StallMgr.OnSale)
            {
                return false;
            }
            if (!MShare.MySelf.m_boUseCboLib && MShare.MySelf.IsIdle() && ((MShare.MySelf.m_nState & 0x04000000) == 0) && ((MShare.MySelf.m_nState & 0x02000000) == 0)
                && (MShare.GetTickCount() - MShare.g_dwDizzyDelayStart > MShare.g_dwDizzyDelayTime))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否可以攻击，控制攻击速度
        /// </summary>
        /// <returns></returns>
        private bool CanNextHit(bool settime = false)
        {
            bool result;
            int nextHitTime;
            if ((MShare.MySelf != null) && MShare.MySelf.StallMgr.OnSale)
            {
                return false;
            }
            var levelFastTime = HUtil32._MIN(370, MShare.MySelf.Abil.Level * 14);
            levelFastTime = HUtil32._MIN(800, levelFastTime + MShare.MySelf.HitSpeed * MShare.ItemSpeed);
            if (MShare.SpeedRate)
            {
                if (MShare.MySelf.m_boAttackSlow)
                {
                    nextHitTime = MShare.HitTime - levelFastTime + 1500 - MShare.g_HitSpeedRate * 20; // 腕力超过时，减慢攻击速度
                }
                else
                {
                    nextHitTime = MShare.HitTime - levelFastTime - MShare.g_HitSpeedRate * 20;
                }
            }
            else
            {
                if (MShare.MySelf.m_boAttackSlow)
                {
                    nextHitTime = MShare.HitTime - levelFastTime + 1500;
                }
                else
                {
                    nextHitTime = MShare.HitTime - levelFastTime;
                }
            }
            if (nextHitTime < 0)
            {
                nextHitTime = 0;
            }
            if (MShare.GetTickCount() - LastHitTick > nextHitTime)
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
            MShare.TargetX = -1;
            MShare.TargetY = -1;
            MShare.MySelf.m_boUseCboLib = false;
            ActionFailLock = true;
            ActionFailLockTime = MShare.GetTickCount();
            MShare.MySelf.MoveFail();
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

        private static bool IsGroupMember(string uname)
        {
            return MShare.g_GroupMembers.IndexOf(uname) >= 0; ;
        }

        private void CheckSpeedHack(long rtime)
        {
            return;
        }

        private void RecalcAutoMovePath()
        {
            if ((MShare.MySelf.m_nTagX > 0) && (MShare.MySelf.m_nTagY > 0))
            {
                if (!GPathBusy)
                {
                    GPathBusy = true;
                    try
                    {
                        Map.ReLoadMapData();
                        PathMap.g_MapPath = Map.FindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.MySelf.m_nTagX, MShare.MySelf.m_nTagY, 0);
                        if (PathMap.g_MapPath != null)
                        {
                            GMoveStep = 1;
                            TimerAutoMove.Enabled = true;
                        }
                        else
                        {
                            MShare.MySelf.m_nTagX = 0;
                            MShare.MySelf.m_nTagY = 0;
                            TimerAutoMove.Enabled = false;
                            ScreenManager.AddChatBoardString($"自动移动目标({MShare.MySelf.m_nTagX}:{MShare.MySelf.m_nTagY})被占据，不可到达");
                        }
                    }
                    finally
                    {
                        GPathBusy = false;
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

        private void ClientGetNeedUpdateAccount(string body)
        {
            //TUserEntry ue = EDcode.DecodeBuffer<TUserEntry>(body);
            //LoginScene.UpdateAccountInfos(ue);
        }

        private void ClientGetMapDescription(CommandMessage msg, string sBody)
        {
            sBody = EDCode.DeCodeString(sBody);
            var sTitle = sBody;
            MShare.MapTitle = sTitle;
            LoadWayPoint();
            if (!MShare.g_gcGeneral[11])
            {
                MShare.LastMapMusic = msg.Recog;
            }
            else
            {
                if (msg.Recog == -1)
                {
                    MShare.LastMapMusic = -1;
                }
                if (MShare.LastMapMusic != msg.Recog)
                {
                    MShare.LastMapMusic = msg.Recog;
                }
            }
        }

        private void ClientGetGameGoldName(CommandMessage msg, string sBody)
        {
            //var sData = string.Empty;
            //if (sBody != "")
            //{
            //    sBody = EDCode.DeCodeString(sBody);
            //    sBody = HUtil32.GetValidStr3(sBody, ref sData, new char[] { '\r' });
            //    //BotConst.GameGoldName = sData;
            //    //BotConst.GamePointName = sBody;
            //}
            MShare.MySelf.m_nGameGold = msg.Recog;
            MShare.MySelf.m_nGamePoint = HUtil32.MakeLong(msg.Param, msg.Tag);
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

        private void ClientGetAddItem(int hint, string body)
        {
            if (!string.IsNullOrEmpty(body))
            {
                var cu = EDCode.DecodeBuffer<ClientItem>(body);
                ClFunc.AddItemBag(cu);
                if (hint != 0)
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
                for (var i = 0; i < MShare.UseItems.Length; i++)
                {
                    if ((MShare.UseItems[i].Item.Name == cu.Item.Name) && (MShare.UseItems[i].MakeIndex == cu.MakeIndex))
                    {
                        MShare.UseItems[i] = cu;
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
            if (!string.IsNullOrEmpty(body))
            {
                var cu = EDCode.DecodeBuffer<ClientItem>(body);
                ClFunc.DelItemBag(cu.Item.Name, cu.MakeIndex);
                for (var i = 0; i < MShare.UseItems.Length; i++)
                {
                    if ((MShare.UseItems[i].Item.Name == cu.Item.Name) && (MShare.UseItems[i].MakeIndex == cu.MakeIndex))
                    {
                        MShare.UseItems[i].Item.Name = "";
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
            var str = string.Empty;
            var iname = string.Empty;
            var cu = new ClientItem();
            body = EDCode.DeCodeString(body);
            while (!string.IsNullOrEmpty(body))
            {
                body = HUtil32.GetValidStr3(body, ref str, HUtil32.Backslash);
                if ((!string.IsNullOrEmpty(iname)) && (!string.IsNullOrEmpty(str)))
                {
                    iindex = HUtil32.StrToInt(str, 0);
                    ClFunc.DelItemBag(iname, iindex);
                    if (wOnlyBag == 0)
                    {
                        for (var i = 0; i < MShare.UseItems.Length; i++)
                        {
                            if ((MShare.UseItems[i].Item.Name == iname) && (MShare.UseItems[i].MakeIndex == iindex))
                            {
                                MShare.UseItems[i].Item.Name = "";
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

        public bool ClientGetBagItmes_CompareItemArr(ClientItem[] itemSaveArr)
        {
            var flag = true;
            for (var i = 0; i < BotConst.MaxBagItemcl; i++)
            {
                if (itemSaveArr[i].Item.Name != "")
                {
                    flag = false;
                    for (var j = 0; j < BotConst.MaxBagItemcl; j++)
                    {
                        if ((MShare.ItemArr[j].Item.Name == itemSaveArr[i].Item.Name) && (MShare.ItemArr[j].MakeIndex == itemSaveArr[i].MakeIndex))
                        {
                            if ((MShare.ItemArr[j].Dura == itemSaveArr[i].Dura) && (MShare.ItemArr[j].DuraMax == itemSaveArr[i].DuraMax))
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
                for (var i = 0; i < BotConst.MaxBagItemcl; i++)
                {
                    if (MShare.ItemArr[i].Item.Name != "")
                    {
                        flag = false;
                        for (var j = 0; j < BotConst.MaxBagItemcl; j++)
                        {
                            if ((MShare.ItemArr[i].Item.Name == itemSaveArr[j].Item.Name) && (MShare.ItemArr[i].MakeIndex == itemSaveArr[j].MakeIndex))
                            {
                                if ((MShare.ItemArr[i].Dura == itemSaveArr[j].Dura) && (MShare.ItemArr[i].DuraMax == itemSaveArr[j].DuraMax))
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
            var Str = string.Empty;
            //var ItemSaveArr = new ClientItem[BotConst.MaxBagItemcl];
            //FillChar(MShare.g_RefineItems, sizeof(TMovingItem) * 3, '\0');
            //FillChar(MShare.g_BuildAcuses, sizeof(MShare.g_BuildAcuses), '\0');
            //FillChar(MShare.g_ItemArr * BotConst.MAXBAGITEMCL, '\0');
            //FillChar(MShare.g_TIItems, sizeof(MShare.g_TIItems), '\0');
            //FillChar(MShare.g_spItems, sizeof(MShare.g_spItems), '\0');
            if (MShare.MovingItem != null)
            {
                if ((MShare.MovingItem.Item.Item.Name != "") && MShare.IsBagItem(MShare.MovingItem.Index))
                {
                    MShare.MovingItem.Item.Item.Name = "";
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
                var cu = EDCode.DecodeClientBuffer<ClientItem>(Str);
                ClFunc.AddItemBag(cu);
            }
            //ClFunc.Loadbagsdat(".\\Config\\" + MShare.g_sServerName + "." + m_sChrName + ".itm-plus", ItemSaveArr);
            //if (ClientGetBagItmes_CompareItemArr())
            //{
            //    Move(ItemSaveArr, MShare.g_ItemArr * BotConst.MAXBAGITEMCL);
            //}
            ClFunc.ArrangeItembag();
            MShare.g_boBagLoaded = true;
            if (MShare.MySelf != null)
            {
                if (!MShare.MySelf.StallMgr.OnSale)
                {
                    for (var i = 0; i < 9; i++)
                    {
                        if (MShare.MySelf.StallMgr.mBlock.Items[i] == null)
                        {
                            continue;
                        }
                        if (MShare.MySelf.StallMgr.mBlock.Items[i].Item.Name != "")
                        {
                            ClFunc.UpdateBagStallItem(MShare.MySelf.StallMgr.mBlock.Items[i], 4);
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < 9; i++)
                    {
                        if (MShare.MySelf.StallMgr.mBlock.Items[i] == null)
                        {
                            continue;
                        }
                        if (MShare.MySelf.StallMgr.mBlock.Items[i].Item.Name != "")
                        {
                            ClFunc.UpdateBagStallItem(MShare.MySelf.StallMgr.mBlock.Items[i], 5);
                        }
                    }
                }
            }
            if (MShare.OpenAutoPlay && (MShare.g_nAPReLogon == 4))
            {
                MShare.g_nAPReLogon = 0;
                MShare.g_nOverAPZone = MShare.g_nOverAPZone2;
                MShare.g_APGoBack = MShare.g_APGoBack2;
                if (MShare.g_APMapPath2 != null)
                {
                    MShare.MapPath = new Point[MShare.g_APMapPath2.Length + 1];
                    for (var k = 0; k <= MShare.g_APMapPath2.Length; k++)
                    {
                        MShare.MapPath[k] = MShare.g_APMapPath2[k];
                    }
                }
                MShare.AutoLastPoint = MShare.AutoLastPoint2;
                MShare.AutoStep = MShare.g_APStep2;
                MShare.g_gcAss[0] = true;
                MShare.AutoTagget = null;
                MShare.AutoPicupItem = null;
                MShare.g_nAPStatus = -1;
                MShare.TargetX = -1;
                MShare.g_APGoBack2 = false;
                MShare.g_APMapPath2 = null;
                GetNearPoint();
                TimerAutoPlay.Enabled = MShare.g_gcAss[0];
                ScreenManager.AddChatBoardString("开始自动挂机...");
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

        private void ClientGetShowItem(int itemid, short x, short y, int looks, string itmname)
        {
            for (var i = 0; i < MShare.g_DropedItemList.Count; i++)
            {
                if (MShare.g_DropedItemList[i].id == itemid)
                {
                    return;
                }
            }
            var dropItem = new TDropItem();
            dropItem.id = itemid;
            dropItem.X = x;
            dropItem.Y = y;
            dropItem.looks = looks;
            dropItem.Name = itmname;
            dropItem.Width = itmname.Length;
            dropItem.Height = itmname.Length;
            HUtil32.GetValidStr3(dropItem.Name, ref itmname, "\\" );
            dropItem.FlashTime = MShare.GetTickCount() - RandomNumber.GetInstance().Random(3000);
            dropItem.BoFlash = false;
            dropItem.boNonSuch = false;
            dropItem.boShowName = BotConst.g_ShowItemList.ContainsKey(itmname);
            dropItem.boPickUp = dropItem.boShowName;
            if (MShare.g_gcAss[5])
            {
                dropItem.boNonSuch = false;
                dropItem.boPickUp = false;
                dropItem.boShowName = false;
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
            MShare.g_DropedItemList.Add(dropItem);
        }

        private void ClientGetHideItem(int itemid, int x, int y)
        {
            TDropItem dropItem;
            for (var i = 0; i < MShare.g_DropedItemList.Count; i++)
            {
                dropItem = MShare.g_DropedItemList[i];
                if (dropItem.id == itemid)
                {
                    MShare.g_DropedItemList.RemoveAt(i);
                    break;
                }
            }
        }

        private void ClientGetSendUseItems(string body)
        {
            int index;
            var str = string.Empty;
            var data = string.Empty;
            while (true)
            {
                if (string.IsNullOrEmpty(body))
                {
                    break;
                }
                body = HUtil32.GetValidStr3(body, ref str, HUtil32.Backslash);
                body = HUtil32.GetValidStr3(body, ref data, HUtil32.Backslash);
                index = HUtil32.StrToInt(str, -1);
                if (index >= 0 && index <= 13)
                {
                    var cu = EDCode.DecodeBuffer<ClientItem>(data);
                    MShare.UseItems[index] = cu;
                }
            }
        }

        public int ClientGetAddMagic_ListSortCompareLevel(object item1, object item2)
        {
            var result = 1;
            if (((ClientMagic)item1).Def.TrainLevel[0] < ((ClientMagic)item2).Def.TrainLevel[0])
            {
                result = -1;
            }
            else if (((ClientMagic)item1).Def.TrainLevel[0] == ((ClientMagic)item2).Def.TrainLevel[0])
            {
                result = 0;
            }
            return result;
        }

        private void ClientGetAddMagic(string body)
        {
            var pcm = EDCode.DecodeBuffer<ClientMagic>(body);
            MShare.MagicArr[pcm.Def.MagicId] = pcm;
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
            MShare.MagicArr[magid] = null;
        }

        public int ClientConvertMagic_ListSortCompareLevel(object item1, object item2)
        {
            var result = 1;
            if (((ClientMagic)item1).Def.TrainLevel[0] < ((ClientMagic)item2).Def.TrainLevel[0])
            {
                result = -1;
            }
            else if (((ClientMagic)item1).Def.TrainLevel[0] == ((ClientMagic)item2).Def.TrainLevel[0])
            {
                result = 0;
            }
            return result;
        }

        private void ClientConvertMagic(int t1, int t2, int id1, int id2, string s)
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

        public int hClientConvertMagic_ListSortCompareLevel(object item1, object item2)
        {
            var result = 1;
            if (((ClientMagic)item1).Def.TrainLevel[0] < ((ClientMagic)item2).Def.TrainLevel[0])
            {
                result = -1;
            }
            else if (((ClientMagic)item1).Def.TrainLevel[0] == ((ClientMagic)item2).Def.TrainLevel[0])
            {
                result = 0;
            }
            return result;
        }

        public int ClientGetMyMagics_ListSortCompareLevel(object item1, object item2)
        {
            var result = 1;
            if (((ClientMagic)item1).Def.TrainLevel[0] < ((ClientMagic)item2).Def.TrainLevel[0])
            {
                result = -1;
            }
            else if (((ClientMagic)item1).Def.TrainLevel[0] == ((ClientMagic)item2).Def.TrainLevel[0])
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
                    MShare.MagicArr[pcm.Def.MagicId] = pcm;
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
            var pcm = MShare.MagicArr[magid];
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
                var pcm = MShare.MagicArr[magid];
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

        private void ClientGetSendItemDlg(int merchant, string str)
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
            if (MShare.OpenAutoPlay && (MShare.g_nAPReLogon == 3))
            {
                MShare.g_nAPReLogon = 4;
                SendClientMessage(Messages.CM_LOGINNOTICEOK, 0, 0, 0, BotConst.CLIENTTYPE);
                return;
            }
            MainOutMessage("确认游戏公告");
            SendClientMessage(Messages.CM_LOGINNOTICEOK, HUtil32.GetTickCount(), 0, 0, 0);
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

        public void MinTimerTimer(object sender, EventArgs e1)
        {
            for (var i = 0; i < PlayScene.ActorList.Count; i++)
            {
                if (IsGroupMember(PlayScene.ActorList[i].UserName))
                {
                    PlayScene.ActorList[i].m_boGrouped = true;
                }
                else
                {
                    PlayScene.ActorList[i].m_boGrouped = false;
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

        public void CheckHackTimerTimer(object sender)
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
            var str = HUtil32.GetValidStr3(body, ref MShare.g_sGuildName, HUtil32.Backslash);
            MShare.g_sGuildRankName = str.Trim();
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

        private void ClientGetPasswordStatus(CommandMessage msg, string body)
        {

        }

        public void SendPassword(string sPassword, int nIdent)
        {
            var defMsg = Messages.MakeMessage(Messages.CM_PASSWORD, 0, nIdent, 0, 0);
            SendSocket(EDCode.EncodeMessage(defMsg) + EDCode.EncodeString(sPassword));
        }

        private void ClientGetServerConfig(CommandMessage msg, string sBody)
        {
            TimerAutoMove = new TimerAutoPlay();
            TimerAutoMove.Enabled = true;
            TimerAutoPlay = new TimerAutoPlay();
            TimerAutoPlay.Enabled = true;
            MShare.OpenAutoPlay = true; //HUtil32.LoByte(HUtil32.LoWord(msg.Recog)) == 1;
            MShare.SpeedRate = msg.Series != 0;
            MShare.SpeedRateShow = MShare.SpeedRate;
            MShare.g_boCanRunMon = HUtil32.HiByte(HUtil32.LoWord(msg.Recog)) == 1;
            MShare.g_boCanRunNpc = HUtil32.LoByte(HUtil32.HiWord(msg.Recog)) == 1;
            MShare.g_boCanRunAllInWarZone = HUtil32.HiByte(HUtil32.HiWord(msg.Recog)) == 1;
            sBody = EDCode.DeCodeString(sBody);
            var clientConf = EDCode.DecodeClientBuffer<ClientConf>(sBody);
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

        public ClientMagic GetMagicById(int magid)
        {
            if ((magid <= 0) || (magid >= 255))
            {
                return null;
            }
            return MShare.MagicArr[magid];
        }

        private void SmartChangePoison(ClientMagic pcm)
        {
            string str;
            string cStr;
            if (MShare.MySelf == null)
            {
                return;
            }
            MShare.MySelf.m_btPoisonDecHealth = 0;
            if (new ArrayList(new int[] { 13, 30, 43, 55, 57 }).Contains(pcm.Def.MagicId))
            {
                str = "符";
                cStr = "符";
            }
            else if (new ArrayList(new int[] { 6, 38 }).Contains(pcm.Def.MagicId))
            {
                if (MShare.MagicTarget != null)
                {
                    str = "药";
                    MShare.g_boExchgPoison = !MShare.g_boExchgPoison;
                    if (MShare.g_boExchgPoison)
                    {
                        MShare.MySelf.m_btPoisonDecHealth = 1;
                        cStr = "灰";
                    }
                    else
                    {
                        MShare.MySelf.m_btPoisonDecHealth = 2;
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
            if ((MShare.UseItems[ItemLocation.Bujuk].Item.StdMode == 25) && (MShare.UseItems[ItemLocation.Bujuk].Item.Shape != 6) && (MShare.UseItems[ItemLocation.Bujuk].Item.Name.IndexOf(cStr, StringComparison.Ordinal) > 0))
            {
                return;
            }
            MShare.g_boCheckTakeOffPoison = false;
            MShare.g_WaitingUseItem.Index = ItemLocation.Bujuk;
            for (var i = 6; i < BotConst.MaxBagItemcl; i++)
            {
                if ((MShare.ItemArr[i].Item.NeedIdentify < 4) && (MShare.ItemArr[i].Item.StdMode == 25) && (MShare.ItemArr[i].Item.Shape != 6) && (MShare.ItemArr[i].Item.Name.IndexOf(str, StringComparison.Ordinal) > 0) && (MShare.ItemArr[i].Item.Name.IndexOf(cStr, StringComparison.Ordinal) > 0))
                {
                    MShare.g_WaitingUseItem.Item = MShare.ItemArr[i];
                    MShare.ItemArr[i].Item.Name = "";
                    MShare.g_boCheckTakeOffPoison = true;
                    SendTakeOnItem(MShare.g_WaitingUseItem.Index, MShare.g_WaitingUseItem.Item.MakeIndex, MShare.g_WaitingUseItem.Item.Item.Name);
                    ClFunc.ArrangeItembag();
                    return;
                }
            }
            if (str == "符")
            {
                ScreenManager.AddChatBoardString("你的[护身符]已经用完");
            }
            else if (MShare.g_boExchgPoison)
            {
                ScreenManager.AddChatBoardString("你的[灰色药粉]已经用完");
            }
            else
            {
                ScreenManager.AddChatBoardString("你的[黄色药粉]已经用完");
            }
        }

        public void TimerAutoMagicTimer(object sender, EventArgs e1)
        {
            ClientMagic pcm;
            if ((MShare.MySelf != null) && MShare.MySelf.StallMgr.OnSale)
            {
                return;
            }
            if ((MShare.MySelf != null) && MShare.g_boAutoSay && (MShare.MySelf.m_sAutoSayMsg != ""))
            {
                //if (MShare.GetTickCount() - FrmDlg.m_sAutoSayMsgTick > 30 * 1000)
                //{
                //    FrmDlg.m_sAutoSayMsgTick = MShare.GetTickCount();
                //    SendSay(MShare.g_MySelf.m_sAutoSayMsg);
                //}
            }
            if ((MShare.MySelf != null) && IsUnLockAction())
            {
                if (CanNextAction() && ServerAcceptNextAction())
                {
                    var nspeed = 0;
                    if (MShare.SpeedRate)
                    {
                        nspeed = MShare.g_MagSpeedRate * 20;
                    }
                    if (MShare.GetTickCount() - MShare.LatestSpellTick > (MShare.SpellTime + MShare.g_dwMagicDelayTime - nspeed))
                    {
                        if (MShare.g_gcTec[4] && ((MShare.MySelf.m_nState & 0x00100000) == 0))
                        {
                            if (MShare.MagicArr[31] != null)
                            {
                                UseMagic(BotConst.ScreenWidth / 2, BotConst.ScreenHeight / 2, MShare.MagicArr[31]);
                                return;
                            }
                        }
                        switch (MShare.MySelf.Job)
                        {
                            case 0:
                                if (MShare.g_gcTec[3] && !MShare.g_boNextTimePursueHit)
                                {
                                    pcm = GetMagicById(56);
                                    if (pcm != null)
                                    {
                                        UseMagic(BotConst.ScreenWidth / 2, BotConst.ScreenHeight / 2, pcm);
                                        return;
                                    }
                                }
                                if (MShare.g_gcTec[11] && !MShare.g_boNextTimeSmiteLongHit2)
                                {
                                    pcm = GetMagicById(113);
                                    if (pcm != null)
                                    {
                                        UseMagic(BotConst.ScreenWidth / 2, BotConst.ScreenHeight / 2, pcm);
                                        return;
                                    }
                                }
                                if (MShare.g_gcTec[2] && !MShare.g_boNextTimeFireHit)
                                {
                                    pcm = GetMagicById(26);
                                    if (pcm != null)
                                    {
                                        UseMagic(BotConst.ScreenWidth / 2, BotConst.ScreenHeight / 2, pcm);
                                        return;
                                    }
                                }
                                if (MShare.g_gcTec[13] && !MShare.g_boCanSLonHit)
                                {
                                    pcm = GetMagicById(66);
                                    if (pcm != null)
                                    {
                                        UseMagic(BotConst.ScreenWidth / 2, BotConst.ScreenHeight / 2, pcm);
                                        return;
                                    }
                                }
                                if (MShare.g_gcTec[9] && !MShare.g_boNextTimeTwinHit)
                                {
                                    pcm = GetMagicById(43);
                                    if (pcm != null)
                                    {
                                        UseMagic(BotConst.ScreenWidth / 2, BotConst.ScreenHeight / 2, pcm);
                                        return;
                                    }
                                }
                                break;
                            case 2:
                                if (MShare.g_gcTec[6] && ((MShare.MySelf.m_nState & 0x00800000) == 0))
                                {
                                    pcm = GetMagicById(18);
                                    if (pcm != null)
                                    {
                                        UseMagic(BotConst.ScreenWidth / 2, BotConst.ScreenHeight / 2, pcm);
                                    }
                                }
                                break;
                        }
                        if (MShare.g_gcTec[7] && (MShare.GetTickCount() - MShare.MySelf.m_dwPracticeTick > HUtil32._MAX(500, MShare.g_gnTecTime[8])))
                        {
                            MShare.MySelf.m_dwPracticeTick = MShare.GetTickCount();
                            pcm = GetMagicById(BotConst.g_gnTecPracticeKey);
                            if (pcm != null)
                            {
                                UseMagic(MShare.MouseX, MShare.MouseY, pcm);
                            }
                        }
                    }
                }
            }
        }

        public int DirToDx(int direction, int tdir)
        {
            int result;
            if (direction == -1)
            {
                direction = 7;
            }
            switch (direction)
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

        public int DirToDy(int direction, int tdir)
        {
            int result;
            if (direction == -1)
            {
                direction = 7;
            }
            switch (direction)
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

        public void TimerAutoMoveTimer(object sender, EventArgs e1)
        {
            short x1 = 0;
            short y1 = 0;
            short x3 = 0;
            short y3 = 0;
            bool boCanRun;
            //if ((MShare.g_MySelf == null) || (Map.m_MapBuf == null) || (!CSocket.Active))
            //{
            //    return;
            //}
            if (PathMap.g_MapPath != null)
            {
                if ((MShare.MySelf.CurrX == MShare.MySelf.m_nTagX) && (MShare.MySelf.CurrY == MShare.MySelf.m_nTagY))
                {
                    TimerAutoMove.Enabled = false;
                    ScreenManager.AddChatBoardString("已经到达终点");
                    PathMap.g_MapPath = Array.Empty<Point>();
                    PathMap.g_MapPath = null;
                    MShare.MySelf.m_nTagX = 0;
                    MShare.MySelf.m_nTagY = 0;
                }
                if (CanNextAction() && ServerAcceptNextAction() && IsUnLockAction())
                {
                    if (GMoveStep <= PathMap.g_MapPath.Length)
                    {
                        MShare.TargetX = (short)PathMap.g_MapPath[GMoveStep].X;
                        MShare.TargetY = (short)PathMap.g_MapPath[GMoveStep].X;
                        while ((Math.Abs(MShare.MySelf.CurrX - MShare.TargetX) <= 1) && (Math.Abs(MShare.MySelf.CurrY - MShare.TargetY) <= 1))
                        {
                            boCanRun = false;
                            if (GMoveStep + 1 <= PathMap.g_MapPath.Length)
                            {
                                x1 = MShare.MySelf.CurrX;
                                y1 = MShare.MySelf.CurrY;
                                short x2 = (short)PathMap.g_MapPath[GMoveStep + 1].X;
                                short y2 = (short)PathMap.g_MapPath[GMoveStep + 1].X;
                                int ndir = ClFunc.GetNextDirection(x1, y1, x2, y2);
                                ClFunc.GetNextPosXY((byte)ndir, ref x1, ref y1);
                                x3 = MShare.MySelf.CurrX;
                                y3 = MShare.MySelf.CurrY;
                                ClFunc.GetNextRunXY(ndir, ref x3, ref y3);
                                if ((PathMap.g_MapPath[GMoveStep + 1].X == x3) && (PathMap.g_MapPath[GMoveStep + 1].X == y3))
                                {
                                    boCanRun = true;
                                }
                            }
                            if (boCanRun && Map.CanMove(x1, y1) && !PlayScene.CrashMan(x1, y1))
                            {
                                GMoveStep++;
                                MShare.TargetX = (short)PathMap.g_MapPath[GMoveStep].X;
                                MShare.TargetY = (short)PathMap.g_MapPath[GMoveStep].X;
                                if (GMoveStep >= PathMap.g_MapPath.Length)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                MShare.TargetX = (short)PathMap.g_MapPath[GMoveStep].X;
                                MShare.TargetY = (short)PathMap.g_MapPath[GMoveStep].X;
                                break;
                            }
                        }
                        if ((Math.Abs(MShare.MySelf.CurrX - MShare.MySelf.m_nTagX) <= 1) && (Math.Abs(MShare.MySelf.CurrY - MShare.MySelf.m_nTagY) <= 1))
                        {
                            MShare.TargetX = MShare.MySelf.m_nTagX;
                            MShare.TargetY = MShare.MySelf.m_nTagY;
                        }
                        if ((Math.Abs(MShare.MySelf.CurrX - MShare.TargetX) <= 1) && (Math.Abs(MShare.MySelf.CurrY - MShare.TargetY) <= 1))
                        {
                            MShare.PlayerAction = PlayerAction.Walk;// 目标座标
                            GMoveBusy = true;
                        }
                        else
                        {
                            if (MShare.MySelf.CanRun() > 0)
                            {
                                MShare.PlayerAction = PlayerAction.Run;
                                GMoveBusy = true;
                            }
                            else
                            {
                                MShare.PlayerAction = PlayerAction.Walk;
                                GMoveBusy = true;
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
            ndir = MShare.MySelf.m_btDir;
            if (RandomNumber.GetInstance().Random(28) == 0)
            {
                ndir = (byte)RandomNumber.GetInstance().Random(8);
            }
            while (i < 16)
            {
                if (!GetNextPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, ndir, 2, ref MShare.TargetX, ref MShare.TargetY))
                {
                    ClFunc.GetFrontPosition(MShare.MySelf.CurrX, MShare.MySelf.CurrY, ndir, ref MShare.TargetX, ref MShare.TargetY);
                    if (!PlayScene.CanWalk(MShare.TargetX, MShare.TargetY))
                    {
                        MShare.MySelf.SendMsg(Messages.CM_TURN, (ushort)MShare.MySelf.CurrX, (ushort)MShare.MySelf.CurrY, RandomNumber.GetInstance().Random(8), 0, 0, "", 0);
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
                    if (PlayScene.CanWalk(MShare.TargetX, MShare.TargetY))
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
            byte ndir = 0;
            var success = false;
            MShare.AutoMove = false;
            if (MShare.MySelf == null)
            {
                return;
            }
            if (!MShare.OpenAutoPlay)
            {
                return;
            }
            if (MShare.MySelf.Death)
            {
                MShare.g_gcAss[0] = false;
                MShare.AutoTagget = null;
                MShare.AutoPicupItem = null;
                MShare.g_nAPStatus = -1;
                MShare.TargetX = -1;
                TimerAutoPlay.Enabled = MShare.g_gcAss[0];
                return;
            }
            MShare.AutoPicupItem = null;
            switch (_heroActor.GetAutoPalyStation())
            {
                case 0:
                    if (!EatItemName("回城卷") && !EatItemName("回城卷包") && !EatItemName("盟重传送石") && !EatItemName("比奇传送石"))
                    {
                        ScreenManager.AddChatBoardString("你的回城卷已用完,回安全区等待!!!");
                    }
                    else
                    {
                        ScreenManager.AddChatBoardString("回安全区等待!!!");
                    }
                    MShare.g_gcAss[0] = false;
                    MShare.AutoTagget = null;
                    MShare.AutoPicupItem = null;
                    MShare.g_nAPStatus = -1;
                    MShare.TargetX = -1;
                    TimerAutoPlay.Enabled = MShare.g_gcAss[0];
                    MShare.AutoMove = true;
                    break;
                case 1:// 此时为该怪物首次被发现，自动寻找路径
                    if (_heroActor.AttackTagget(MShare.AutoTagget))
                    {
                        return;
                    }
                    if (MShare.AutoTagget != null)
                    {
                        MShare.TargetX = MShare.AutoTagget.CurrX;
                        MShare.TargetY = MShare.AutoTagget.CurrY;
                        _heroActor.AutoFindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.AutoTagget.CurrX, MShare.AutoTagget.CurrY);
                    }
                    MShare.TargetX = -1;
                    MShare.g_nAPStatus = 1;
                    MShare.AutoMove = true;
                    break;
                case 2:// 此时该物品为首次发现，自动寻找路径
                    if ((MShare.AutoPicupItem != null) && ((MShare.g_nAPStatus != 2) || (MShare.AutoPathList.Count == 0)))
                    {
                        MShare.TargetX = MShare.AutoPicupItem.X;
                        MShare.TargetY = MShare.AutoPicupItem.Y;
                        _heroActor.AutoFindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.TargetX, MShare.TargetY);
                        MShare.TargetX = -1;
                        MainOutMessage( $"物品目标：{MShare.AutoPicupItem.Name}({MShare.AutoPicupItem.X},{MShare.AutoPicupItem.Y}) 正在去拾取");
                    }
                    else if (MShare.AutoPicupItem != null)
                    {
                        MShare.TargetX = MShare.AutoPicupItem.X;
                        MShare.TargetY = MShare.AutoPicupItem.Y;
                        _heroActor.AutoFindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.TargetX, MShare.TargetY);
                        MShare.TargetX = -1;
                        MainOutMessage( $"物品目标：{MShare.AutoPicupItem.Name}({MShare.AutoPicupItem.X},{MShare.AutoPicupItem.Y}) 正在去拾取");
                    }
                    MShare.g_nAPStatus = 2;
                    MShare.AutoMove = true;
                    break;
                case 3:
                    if ((MShare.MapPath != null) && (MShare.AutoStep >= 0) && (MShare.AutoStep <= MShare.MapPath.Length))
                    {
                        if (MShare.MapPath.Length > 0)
                        {
                            MShare.TargetX = (short)MShare.MapPath[MShare.AutoStep].X;
                            MShare.TargetY = (short)MShare.MapPath[MShare.AutoStep].X;
                            _heroActor.AutoFindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.TargetX, MShare.TargetY);
                            MainOutMessage( $"循路搜寻目标({MShare.TargetX},{MShare.TargetY})");
                            MShare.TargetX = -1;
                        }
                        else
                        {
                            if ((MShare.TargetX == -1) || (MShare.AutoPathList.Count == 0))
                            {
                                RunAutoPlayRandomTag(ref success, ref ndir);
                                if (success)
                                {
                                    _heroActor.AutoFindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.TargetX, MShare.TargetY);
                                }
                                MainOutMessage($"定点随机搜寻目标({MShare.MapPath[MShare.AutoStep].X},{MShare.MapPath[MShare.AutoStep].X})");
                                MShare.TargetX = -1;
                            }
                        }
                    }
                    else if ((MShare.TargetX == -1) || (MShare.AutoPathList.Count == 0))
                    {
                        RunAutoPlayRandomTag(ref success, ref ndir);
                        if (success)
                        {
                            _heroActor.AutoFindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.TargetX, MShare.TargetY);
                        }
                        MainOutMessage("随机搜寻目标...");
                        MShare.TargetX = -1;
                    }
                    MShare.g_nAPStatus = 3;
                    MShare.AutoMove = true;
                    break;
                case 4:
                    if ((MShare.MapPath != null) && (MShare.AutoStep >= 0) && (MShare.AutoStep <= MShare.MapPath.Length))
                    {
                        if (MShare.AutoLastPoint.X >= 0)
                        {
                            MShare.TargetX = (short)MShare.AutoLastPoint.X;
                            MShare.TargetY = (short)MShare.AutoLastPoint.X;
                            _heroActor.AutoFindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.TargetX, MShare.TargetY);
                        }
                        else
                        {
                            MShare.TargetX = (short)MShare.MapPath[MShare.AutoStep].X;
                            MShare.TargetY = (short)MShare.MapPath[MShare.AutoStep].X;
                            _heroActor.AutoFindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.TargetX, MShare.TargetY);
                        }
                        MainOutMessage($"超出搜寻范围,返回({MShare.TargetX},{MShare.TargetY})");
                        MShare.TargetX = -1;
                    }
                    else if ((MShare.TargetX == -1) || (MShare.AutoPathList.Count == 0))
                    {
                        RunAutoPlayRandomTag(ref success, ref ndir);
                        if (success)
                        {
                            _heroActor.AutoFindPath(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.TargetX, MShare.TargetY);
                        }
                        MainOutMessage($"超出搜寻范围,随机搜寻目标({MShare.TargetX},{MShare.TargetY})");
                        MShare.TargetX = -1;
                    }
                    MShare.g_nAPStatus = 3;
                    MShare.AutoMove = true;
                    break;
            } 
            if ((MShare.AutoPathList.Count > 0) && ((MShare.TargetX == -1) || ((MShare.TargetX == MShare.MySelf.CurrX) && (MShare.TargetY == MShare.MySelf.CurrY))))
            {
                var findMap = MShare.AutoPathList[0];
                MShare.TargetX = findMap.X;
                MShare.TargetY = findMap.Y;
                if (MShare.g_nAPStatus >= 1 && MShare.g_nAPStatus <= 4)
                {
                    if ((Math.Abs(MShare.MySelf.CurrX - MShare.TargetX) <= 1) && (Math.Abs(MShare.MySelf.CurrY - MShare.TargetY) <= 1))
                    {
                        if (PlayScene.CanWalk(MShare.TargetX, MShare.TargetY))
                        {
                            if ((MShare.g_nAPStatus == 2) && (MShare.AutoPicupItem != null))
                            {
                                if ((Math.Abs(MShare.MySelf.CurrX - MShare.AutoPicupItem.X) > 1) || (Math.Abs(MShare.MySelf.CurrY - MShare.AutoPicupItem.Y) > 1))
                                {
                                    RunAutoPlayAAAA(findMap.X, findMap.Y);
                                }
                                else
                                {
                                    MShare.AutoPathList.RemoveAt(0);
                                    return;
                                }
                            }
                        }
                    }
                }
                MShare.AutoPathList.RemoveAt(0);
            }
            if (MShare.AutoMove && (MShare.AutoPathList.Count > 0))
            {
                _heroActor.InitQueue2();
            }
        }

        private void RunAutoPlayAAAA(short X,short Y)
        {
            var ndir = 0;
            if (MShare.AutoPathList.Count > 2)
            {
                ndir = ClFunc.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.AutoPathList[2].X, MShare.AutoPathList[2].Y);
            }
            else
            {
                ndir = ClFunc.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, MShare.TargetX, MShare.TargetY);
            }
            short x1 = MShare.MySelf.CurrX;
            short y1 = MShare.MySelf.CurrY;
            ClFunc.GetNextRunXY(ndir, ref x1, ref y1);
            if (Map.CanMove(x1, y1))
            {
                if (PlayScene.CrashMan(x1, y1))
                {
                    MShare.TargetX = X;
                    MShare.TargetY = Y;
                    MShare.PlayerAction = PlayerAction.Walk;
                }
                else
                {
                    MShare.TargetX = x1;
                    MShare.TargetY = y1;
                    MShare.PlayerAction = PlayerAction.Run;
                }
            }
        }

        private void ProcessActMsg(string datablock)
        {
            var tagstr = string.Empty;
            if ((datablock[1] == 'G') && (datablock[2] == 'D') && (datablock[3] == '/'))
            {
                string data = datablock[1..];
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
                    GMoveBusy = false;
                    GMoveErr = 0;
                    if (TimerAutoMove.Enabled)
                    {
                        GMoveStep++;
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
                        // DScreen.AddChatBoardString('[速度检测] 时间差：' + IntToStr(cltime - svtime), clWhite);
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
                tagstr = datablock[1..^0];
            }
            switch (tagstr)
            {
                case "DIG":
                    MShare.MySelf.m_boDigFragment = true;
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
                    ScreenManager.AddChatBoardString("双龙斩开启");
                    break;
                case "UCRS":
                    MShare.g_boCanCrsHit = false;
                    ScreenManager.AddChatBoardString("双龙斩关闭");
                    break;
                case "TWN":
                    MShare.g_boNextTimeTwinHit = true;
                    MShare.g_dwLatestTwinHitTick = MShare.GetTickCount();
                    ScreenManager.AddChatBoardString("召集雷电力量成功");
                    break;
                case "UTWN":
                    MShare.g_boNextTimeTwinHit = false;
                    ScreenManager.AddChatBoardString("雷电力量消失");
                    break;
                case "SQU":
                    MShare.g_boCanSquHit = true;
                    ScreenManager.AddChatBoardString("[龙影剑法] 开启");
                    break;
                case "FIR":
                    MShare.g_boNextTimeFireHit = true;
                    MShare.LatestFireHitTick = MShare.GetTickCount();
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
                    ScreenManager.AddChatBoardString("[血魂一击] 已准备...");
                    break;
                case "SMIL":
                    MShare.g_boNextTimeSmiteLongHit = true;
                    MShare.g_dwLatestSmiteLongHitTick = MShare.GetTickCount();
                    break;
                case "SMIL2":
                    MShare.g_boNextTimeSmiteLongHit2 = true;
                    MShare.g_dwLatestSmiteLongHitTick2 = MShare.GetTickCount();
                    ScreenManager.AddChatBoardString("[断空斩] 已准备...");
                    break;
                case "SMIW":
                    MShare.g_boNextTimeSmiteWideHit = true;
                    MShare.g_dwLatestSmiteWideHitTick = MShare.GetTickCount();
                    break;
                case "SMIW2":
                    MShare.g_boNextTimeSmiteWideHit2 = true;
                    MShare.g_dwLatestSmiteWideHitTick2 = MShare.GetTickCount();
                    ScreenManager.AddChatBoardString("[倚天辟地] 已准备");
                    break;
                case "MDS":
                    ScreenManager.AddChatBoardString("[美杜莎之瞳] 技能可施展");
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
                    ScreenManager.AddChatBoardString("[龙影剑法] 关闭");
                    break;
                case "SLON":
                    MShare.g_boCanSLonHit = true;
                    MShare.LatestSLonHitTick = MShare.GetTickCount();
                    ScreenManager.AddChatBoardString("[开天斩] 力量凝聚...");
                    break;
                case "USLON":
                    MShare.g_boCanSLonHit = false;
                    ScreenManager.AddChatBoardString("[开天斩] 力量消失");
                    break;
            }
        }

        private static void GetNearPoint()
        {
            if ((MShare.MapPath != null) && (MShare.MapPath.Length > 0))
            {
                var n14 = 0;
                MShare.AutoLastPoint.X = -1;
                var n10 = 999;
                for (var i = 0; i < MShare.MapPath.Length; i++)
                {
                    var nC = Math.Abs(MShare.MapPath[i].X - MShare.MySelf.CurrX) + Math.Abs(MShare.MapPath[i].X - MShare.MySelf.CurrY);
                    if (nC < n10)
                    {
                        n10 = nC;
                        n14 = i;
                    }
                }
                MShare.AutoStep = n14;
            }
        }

        public bool GetNextPosition(short sx, short sy, int ndir, short nFlag, ref short snx, ref short sny)
        {
            bool result;
            snx = sx;
            sny = sy;
            switch (ndir)
            {
                case Direction.Up:
                    if (sny > nFlag - 1)
                    {
                        sny -= nFlag;
                    }
                    break;
                case Direction.Down:
                    if (sny < (Map.m_MapHeader.wHeight - nFlag))
                    {
                        sny += nFlag;
                    }
                    break;
                case Direction.Left:
                    if (snx > nFlag - 1)
                    {
                        snx -= nFlag;
                    }
                    break;
                case Direction.Right:
                    if (snx < (Map.m_MapHeader.wWidth - nFlag))
                    {
                        snx += nFlag;
                    }
                    break;
                case Direction.UpLeft:
                    if ((snx > nFlag - 1) && (sny > nFlag - 1))
                    {
                        snx -= nFlag;
                        sny -= nFlag;
                    }
                    break;
                case Direction.UpRight:
                    if ((snx > nFlag - 1) && (sny < (Map.m_MapHeader.wHeight - nFlag)))
                    {
                        snx += nFlag;
                        sny -= nFlag;
                    }
                    break;
                case Direction.DownLeft:
                    if ((snx < (Map.m_MapHeader.wWidth - nFlag)) && (sny > nFlag - 1))
                    {
                        snx -= nFlag;
                        sny += nFlag;
                    }
                    break;
                case Direction.DownRight:
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
                Actor actor = PlayScene.FindActorXY(sx, sy);
                if (actor != null)
                {
                    if (_heroActor.IsProperTarget(actor))
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

        public bool GetAdvPosition(Actor targetCret, ref short nX, ref short nY)
        {
            var result = false;
            nX = MShare.MySelf.CurrX;
            nY = MShare.MySelf.CurrY;
            var btDir = ClFunc.GetNextDirection(MShare.MySelf.CurrX, MShare.MySelf.CurrY, targetCret.CurrX, targetCret.CurrY);
            var wvar1 = MShare.MySelf;
            switch (btDir)
            {
                case Direction.Up:
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
                    if (!PlayScene.CanWalk(nX, nY))
                    {
                        nY = (short)(wvar1.CurrY + 2);
                    }
                    break;
                case Direction.Down:
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
                    if (!PlayScene.CanWalk(nX, nY))
                    {
                        nY = (short)(wvar1.CurrY - 2);
                    }
                    break;
                case Direction.Left:
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
                    if (!PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(wvar1.CurrX + 2);
                    }
                    break;
                case Direction.Right:
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
                    if (!PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(wvar1.CurrX - 2);
                    }
                    break;
                case Direction.UpLeft:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nX += 2;
                    }
                    else
                    {
                        nY += 2;
                        nY -= 1;
                    }
                    if (!PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(wvar1.CurrX + 2);
                        nY = (short)(wvar1.CurrY + 2);
                    }
                    break;
                case Direction.UpRight:
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
                    if (!PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(wvar1.CurrX - 2);
                        nY = (short)(wvar1.CurrY + 2);
                    }
                    break;
                case Direction.DownLeft:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nX += 2;
                    }
                    else
                    {
                        nY -= 2;
                    }
                    if (!PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(wvar1.CurrX + 2);
                        nY = (short)(wvar1.CurrY - 2);
                    }
                    break;
                case Direction.DownRight:
                    if (RandomNumber.GetInstance().Random(2) == 0)
                    {
                        nX -= 2;
                    }
                    else
                    {
                        nY -= 2;
                    }
                    if (!PlayScene.CanWalk(nX, nY))
                    {
                        nX = (short)(wvar1.CurrX - 2);
                        nY = (short)(wvar1.CurrY - 2);
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

        public int GetMagicLv(Actor actor, int magid)
        {
            if (actor == null)
            {
                return 0;
            }
            if ((magid <= 0) || (magid >= 255))
            {
                return 0;
            }
            return MShare.MagicArr[magid] != null ? MShare.MagicArr[magid].Level : 0;
        }

        private void MainOutMessage(string msg)
        {
            BotShare.logger.Info($"机器人:[{ChrName}] {msg}");
        }

        private void MainOutErrorMessage(string msg)
        {
            BotShare.logger.Error($"机器人:[{ChrName}] {msg}");
        }

        private void MainOutWarnMessage(string msg)
        {
            BotShare.logger.Warn($"机器人:[{ChrName}] {msg}");
        }
    }
}