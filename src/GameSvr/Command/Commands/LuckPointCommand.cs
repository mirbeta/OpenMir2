using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 查看指定玩家幸运点
    /// </summary>
    [GameCommand("LuckPoint", "查看指定玩家幸运点", M2Share.g_sGameCommandLuckPointHelpMsg, 10)]
    public class LuckPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void LuckPoint(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sCtr = @Params.Length > 1 ? @Params[1] : "";
            var sPoint = @Params.Length > 2 ? @Params[2] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sCtr == "")
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandLuckPointMsg, sHumanName, PlayObject.m_nBodyLuckLevel, PlayObject.m_dBodyLuck, PlayObject.m_nLuck), MsgColor.Green, MsgType.Hint);
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