using Microsoft.Extensions.Hosting;
using SystemModule;

namespace GameSvr
{
    public class TimedService : BackgroundService
    {
        private readonly GameApp _mirApp;
        private int _checkIntervalTime;
        private int _saveIntervalTime;
        private int _clearIntervalTime;

        public TimedService(GameApp mirApp)
        {
            _mirApp = mirApp;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _checkIntervalTime = HUtil32.GetTickCount();
            _saveIntervalTime = HUtil32.GetTickCount();
            _clearIntervalTime = HUtil32.GetTickCount();
            while (!stoppingToken.IsCancellationRequested)
            {
                ServiceTimer();
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        private void ServiceTimer()
        {
            if ((HUtil32.GetTickCount() - _checkIntervalTime) > 3000) //3s一次检查链接
            {
                M2Share.DataServer.CheckConnected();
                IdSrvClient.Instance.CheckConnected();
                SnapsmClient.Instance.CheckConnected();
                _checkIntervalTime = HUtil32.GetTickCount();
            }
            if ((HUtil32.GetTickCount() - _saveIntervalTime) > 50000) //保存游戏变量等
            {
                _mirApp.SaveItemNumber();
                _saveIntervalTime = HUtil32.GetTickCount();
            }
            if ((HUtil32.GetTickCount() - _clearIntervalTime) > 60000)
            {
                M2Share.ObjectManager.ClearObject();
                _clearIntervalTime = HUtil32.GetTickCount();
            }
        }
    }
}