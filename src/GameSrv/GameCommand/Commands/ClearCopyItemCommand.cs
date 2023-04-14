using GameSrv.Player;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 清除游戏中指定玩家复制物品
    /// </summary>
    [Command("ClearCopyItem", "清除游戏中指定玩家复制物品", "人物名称", 10)]
    public class ClearCopyItemCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            UserItem userItem;
            UserItem userItem1;
            string s14;
            if (string.IsNullOrEmpty(sHumanName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var targerObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (targerObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = targerObject.ItemList.Count - 1; i >= 0; i--) {
                if (targerObject.ItemList.Count <= 0) {
                    break;
                }

                userItem = targerObject.ItemList[i];
                s14 = M2Share.WorldEngine.GetStdItemName(userItem.Index);
                for (var j = i - 1; j >= 0; j--) {
                    userItem1 = targerObject.ItemList[j];
                    if (M2Share.WorldEngine.GetStdItemName(userItem1.Index) == s14 && userItem.MakeIndex == userItem1.MakeIndex) {
                        playObject.ItemList.RemoveAt(j);
                        break;
                    }
                }
            }

            for (var i = targerObject.StorageItemList.Count - 1; i >= 0; i--) {
                if (targerObject.StorageItemList.Count <= 0) {
                    break;
                }
                userItem = targerObject.StorageItemList[i];
                s14 = M2Share.WorldEngine.GetStdItemName(userItem.Index);
                for (var j = i - 1; j >= 0; j--) {
                    userItem1 = targerObject.StorageItemList[j];
                    if (M2Share.WorldEngine.GetStdItemName(userItem1.Index) == s14 &&
                        userItem.MakeIndex == userItem1.MakeIndex) {
                        playObject.StorageItemList.RemoveAt(j);
                        break;
                    }
                }
            }
        }
    }
}