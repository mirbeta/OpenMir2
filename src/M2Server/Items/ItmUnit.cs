using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace M2Server
{
    public class ItemUnit
    {
        private IList<TItemName> _mItemNameList = null;

        public ItemUnit()
        {
            _mItemNameList = new List<TItemName>();
        }

        ~ItemUnit()
        {
            for (var i = 0; i < _mItemNameList.Count; i ++ )
            {
                _mItemNameList[i] = null;
            }
            _mItemNameList = null;
        }

        public string GetCustomItemName(int nMakeIndex, int nItemIndex)
        {
            var result = string.Empty;
            TItemName itemName;
            for (var i = _mItemNameList.Count - 1; i >= 0; i--)
            {
                itemName = _mItemNameList[i];
                if (itemName.nMakeIndex != nMakeIndex || itemName.nItemIndex != nItemIndex) continue;
                result = itemName.sItemName;
                break;
            }
            return result;
        }

        public bool AddCustomItemName(int nMakeIndex, int nItemIndex, string sItemName)
        {
            var result = false;
            TItemName itemName;
            for (var i = _mItemNameList.Count - 1; i >= 0; i--)
            {
                itemName = _mItemNameList[i];
                if (itemName.nMakeIndex == nMakeIndex && itemName.nItemIndex == nItemIndex)
                {
                    return result;
                }
            }
            itemName = new TItemName();
            itemName.nMakeIndex = nMakeIndex;
            itemName.nItemIndex = nItemIndex;
            itemName.sItemName = sItemName;
            _mItemNameList.Add(itemName);
            result = true;
            return result;
        }

        public void DelCustomItemName(int nMakeIndex, int nItemIndex)
        {
            for (var i = 0; i < _mItemNameList.Count; i++)
            {
                var itemName = _mItemNameList[i];
                if (itemName.nMakeIndex == nMakeIndex && itemName.nItemIndex == nItemIndex)
                {
                    itemName = null;
                    _mItemNameList.RemoveAt(i);
                    return;
                }
            }
        }

        public void LoadCustomItemName()
        {
            string sLineText;
            var sMakeIndex = string.Empty;
            var sItemIndex = string.Empty;
            var sItemName = string.Empty;
            var sFileName = Path.Combine(M2Share.g_Config.sEnvirDir, "ItemNameList.txt");
            var loadList = new StringList();
            if (File.Exists(sFileName))
            {
                _mItemNameList.Clear();
                loadList.LoadFromFile(sFileName);
                for (var i = 0; i < loadList.Count; i++)
                {
                    sLineText = loadList[i].Trim();
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sMakeIndex, new string[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIndex, new string[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemName, new string[] { " ", "\t" });
                    var nMakeIndex = HUtil32.Str_ToInt(sMakeIndex, -1);
                    var nItemIndex = HUtil32.Str_ToInt(sItemIndex, -1);
                    if (nMakeIndex < 0 || nItemIndex < 0) continue;
                    var itemName = new TItemName();
                    itemName.nMakeIndex = nMakeIndex;
                    itemName.nItemIndex = nItemIndex;
                    itemName.sItemName = sItemName;
                    _mItemNameList.Add(itemName);
                }
            }
            else
            {
                loadList.SaveToFile(sFileName);
            }
            loadList = null;
        }

        public void SaveCustomItemName()
        {
            var sFileName = Path.Combine(M2Share.g_Config.sEnvirDir, "ItemNameList.txt");
            var saveList = new StringList();
            for (var i = _mItemNameList.Count - 1; i >= 0; i--)
            {
                var itemName = _mItemNameList[i];
                saveList.Add(itemName.nMakeIndex + "\t" + itemName.nItemIndex + "\t" + itemName.sItemName);
            }
            saveList.SaveToFile(sFileName);
            saveList = null;
        }
        
        public int GetItemAddValuePointColor(TUserItem UserItem)
        {
            int result = 0;
            int ItemVlue= 0;
            MirItem StdItem;
            // if (M2Share.g_Config.boRandomnameColor)
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
            //                 result = M2Share.g_Config.nRandom1nameColor;
            //                 break;
            //             case 2:
            //                 result = M2Share.g_Config.nRandom2nameColor;
            //                 break;
            //             case 3:
            //                 result = M2Share.g_Config.nRandom3nameColor;
            //                 break;
            //             case 4:
            //                 result = M2Share.g_Config.nRandom4nameColor;
            //                 break;
            //             default:
            //                 result = M2Share.g_Config.nRandom5nameColor;
            //                 break;
            //         }
            //     }
            //     else
            //     {
            //         StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            //         if (StdItem != null)
            //         {
            //             switch(StdItem.StdMode)
            //             {
            //                 case 0:
            //                 case 1:
            //                 case 3:
            //                     result = M2Share.g_Config.nRandom8nameColor;
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
            //                     result = M2Share.g_Config.nRandom7nameColor;
            //                     break;
            //                 default:
            //                     result = M2Share.g_Config.nRandom6nameColor;
            //                     break;
            //             }
            //         }
            //     }
            // }
            // else
            // {
            //     StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
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
    
    public class ItmUnit
    {
        /// <summary>
        /// 获取自定义装备名称
        /// </summary>
        /// <param name="userItem"></param>
        /// <returns></returns>
        public static string GetItemName(TUserItem userItem)
        {
            var result = string.Empty;
            if (userItem.btValue[13] == 1)
            {
                result = M2Share.ItemUnit.GetCustomItemName(userItem.MakeIndex, userItem.wIndex);
            }
            if (string.IsNullOrEmpty(result))
            {
                result = M2Share.UserEngine.GetStdItemName(userItem.wIndex);
            }
            return result;
        }
    }
}


