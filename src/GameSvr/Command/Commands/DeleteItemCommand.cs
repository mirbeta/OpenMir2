using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 删除指定玩家包裹物品
    /// </summary>
    [GameCommand("DeleteItem", "删除人物身上指定的物品", help: "人物名称 物品名称 数量", 10)]
    public class DeleteItemCommand : BaseCommond
    {
        [DefaultCommand]
        public void DeleteItem(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : ""; //玩家名称
            var sItemName = @Params.Length > 1 ? @Params[1] : ""; //物品名称
            var nCount = @Params.Length > 2 ? int.Parse(@Params[2]) : 0; //数量
            int nItemCount;
            GoodItem StdItem;
            TUserItem UserItem;
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sItemName))
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            nItemCount = 0;
            for (var i = m_PlayObject.m_ItemList.Count - 1; i >= 0; i--)
            {
                if (m_PlayObject.m_ItemList.Count <= 0)
                {
                    break;
                }

                UserItem = m_PlayObject.m_ItemList[i];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null && string.Compare(sItemName, StdItem.Name, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.SendDelItems(UserItem);
                    m_PlayObject.m_ItemList.RemoveAt(i);
                    UserItem = null;
                    nItemCount++;
                    if (nItemCount >= nCount)
                    {
                        break;
                    }
                }
            }
        }
    }
}