using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

namespace M2Server.Items
{
    public class CustomItem
    {
        private IList<ItemName> ItemNameList;

        public CustomItem()
        {
            ItemNameList = new List<ItemName>();
        }

        ~CustomItem()
        {
            for (var i = 0; i < ItemNameList.Count; i++)
            {
                ItemNameList[i] = null;
            }
            ItemNameList = null;
        }

        public string GetCustomItemName(int nMakeIndex, int nItemIndex)
        {
            var result = string.Empty;
            for (var i = ItemNameList.Count - 1; i >= 0; i--)
            {
                var itemName = ItemNameList[i];
                if (itemName.nMakeIndex != nMakeIndex || itemName.nItemIndex != nItemIndex) continue;
                result = itemName.sItemName;
                break;
            }
            return result;
        }

        public bool AddCustomItemName(int nMakeIndex, int nItemIndex, string sItemName)
        {
            ItemName itemName;
            for (var i = ItemNameList.Count - 1; i >= 0; i--)
            {
                itemName = ItemNameList[i];
                if (itemName.nMakeIndex == nMakeIndex && itemName.nItemIndex == nItemIndex)
                {
                    return false;
                }
            }
            itemName = new ItemName();
            itemName.nMakeIndex = nMakeIndex;
            itemName.nItemIndex = nItemIndex;
            itemName.sItemName = sItemName;
            ItemNameList.Add(itemName);
            return true;
        }

        public void DelCustomItemName(int nMakeIndex, int nItemIndex)
        {
            for (var i = 0; i < ItemNameList.Count; i++)
            {
                var itemName = ItemNameList[i];
                if (itemName.nMakeIndex == nMakeIndex && itemName.nItemIndex == nItemIndex)
                {
                    ItemNameList.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// 读取自定义物品名称
        /// </summary>
        public void LoadCustomItemName()
        {
            var sMakeIndex = string.Empty;
            var sItemIndex = string.Empty;
            var sItemName = string.Empty;
            var sFileName = M2Share.GetEnvirFilePath("ItemNameList.txt");
            using var loadList = new StringList();
            if (File.Exists(sFileName))
            {
                ItemNameList.Clear();
                loadList.LoadFromFile(sFileName);
                for (var i = 0; i < loadList.Count; i++)
                {
                    var sLineText = loadList[i].Trim();
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sMakeIndex, new[] { ' ', '\t' });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIndex, new[] { ' ', '\t' });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemName, new[] { ' ', '\t' });
                    var nMakeIndex = HUtil32.StrToInt(sMakeIndex, -1);
                    var nItemIndex = HUtil32.StrToInt(sItemIndex, -1);
                    if (nMakeIndex < 0 || nItemIndex < 0) continue;
                    var itemName = new ItemName();
                    itemName.nMakeIndex = nMakeIndex;
                    itemName.nItemIndex = nItemIndex;
                    itemName.sItemName = sItemName;
                    ItemNameList.Add(itemName);
                }
            }
            else
            {
                loadList.SaveToFile(sFileName);
            }
        }

        /// <summary>
        /// 获取自定义装备名称
        /// </summary>
        /// <param name="userItem"></param>
        /// <returns></returns>
        public static string GetItemName(UserItem userItem)
        {
            var result = ModuleShare.ItemSystem.GetStdItemName(userItem.Index);
            if (userItem.Desc[13] == 1)
            {
                result = M2Share.CustomItemMgr.GetCustomItemName(userItem.MakeIndex, userItem.Index);
            }
            return result;
        }

        /// <summary>
        /// 保存自定义物品名称
        /// </summary>
        public void SaveCustomItemName()
        {
            var sFileName = M2Share.GetEnvirFilePath("ItemNameList.txt");
            var saveList = new StringList();
            for (var i = ItemNameList.Count - 1; i >= 0; i--)
            {
                var itemName = ItemNameList[i];
                saveList.Add(itemName.nMakeIndex + "\t" + itemName.nItemIndex + "\t" + itemName.sItemName);
            }
            saveList.SaveToFile(sFileName);
        }

        /// <summary>
        /// 获取物品名称颜色
        /// </summary>
        /// <param name="UserItem"></param>
        /// <returns></returns>
        public static int GetItemAddValuePointColor(UserItem UserItem)
        {
            var result = 0;
            // if (Settings.Config.boRandomnameColor)
            // {
            //     for (var I = 0; I <= 7; I ++ )
            //     {
            //         if (UserItem.btValue[I] != 0)
            //         {
            //             ItemVlue ++;
            //         }
            //     }
            //     if (ItemVlue > 0)
            //     {
            //         switch(ItemVlue)
            //         {
            //             case 1:
            //                 result = Settings.Config.nRandom1nameColor;
            //                 break;
            //             case 2:
            //                 result = Settings.Config.nRandom2nameColor;
            //                 break;
            //             case 3:
            //                 result = Settings.Config.nRandom3nameColor;
            //                 break;
            //             case 4:
            //                 result = Settings.Config.nRandom4nameColor;
            //                 break;
            //             default:
            //                 result = Settings.Config.nRandom5nameColor;
            //                 break;
            //         }
            //     }
            //     else
            //     {
            //         StdItem = ModuleShare.ItemSystem.GetStdItem(UserItem.Index);
            //         if (StdItem != null)
            //         {
            //             switch(StdItem.StdMode)
            //             {
            //                 case 0:
            //                 case 1:
            //                 case 3:
            //                     result = Settings.Config.nRandom8nameColor;
            //                     break;
            //                 case 5:
            //                 case 6:
            //                 case 7:
            //                 case 99:
            //                 case 10:
            //                 case 11:
            //                 case 15:
            //                 case 16:
            //                 case 70:
            //                 case 77:
            //                 case 88:
            //                 case 19:
            //                 case 20:
            //                 case 21:
            //                 case 22:
            //                 case 23:
            //                 case 24:
            //                 case 26:
            //                 case 52:
            //                 case 53:
            //                 case 54:
            //                 case 62:
            //                 case 63:
            //                 case 64:
            //                     result = Settings.Config.nRandom7nameColor;
            //                     break;
            //                 default:
            //                     result = Settings.Config.nRandom6nameColor;
            //                     break;
            //             }
            //         }
            //     }
            // }
            // else
            // {
            //     StdItem = ModuleShare.ItemSystem.GetStdItem(UserItem.Index);
            //     if (StdItem != null)
            //     {
            //         if (StdItem.NameColor != 0)
            //         {
            //             result = StdItem.NameColor;
            //         }
            //         else
            //         {
            //             result = 255;
            //         }
            //     }
            // }
            return result;
        }
    }
}