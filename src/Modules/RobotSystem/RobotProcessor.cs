using M2Server;
using NLog;
using SystemModule;
using SystemModule.Data;

namespace RobotSystem
{
    public class RobotProcessor : TimerScheduledService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 假人列表
        /// </summary>
        public readonly Queue<RoBotLogon> RobotLogonQueue;
        public int RobotLogonQueueCount => RobotLogonQueue.Count;
        public int ProcessedMonsters { get; private set; }
        /// <summary>
        /// 处理假人间隔
        /// </summary>
        public long RobotLogonTick { get; set; }
        private int ProcBotHubIdx { get; set; }
        protected readonly IList<IRobotPlayer> BotPlayObjectList;
        public int RobotPlayerCount => BotPlayObjectList.Count;

        public RobotProcessor() : base(TimeSpan.FromMilliseconds(100), "RobotProcessor")
        {
            RobotLogonQueue = new Queue<RoBotLogon>();
            BotPlayObjectList = new List<IRobotPlayer>();
        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            logger.Debug("初始化Robot(机器人)处理插件...");
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            logger.Info("机器人管理线程初始化完成...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            logger.Info("机器人管理线程停止ֹ...");
        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            try
            {
                ProcessedMonsters = 0;
                ProcessRobotPlayData();
                /*foreach (var map in Kernel.MapMgr.GameMaps.Values)
                    ProcessedMonsters += await map.OnTimerAsync();
                await Kernel.RoleManager.OnRoleTimerAsync();*/
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error("[Exception] RobotProcessor::ExecuteInternal");
                logger.Error(ex);
            }
            return Task.CompletedTask;
        }

        public void AddRobotLogon(RoBotLogon ai)
        {
            RobotLogonQueue.Enqueue(ai);
        }

        private void RegenRobotPlayer(RoBotLogon ai)
        {
            var playObject = CreateRobotPlayObject(ai);
            if (playObject != null)
            {
                short homeX = 0;
                short homeY = 0;
                //playObject.HomeMap = GetHomeInfo(playObject.Job, ref homeX, ref homeY);
                playObject.HomeX = homeX;
                playObject.HomeY = homeY;
                playObject.MapFileName = playObject.HomeMap;
                playObject.UserAccount = "假人" + ai.sChrName;
                //playObject.Start(FindPathType.Dynamic);
                BotPlayObjectList.Add(playObject);
            }
        }

        public void ProcessRobotPlayData()
        {
            const string sExceptionMsg = "[Exception] WorldServer::ProcessRobotPlayData";
            //人工智障开始登陆
            if (RobotLogonQueue.Count > 0)
            {
                if (HUtil32.GetTickCount() - RobotLogonTick > 1000)
                {
                    RobotLogonTick = HUtil32.GetTickCount();
                    if (RobotLogonQueue.Count > 0)
                    {
                        RegenRobotPlayer(RobotLogonQueue.Dequeue());
                    }
                }
            }
            try
            {
                var dwCurTick = HUtil32.GetTickCount();
                var nIdx = ProcBotHubIdx;
                var boCheckTimeLimit = false;
                var dwCheckTime = HUtil32.GetTickCount();
                while (true)
                {
                    if (BotPlayObjectList.Count <= nIdx) break;
                    var robotPlayer = BotPlayObjectList[nIdx];
                    if (dwCurTick - robotPlayer.RunTick > robotPlayer.RunTime)
                    {
                        robotPlayer.RunTick = dwCurTick;
                        if (!robotPlayer.Ghost)
                        {
                            if (!robotPlayer.LoginNoticeOk)
                            {
                                robotPlayer.RunNotice();
                            }
                            else
                            {
                                if (!robotPlayer.BoReadyRun)
                                {
                                    robotPlayer.BoReadyRun = true;
                                    robotPlayer.UserLogon();
                                }
                                else
                                {
                                    if ((HUtil32.GetTickCount() - robotPlayer.SearchTick) > robotPlayer.SearchTime)
                                    {
                                        robotPlayer.SearchTick = HUtil32.GetTickCount();
                                        robotPlayer.SearchViewRange();
                                        robotPlayer.GameTimeChanged();
                                    }
                                    robotPlayer.Run();
                                }
                            }
                        }
                        else
                        {
                            BotPlayObjectList.Remove(robotPlayer);
                            robotPlayer.Disappear();
                            //AddToHumanFreeList(robotPlayer);
                            //robotPlayer.DealCancelA();
                            //SaveHumanRcd(robotPlayer);
                            // GameShare.SocketMgr.CloseUser(robotPlayer.GateIdx, robotPlayer.SocketId);
                            // SendServerGroupMsg(Messages.SS_202, M2Share.ServerIndex, robotPlayer.ChrName);
                            continue;
                        }
                    }
                    nIdx++;
                    if ((HUtil32.GetTickCount() - dwCheckTime) > M2Share.HumLimit)
                    {
                        boCheckTimeLimit = true;
                        ProcBotHubIdx = nIdx;
                        break;
                    }
                }
                if (!boCheckTimeLimit) ProcBotHubIdx = 0;
            }
            catch (Exception ex)
            {
                logger.Error(sExceptionMsg);
                logger.Error(ex.StackTrace);
            }
        }

        private static IRobotPlayer CreateRobotPlayObject(RoBotLogon ai)
        {
            return null;
            //var envirnoment = SystemShare.MapMgr.FindMap(ai.sMapName);
            //if (envirnoment == null)
            //{
            //    return null;
            //}
            //var cert = new RobotPlayer();
            //cert.Envir = envirnoment;
            //cert.MapName = ai.sMapName;
            //cert.CurrX = ai.nX;
            //cert.CurrY = ai.nY;
            //cert.Dir = (byte)M2Share.RandomNumber.Random(8);
            //cert.ChrName = ai.sChrName;
            //cert.WAbil = cert.Abil;
            //if (M2Share.RandomNumber.Random(100) < cert.CoolEyeCode)
            //{
            //    cert.CoolEye = true;
            //}
            ////Cert.m_sIPaddr = GetIPAddr;// Mac问题
            ////Cert.m_sIPLocal = GetIPLocal(Cert.m_sIPaddr);
            //cert.ConfigFileName = ai.sConfigFileName;
            //cert.FilePath = ai.sFilePath;
            //cert.ConfigListFileName = ai.sConfigListFileName;
            //cert.HeroConfigListFileName = ai.sHeroConfigListFileName;// 英雄配置列表目录
            //cert.Initialize();
            //cert.RecalcLevelAbilitys();
            //cert.RecalcAbilitys();
            //cert.Abil.HP = cert.Abil.MaxHP;
            //cert.Abil.MP = cert.Abil.MaxMP;
            //if (cert.AddtoMapSuccess)
            //{
            //    bool mapSuccess = false;
            //    int n20;
            //    if (cert.Envir.Width < 50)
            //    {
            //        n20 = 2;
            //    }
            //    else
            //    {
            //        n20 = 3;
            //    }
            //    int n24;
            //    if (cert.Envir.Height < 250)
            //    {
            //        if (cert.Envir.Height < 30)
            //        {
            //            n24 = 2;
            //        }
            //        else
            //        {
            //            n24 = 20;
            //        }
            //    }
            //    else
            //    {
            //        n24 = 50;
            //    }
            //    var n1C = 0;
            //    while (true)
            //    {
            //        if (!cert.Envir.CanWalk(cert.CurrX, cert.CurrY, false))
            //        {
            //            if ((cert.Envir.Width - n24 - 1) > cert.CurrX)
            //            {
            //                cert.CurrX += (short)n20;
            //            }
            //            else
            //            {
            //                cert.CurrX = (byte)(M2Share.RandomNumber.Random(cert.Envir.Width / 2) + n24);
            //                if (cert.Envir.Height - n24 - 1 > cert.CurrY)
            //                {
            //                    cert.CurrY += (short)n20;
            //                }
            //                else
            //                {
            //                    cert.CurrY = (byte)(M2Share.RandomNumber.Random(cert.Envir.Height / 2) + n24);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            mapSuccess = cert.Envir.AddMapObject(cert.CurrX, cert.CurrY, cert.CellType, cert.ActorId, cert);
            //            break;
            //        }
            //        n1C++;
            //        if (n1C >= 31)
            //        {
            //            break;
            //        }
            //    }
            //    if (!mapSuccess)
            //    {
            //        cert = null;
            //    }
            //}
            //return cert;
        }
    }
}