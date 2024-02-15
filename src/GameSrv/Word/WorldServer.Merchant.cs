using SystemModule.Actors;
using SystemModule.Maps;

namespace GameSrv.Word
{
    public partial class WorldServer
    {
        private void MerchantInitialize()
        {
            for (int i = MerchantList.Count - 1; i >= 0; i--)
            {
                IMerchant merchant = MerchantList[i];
                merchant.Envir = SystemShare.MapMgr.FindMap(merchant.MapName);
                if (merchant.Envir != null)
                {
                    merchant.OnEnvirnomentChanged();
                    merchant.Initialize();
                    if (merchant.AddtoMapSuccess && !merchant.IsHide)
                    {
                        LogService.Warn("Merchant Initalize fail..." + merchant.ChrName + ' ' + merchant.MapName + '(' + merchant.CurrX + ':' + merchant.CurrY + ')');
                        MerchantList.RemoveAt(i);
                    }
                    else
                    {
                        merchant.LoadMerchantScript();
                        merchant.LoadNpcData();
                    }
                }
                else
                {
                    LogService.Error(merchant.ChrName + " - Merchant Initalize fail... (m.PEnvir=nil)");
                    MerchantList.RemoveAt(i);
                }
            }
        }

        private void NpCinitialize()
        {
            for (int i = QuestNpcList.Count - 1; i >= 0; i--)
            {
                INormNpc normNpc = QuestNpcList[i];
                normNpc.Envir = SystemShare.MapMgr.FindMap(normNpc.MapName);
                if (normNpc.Envir != null)
                {
                    normNpc.OnEnvirnomentChanged();
                    normNpc.Initialize();
                    if (normNpc.AddtoMapSuccess && !normNpc.IsHide)
                    {
                        LogService.Warn(normNpc.ChrName + " Npc Initalize fail... ");
                        QuestNpcList.RemoveAt(i);
                    }
                    else
                    {
                        normNpc.LoadNpcScript();
                    }
                }
                else
                {
                    LogService.Error(normNpc.ChrName + " Npc Initalize fail... (npc.PEnvir=nil) ");
                    QuestNpcList.RemoveAt(i);
                }
            }
        }

        public void ProcessMerchants()
        {
            bool boProcessLimit = false;
            const string sExceptionMsg = "[Exception] WorldServer::ProcessMerchants";
            int dwRunTick = HUtil32.GetTickCount();
            try
            {
                int dwCurrTick = HUtil32.GetTickCount();
                for (int i = MerchantPosition; i < MerchantList.Count; i++)
                {
                    IMerchant merchantNpc = MerchantList[i];
                    if (!merchantNpc.Ghost)
                    {
                        if ((dwCurrTick - merchantNpc.RunTick) > merchantNpc.RunTime)
                        {
                            merchantNpc.RunTick = dwCurrTick;
                            merchantNpc.Run();
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - merchantNpc.GhostTick) > 60 * 1000)
                        {
                            merchantNpc = null;
                            MerchantList.RemoveAt(i);
                            break;
                        }
                    }
                    if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.NpcLimit)
                    {
                        MerchantPosition = i;
                        boProcessLimit = true;
                        break;
                    }
                }
                if (!boProcessLimit)
                {
                    MerchantPosition = 0;
                }
            }
            catch
            {
                LogService.Error(sExceptionMsg);
            }
            ProcessMerchantTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (ProcessMerchantTimeMin > ProcessMerchantTimeMax)
            {
                ProcessMerchantTimeMax = ProcessMerchantTimeMin;
            }
            if (ProcessNpcTimeMin > ProcessNpcTimeMax)
            {
                ProcessNpcTimeMax = ProcessNpcTimeMin;
            }
        }

        public void ProcessNpcs()
        {
            int dwRunTick = HUtil32.GetTickCount();
            bool boProcessLimit = false;
            try
            {
                for (int i = NpcPosition; i < QuestNpcList.Count; i++)
                {
                    INormNpc normNpc = QuestNpcList[i];
                    if (!normNpc.Ghost)
                    {
                        if ((HUtil32.GetTickCount() - normNpc.RunTick) > normNpc.RunTime)
                        {
                            normNpc.RunTick = HUtil32.GetTickCount();
                            normNpc.Run();
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - normNpc.GhostTick) > 60 * 1000)
                        {
                            QuestNpcList.RemoveAt(i);
                            break;
                        }
                    }
                    if ((HUtil32.GetTickCount() - dwRunTick) > M2Share.NpcLimit)
                    {
                        NpcPosition = i;
                        boProcessLimit = true;
                        break;
                    }
                }
                if (!boProcessLimit)
                {
                    NpcPosition = 0;
                }
            }
            catch
            {
                LogService.Error("[Exceptioin] WorldServer.ProcessNpcs");
            }
            ProcessNpcTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (ProcessNpcTimeMin > ProcessNpcTimeMax)
            {
                ProcessNpcTimeMax = ProcessNpcTimeMin;
            }
        }

        public int GetMerchantList(IEnvirnoment envir, int nX, int nY, int nRange, IList<IMerchant> tmpList)
        {
            for (int i = 0; i < MerchantList.Count; i++)
            {
                IMerchant merchant = MerchantList[i];
                if (merchant.Envir == envir && Math.Abs(merchant.CurrX - nX) <= nRange &&
                    Math.Abs(merchant.CurrY - nY) <= nRange)
                {
                    tmpList.Add(merchant);
                }
            }
            return tmpList.Count;
        }

        public int GetNpcList(IEnvirnoment envir, int nX, int nY, int nRange, IList<INormNpc> tmpList)
        {
            for (int i = 0; i < QuestNpcList.Count; i++)
            {
                INormNpc npc = QuestNpcList[i];
                if (npc.Envir == envir && Math.Abs(npc.CurrX - nX) <= nRange &&
                    Math.Abs(npc.CurrY - nY) <= nRange)
                {
                    tmpList.Add(npc);
                }
            }
            return tmpList.Count;
        }

        public void ReloadMerchantList()
        {
            for (int i = 0; i < MerchantList.Count; i++)
            {
                IMerchant merchant = MerchantList[i];
                if (!merchant.Ghost)
                {
                    merchant.ClearScript();
                    merchant.LoadMerchantScript();
                }
            }
        }

        public void ReloadNpcList()
        {
            for (int i = 0; i < QuestNpcList.Count; i++)
            {
                INormNpc npc = QuestNpcList[i];
                npc.ClearScript();
                npc.LoadNpcScript();
            }
        }

        private void ClearMerchantData()
        {
            for (int i = 0; i < MerchantList.Count; i++)
            {
                MerchantList[i].ClearData();
            }
        }
    }
}