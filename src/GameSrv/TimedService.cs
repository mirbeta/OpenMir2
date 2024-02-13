using GameSrv.Word;
using PlanesSystem;
using SystemModule.Enums;

namespace GameSrv
{
    public class TimedService : BackgroundService
    {
        private readonly PeriodicTimer _timer;
        private int CheckIntervalTime { get; set; }
        private int SaveIntervalTime { get; set; }
        private int ClearIntervalTime { get; set; }
        private int ScheduledSaveIntervalTime { get; set; }
        private int PlayerHighestRankTime { get; set; }
        /// <summary>
        /// 是否正在保存数据
        /// </summary>
        private bool ScheduledSaveData { get; set; }
        private int SendOnlineTick { get; set; }

        public TimedService()
        {
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            int currentTick = HUtil32.GetTickCount();
            CheckIntervalTime = currentTick;
            SaveIntervalTime = currentTick;
            ClearIntervalTime = currentTick;
            ScheduledSaveIntervalTime = currentTick;
            PlayerHighestRankTime = currentTick;
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    ExecuteInternal();
                }
            }
            catch (OperationCanceledException)
            {
                LogService.Debug("TimedService is stopping.");
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            LogService.Info("后台服务停止");
            _timer.Dispose();
            return base.StopAsync(cancellationToken);
        }

        private void ExecuteInternal()
        {
            if (!M2Share.StartReady)
            {
                return;
            }

            int currentTick = HUtil32.GetTickCount();
            if ((currentTick - CheckIntervalTime) > 10 * 1000) //10s一次检查链接
            {
                CheckIntervalTime = HUtil32.GetTickCount();
                PlanesClient.Instance.CheckConnected();
                //await GameShare.ChatService.Ping();
            }
            if ((currentTick - SaveIntervalTime) > 60 * 1000) //保存游戏变量等
            {
                SaveIntervalTime = HUtil32.GetTickCount();
                SaveItemNumber();
                ProcessGameNotice();
            }
            if ((currentTick - ClearIntervalTime) > 60 * 10000) //定时清理游戏对象
            {
                ClearIntervalTime = HUtil32.GetTickCount();
                GameShare.Statistics.ShowServerStatus();
                SystemShare.ActorMgr.CleanObject();
            }
            if ((currentTick - PlayerHighestRankTime) > 60 * 1000) //定时更新玩家最高属性排行榜
            {
                PlayerHighestRankTime = HUtil32.GetTickCount();
                PlayerHighestRank();
            }
            if (currentTick - ScheduledSaveIntervalTime > 60 * 10000) //定时保存玩家数据
            {
                ScheduledSaveIntervalTime = HUtil32.GetTickCount();
                TimingSaveData();
            }
        }

