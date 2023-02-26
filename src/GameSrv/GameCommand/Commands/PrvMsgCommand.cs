using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 拒绝发言
    /// </summary>
    [Command("PrvMsg", "拒绝发言", CommandHelp.GameCommandPrvMsgHelpMsg, 10)]
    public class PrvMsgCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            for (int i = PlayObject.LockWhisperList.Count - 1; i >= 0; i--) {
                if (PlayObject.LockWhisperList.Count <= 0) {
                    break;
                }
                //if ((PlayObject.m_BlockWhisperList[i]).CompareTo((sHumanName)) == 0)
                //{
                //    PlayObject.m_BlockWhisperList.RemoveAt(i);
                //    PlayObject.SysMsg(string.Format(Settings.GameCommandPrvMsgUnLimitMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
                //    return;
                //}
            }
            PlayObject.LockWhisperList.Add(sHumanName);
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandPrvMsgLimitMsg, sHumanName), MsgColor.Green, MsgType.Hint);
        }
    }
}