using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("ShowDenyChrNameLogon", "", 10)]
    public class ShowDenyChrNameLogonCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            try {
                if (M2Share.DenyChrNameList.Count <= 0) {
                    playObject.SysMsg("禁止登录角色列表为空。", MsgColor.Green, MsgType.Hint);
                    return;
                }
                for (int i = 0; i < M2Share.DenyChrNameList.Count; i++) {
                    //PlayObject.SysMsg(Settings.g_DenyChrNameList[i], TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            finally {
            }
        }
    }
}