using System.Collections.Generic;
using System.Threading;
using SystemModule;

namespace M2Server
{
    public class ServerBase
    {
        private readonly Thread _runServer;
        /// <summary>
        /// 运行时间
        /// </summary>
        private int _runTimeTick = 0;
        /// <summary>
        /// 游戏公告定时器
        /// </summary>
        private Timer _gamenoticeTimer = null;
        /// <summary>
        /// 全局变量定时器
        /// </summary>
        private Timer _saveVariableTimer = null;

        protected ServerBase()
        {
            _runServer = new Thread(Run) { IsBackground = true };
        }

        protected void Start()
        {
            _runServer.Start();
            M2Share.GateServer.Start();
            M2Share.UserEngine.Start();
            M2Share.DataServer.Start();
            _gamenoticeTimer = new Timer(ProcessGameNotice, null, 5000, 5000);
            _saveVariableTimer = new Timer(SaveItemNumber, null, 15000, 20000);
        }

        protected void Stop()
        {
            M2Share.UserEngine.Stop();
            M2Share.DataServer.Stop();
        }

        private void Run()
        {
            while (M2Share.boStartReady)
            {
                M2Share.RunSocket.Run();
                IdSrvClient.Instance.Run();
                M2Share.UserEngine.Run();
                ProcessGameRun();
                if (M2Share.nServerIndex == 0)
                {
                    InterServerMsg.Instance.Run();
                }
                else
                {
                    InterMsgClient.Instance.Run();
                }
                Thread.Sleep(50);
            }
        }

        private void ProcessGameNotice(object obj)
        {
            if (M2Share.g_Config.boSendOnlineCount && HUtil32.GetTickCount() - M2Share.g_dwSendOnlineTick >
                    M2Share.g_Config.dwSendOnlineTime)
            {
                M2Share.g_dwSendOnlineTick = HUtil32.GetTickCount();
                var sMsg = M2Share.g_sSendOnlineCountMsg.Replace("%c", HUtil32.Round(M2Share.UserEngine.OnlinePlayObject * (M2Share.g_Config.nSendOnlineCountRate / 10)).ToString());
                M2Share.UserEngine.SendBroadCastMsg(sMsg, TMsgType.t_System);
            }
        }

        private static void SaveItemNumber(object sender)
        {
            M2Share.ServerConf.SaveVariable();
        }

        private void ProcessGameRun()
        {
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                M2Share.EventManager.Run();
                M2Share.RobotManage.Run();
                if (HUtil32.GetTickCount() - _runTimeTick > 10000)
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
                        if (M2Share.g_DenySayMsgList.TryRemove(denyList[i],out var denyName))
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