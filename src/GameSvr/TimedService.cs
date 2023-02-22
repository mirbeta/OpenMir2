using GameSvr.Planes;
using GameSvr.Services;
using Microsoft.Extensions.Hosting;
using NLog;

namespace GameSvr
{
    public class TimedService : BackgroundService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private PeriodicTimer _timer;
        private int _checkIntervalTime;
        private int _saveIntervalTime;
        private int _clearIntervalTime;
        private int _scheduledSaveIntervalTime;
        /// <summary>
        /// 是否正在保存数据
        /// </summary>
        private bool _scheduledSaveData;

        public TimedService()
        {
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            int currentTick = HUtil32.GetTickCount();
            _checkIntervalTime = currentTick;
            _saveIntervalTime = currentTick;
            _clearIntervalTime = currentTick;
            _scheduledSaveIntervalTime = currentTick;
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    ServerRunTimer();
                }
            }
            catch (OperationCanceledException e)
            {
                _logger.Debug("TimedService is stopping.");
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Info("后台服务停止");
            _timer.Dispose();
            return base.StopAsync(cancellationToken);
        }

        private void ServerRunTimer()
        {
            if (M2Share.StartReady)
            {
                int currentTick = HUtil32.GetTickCount();
                if ((currentTick - _checkIntervalTime) > 10 * 1000) //10s一次检查链接
                {
                    M2Share.DataServer.CheckConnected();
                    IdSrvClient.Instance.CheckConnected();
                    PlanesClient.Instance.CheckConnected();
                    _checkIntervalTime = HUtil32.GetTickCount();
                }
                if ((currentTick - _saveIntervalTime) > 50 * 1000) //保存游戏变量等
                {
                    ServerBase.SaveItemNumber();
                    _saveIntervalTime = HUtil32.GetTickCount();
                }
                if ((currentTick - _clearIntervalTime) > 600000) //定时清理游戏对象
                {
                    M2Share.ActorMgr.ClearObject();
                    _clearIntervalTime = HUtil32.GetTickCount();
                }
                if (currentTick - _scheduledSaveIntervalTime > 60 * 10000) //定时保存玩家数据
                {
                    TimingSaveData();
                    _scheduledSaveIntervalTime = HUtil32.GetTickCount();
                }
            }
        }

        private void TimingSaveData()
        {
            if (_scheduledSaveData)
            {
                return;
            }
            _logger.Debug("定时保存角色数据");
            if (M2Share.WorldEngine.PlayObjectCount > 0)
            {
                _scheduledSaveData = true;
                foreach (Player.PlayObject play in M2Share.WorldEngine.PlayObjects)
                {
                    if (M2Share.FrontEngine.InSaveRcdList(play.ChrName))
                    {
                        continue;
                    }
                    M2Share.WorldEngine.SaveHumanRcd(play);
                }
                _scheduledSaveData = false;
            }
            _logger.Debug("定时保存角色数据完毕.");
        }
    }
}