using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家职业
    /// </summary>
    [GameCommand("ChangeJob", "调整指定玩家职业", M2Share.g_sGameCommandChangeJobHelpMsg, 10)]
    public class ChangeJobCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeJob(string[] @params, TPlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            TPlayObject m_PlayObject;
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sJobName = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || sJobName == "")
            {
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), MsgColor.Red, MsgType.Hint);
                return;
            }
            m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                if (string.Compare(sJobName, "Warr", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.m_btJob = 0;
                }
                if (string.Compare(sJobName, "Wizard", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.m_btJob = 1;
                }
                if (string.Compare(sJobName, "Taos", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.m_btJob = 2;
                }
                m_PlayObject.HasLevelUp(1);
                m_PlayObject.SysMsg(M2Share.g_sGameCommandChangeJobHumanMsg, MsgColor.Green, MsgType.Hint);
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandChangeJobMsg, sHumanName), MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}