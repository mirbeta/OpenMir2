using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("DenyAccountLogon", "", "登录帐号 是否永久封(0,1)", 10)]
    public class DenyAccountLogonCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sAccount = @Params.Length > 0 ? @Params[0] : "";
            string sFixDeny = @Params.Length > 1 ? @Params[1] : "";
            if (string.IsNullOrEmpty(sAccount)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            try {
                if (!string.IsNullOrEmpty(sFixDeny) && sFixDeny[0] == '1') {
                    //Settings.g_DenyAccountList.Add(sAccount, ((1) as Object));
                    M2Share.SaveDenyAccountList();
                    PlayObject.SysMsg(sAccount + "已加入禁止登录帐号列表", MsgColor.Green, MsgType.Hint);
                }
                else {
                    //Settings.g_DenyAccountList.Add(sAccount, ((0) as Object));
                    PlayObject.SysMsg(sAccount + "已加入临时禁止登录帐号列表", MsgColor.Green, MsgType.Hint);
                }
            }
            finally {
            }
        }
    }
}