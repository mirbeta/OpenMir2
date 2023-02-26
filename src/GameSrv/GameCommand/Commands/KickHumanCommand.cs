using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 剔除指定玩家下线
    /// </summary>
    [Command("KickHuman", "剔除指定玩家下线", CommandHelp.GameCommandHumanLocalHelpMsg, 10)]
    public class KickHumanCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject PlayObject) {
            if (@params == null) {
                return;
            }
            string sHumName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumName)) {
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null) {
                m_PlayObject.BoKickFlag = true;
                m_PlayObject.BoEmergencyClose = true;
                //m_PlayObject.m_boPlayOffLine = false;
                //m_PlayObject.m_boNotOnlineAddExp = false;
            }
            else {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}