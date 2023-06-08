using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家配偶名称
    /// </summary>
    [Command("ChangeDearName", "调整指定玩家配偶名称", help: "人物名称 配偶名称(如果为 无 则清除)", 10)]
    public class ChangeDearNameCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sDearName = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sDearName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null) {
                if (string.Compare(sDearName, "无", StringComparison.OrdinalIgnoreCase) == 0) {
                    mPlayObject.DearName = "";
                    mPlayObject.RefShowName();
                    playObject.SysMsg(sHumanName + " 的配偶名清除成功。", MsgColor.Green, MsgType.Hint);
                }
                else {
                    mPlayObject.DearName = sDearName;
                    mPlayObject.RefShowName();
                    playObject.SysMsg(sHumanName + " 的配偶名更改成功。", MsgColor.Green, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}