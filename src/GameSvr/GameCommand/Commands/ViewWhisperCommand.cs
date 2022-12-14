using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 监听指定玩家私聊信息
    /// </summary>
    [Command("ViewWhisper", "监听指定玩家私聊信息", CommandHelp.GameCommandViewWhisperHelpMsg, 10)]
    public class ViewWhisperCommand : Command
    {
        [ExecuteCommand]
        public void ViewWhisper(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sChrName = @Params.Length > 0 ? @Params[0] : "";
            if (sChrName == "" || sChrName != "" && sChrName[1] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sChrName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.MGetWhisperHuman == PlayObject)
                {
                    m_PlayObject.MGetWhisperHuman = null;
                    PlayObject.SysMsg(string.Format(CommandHelp.GameCommandViewWhisperMsg1, sChrName), MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    m_PlayObject.MGetWhisperHuman = PlayObject;
                    PlayObject.SysMsg(string.Format(CommandHelp.GameCommandViewWhisperMsg2, sChrName), MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sChrName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}