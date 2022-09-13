using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 查看指定玩家PK值
    /// </summary>
    [GameCommand("PKpoint", "查看指定玩家PK值", GameCommandConst.GameCommandPKPointHelpMsg, 10)]
    public class PKpointCommand : BaseCommond
    {
        [DefaultCommand]
        public void PKpoint(string[] @Params, PlayObject PlayObject)
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
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandPKPointMsg, sHumanName, m_PlayObject.PkPoint), MsgColor.Green, MsgType.Hint);
        }
    }
}