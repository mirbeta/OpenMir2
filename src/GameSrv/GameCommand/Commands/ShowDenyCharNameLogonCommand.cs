using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("ShowDenyChrNameLogon", "", 10)]
    public class ShowDenyChrNameLogonCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            try {
                if (GameShare.DenyChrNameList.Count <= 0) {
                    playObject.SysMsg("禁止登录角色列表为空。", MsgColor.Green, MsgType.Hint);
                    return;
                }
                for (var i = 0; i < GameShare.DenyChrNameList.Count; i++) {
                    //PlayObject.SysMsg(Settings.g_DenyChrNameList[i], MsgColor.c_Green, MsgType.t_Hint);
                }
            }
            finally {
            }
        }
    }
}