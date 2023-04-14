using GameSrv.Items;
using GameSrv.Player;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 删除指定玩家包裹物品
    /// </summary>
    [Command("DeleteItem", "删除人物身上指定的物品", help: "人物名称 物品名称 数量", 10)]
    public class DeleteItemCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : ""; //玩家名称
            var sItemName = @params.Length > 1 ? @params[1] : ""; //物品名称
            var nCount = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0; //数量
            StdItem stdItem;
            UserItem userItem;
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sItemName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var nItemCount = 0;
            for (var i = mPlayObject.ItemList.Count - 1; i >= 0; i--) {
                if (mPlayObject.ItemList.Count <= 0) {
                    break;
                }

                userItem = mPlayObject.ItemList[i];
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null && string.Compare(sItemName, stdItem.Name, StringComparison.OrdinalIgnoreCase) == 0) {
                    mPlayObject.SendDelItems(userItem);
                    mPlayObject.ItemList.RemoveAt(i);
                    nItemCount++;
                    if (nItemCount >= nCount) {
                        break;
                    }
                }
            }
        }
    }
}