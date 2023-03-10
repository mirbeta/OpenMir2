using GameSrv.Planes;
using GameSrv.Services;
using NLog;
using SystemModule.Enums;

namespace GameSrv
{
    public class ServerBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        protected ServerBase()
        {
            
        }

        public void StartWorld(CancellationToken stoppingToken)
        {
            M2Share.SystemProcess.StartAsync();
            //await M2Share.UserProcessor.StartAsync();
            //await M2Share.RobotProcessor.StartAsync();
            //await M2Share.MerchantProcessor.StartAsync();
            M2Share.EventMgr.Start();
            M2Share.RobotMgr.Start();
            M2Share.DataServer.Start();
            M2Share.GateMgr.Start(stoppingToken);
            M2Share.UsrRotCountTick = HUtil32.GetTickCount();
            _logger.Info("启动游戏世界和环境服务线程...");
        }

        public static void Stop()
        {
            M2Share.DataServer.Stop();
            M2Share.GateMgr.Stop();
            M2Share.EventMgr.Stop();
            M2Share.RobotMgr.Stop();
            M2Share.SystemProcess.CloseAsync();
            M2Share.UserProcessor.CloseAsync();
            M2Share.RobotProcessor.CloseAsync();
            M2Share.MerchantProcessor.CloseAsync();
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