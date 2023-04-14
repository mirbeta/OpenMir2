using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定物品名称
    /// </summary>
    [Command("ChangeItemName", "调整指定物品名称", 10, Help = "物品编号 物品ID号 物品名称")]
    public class ChangeItemNameCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sMakeIndex = @params.Length > 0 ? @params[0] : "";
            string sItemIndex = @params.Length > 1 ? @params[1] : "";
            string sItemName = @params.Length > 2 ? @params[2] : "";
            if (string.IsNullOrEmpty(sMakeIndex) || string.IsNullOrEmpty(sItemIndex) || string.IsNullOrEmpty(sItemName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            int nMakeIndex = HUtil32.StrToInt(sMakeIndex, -1);
            int nItemIndex = HUtil32.StrToInt(sItemIndex, -1);
            if (nMakeIndex <= 0 || nItemIndex < 0) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.CustomItemMgr.AddCustomItemName(nMakeIndex, nItemIndex, sItemName)) {
                M2Share.CustomItemMgr.SaveCustomItemName();
                playObject.SysMsg("物品名称设置成功。", MsgColor.Green, MsgType.Hint);
                return;
            }
            playObject.SysMsg("此物品，已经设置了其它的名称!!!", MsgColor.Red, MsgType.Hint);
        }
    }
}