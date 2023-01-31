using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 剔除指定玩家下线
    /// </summary>
    [Command("KickHuman", "剔除指定玩家下线", CommandHelp.GameCommandHumanLocalHelpMsg, 10)]
    public class KickHumanCommand : Command
    {
        [ExecuteCommand]
        public static void KickHuman(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            string sHumName = @params.Length > 0 ? @params[0] : "";
            if (sHumName == "")
            {
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                m_PlayObject.BoKickFlag = true;
                m_PlayObject.BoEmergencyClose = true;
                //m_PlayObject.m_boPlayOffLine = false;
                //m_PlayObject.m_boNotOnlineAddExp = false;
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}