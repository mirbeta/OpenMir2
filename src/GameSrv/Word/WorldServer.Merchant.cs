using M2Server;
using M2Server.Actor;

namespace GameSrv.Word
{
    public partial class WorldServer
    {
        private void MerchantInitialize()
        {
            for (var i = MerchantList.Count - 1; i >= 0; i--)
            {
                var merchant = MerchantList[i];
                merchant.Envir = GameShare.MapMgr.FindMap(merchant.MapName);
                if (merchant.Envir != null)
                {
                    merchant.OnEnvirnomentChanged();
                    merchant.Initialize();
                    if (merchant.AddtoMapSuccess && !merchant.IsHide)
                    {
                        _logger.Warn("Merchant Initalize fail..." + merchant.ChrName + ' ' + merchant.MapName + '(' + merchant.CurrX + ':' + merchant.CurrY + ')');
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
                    _logger.Error(merchant.ChrName + " - Merchant Initalize fail... (m.PEnvir=nil)");
                    MerchantList.RemoveAt(i);
                }
            }
        }

        private void NpCinitialize()
        {
            for (var i = QuestNpcList.Count - 1; i >= 0; i--)
            {
                var normNpc = QuestNpcList[i];
                normNpc.Envir = GameShare.MapMgr.FindMap(normNpc.MapName);
                if (normNpc.Envir != null)
                {
                    normNpc.OnEnvirnomentChanged();
                    normNpc.Initialize();
                    if (normNpc.AddtoMapSuccess && !normNpc.IsHide)
                    {
                        _logger.Warn(normNpc.ChrName + " Npc Initalize fail... ");
                        QuestNpcList.RemoveAt(i);
                    }
                    else
                    {
                        normNpc.LoadNPCScript();
                    }
                }
                else
                {
                    _logger.Error(normNpc.ChrName + " Npc Initalize fail... (npc.PEnvir=nil) ");
                    QuestNpcList.RemoveAt(i);
                }
            }
        }

        public void ProcessMerchants()
        {
            var boProcessLimit = false;
            const string sExceptionMsg = "[Exception] WorldServer::ProcessMerchants";
            var dwRunTick = HUtil32.GetTickCount();
            try
            {
                var dwCurrTick = HUtil32.GetTickCount();
                for (var i = MerchantPosition; i < MerchantList.Count; i++)
                {
                    var merchantNpc = MerchantList[i];
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
                _logger.Error(sExceptionMsg);
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
            var dwRunTick = HUtil32.GetTickCount();
            var boProcessLimit = false;
            try
            {
                for (var i = NpcPosition; i < QuestNpcList.Count; i++)
                {
                    var normNpc = QuestNpcList[i];
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
                if (!boProcessLimit) NpcPosition = 0;
            }
            catch
            {
                _logger.Error("[Exceptioin] WorldServer.ProcessNpcs");
            }
            ProcessNpcTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (ProcessNpcTimeMin > ProcessNpcTimeMax) ProcessNpcTimeMax = ProcessNpcTimeMin;
        }

        public int GetMerchantList(Envirnoment envir, int nX, int nY, int nRange, IList<IMerchant> tmpList)
        {
            for (var i = 0; i < MerchantList.Count; i++)
            {
                var merchant = MerchantList[i];
                if (merchant.Envir == envir && Math.Abs(merchant.CurrX - nX) <= nRange &&
                    Math.Abs(merchant.CurrY - nY) <= nRange) tmpList.Add(merchant);
            }
            return tmpList.Count;
        }

        public int GetNpcList(Envirnoment envir, int nX, int nY, int nRange, IList<BaseObject> tmpList)
        {
            for (var i = 0; i < QuestNpcList.Count; i++)
            {
                var npc = QuestNpcList[i];
                if (npc.Envir == envir && Math.Abs(npc.CurrX - nX) <= nRange &&
                    Math.Abs(npc.CurrY - nY) <= nRange) tmpList.Add(npc);
            }
            return tmpList.Count;
        }

        public void ReloadMerchantList()
        {
            for (var i = 0; i < MerchantList.Count; i++)
            {
                var merchant = MerchantList[i];
                if (!merchant.Ghost)
                {
                    merchant.ClearScript();
                    merchant.LoadMerchantScript();
                }
            }
        }

        public void ReloadNpcList()
        {
            for (var i = 0; i < QuestNpcList.Count; i++)
            {
                var npc = QuestNpcList[i];
                npc.ClearScript();
                npc.LoadNPCScript();
            }
        }

        private void ClearMerchantData()
        {
            for (var i = 0; i < MerchantList.Count; i++)
            {
                MerchantList[i].ClearData();
            }
        }
    }
}