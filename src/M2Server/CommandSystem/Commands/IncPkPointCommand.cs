using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 调整指定玩家PK值
    /// </summary>
    [GameCommand("IncPkPoint", "调整指定玩家PK值", 10)]
    public class IncPkPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void IncPkPoint(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var nPoint = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (sHumanName != "" && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandIncPkPointHelpMsg),
                    TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_PlayObject.m_nPkPoint += nPoint;
            m_PlayObject.RefNameColor();
            if (nPoint > 0)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandIncPkPointAddPointMsg, sHumanName, nPoint), TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandIncPkPointDecPointMsg, sHumanName, -nPoint), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}