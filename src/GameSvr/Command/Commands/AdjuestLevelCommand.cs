using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家等级
    /// </summary>
    [GameCommand("AdjuestLevel", "调整指定玩家等级", "人物名称 等级", 10)]
    public class AdjuestLevelCommand : BaseCommond
    {
        [DefaultCommand]
        public void AdjuestLevel(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var nLevel = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            int nOLevel;
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                nOLevel = m_PlayObject.m_Abil.Level;
                m_PlayObject.HasLevelUp(1);
                M2Share.AddGameDataLog("17" + "\09" + m_PlayObject.m_sMapName + "\09" + m_PlayObject.m_nCurrX + "\09" + m_PlayObject.m_nCurrY + "\09"
                    + m_PlayObject.m_sCharName + "\09" + m_PlayObject.m_Abil.Level + "\09" + PlayObject.m_sCharName + "\09" + "+(" + nLevel + ")" + "\09" + "0");
                PlayObject.SysMsg(sHumanName + " 等级调整完成。", MsgColor.Green, MsgType.Hint);
                if (M2Share.g_Config.boShowMakeItemMsg)
                {
                    M2Share.MainOutMessage("[等级调整] " + PlayObject.m_sCharName + "(" + m_PlayObject.m_sCharName + " " + nOLevel + " -> " + m_PlayObject.m_Abil.Level + ")");
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, new string[] { sHumanName }), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}