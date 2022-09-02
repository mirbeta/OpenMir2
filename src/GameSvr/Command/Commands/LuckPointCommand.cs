using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家幸运点
    /// </summary>
    [GameCommand("LuckPoint", "查看指定玩家幸运点", GameCommandConst.g_sGameCommandLuckPointHelpMsg, 10)]
    public class LuckPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void LuckPoint(string[] @Params, PlayObject PlayObject)
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
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (sCtr == "")
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandLuckPointMsg, sHumanName, mPlayObject.m_nBodyLuckLevel, mPlayObject.m_dBodyLuck, mPlayObject.m_nLuck), MsgColor.Green, MsgType.Hint);
                return;
            }
            var nPoint = HUtil32.Str_ToInt(sPoint, 0);
            var cMethod = sCtr[0];
            switch (cMethod)
            {
                case '=':
                    mPlayObject.m_nLuck = nPoint;
                    break;
                case '-':
                    if (mPlayObject.m_nLuck >= nPoint)
                    {
                        mPlayObject.m_nLuck -= nPoint;
                    }
                    else
                    {
                        mPlayObject.m_nLuck = 0;
                    }
                    break;
                case '+':
                    mPlayObject.m_nLuck += nPoint;
                    break;
            }
        }
    }
}