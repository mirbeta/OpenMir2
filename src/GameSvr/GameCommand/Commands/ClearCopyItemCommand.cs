using GameSvr.Player;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 清除游戏中指定玩家复制物品
    /// </summary>
    [Command("ClearCopyItem", "清除游戏中指定玩家复制物品", "人物名称", 10)]
    public class ClearCopyItemCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            UserItem UserItem;
            UserItem UserItem1;
            string s14;
            if (string.IsNullOrEmpty(sHumanName)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject TargerObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (TargerObject == null) {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            for (int i = TargerObject.ItemList.Count - 1; i >= 0; i--) {
                if (TargerObject.ItemList.Count <= 0) {
                    break;
                }

                UserItem = TargerObject.ItemList[i];
                s14 = M2Share.WorldEngine.GetStdItemName(UserItem.Index);
                for (int j = i - 1; j >= 0; j--) {
                    UserItem1 = TargerObject.ItemList[j];
                    if (M2Share.WorldEngine.GetStdItemName(UserItem1.Index) == s14 && UserItem.MakeIndex == UserItem1.MakeIndex) {
                        PlayObject.ItemList.RemoveAt(j);
                        break;
                    }
                }
            }

            for (int i = TargerObject.StorageItemList.Count - 1; i >= 0; i--) {
                if (TargerObject.StorageItemList.Count <= 0) {
                    break;
                }
                UserItem = TargerObject.StorageItemList[i];
                s14 = M2Share.WorldEngine.GetStdItemName(UserItem.Index);
                for (int j = i - 1; j >= 0; j--) {
                    UserItem1 = TargerObject.StorageItemList[j];
                    if (M2Share.WorldEngine.GetStdItemName(UserItem1.Index) == s14 &&
                        UserItem.MakeIndex == UserItem1.MakeIndex) {
                        PlayObject.StorageItemList.RemoveAt(j);
                        break;
                    }
                }
            }
        }
    }
}