        private void PlayerHighestRank()
        {
            try
            {
                // 计算在线最高等级、PK、攻击力、魔法、道术 的人物
                //if (M2Share.HighLevelHuman == ActorId && (Death || Ghost))
                //{
                //    M2Share.HighLevelHuman = 0;
                //}
                //if (M2Share.HighPKPointHuman == ActorId && (Death || Ghost))
                //{
                //    M2Share.HighPKPointHuman = 0;
                //}
                //if (M2Share.HighDCHuman == ActorId && (Death || Ghost))
                //{
                //    M2Share.HighDCHuman = 0;
                //}
                //if (M2Share.HighMCHuman == ActorId && (Death || Ghost))
                //{
                //    M2Share.HighMCHuman = 0;
                //}
                //if (M2Share.HighSCHuman == ActorId && (Death || Ghost))
                //{
                //    M2Share.HighSCHuman = 0;
                //}
                //if (M2Share.HighOnlineHuman == ActorId && (Death || Ghost))
                //{
                //    M2Share.HighOnlineHuman = 0;
                //}
                //if (Permission < 6)
                //{
                //    // 最高等级
                //    BaseObject targetObject = SystemShare.ActorMgr.Get(M2Share.HighLevelHuman);
                //    if (M2Share.HighLevelHuman == 0 || targetObject.Ghost)
                //    {
                //        M2Share.HighLevelHuman = ActorId;
                //    }
                //    else
                //    {
                //        if (Abil.Level > targetObject.Abil.Level)
                //        {
                //            M2Share.HighLevelHuman = ActorId;
                //        }
                //    }

                //    // 最高PK
                //    targetObject = SystemShare.ActorMgr.Get(M2Share.HighPKPointHuman);
                //    if (M2Share.HighPKPointHuman == 0 || targetObject.Ghost)
                //    {
                //        if (PkPoint > 0)
                //        {
                //            M2Share.HighPKPointHuman = ActorId;
                //        }
                //    }
                //    else
                //    {
                //        if (PkPoint > ((PlayObject)targetObject).PkPoint)
                //        {
                //            M2Share.HighPKPointHuman = ActorId;
                //        }
                //    }

                //    // 最高攻击力
                //    targetObject = SystemShare.ActorMgr.Get(M2Share.HighDCHuman);
                //    if (M2Share.HighDCHuman == 0 || targetObject.Ghost)
                //    {
                //        M2Share.HighDCHuman = ActorId;
                //    }
                //    else
                //    {
                //        if (HUtil32.HiWord(WAbil.DC) > HUtil32.HiWord(targetObject.WAbil.DC))
                //        {
                //            M2Share.HighDCHuman = ActorId;
                //        }
                //    }

                //    // 最高魔法
                //    targetObject = SystemShare.ActorMgr.Get(M2Share.HighMCHuman);
                //    if (M2Share.HighMCHuman == 0 || targetObject.Ghost)
                //    {
                //        M2Share.HighMCHuman = ActorId;
                //    }
                //    else
                //    {
                //        if (HUtil32.HiWord(WAbil.MC) > HUtil32.HiWord(targetObject.WAbil.MC))
                //        {
                //            M2Share.HighMCHuman = ActorId;
                //        }
                //    }

                //    // 最高道术
                //    targetObject = SystemShare.ActorMgr.Get(M2Share.HighSCHuman);
                //    if (M2Share.HighSCHuman == 0 || targetObject.Ghost)
                //    {
                //        M2Share.HighSCHuman = ActorId;
                //    }
                //    else
                //    {
                //        if (HUtil32.HiWord(WAbil.SC) > HUtil32.HiWord(targetObject.WAbil.SC))
                //        {
                //            M2Share.HighSCHuman = ActorId;
                //        }
                //    }

                //    // 最长在线时间
                //    targetObject = SystemShare.ActorMgr.Get(M2Share.HighOnlineHuman);
                //    if (M2Share.HighOnlineHuman == 0 || targetObject.Ghost)
                //    {
                //        M2Share.HighOnlineHuman = ActorId;
                //    }
                //    else
                //    {
                //        if (LogonTick < ((PlayObject)targetObject).LogonTick)
                //        {
                //            M2Share.HighOnlineHuman = ActorId;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                LogService.Error(ex);
            }
        }

        private void TimingSaveData()
        {
            if (ScheduledSaveData)
            {
                return;
            }
            LogService.Debug("定时保存角色数据");
            if (SystemShare.WorldEngine.PlayObjectCount > 0)
            {
                ScheduledSaveData = true;
                foreach (SystemModule.Actors.IPlayerActor play in SystemShare.WorldEngine.GetPlayObjects())
                {
                    if (M2Share.FrontEngine.InSaveRcdList(play.ChrName))
                    {
                        continue;
                    }
                    WorldServer.SaveHumanRcd(play);
                }
                ScheduledSaveData = false;
            }
            LogService.Debug("定时保存角色数据完毕.");
        }

        private void ProcessGameNotice()
        {
            if (SystemShare.Config.SendOnlineCount && (HUtil32.GetTickCount() - SendOnlineTick) > SystemShare.Config.SendOnlineTime)
            {
                SendOnlineTick = HUtil32.GetTickCount();
                string sMsg = string.Format(MessageSettings.SendOnlineCountMsg, HUtil32.Round(SystemShare.WorldEngine.OnlinePlayObject * (SystemShare.Config.SendOnlineCountRate / 10.0)));
                SystemShare.WorldEngine.SendBroadCastMsg(sMsg, MsgType.System);
            }
        }

        public void SaveItemNumber()
        {
            SystemShare.ServerConf.SaveVariable();
        }
    }
}