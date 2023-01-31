using GameSvr.Items;
using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 取指定玩家物品
    /// </summary>
    [Command("GetUserItems", "取指定玩家物品", "人物名称 物品名称 数量 类型(0,1,2)", 10)]
    public class GetUserItemsCommand : Command
    {
        [ExecuteCommand]
        public void GetUserItems(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sItemName = @Params.Length > 1 ? @Params[1] : "";
            string sItemCount = @Params.Length > 2 ? @Params[2] : "";
            string sType = @Params.Length > 3 ? @Params[3] : "";

            int nItemCount;
            StdItem StdItem;
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sItemName) || sItemCount == "" || sType == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            int nCount = HUtil32.StrToInt(sItemCount, 0);
            int nType = HUtil32.StrToInt(sType, 0);
            UserItem UserItem;
            switch (nType)
            {
                case 0:
                    nItemCount = 0;
                    for (int i = 0; i < m_PlayObject.UseItems.Length; i++)
                    {
                        if (m_PlayObject.ItemList.Count >= 46)
                        {
                            break;
                        }
                        UserItem = m_PlayObject.UseItems[i];
                        StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                        if (StdItem != null && string.Compare(sItemName, StdItem.Name, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (!m_PlayObject.IsEnoughBag())
                            {
                                break;
                            }
                            UserItem = new UserItem(); ;
                            UserItem = m_PlayObject.UseItems[i];
                            m_PlayObject.ItemList.Add(UserItem);
                            m_PlayObject.SendAddItem(UserItem);
                            m_PlayObject.UseItems[i].Index = 0;
                            nItemCount++;
                            if (nItemCount >= nCount)
                            {
                                break;
                            }
                        }
                    }
                    m_PlayObject.SendUseItems();
                    break;

                case 1:
                    nItemCount = 0;
                    for (int i = m_PlayObject.ItemList.Count - 1; i >= 0; i--)
                    {
                        if (m_PlayObject.ItemList.Count >= 46)
                        {
                            break;
                        }
                        if (m_PlayObject.ItemList.Count <= 0)
                        {
                            break;
                        }
                        UserItem = m_PlayObject.ItemList[i];
                        StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                        if (StdItem != null && string.Compare(sItemName, StdItem.Name, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (!m_PlayObject.IsEnoughBag())
                            {
                                break;
                            }
                            m_PlayObject.SendDelItems(UserItem);
                            m_PlayObject.ItemList.RemoveAt(i);
                            m_PlayObject.ItemList.Add(UserItem);
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
                    for (int i = m_PlayObject.StorageItemList.Count - 1; i >= 0; i--)
                    {
                        if (m_PlayObject.ItemList.Count >= 46)
                        {
                            break;
                        }
                        if (m_PlayObject.StorageItemList.Count <= 0)
                        {
                            break;
                        }
                        UserItem = m_PlayObject.StorageItemList[i];
                        StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                        if (StdItem != null && string.Compare(sItemName, StdItem.Name, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (!m_PlayObject.IsEnoughBag())
                            {
                                break;
                            }
                            m_PlayObject.StorageItemList.RemoveAt(i);
                            m_PlayObject.ItemList.Add(UserItem);
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