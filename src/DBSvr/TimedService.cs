using DBSvr.Services;
using DBSvr.Storage;
using DBSvr.Storage.Impl;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace DBSvr
{
    public class TimedService : BackgroundService
    {
        private readonly MirLog _logger;
        private readonly UserSocService _userSoc;
        private readonly LoginSvrService _loginSoc;
        private readonly HumDataService _dataService;
        private int _lastSocketTick;
        private int _lastKeepTick;
        private int _lastClearTick;
        private int _makeDBTick;
        private readonly IMemoryStorageServive _memoryStorageServive;
        private readonly IPlayDataStorage _playDataStorage;

        public TimedService(MirLog logger, UserSocService userSoc, LoginSvrService loginSoc, HumDataService dataService,
            IMemoryStorageServive memoryStorageServive, IPlayDataStorage playDataStorage)
        {
            _logger = logger;
            _userSoc = userSoc;
            _loginSoc = loginSoc;
            _dataService = dataService;
            _memoryStorageServive = memoryStorageServive;
            _playDataStorage = playDataStorage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _lastSocketTick = HUtil32.GetTickCount();
            _lastKeepTick = HUtil32.GetTickCount();
            _lastClearTick = HUtil32.GetTickCount();
            _makeDBTick = HUtil32.GetTickCount();
            while (!stoppingToken.IsCancellationRequested)
            {
                if (HUtil32.GetTickCount() - _lastKeepTick > 7000)
                {
                    _lastKeepTick = HUtil32.GetTickCount();
                    var userCount = _userSoc.GetUserCount();
                    _loginSoc.SendKeepAlivePacket(userCount);
                }
                if (HUtil32.GetTickCount() - _lastSocketTick > 10000)
                {
                    _lastSocketTick = HUtil32.GetTickCount();
                    _loginSoc.CheckConnection();
                }
                if (HUtil32.GetTickCount() - _lastClearTick > 10000)
                {
                    _lastClearTick = HUtil32.GetTickCount();
                    _dataService.ClearTimeoutSession();
                }
                if (HUtil32.GetTickCount() - _makeDBTick > 300000) //5分钟刷新一次缓存数据到数据库
                {
                    ProcessMemeryStorage();
                    _makeDBTick = HUtil32.GetTickCount();
                }
                await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken);
            }
        }

        private void ProcessMemeryStorage()
        {
            //从内存获取保存数据，刷新到数据库，减少数据库压力，和防止大量数据保存超时
            _logger.LogInformation("开始刷新缓存数据.");
            var playList = _memoryStorageServive.GetAll();
            if (playList.Any())
            {
                _logger.LogInformation("角色缓存数据.");
                foreach (var play in playList)
                {
                    if (_playDataStorage.Update(play.Header.sName, play))
                    {

                    }
                    _memoryStorageServive.Delete(play.Header.sName);//处理完从缓存删除
                }
            }
            _logger.LogInformation("缓存数据刷新完成.");
        }
    }
}