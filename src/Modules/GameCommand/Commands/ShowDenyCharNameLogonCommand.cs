using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    [Command("ShowDenyChrNameLogon", "", 10)]
    public class ShowDenyChrNameLogonCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            try {
                if (SystemShare.DenyChrNameList.Count <= 0) {
                    PlayerActor.SysMsg("禁止登录角色列表为空。", MsgColor.Green, MsgType.Hint);
                    return;
                }
                for (var i = 0; i < SystemShare.DenyChrNameList.Count; i++) {
                    //PlayerActor.SysMsg(Settings.g_DenyChrNameList[i], MsgColor.c_Green, MsgType.t_Hint);
                }
            }
            finally {
            }
        }
    }
}