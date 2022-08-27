using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家经验值
    /// </summary>
    [GameCommand("AdjuestExp", "调整指定人物的经验值", "物名称 经验值", 10)]
    public class AdjuestExpCommand : BaseCommond
    {
        [DefaultCommand]
        public void AdjuestExp(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sExp = @Params.Length > 1 ? @Params[1] : "";
            var dwExp = 0;
            var dwOExp = 0;
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            dwExp = HUtil32.Str_ToInt(sExp, 0);
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                dwOExp = PlayObject.m_Abil.Exp;
                m_PlayObject.m_Abil.Exp = dwExp;
                m_PlayObject.HasLevelUp(m_PlayObject.m_Abil.Level - 1);
                PlayObject.SysMsg(sHumanName + " 经验调整完成。", MsgColor.Green, MsgType.Hint);
                if (M2Share.g_Config.boShowMakeItemMsg)
                {
                    M2Share.MainOutMessage("[经验调整] " + PlayObject.m_sCharName + '(' + m_PlayObject.m_sCharName + ' ' + dwOExp + " -> " + m_PlayObject.m_Abil.Exp + ')');
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}