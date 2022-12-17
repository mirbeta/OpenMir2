using GameSvr.Planes;
using GameSvr.Services;
using NLog;
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
        private Thread _worldThread;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected ServerBase()
        {
            _runTimeTick = HUtil32.GetTickCount();
        }

        public void StartWorld(CancellationToken stoppingToken)
        {
            M2Share.DataServer.Start();
            M2Share.g_dwUsrRotCountTick = HUtil32.GetTickCount();
            M2Share.GateMgr.Start(stoppingToken);
            _worldThread = new Thread(Execute);
            _worldThread.IsBackground = true;
            _worldThread.Start();
        }

        public void Stop()
        {
            M2Share.DataServer.Stop();
            M2Share.GateMgr.Stop();
        }

        private void Execute()
        {
            while (M2Share.StartReady)
            {
                IdSrvClient.Instance.Run();
                M2Share.WorldEngine.Run();
                ProcessGameRun();
                if (M2Share.ServerIndex == 0)
                {
                    PlanesServer.Instance.Run();
                }
                else
                {
                    PlanesClient.Instance.Run();
                }
                Thread.Sleep(20);
            }
        }

        private void ProcessGameNotice()
        {
            if (M2Share.Config.SendOnlineCount && (HUtil32.GetTickCount() - M2Share.g_dwSendOnlineTick) > M2Share.Config.SendOnlineTime)
            {
                M2Share.g_dwSendOnlineTick = HUtil32.GetTickCount();
                var sMsg = string.Format(M2Share.g_sSendOnlineCountMsg, HUtil32.Round(M2Share.WorldEngine.OnlinePlayObject * (M2Share.Config.SendOnlineCountRate / 10)));
                M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.System);
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
                M2Share.WorldEngine.Execute();
                M2Share.RobotMgr.Run();
                M2Share.EventMgr.Run();
                if ((HUtil32.GetTickCount() - _runTimeTick) > 10000)
                {
                    _runTimeTick = HUtil32.GetTickCount();
                    M2Share.GuildMgr.Run();
                    M2Share.CastleMgr.Run();
                    M2Share.GateMgr.Run();
                    if (!M2Share.DenySayMsgList.IsEmpty)
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
                                _logger.Debug($"解除玩家[{denyList[i]}]禁言");
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