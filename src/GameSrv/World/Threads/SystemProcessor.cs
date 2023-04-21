using GameSrv.Planes;
using GameSrv.Services;
using NLog;

namespace GameSrv.World.Threads
{
    public class SystemProcessor : TimerScheduledService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int RunTimeTick;
        private int ShowOnlineTick { get; set; }
        private int SendOnlineHumTime { get; set; }

        public SystemProcessor() : base(TimeSpan.FromMilliseconds(50), "SystemThread")
        {

        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        protected override void Startup(CancellationToken stoppingToken)
        {
            RunTimeTick = HUtil32.GetTickCount();
            ShowOnlineTick = HUtil32.GetTickCount();
            SendOnlineHumTime = HUtil32.GetTickCount();
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            
        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            Run();
            IdSrvClient.Instance.Run();
            GameShare.WorldEngine.PrcocessData();
            ProcessGameRun();
            if (GameShare.ServerIndex == 0)
            {
                PlanesServer.Instance.Run();
            }
            else
            {
                PlanesClient.Instance.Run();
            }
            GetGameTime();
            return Task.CompletedTask;
        }

        private void Run()
        {
            if ((HUtil32.GetTickCount() - ShowOnlineTick) > GameShare.Config.ConsoleShowUserCountTime)
            {
                ShowOnlineTick = HUtil32.GetTickCount();
                GameShare.NoticeMgr.LoadingNotice();
                _logger.Info("在线数: " + GameShare.WorldEngine.PlayObjectCount);
                GameShare.CastleMgr.Save();
            }
            if ((HUtil32.GetTickCount() - SendOnlineHumTime) > 10000)
            {
                SendOnlineHumTime = HUtil32.GetTickCount();
                IdSrvClient.Instance.SendOnlineHumCountMsg(GameShare.WorldEngine.OnlinePlayObject);
            }
        }

        private void ProcessGameRun()
        {
            HUtil32.EnterCriticalSections(GameShare.ProcessHumanCriticalSection);
            try
            {
                if ((HUtil32.GetTickCount() - RunTimeTick) > 10000)
                {
                    RunTimeTick = HUtil32.GetTickCount();
                    GameShare.GuildMgr.Run();
                    GameShare.CastleMgr.Run();
                    GameShare.SocketMgr.Run();
                    if (!GameShare.DenySayMsgList.IsEmpty)
                    {
                        List<string> denyList = new List<string>(GameShare.DenySayMsgList.Count);
                        foreach (KeyValuePair<string, long> item in GameShare.DenySayMsgList)
                        {
                            if (HUtil32.GetTickCount() > item.Value)
                            {
                                denyList.Add(item.Key);
                            }
                        }
                        for (int i = 0; i < denyList.Count; i++)
                        {
                            if (GameShare.DenySayMsgList.TryRemove(denyList[i], out long _))
                            {
                                GameShare.Logger.Debug($"解除玩家禁言[{denyList[i]}]");
                            }
                        }
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSections(GameShare.ProcessHumanCriticalSection);
            }
        }

        private static void GetGameTime()
        {
            switch (DateTime.Now.Hour)
            {
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 16:
                case 17:
                case 18:
                case 19:
                case 21:
                    GameShare.GameTime = 1;//白天
                    break;
                case 11:
                case 23:
                case 20:
                    GameShare.GameTime = 2;//日落
                    break;
                case 4:
                case 15:
                    GameShare.GameTime = 0;//日出
                    break;
                case 0:
                case 1:
                case 2:
                case 3:
                case 12:
                case 13:
                case 14:
                case 22:
                    GameShare.GameTime = 3;//夜晚
                    break;
            }
        }
    }
}