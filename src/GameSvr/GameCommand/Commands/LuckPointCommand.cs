using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家幸运点
    /// </summary>
    [Command("LuckPoint", "查看指定玩家幸运点", CommandHelp.GameCommandLuckPointHelpMsg, 10)]
    public class LuckPointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sCtr = @Params.Length > 1 ? @Params[1] : "";
            string sPoint = @Params.Length > 2 ? @Params[2] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sCtr == "") {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandLuckPointMsg, sHumanName, mPlayObject.BodyLuckLevel, mPlayObject.BodyLuck, mPlayObject.Luck), MsgColor.Green, MsgType.Hint);
                return;
            }
            byte nPoint = (byte)HUtil32.StrToInt(sPoint, 0);
            char cMethod = sCtr[0];
            switch (cMethod) {
                case '=':
                    mPlayObject.Luck = nPoint;
                    break;
                case '-':
                    if (mPlayObject.Luck >= nPoint) {
                        mPlayObject.Luck -= nPoint;
                    }
                    else {
                        mPlayObject.Luck = 0;
                    }
                    break;
                case '+':
                    mPlayObject.Luck += nPoint;
                    break;
            }
        }
    }
}