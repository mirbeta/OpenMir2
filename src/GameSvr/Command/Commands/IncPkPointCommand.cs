using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家PK值
    /// </summary>
    [GameCommand("IncPkPoint", "调整指定玩家PK值", GameCommandConst.GameCommandIncPkPointHelpMsg, 10)]
    public class IncPkPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void IncPkPoint(string[] @Params, PlayObject PlayObject)
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
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            m_PlayObject.PkPoint += nPoint;
            m_PlayObject.RefNameColor();
            if (nPoint > 0)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandIncPkPointAddPointMsg, sHumanName, nPoint), MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandIncPkPointDecPointMsg, sHumanName, -nPoint), MsgColor.Green, MsgType.Hint);
            }
        }
    }
}