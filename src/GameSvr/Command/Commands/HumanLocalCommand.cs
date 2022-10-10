using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 查看指定玩家所在IP地址
    /// </summary>
    [Command("HumanLocal", "查看指定玩家所在IP地址", CommandHelp.GameCommandHumanLocalHelpMsg, 10)]
    public class HumanLocalCommand : Commond
    {
        [ExecuteCommand]
        public void HumanLocal(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var m_sIPLocal = "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
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