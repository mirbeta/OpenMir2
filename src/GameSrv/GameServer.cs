using GameSrv.Planes;
using GameSrv.Services;
using NLog;
using SystemModule.Enums;

namespace GameSrv
{
    public class ServerBase
    {
        /// <summary>
        /// 运行时间
        /// </summary>
        private int _runTimeTick;
        private readonly Thread _worldThread;
        private readonly Thread _worldDataThread;
        private readonly Thread _worldBotThread;
        private readonly Thread _worldMerchantThread;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected ServerBase()
        {
            _runTimeTick = HUtil32.GetTickCount();
            _worldThread = new Thread(Execute) { IsBackground = true };
            _worldDataThread = new Thread(DataExecute) { IsBackground = true };
            _worldBotThread = new Thread(RobotExecute) { IsBackground = true };
            _worldMerchantThread = new Thread(MerchantExecute) { IsBackground = true };
        }

        public void StartWorld(CancellationToken stoppingToken)
        {
            M2Share.DataServer.Start();
            M2Share.UsrRotCountTick = HUtil32.GetTickCount();
            M2Share.GateMgr.Start(stoppingToken);
            _worldThread.Start();
            _worldDataThread.Start();
            _worldBotThread.Start();
            _worldMerchantThread.Start();
            M2Share.EventMgr.Start();
            M2Share.RobotMgr.Start();
            _logger.Info("启动游戏世界和环境服务线程...");
        }

        public void Stop()
        {
            M2Share.DataServer.Stop();
            M2Share.GateMgr.Stop();
            M2Share.EventMgr.Stop();
            M2Share.RobotMgr.Stop();
            _worldThread.Interrupt();
            _worldDataThread.Interrupt();
            _worldBotThread.Interrupt();
            _worldMerchantThread.Interrupt();
        }

        private void Execute()
        {
            while (M2Share.StartReady)
            {
                IdSrvClient.Instance.Run();
                M2Share.WorldEngine.PrcocessData();
                ProcessGameRun();
                if (M2Share.ServerIndex == 0)
                {
                    PlanesServer.Instance.Run();
                }
                else
                {
                    PlanesClient.Instance.Run();
                }
                Thread.SpinWait(20);//SpinWait 无法使你准确控制等待时间，主要是使用一些锁时用到，例如 Monitor.Enter。
            }
        }

        private void ProcessGameRun()
        {
            HUtil32.EnterCriticalSections(M2Share.ProcessHumanCriticalSection);
            try
            {
                if ((HUtil32.GetTickCount() - _runTimeTick) > 10000)
                {
                    _runTimeTick = HUtil32.GetTickCount();
                    M2Share.WorldEngine.Run();
                    M2Share.GuildMgr.Run();
                    M2Share.CastleMgr.Run();
                    M2Share.GateMgr.Run();
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
                            if (M2Share.DenySayMsgList.TryRemove(denyList[i], out long denyName))
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

        private void DataExecute()
        {
            while (M2Share.StartReady)
            {
                M2Share.WorldEngine.ProcessHumans();
                Thread.SpinWait(20);
            }
        }

        private void MerchantExecute()
        {
            while (M2Share.StartReady)
            {
                M2Share.WorldEngine.ProcessNpcs();
                M2Share.WorldEngine.ProcessMerchants();
                Thread.SpinWait(20);
            }
        }

        private void RobotExecute()
        {
            while (M2Share.StartReady)
            {
                M2Share.WorldEngine.ProcessRobotPlayData();
                Thread.SpinWait(20);
            }
        }

        private static void ProcessGameNotice()
        {
            if (M2Share.Config.SendOnlineCount && (HUtil32.GetTickCount() - M2Share.SendOnlineTick) > M2Share.Config.SendOnlineTime)
            {
                M2Share.SendOnlineTick = HUtil32.GetTickCount();
                string sMsg = string.Format(Settings.SendOnlineCountMsg, HUtil32.Round(M2Share.WorldEngine.OnlinePlayObject * (M2Share.Config.SendOnlineCountRate / 10)));
                M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.System);
            }
        }

        public static void SaveItemNumber()
        {
            ProcessGameNotice();
            M2Share.ServerConf.SaveVariable();
        }
    }
}