using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 查看指定玩家所在IP地址
    /// </summary>
    [Command("HumanLocal", "查看指定玩家所在IP地址", CommandHelp.GameCommandHumanLocalHelpMsg, 10)]
    public class HumanLocalCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string mSIpLocal = "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            // GetIPLocal(PlayObject.m_sIPaddr)
            playObject.SysMsg(string.Format(CommandHelp.GameCommandHumanLocalMsg, sHumanName, mSIpLocal), MsgColor.Green, MsgType.Hint);
        }
    }
}