using System.Collections.Generic;
using System.Threading;
using SystemModule;

namespace GameSvr
{
    public class ServerBase
    {
        private readonly Thread _runServer;
        /// <summary>
        /// 运行时间
        /// </summary>
        private int _runTimeTick = 0;

        protected ServerBase()
        {
            _runServer = new Thread(Run) { IsBackground = true };
        }

        public void Start()
        {
            _runServer.Start();
            M2Share.UserEngine.Start();
            M2Share.DataServer.Start();
            M2Share.g_dwUsrRotCountTick = HUtil32.GetTickCount();
        }

        public void Stop()
        {
            M2Share.DataServer.Stop();
            M2Share.GateManager.Stop();
            M2Share.UserEngine.Stop();
            _runServer.Interrupt();
        }

        private void Run()
        {
            while (M2Share.boStartReady)
            {
                M2Share.GateManager.Run();
                IdSrvClient.Instance.Run();
                M2Share.UserEngine.Run();
                ProcessGameRun();
                if (M2Share.nServerIndex == 0)
                {
                    SnapsmService.Instance.Run();
                }
                else
                {
                    SnapsmClient.Instance.Run();
                }
                Thread.Sleep(10);
            }
        }

        private void ProcessGameNotice()
        {
            if (M2Share.g_Config.boSendOnlineCount && (HUtil32.GetTickCount() - M2Share.g_dwSendOnlineTick) > M2Share.g_Config.dwSendOnlineTime)
            {
                M2Share.g_dwSendOnlineTick = HUtil32.GetTickCount();
                var sMsg = M2Share.g_sSendOnlineCountMsg.Replace("%c", HUtil32.Round(M2Share.UserEngine.OnlinePlayObject * (M2Share.g_Config.nSendOnlineCountRate / 10)).ToString());
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
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                M2Share.EventManager.Run();
                M2Share.RobotManage.Run();
                if ((HUtil32.GetTickCount() - _runTimeTick) > 10000)
                {
                    _runTimeTick = HUtil32.GetTickCount();
                    M2Share.GuildManager.Run();
                    M2Share.CastleManager.Run();
                    var denyList = new List<string>(M2Share.g_DenySayMsgList.Count);
                    foreach (var item in M2Share.g_DenySayMsgList)
                    {
                        if (HUtil32.GetTickCount() > item.Value)
                        {
                            denyList.Add(item.Key);
                        }
                    }
                    for (var i = 0; i < denyList.Count; i++)
                    {
                        if (M2Share.g_DenySayMsgList.TryRemove(denyList[i], out var denyName))
                        {
                            M2Share.MainOutMessage($"解除玩家[{denyList[i]}]禁言");
                        }
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
        }
    }
}