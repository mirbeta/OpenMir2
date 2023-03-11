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
            M2Share.WorldEngine.PrcocessData();
            ProcessGameRun();
            if (M2Share.ServerIndex == 0)
            {
                PlanesServer.Instance.Run();
            }
            else
            {
                PlanesClient.Instance.Run();
            }
            return Task.CompletedTask;
        }

        private void Run()
        {
            if ((HUtil32.GetTickCount() - ShowOnlineTick) > M2Share.Config.ConsoleShowUserCountTime)
            {
                ShowOnlineTick = HUtil32.GetTickCount();
                M2Share.NoticeMgr.LoadingNotice();
                _logger.Info("在线数: " + M2Share.WorldEngine.PlayObjectCount);
                M2Share.CastleMgr.Save();
            }
            if ((HUtil32.GetTickCount() - SendOnlineHumTime) > 10000)
            {
                SendOnlineHumTime = HUtil32.GetTickCount();
                IdSrvClient.Instance.SendOnlineHumCountMsg(M2Share.WorldEngine.OnlinePlayObject);
            }
        }

        private void ProcessGameRun()
        {
            HUtil32.EnterCriticalSections(M2Share.ProcessHumanCriticalSection);
            try
            {
                if ((HUtil32.GetTickCount() - RunTimeTick) > 10000)
                {
                    RunTimeTick = HUtil32.GetTickCount();
                    M2Share.GuildMgr.Run();
                    M2Share.CastleMgr.Run();
                    M2Share.GateMgr.Run();
                    if (!M2Share.DenySayMsgList.IsEmpty)
                    {
                        List<string> denyList = new List<string>(M2Share.DenySayMsgList.Count);
                        foreach (KeyValuePair<string, long> item in M2Share.DenySayMsgList)
                        {
                            if (HUtil32.GetTickCount() > item.Value)
                            {
                                denyList.Add(item.Key);
                            }
                        }
                        for (int i = 0; i < denyList.Count; i++)
                        {
                            if (M2Share.DenySayMsgList.TryRemove(denyList[i], out long _))
                            {
                                M2Share.Logger.Debug($"解除玩家禁言[{denyList[i]}]");
                            }
                        }
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSections(M2Share.ProcessHumanCriticalSection);
            }
        }
    }
}