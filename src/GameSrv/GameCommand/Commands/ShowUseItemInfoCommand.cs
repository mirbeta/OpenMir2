using GameSrv.Player;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 显示物品信息
    /// </summary>
    [Command("ShowUseItem", "显示物品信息", CommandHelp.GameCommandShowUseItemInfoHelpMsg, 10)]
    public class ShowUseItemInfoCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?') {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            for (int i = 0; i < mPlayObject.UseItems.Length; i++) {
                UserItem userItem = mPlayObject.UseItems[i];
                if (userItem.Index == 0) {
                    continue;
                }
                playObject.SysMsg(string.Format("%s[%s]IDX[%d]系列号[%d]持久[%d-%d]", M2Share.GetUseItemName(i), M2Share.WorldEngine.GetStdItemName(userItem.Index), userItem.Index,
                    userItem.MakeIndex, userItem.Dura, userItem.DuraMax), MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}