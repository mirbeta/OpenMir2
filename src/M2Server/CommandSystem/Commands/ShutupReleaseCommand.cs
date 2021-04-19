using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("ShutupRelease", "", 10)]
    public class ShutupReleaseCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShutupRelease(string[] @params, TPlayObject PlayObject)
        {
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var boAll = @params.Length > 1 ? bool.Parse(@params[1]) : false;

            if (sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandShutupReleaseHelpMsg),
                    TMsgColor.c_Red, TMsgType.t_Hint);
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