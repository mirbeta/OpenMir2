using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("Hair", "修改玩家发型", "人物名称 类型值", 10)]
    public class HairCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            int nHair = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            if (string.IsNullOrEmpty(sHumanName) || nHair < 0) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null) {
                mPlayObject.Hair = (byte)nHair;
                mPlayObject.FeatureChanged();
                playObject.SysMsg(sHumanName + " 的头发已改变。", MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}