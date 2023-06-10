using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("ShutupRelease", "恢复禁言", CommandHelp.GameCommandShutupReleaseHelpMsg, 10)]
    public class ShutupReleaseCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var boAll = @params.Length > 1 ? bool.Parse(@params[1]) : false;
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            HUtil32.EnterCriticalSection(SystemShare.DenySayMsgList);
            try
            {
                //if (Settings.g_DenySayMsgList.ContainsKey(sHumanName))
                //{
                //    Settings.g_DenySayMsgList.Remove(sHumanName);
                //    IPlayerActor m_IPlayerActor = M2Share.WorldEngine.GeIPlayerActor(sHumanName);
                //    //IPlayerActor = M2Share.WorldEngine.GeIPlayerActor(sHumanName);
                //    if (m_IPlayerActor != null)
                //    {
                //        m_IPlayerActor.SysMsg(Settings.GameCommandShutupReleaseCanSendMsg, MsgColor.c_Red, MsgType.t_Hint);
                //    }
                //    if (boAll)
                //    {
                //        //M2Share.WorldEngine.SendServerGroupMsg(SS_210, nServerIndex, sHumanName);
                //    }
                //    PlayerActor.SysMsg(string.Format(Settings.GameCommandShutupReleaseHumanCanSendMsg, sHumanName),
                //        MsgColor.c_Green, MsgType.t_Hint);
                //}
            }
            finally
            {
                HUtil32.LeaveCriticalSection(SystemShare.DenySayMsgList);
            }
        }
    }
}