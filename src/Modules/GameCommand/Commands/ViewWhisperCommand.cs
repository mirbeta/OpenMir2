using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 监听指定玩家私聊信息
    /// </summary>
    [Command("ViewWhisper", "监听指定玩家私聊信息", CommandHelp.GameCommandViewWhisperHelpMsg, 10)]
    public class ViewWhisperCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            var sChrName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sChrName) || !string.IsNullOrEmpty(sChrName) && sChrName[1] == '?') {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = ModuleShare.WorldEngine.GetPlayObject(sChrName);
            if (mIPlayerActor != null) {
                if (mIPlayerActor.WhisperHuman == PlayerActor) {
                    mIPlayerActor.WhisperHuman = null;
                    PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandViewWhisperMsg1, sChrName), MsgColor.Green, MsgType.Hint);
                }
                else {
                    mIPlayerActor.WhisperHuman = PlayerActor;
                    PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandViewWhisperMsg2, sChrName), MsgColor.Green, MsgType.Hint);
                }
            }
            else {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sChrName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}