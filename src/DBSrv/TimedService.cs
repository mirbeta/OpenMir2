using DBSrv.Conf;
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
        private readonly PeriodicTimer _timer;
        private readonly SettingConf _config;
        private readonly UserService _userService;
        private readonly SessionService _sessionService;
        private readonly DataService _dataService;
        private readonly MarketService _marketService;
        private readonly ICacheStorage _cacheStorage;
        private readonly IPlayDataStorage _playDataStorage;

        public TimedService(SettingConf config, ICacheStorage cacheStorage, UserService userService, SessionService session, DataService dataService, IPlayDataStorage playDataStorage, MarketService marketService)
        {
            _config = config;
            _userService = userService;
            _sessionService = session;
            _dataService = dataService;
            _cacheStorage = cacheStorage;
            _playDataStorage = playDataStorage;
            _marketService = marketService;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var currentTick = HUtil32.GetTickCount();
            var lastSocketTick = currentTick;
            var lastKeepTick = currentTick;
            var lastClearTick = currentTick;
            var syncSaveTick = currentTick;
            var marketPushTick = currentTick;
            try
            {
                while (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    currentTick = HUtil32.GetTickCount();
                    if (currentTick - lastSocketTick > 10000)
                    {
                        lastSocketTick = HUtil32.GetTickCount();
                        _sessionService.CheckConnection();
                    }
                    if (currentTick - lastKeepTick > 7000)
                    {
                        lastKeepTick = HUtil32.GetTickCount();
                        _sessionService.SendKeepAlivePacket(_userService.GetUserCount());
                    }
                    if (currentTick - lastClearTick > 10000)
                    {
                        lastClearTick = HUtil32.GetTickCount();
                        _dataService.ClearTimeoutSession();
                    }
                    if (currentTick - syncSaveTick > 300000) //5分钟刷新一次缓存数据到数据库
                    {
                        syncSaveTick = HUtil32.GetTickCount();
                        ProcessCacheStorage();
                    }
                    if (currentTick - marketPushTick > _config.SyncMarketInterval) //自定义时间推送一次拍卖行数据到各个GameSrv
                    {
                        marketPushTick = HUtil32.GetTickCount();
                        _marketService.PushMarketData();
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