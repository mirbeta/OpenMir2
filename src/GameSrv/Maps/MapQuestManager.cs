using M2Server.Player;

namespace GameSrv.Maps
{
    public class MapQuestManager
    {
        public Dictionary<string, Merchant> questDict = new Dictionary<string, Merchant>();
        public IList<MapQuestInfo> QuestList = new List<MapQuestInfo>();

        public bool CreateQuest(int nFlag, int nValue, string sMonName, string sItem, string sQuest, bool boGrouped)
        {
            if (nFlag < 0)
            {
                return false;
            }
            MapQuestInfo mapQuest = new MapQuestInfo
            {
                Flag = nFlag
            };
            if (nValue > 1)
            {
                nValue = 1;
            }
            mapQuest.Value = nValue;
            if (sMonName == "*")
            {
                sMonName = "";
            }
            mapQuest.MonName = sMonName;
            if (sItem == "*")
            {
                sItem = "";
            }
            mapQuest.ItemName = sItem;
            if (sQuest == "*")
            {
                sQuest = "";
            }
            mapQuest.Grouped = boGrouped;
            Merchant questMerchant = new Merchant
            {
                MapName = "0",
                CurrX = 0,
                CurrY = 0,
                ChrName = sQuest,
                NpcFlag = 0,
                Appr = 0,
                FilePath = "MapQuest_def",
                IsHide = true,
                IsQuest = false
            };
            SystemShare.WorldEngine.AddQuestNpc(questMerchant);
            mapQuest.NPC = questMerchant;
            QuestList.Add(mapQuest);
            return true;
        }

        public Merchant GetQuestNpc(PlayObject baseObject, string sChrName, string itemName, bool boFlag)
        {
            for (int i = 0; i < QuestList.Count; i++)
            {
                MapQuestInfo mapQuestFlag = QuestList[i];
                int nFlagValue = baseObject.GetQuestFalgStatus(mapQuestFlag.Flag);
                if (nFlagValue == mapQuestFlag.Value)
                {
                    if (boFlag == mapQuestFlag.Grouped || !boFlag)
                    {
                        bool bo1D = false;
                        if (!string.IsNullOrEmpty(mapQuestFlag.MonName) && !string.IsNullOrEmpty(mapQuestFlag.ItemName))
                        {
                            if (mapQuestFlag.MonName == sChrName && mapQuestFlag.ItemName == itemName)
                            {
                                bo1D = true;
                            }
                        }
                        if (!string.IsNullOrEmpty(mapQuestFlag.MonName) && string.IsNullOrEmpty(mapQuestFlag.ItemName))
                        {
                            if (mapQuestFlag.MonName == sChrName && string.IsNullOrEmpty(itemName))
                            {
                                bo1D = true;
                            }
                        }
                        if (string.IsNullOrEmpty(mapQuestFlag.MonName) && !string.IsNullOrEmpty(mapQuestFlag.ItemName))
                        {
                            if (mapQuestFlag.ItemName == itemName)
                            {
                                bo1D = true;
                            }
                        }
                        if (bo1D)
                        {
                            return (Merchant)mapQuestFlag.NPC;
                        }
                    }
                }
            }
            return null;
        }
    }
}