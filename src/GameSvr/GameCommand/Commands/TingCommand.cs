using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 将指定人物随机传送(支持权限分配)
    /// </summary>
    [Command("Ting", "将指定人物随机传送(支持权限分配)", CommandHelp.GameCommandTingHelpMsg, 10)]
    public class TingCommand : GameCommand
    {
        [ExecuteCommand]
        public void Ting(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                m_PlayObject.MapRandomMove(m_PlayObject.HomeMap, 0);
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}