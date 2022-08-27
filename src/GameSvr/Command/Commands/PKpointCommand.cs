using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 查看指定玩家PK值
    /// </summary>
    [GameCommand("PKpoint", "查看指定玩家PK值", M2Share.g_sGameCommandPKPointHelpMsg, 10)]
    public class PKpointCommand : BaseCommond
    {
        [DefaultCommand]
        public void PKpoint(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (!string.IsNullOrEmpty(sHumanName))
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
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandPKPointMsg, sHumanName, m_PlayObject.m_nPkPoint), MsgColor.Green, MsgType.Hint);
        }
    }
}