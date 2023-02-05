using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("ShutupRelease", "恢复禁言", CommandHelp.GameCommandShutupReleaseHelpMsg, 10)]
    public class ShutupReleaseCommand : GameCommand
    {
        [ExecuteCommand]
        public void ShutupRelease(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            bool boAll = @params.Length > 1 ? bool.Parse(@params[1]) : false;
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            HUtil32.EnterCriticalSection(M2Share.DenySayMsgList);
            try
            {
                //if (Settings.g_DenySayMsgList.ContainsKey(sHumanName))
                //{
                //    Settings.g_DenySayMsgList.Remove(sHumanName);
                //    TPlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
                //    //PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
                //    if (m_PlayObject != null)
                //    {
                //        m_PlayObject.SysMsg(Settings.GameCommandShutupReleaseCanSendMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                //    }
                //    if (boAll)
                //    {
                //        //M2Share.WorldEngine.SendServerGroupMsg(SS_210, nServerIndex, sHumanName);
                //    }
                //    PlayObject.SysMsg(string.Format(Settings.GameCommandShutupReleaseHumanCanSendMsg, sHumanName),
                //        TMsgColor.c_Green, TMsgType.t_Hint);
                //}
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.DenySayMsgList);
            }
        }
    }
}