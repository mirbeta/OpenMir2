using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家PK值
    /// </summary>
    [GameCommand("IncPkPoint", "调整指定玩家PK值", M2Share.g_sGameCommandIncPkPointHelpMsg, 10)]
    public class IncPkPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void IncPkPoint(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var nPoint = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            m_PlayObject.m_nPkPoint += nPoint;
            m_PlayObject.RefNameColor();
            if (nPoint > 0)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandIncPkPointAddPointMsg, sHumanName, nPoint), MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandIncPkPointDecPointMsg, sHumanName, -nPoint), MsgColor.Green, MsgType.Hint);
            }
        }
    }
}