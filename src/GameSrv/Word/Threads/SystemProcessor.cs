using M2Server;
using NLog;
using PlanesSystem;
using SystemModule;

namespace GameSrv.Word.Threads
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
            M2Share.NetChannel.Run();
            M2Share.LoginSession.Run();
            SystemShare.WorldEngine.Run();
            M2Share.AutoBot.Run();
            if (M2Share.ServerIndex == 0)
            {
                PlanesServer.Instance.Run();
            }
            else
            {
                PlanesClient.Instance.Run();
            }
            GetGameTime();
            ShowOnline();
            ProcessGameRun();
            return Task.CompletedTask;
        }

        private void ShowOnline()
        {
            if ((HUtil32.GetTickCount() - ShowOnlineTick) > SystemShare.Config.ConsoleShowUserCountTime)
            {
                ShowOnlineTick = HUtil32.GetTickCount();
                M2Share.NoticeMgr.LoadingNotice();
                _logger.Info("在线数: " + SystemShare.WorldEngine.PlayObjectCount);
                SystemShare.CastleMgr.Save();
            }
            if ((HUtil32.GetTickCount() - SendOnlineHumTime) > 10000)
            {
                SendOnlineHumTime = HUtil32.GetTickCount();
                M2Share.LoginSession.SendOnlineHumCountMsg(SystemShare.WorldEngine.OnlinePlayObject);
            }
        }

        private void ProcessGameRun()
        {
            if ((HUtil32.GetTickCount() - RunTimeTick) > 10000)
            {
                RunTimeTick = HUtil32.GetTickCount();
                SystemShare.GuildMgr.Run();
                SystemShare.CastleMgr.Run();
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
                    M2Share.GameTime = 1;//白天
                    break;
                case 11:
                case 23:
                case 20:
                    M2Share.GameTime = 2;//日落
                    break;
                case 4:
                case 15:
                    M2Share.GameTime = 0;//日出
                    break;
                case 0:
                case 1:
                case 2:
                case 3:
                case 12:
                case 13:
                case 14:
                case 22:
                    M2Share.GameTime = 3;//夜晚
                    break;
            }
        }
    }
}