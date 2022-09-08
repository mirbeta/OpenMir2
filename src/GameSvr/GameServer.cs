using GameSvr.Services;
using GameSvr.Snaps;
using Microsoft.Extensions.Logging;
using SystemModule;
using SystemModule.Data;

namespace GameSvr
{
    public class ServerBase
    {
        /// <summary>
        /// 运行时间
        /// </summary>
        private int _runTimeTick;
        protected readonly ILogger<ServerBase> _logger;

        protected ServerBase(ILogger<ServerBase> logger)
        {
            _logger = logger;
            _runTimeTick = HUtil32.GetTickCount();
        }

        public void StartService()
        {
            M2Share.DataServer.Start();
            M2Share.g_dwUsrRotCountTick = HUtil32.GetTickCount();
        }

        public void Start(CancellationToken stoppingToken)
        {
            Task.Factory.StartNew(async () =>
            {
                Execute();
                await Task.Delay(20);
            }, stoppingToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void Stop()
        {
            M2Share.DataServer.Stop();
            M2Share.GateMgr.Stop();
        }

        private void Execute()
        {
            M2Share.GateMgr.Run();
            IdSrvClient.Instance.Run();
            M2Share.UserEngine.Run();
            ProcessGameRun();
            if (M2Share.ServerIndex == 0)
            {
                SnapsmService.Instance.Run();
            }
            else
            {
                SnapsmClient.Instance.Run();
            }
        }

        private void ProcessGameNotice()
        {
            if (M2Share.Config.SendOnlineCount && (HUtil32.GetTickCount() - M2Share.g_dwSendOnlineTick) > M2Share.Config.SendOnlineTime)
            {
                M2Share.g_dwSendOnlineTick = HUtil32.GetTickCount();
                var sMsg = string.Format(M2Share.g_sSendOnlineCountMsg, HUtil32.Round(M2Share.UserEngine.OnlinePlayObject * (M2Share.Config.SendOnlineCountRate / 10)));
                M2Share.UserEngine.SendBroadCastMsg(sMsg, MsgType.System);
            }
        }

        public void SaveItemNumber()
        {
            ProcessGameNotice();
            M2Share.ServerConf.SaveVariable();
        }

        private void ProcessGameRun()
        {
            HUtil32.EnterCriticalSections(M2Share.ProcessHumanCriticalSection);
            try
            {
                M2Share.EventMgr.Run();
                M2Share.RobotMgr.Run();
                if ((HUtil32.GetTickCount() - _runTimeTick) > 10000)
                {
                    _runTimeTick = HUtil32.GetTickCount();
                    M2Share.GuildMgr.Run();
                    M2Share.CastleMgr.Run();
                    if (M2Share.DenySayMsgList.Count > 0)
                    {
                        var denyList = new List<string>(M2Share.DenySayMsgList.Count);
                        foreach (var item in M2Share.DenySayMsgList)
                        {
                            if (HUtil32.GetTickCount() > item.Value)
                            {
                                denyList.Add(item.Key);
                            }
                        }
                        for (var i = 0; i < denyList.Count; i++)
                        {
                            if (M2Share.DenySayMsgList.TryRemove(denyList[i], out var denyName))
                            {
                                _logger.LogDebug($"解除玩家[{denyList[i]}]禁言");
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