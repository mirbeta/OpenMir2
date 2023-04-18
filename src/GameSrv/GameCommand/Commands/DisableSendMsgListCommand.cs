using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("DisableSendMsgList", "", 10)]
    public class DisableSendMsgListCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (M2Share.DisableSendMsgList.Count <= 0) {
                playObject.SysMsg("禁言列表为空!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            playObject.SysMsg("禁言列表:", MsgColor.Blue, MsgType.Hint);
            for (var i = 0; i < M2Share.DisableSendMsgList.Count; i++) {
                //PlayObject.SysMsg(Settings.g_DisableSendMsgList[i], MsgColor.c_Green, MsgType.t_Hint);
            }
        }
    }
}