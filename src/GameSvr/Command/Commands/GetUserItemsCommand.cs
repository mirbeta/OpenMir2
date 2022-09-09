using GameSvr.Items;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;
using StdItem = GameSvr.Items.StdItem;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 取指定玩家物品
    /// </summary>
    [GameCommand("GetUserItems", "取指定玩家物品", "人物名称 物品名称 数量 类型(0,1,2)", 10)]
    public class GetUserItemsCommand : BaseCommond
    {
        [DefaultCommand]
        public void GetUserItems(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sItemName = @Params.Length > 1 ? @Params[1] : "";
            var sItemCount = @Params.Length > 2 ? @Params[2] : "";
            var sType = @Params.Length > 3 ? @Params[3] : "";

            int nItemCount;
            StdItem StdItem;
            UserItem UserItem = null;
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sItemName) || sItemCount == "" || sType == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var nCount = HUtil32.Str_ToInt(sItemCount, 0);
            var nType = HUtil32.Str_ToInt(sType, 0);
            switch (nType)
            {
                case 0:
                    nItemCount = 0;
                    for (var i = 0; i < m_PlayObject.UseItems.Length; i++)
                    {
                        if (m_PlayObject.ItemList.Count >= 46)
                        {
                            break;
                        }
                        UserItem = m_PlayObject.UseItems[i];
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem != null && string.Compare(sItemName, StdItem.Name, StringComparison.Ordinal) == 0)
                        {
                            if (!m_PlayObject.IsEnoughBag())
                            {
                                break;
                            }
                            UserItem = new UserItem(); ;
                            UserItem = m_PlayObject.UseItems[i];
                            m_PlayObject.ItemList.Add(UserItem);
                            m_PlayObject.SendAddItem(UserItem);
                            m_PlayObject.UseItems[i].wIndex = 0;
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
                    for (var i = m_PlayObject.ItemList.Count - 1; i >= 0; i--)
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
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
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
                    for (var i = m_PlayObject.StorageItemList.Count - 1; i >= 0; i--)
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
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
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