using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 清除游戏中指定玩家复制物品
    /// </summary>
    [GameCommand("ClearCopyItem", "清除游戏中指定玩家复制物品", "人物名称", 10)]
    public class ClearCopyItemCommand : BaseCommond
    {
        [DefaultCommand]
        public void ClearCopyItem(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            TPlayObject TargerObject;
            TUserItem UserItem;
            TUserItem UserItem1;
            string s14;
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            TargerObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (TargerObject == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = TargerObject.m_ItemList.Count - 1; i >= 0; i--)
            {
                if (TargerObject.m_ItemList.Count <= 0)
                {
                    break;
                }

                UserItem = TargerObject.m_ItemList[i];
                s14 = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                for (var j = i - 1; j >= 0; j--)
                {
                    UserItem1 = TargerObject.m_ItemList[j];
                    if (M2Share.UserEngine.GetStdItemName(UserItem1.wIndex) == s14 && UserItem.MakeIndex == UserItem1.MakeIndex)
                    {
                        PlayObject.m_ItemList.RemoveAt(j);
                        break;
                    }
                }
            }

            for (var i = TargerObject.m_StorageItemList.Count - 1; i >= 0; i--)
            {
                if (TargerObject.m_StorageItemList.Count <= 0)
                {
                    break;
                }
                UserItem = TargerObject.m_StorageItemList[i];
                s14 = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                for (var j = i - 1; j >= 0; j--)
                {
                    UserItem1 = TargerObject.m_StorageItemList[j];
                    if (M2Share.UserEngine.GetStdItemName(UserItem1.wIndex) == s14 &&
                        UserItem.MakeIndex == UserItem1.MakeIndex)
                    {
                        PlayObject.m_StorageItemList.RemoveAt(j);
                        break;
                    }
                }
            }
        }
    }
}