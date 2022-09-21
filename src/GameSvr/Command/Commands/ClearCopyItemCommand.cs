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
        public void ClearCopyItem(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            UserItem UserItem;
            UserItem UserItem1;
            string s14;
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var TargerObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (TargerObject == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = TargerObject.ItemList.Count - 1; i >= 0; i--)
            {
                if (TargerObject.ItemList.Count <= 0)
                {
                    break;
                }

                UserItem = TargerObject.ItemList[i];
                s14 = M2Share.WorldEngine.GetStdItemName(UserItem.Index);
                for (var j = i - 1; j >= 0; j--)
                {
                    UserItem1 = TargerObject.ItemList[j];
                    if (M2Share.WorldEngine.GetStdItemName(UserItem1.Index) == s14 && UserItem.MakeIndex == UserItem1.MakeIndex)
                    {
                        PlayObject.ItemList.RemoveAt(j);
                        break;
                    }
                }
            }

            for (var i = TargerObject.StorageItemList.Count - 1; i >= 0; i--)
            {
                if (TargerObject.StorageItemList.Count <= 0)
                {
                    break;
                }
                UserItem = TargerObject.StorageItemList[i];
                s14 = M2Share.WorldEngine.GetStdItemName(UserItem.Index);
                for (var j = i - 1; j >= 0; j--)
                {
                    UserItem1 = TargerObject.StorageItemList[j];
                    if (M2Share.WorldEngine.GetStdItemName(UserItem1.Index) == s14 &&
                        UserItem.MakeIndex == UserItem1.MakeIndex)
                    {
                        PlayObject.StorageItemList.RemoveAt(j);
                        break;
                    }
                }
            }
        }
    }
}