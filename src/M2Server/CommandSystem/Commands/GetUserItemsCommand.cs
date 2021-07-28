using SystemModule;
using System;
using System.Runtime.InteropServices;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 取指定玩家物品
    /// </summary>
    [GameCommand("GetUserItems", "取指定玩家物品", 10)]
    public class GetUserItemsCommand : BaseCommond
    {
        [DefaultCommand]
        public void GetUserItems(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sItemName = @Params.Length > 1 ? @Params[1] : "";
            var sItemCount = @Params.Length > 2 ? @Params[2] : "";
            var sType = @Params.Length > 3 ? @Params[3] : "";

            int nItemCount;
            int nCount;
            int nType;
            TItem StdItem;
            TUserItem UserItem = null;
            if (sHumanName == "" || sItemName == "" || sItemCount == "" || sType == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称 物品名称 数量 类型(0,1,2))", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nCount = HUtil32.Str_ToInt(sItemCount, 0);
            nType = HUtil32.Str_ToInt(sType, 0);
            switch (nType)
            {
                case 0:
                    nItemCount = 0;
                    for (var i = m_PlayObject.m_UseItems.GetLowerBound(0); i <= m_PlayObject.m_UseItems.GetUpperBound(0); i++)
                    {
                        if (m_PlayObject.m_ItemList.Count >= 46)
                        {
                            break;
                        }
                        UserItem = m_PlayObject.m_UseItems[i];
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem != null && sItemName.ToLower().CompareTo(StdItem.Name.ToLower()) == 0)
                        {
                            if (!m_PlayObject.IsEnoughBag())
                            {
                                break;
                            }
                            UserItem = new TUserItem(); ;
                            UserItem = m_PlayObject.m_UseItems[i];
                            m_PlayObject.m_ItemList.Add(UserItem);
                            m_PlayObject.SendAddItem(UserItem);
                            m_PlayObject.m_UseItems[i].wIndex = 0;
                            nItemCount++;
                            if (nItemCount >= nCount)
                            {
                                break;
                            }
                        }
                    }
                    m_PlayObject.SendUseitems();
                    break;

                case 1:
                    nItemCount = 0;
                    for (var i = m_PlayObject.m_ItemList.Count - 1; i >= 0; i--)
                    {
                        if (m_PlayObject.m_ItemList.Count >= 46)
                        {
                            break;
                        }
                        if (m_PlayObject.m_ItemList.Count <= 0)
                        {
                            break;
                        }
                        UserItem = m_PlayObject.m_ItemList[i];
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem != null && sItemName.ToLower().CompareTo(StdItem.Name.ToLower()) == 0)
                        {
                            if (!m_PlayObject.IsEnoughBag())
                            {
                                break;
                            }
                            m_PlayObject.SendDelItems(UserItem);
                            m_PlayObject.m_ItemList.RemoveAt(i);
                            m_PlayObject.m_ItemList.Add(UserItem);
                            m_PlayObject.SendAddItem(UserItem);
                            nItemCount++;
                            if (nItemCount >= nCount)
                            {
                                break;
                            }
                        }
                    }
                    break;

                case 2:
                    nItemCount = 0;
                    for (var i = m_PlayObject.m_StorageItemList.Count - 1; i >= 0; i--)
                    {
                        if (m_PlayObject.m_ItemList.Count >= 46)
                        {
                            break;
                        }
                        if (m_PlayObject.m_StorageItemList.Count <= 0)
                        {
                            break;
                        }
                        UserItem = m_PlayObject.m_StorageItemList[i];
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem != null && sItemName.ToLower().CompareTo(StdItem.Name.ToLower()) == 0)
                        {
                            if (!m_PlayObject.IsEnoughBag())
                            {
                                break;
                            }
                            m_PlayObject.m_StorageItemList.RemoveAt(i);
                            m_PlayObject.m_ItemList.Add(UserItem);
                            m_PlayObject.SendAddItem(UserItem);
                            nItemCount++;
                            if (nItemCount >= nCount)
                            {
                                break;
                            }
                        }
                    }
                    break;
            }
        }
    }
}