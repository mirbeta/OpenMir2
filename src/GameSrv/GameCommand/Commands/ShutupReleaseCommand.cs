using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("ShutupRelease", "恢复禁言", CommandHelp.GameCommandShutupReleaseHelpMsg, 10)]
    public class ShutupReleaseCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var boAll = @params.Length > 1 ? bool.Parse(@params[1]) : false;
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            HUtil32.EnterCriticalSection(M2Share.DenySayMsgList);
            try {
                //if (Settings.g_DenySayMsgList.ContainsKey(sHumanName))
                //{
                //    Settings.g_DenySayMsgList.Remove(sHumanName);
                //    PlayObject m_PlayObject = M2Share.WorldEngine.GePlayObject(sHumanName);
                //    //PlayObject = M2Share.WorldEngine.GePlayObject(sHumanName);
                //    if (m_PlayObject != null)
                //    {
                //        m_PlayObject.SysMsg(Settings.GameCommandShutupReleaseCanSendMsg, MsgColor.c_Red, MsgType.t_Hint);
                //    }
                //    if (boAll)
                //    {
                //        //M2Share.WorldEngine.SendServerGroupMsg(SS_210, nServerIndex, sHumanName);
                //    }
                //    PlayObject.SysMsg(string.Format(Settings.GameCommandShutupReleaseHumanCanSendMsg, sHumanName),
                //        MsgColor.c_Green, MsgType.t_Hint);
                //}
            }
            finally {
                HUtil32.LeaveCriticalSection(M2Share.DenySayMsgList);
            }
        }
    }
}