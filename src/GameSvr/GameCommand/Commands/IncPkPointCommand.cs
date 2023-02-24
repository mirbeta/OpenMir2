using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家PK值
    /// </summary>
    [Command("IncPkPoint", "调整指定玩家PK值", CommandHelp.GameCommandIncPkPointHelpMsg, 10)]
    public class IncPkPointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            int nPoint = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (string.IsNullOrEmpty(sHumanName)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null) {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            m_PlayObject.PkPoint += nPoint;
            m_PlayObject.RefNameColor();
            if (nPoint > 0) {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandIncPkPointAddPointMsg, sHumanName, nPoint), MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandIncPkPointDecPointMsg, sHumanName, -nPoint), MsgColor.Green, MsgType.Hint);
            }
        }
    }
}