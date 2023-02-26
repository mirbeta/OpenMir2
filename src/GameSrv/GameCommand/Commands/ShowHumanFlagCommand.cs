using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 取用户任务状态
    /// </summary>
    [Command("ShowHumanFlag", "取用户任务状态", CommandHelp.GameCommandShowHumanFlagHelpMsg, 10)]
    public class ShowHumanFlagCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sFlag = @Params.Length > 1 ? @Params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null) {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            int nFlag = HUtil32.StrToInt(sFlag, 0);
            if (m_PlayObject.GetQuestFalgStatus(nFlag) == 1) {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandShowHumanFlagONMsg, m_PlayObject.ChrName, nFlag), MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandShowHumanFlagOFFMsg, m_PlayObject.ChrName, nFlag), MsgColor.Green, MsgType.Hint);
            }
        }
    }
}