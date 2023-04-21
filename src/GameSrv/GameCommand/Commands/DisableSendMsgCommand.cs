using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("DisableSendMsg", "", "人物名称", 10)]
    public class DisableSendMsgCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = GameShare.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null) {
                mPlayObject.FilterSendMsg = true;
            }
            GameShare.DisableSendMsgList.Add(sHumanName);
            GameShare.SaveDisableSendMsgList();
            playObject.SysMsg(sHumanName + " 已加入禁言列表。", MsgColor.Green, MsgType.Hint);
        }
    }
}