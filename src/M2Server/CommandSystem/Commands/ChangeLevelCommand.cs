using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 调整当前玩家等级
    /// </summary>
    [GameCommand("ChangeLevel", "调整当前玩家等级", 10)]
    public class ChangeLevelCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeLevel(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sParam1 = @Params.Length > 0 ? @Params[0] : "";
            if (sParam1 != "" && sParam1[0] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var nLevel = HUtil32.Str_ToInt(sParam1, 1);
            int nOLevel = PlayObject.m_Abil.Level;
            PlayObject.m_Abil.Level = (short)HUtil32._MIN(M2Share.MAXUPLEVEL, nLevel);
            PlayObject.HasLevelUp(1);// 等级调整记录日志
            M2Share.AddGameDataLog("17" + "\09" + PlayObject.m_sMapName + "\09" + PlayObject.m_nCurrX + "\09" + PlayObject.m_nCurrY
                + "\09" + PlayObject.m_sCharName + "\09" + PlayObject.m_Abil.Level + "\09" + "0" + "\09" + "=(" + nLevel + ")" + "\09" + "0");
            if (M2Share.g_Config.boShowMakeItemMsg)
            {
                M2Share.MainOutMessage(string.Format(M2Share.g_sGameCommandLevelConsoleMsg, PlayObject.m_sCharName, nOLevel, PlayObject.m_Abil.Level));
            }
        }
    }
}