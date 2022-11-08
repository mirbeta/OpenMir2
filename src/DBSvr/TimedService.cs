using DBSvr.Services;
using DBSvr.Storage;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Logger;
using SystemModule.Packet.ServerPackets;

namespace DBSvr
{
    public class TimedService : BackgroundService
    {
        private int _lastSocketTick;
        private int _lastKeepTick;
        private int _lastClearTick;
        private int _syncSaveTick;
        private readonly MirLogger _logger;
        private readonly UserService _userSoc;
        private readonly LoginService _loginSoc;
        private readonly HumDataService _dataService;
        private readonly ICacheStorage _cacheStorage;
        private readonly IPlayDataStorage _playDataStorage;

        public TimedService(MirLogger logger, UserService userSoc, LoginService loginSoc, HumDataService dataService, ICacheStorage cacheStorage, IPlayDataStorage playDataStorage)
        {
            _logger = logger;
            _userSoc = userSoc;
            _loginSoc = loginSoc;
            _dataService = dataService;
            _cacheStorage = cacheStorage;
            _playDataStorage = playDataStorage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var currentTick = HUtil32.GetTickCount();
            _lastSocketTick = currentTick;
            _lastKeepTick = currentTick;
            _lastClearTick = currentTick;
            _syncSaveTick = currentTick;
            while (!stoppingToken.IsCancellationRequested)
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
                await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken);
            }
        }

        private void ProcessCacheStorage()
        {
            //从内存获取保存数据，刷新到数据库，减少数据库压力，和防止大量数据保存超时
            _logger.LogInformation("同步缓存数据.");
            IList<HumDataInfo> playList = _cacheStorage.QueryCacheData();
            if (playList.Any())
            {
                _logger.LogInformation($"同步缓存数据[{playList.Count()}].");
                foreach (var play in playList)
                {
                    if (_playDataStorage.Update(play.Header.sName, play))
                    {
                        _logger.DebugLog($"{play.Header.sName}同步成功.");
                    }
                    else
                    {
                        _logger.DebugLog($"{play.Header.sName}同步失败.");
                    }
                    _cacheStorage.Delete(play.Header.sName);//处理完从缓存删除
                }
            }
            _logger.LogInformation("同步缓存数据完成.");
        }
    }
}