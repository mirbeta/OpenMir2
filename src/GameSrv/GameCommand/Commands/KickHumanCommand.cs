using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 剔除指定玩家下线
    /// </summary>
    [Command("KickHuman", "剔除指定玩家下线", CommandHelp.GameCommandHumanLocalHelpMsg, 10)]
    public class KickHumanCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumName)) {
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (mPlayObject != null) {
                mPlayObject.BoKickFlag = true;
                mPlayObject.BoEmergencyClose = true;
                //m_PlayObject.m_boPlayOffLine = false;
                //m_PlayObject.m_boNotOnlineAddExp = false;
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}