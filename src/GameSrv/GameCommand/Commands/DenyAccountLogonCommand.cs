using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("DenyAccountLogon", "", "登录帐号 是否永久封(0,1)", 10)]
    public class DenyAccountLogonCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sAccount = @params.Length > 0 ? @params[0] : "";
            var sFixDeny = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sAccount)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            try {
                if (!string.IsNullOrEmpty(sFixDeny) && sFixDeny[0] == '1') {
                    //Settings.g_DenyAccountList.Add(sAccount, ((1) as Object));
                    GameShare.SaveDenyAccountList();
                    playObject.SysMsg(sAccount + "已加入禁止登录帐号列表", MsgColor.Green, MsgType.Hint);
                }
                else {
                    //Settings.g_DenyAccountList.Add(sAccount, ((0) as Object));
                    playObject.SysMsg(sAccount + "已加入临时禁止登录帐号列表", MsgColor.Green, MsgType.Hint);
                }
            }
            finally {
            }
        }
    }
}