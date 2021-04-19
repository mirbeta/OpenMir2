using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 查看指定玩家幸运点
    /// </summary>
    [GameCommand("LuckPoint", "查看指定玩家幸运点", 10)]
    public class LuckPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void LuckPoint(string[] @Params, TPlayObject PlayObject)
        {
            var nPermission = @Params.Length > 0 ? Convert.ToInt32(@Params[0]) : 0;
            var sHumanName = @Params.Length > 1 ? @Params[1] : "";
            var sCtr = @Params.Length > 2 ? @Params[2] : "";
            var sPoint = @Params.Length > 3 ? @Params[3] : "";
            if (PlayObject.m_btPermission < nPermission)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if ((sHumanName == "") || ((sHumanName != "") && (sHumanName[0] == '?')))
            {
                PlayObject.SysMsg(
                    string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name,
                        M2Share.g_sGameCommandLuckPointHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red,
                    TMsgType.t_Hint);
                return;
            }
            if (sCtr == "")
            {
                PlayObject.SysMsg(
                    string.Format(M2Share.g_sGameCommandLuckPointMsg, sHumanName, PlayObject.m_nBodyLuckLevel,
                        PlayObject.m_dBodyLuck, PlayObject.m_nLuck), TMsgColor.c_Green, TMsgType.t_Hint);
                return;
            }
            var nPoint = HUtil32.Str_ToInt(sPoint, 0);
            var cMethod = sCtr[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.m_nLuck = nPoint;
                    break;
                case '-':
                    if (PlayObject.m_nLuck >= nPoint)
                    {
                        PlayObject.m_nLuck -= nPoint;
                    }
                    else
                    {
                        PlayObject.m_nLuck = 0;
                    }
                    break;
                case '+':
                    PlayObject.m_nLuck += nPoint;
                    break;
            }
        }
    }
}