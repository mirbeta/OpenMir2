using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 清除指定玩家PK值
    /// </summary>
    [GameCommand("FreePenalty", "清除指定玩家PK值", "人物名称", 10)]
    public class FreePenaltyCommand : BaseCommond
    {
        [DefaultCommand]
        public void FreePenalty(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (!string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_PlayObject.m_nPkPoint = 0;
            m_PlayObject.RefNameColor();
            m_PlayObject.SysMsg(M2Share.g_sGameCommandFreePKHumanMsg, TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandFreePKMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}