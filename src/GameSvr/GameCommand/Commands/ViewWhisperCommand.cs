using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 监听指定玩家私聊信息
    /// </summary>
    [Command("ViewWhisper", "监听指定玩家私聊信息", CommandHelp.GameCommandViewWhisperHelpMsg, 10)]
    public class ViewWhisperCommand : GameCommand
    {
        [ExecuteCommand]
        public void ViewWhisper(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sChrName = @Params.Length > 0 ? @Params[0] : "";
            if (sChrName == "" || sChrName != "" && sChrName[1] == '?')
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sChrName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.WhisperHuman == PlayObject)
                {
                    m_PlayObject.WhisperHuman = null;
                    PlayObject.SysMsg(string.Format(CommandHelp.GameCommandViewWhisperMsg1, sChrName), MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    m_PlayObject.WhisperHuman = PlayObject;
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