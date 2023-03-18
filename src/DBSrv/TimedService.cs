using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DBSrv.Services;
using DBSrv.Storage;
using Microsoft.Extensions.Hosting;
using NLog;
using SystemModule;
using SystemModule.Packets.ServerPackets;

namespace DBSrv
{
    public class TimedService : BackgroundService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int _lastSocketTick;
        private int _lastKeepTick;
        private int _lastClearTick;
        private int _syncSaveTick;
        private readonly PeriodicTimer _timer;
        private readonly UserService _userSoc;
        private readonly LoginSessionService _loginSoc;
        private readonly PlayerDataService _dataService;
        private readonly ICacheStorage _cacheStorage;
        private readonly IPlayDataStorage _playDataStorage;
        
        public TimedService( UserService userSoc, LoginSessionService loginSoc, PlayerDataService dataService, ICacheStorage cacheStorage, IPlayDataStorage playDataStorage)
        {
            _userSoc = userSoc;
            _loginSoc = loginSoc;
            _dataService = dataService;
            _cacheStorage = cacheStorage;
            _playDataStorage = playDataStorage;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var currentTick = HUtil32.GetTickCount();
            _lastSocketTick = currentTick;
            _lastKeepTick = currentTick;
            _lastClearTick = currentTick;
            _syncSaveTick = currentTick;
            try
            {
                while (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    currentTick = HUtil32.GetTickCount();
                    if (currentTick - _lastKeepTick > 7000)
                    {
                        _lastKeepTick = HUtil32.GetTickCount();
                        _loginSoc.SendKeepAlivePacket(_userSoc.GetUserCount());
                    }
                    if (currentTick - _lastSocketTick > 10000)
                    {
                        _lastSocketTick = HUtil32.GetTickCount();
                        _loginSoc.CheckConnection();
                    }
                    if (currentTick - _lastClearTick > 10000)
                    {
                        _lastClearTick = HUtil32.GetTickCount();
                        _dataService.ClearTimeoutSession();
                    }
                    if (currentTick - _syncSaveTick > 300000)//5分钟刷新一次缓存数据到数据库
                    {
                        _syncSaveTick = HUtil32.GetTickCount();
                        ProcessCacheStorage();
                    }
                }
            }
            catch (OperationCanceledException operationCancelledException)
            {
                _logger.Warn(operationCancelledException, "service stopped");
            }
        }

        private void ProcessCacheStorage()
        {
            //从内存获取保存数据，刷新到数据库，减少数据库压力，和防止大量数据保存超时
            _logger.Info("同步玩家缓存数据.");
            using IEnumerator<PlayerDataInfo> playList = _cacheStorage.QueryCacheData();
            while (playList.MoveNext())
            {
                var play = playList.Current;
                if (play == null)
                {
                    continue;
                }
                if (_playDataStorage.Update(play.Header.Name, play))
                {
                    _logger.Debug($"{play.Header.Name}同步成功.");
                }
                else
                {
                    _logger.Debug($"{play.Header.Name}同步失败.");
                }
                _cacheStorage.Delete(play.Header.Name);//处理完从缓存删除
            }
            _logger.Info("同步玩家缓存数据完成.");
        }
    }
}