using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家权限
    /// </summary>
    [Command("SetPermission", "调整指定玩家权限", "人物名称 权限等级(0 - 10)", 10)]
    public class SetPermissionCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sPermission = @params.Length > 1 ? @params[1] : "";
            var nPerission = HUtil32.StrToInt(sPermission, 0);
            const string sOutFormatMsg = "[权限调整] {0} [{1} {2} -> {3}]";
            if (string.IsNullOrEmpty(sHumanName) || !(nPerission >= 0 && nPerission <= 10)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.Config.ShowMakeItemMsg) {
                M2Share.Logger.Warn(string.Format(sOutFormatMsg, playObject.ChrName, mPlayObject.ChrName, mPlayObject.Permission, nPerission));
            }
            mPlayObject.Permission = (byte)nPerission;
            playObject.SysMsg(sHumanName + " 当前权限为: " + mPlayObject.Permission, MsgColor.Red, MsgType.Hint);
        }
    }
}