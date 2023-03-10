using GameSrv.Planes;
using GameSrv.Services;

namespace GameSrv.World.Threads
{
    public class SystemProcessor : TimerBase
    {
        private int runTimeTick;
        
        public SystemProcessor() : base(200, "SystemThread")
        {
            runTimeTick = HUtil32.GetTickCount();
        }

        protected override bool OnElapseAsync()
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
            return true;
        }

        private void ProcessGameRun()
        {
            HUtil32.EnterCriticalSections(M2Share.ProcessHumanCriticalSection);
            try
            {
                if ((HUtil32.GetTickCount() - runTimeTick) > 10000)
                {
                    runTimeTick = HUtil32.GetTickCount();
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
                            if (M2Share.DenySayMsgList.TryRemove(denyList[i], out long _))
                            {
                                M2Share.Logger.Debug($"������[{denyList[i]}]����");
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