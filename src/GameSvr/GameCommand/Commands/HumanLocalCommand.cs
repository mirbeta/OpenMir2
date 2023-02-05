using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 查看指定玩家所在IP地址
    /// </summary>
    [Command("HumanLocal", "查看指定玩家所在IP地址", CommandHelp.GameCommandHumanLocalHelpMsg, 10)]
    public class HumanLocalCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string m_sIPLocal = "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            // GetIPLocal(PlayObject.m_sIPaddr)
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandHumanLocalMsg, sHumanName, m_sIPLocal), MsgColor.Green, MsgType.Hint);
        }
    }
}