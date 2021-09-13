using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家职业
    /// </summary>
    [GameCommand("ChangeJob", "调整指定玩家职业", 10)]
    public class ChangeJobCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeJob(string[] @params, TPlayObject PlayObject)
        {
            TPlayObject m_PlayObject;
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sJobName = @params.Length > 1 ? @params[1] : "";
            if (sHumanName == "" || sJobName == "")
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandChangeJobHelpMsg),
                    TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                if (string.Compare(sJobName, "Warr", true) == 0)
                {
                    m_PlayObject.m_btJob = 0;
                }
                if (string.Compare(sJobName, "Wizard", true) == 0)
                {
                    m_PlayObject.m_btJob = 1;
                }
                if (string.Compare(sJobName, "Taos", true) == 0)
                {
                    m_PlayObject.m_btJob = 2;
                }
                m_PlayObject.HasLevelUp(1);
                m_PlayObject.SysMsg(M2Share.g_sGameCommandChangeJobHumanMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandChangeJobMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}