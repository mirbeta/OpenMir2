using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("ShutupRelease", "恢复禁言", GameCommandConst.g_sGameCommandShutupReleaseHelpMsg, 10)]
    public class ShutupReleaseCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShutupRelease(string[] @params, TPlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var boAll = @params.Length > 1 ? bool.Parse(@params[1]) : false;
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            HUtil32.EnterCriticalSection(M2Share.g_DenySayMsgList);
            try
            {
                //if (M2Share.g_DenySayMsgList.ContainsKey(sHumanName))
                //{
                //    M2Share.g_DenySayMsgList.Remove(sHumanName);
                //    TPlayObject m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
                //    //PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
                //    if (m_PlayObject != null)
                //    {
                //        m_PlayObject.SysMsg(M2Share.g_sGameCommandShutupReleaseCanSendMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                //    }
                //    if (boAll)
                //    {
                //        //M2Share.UserEngine.SendServerGroupMsg(SS_210, nServerIndex, sHumanName);
                //    }
                //    PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandShutupReleaseHumanCanSendMsg, sHumanName),
                //        TMsgColor.c_Green, TMsgType.t_Hint);
                //}
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.g_DenySayMsgList);
            }
        }
    }
}