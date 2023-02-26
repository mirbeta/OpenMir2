using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 添加IP地址到禁止登录列表
    /// </summary>
    [Command("DenyIPaddrLogon", "添加IP地址到禁止登录列表", "IP地址 是否永久封(0,1)", 10)]
    public class DenyIPaddrLogonCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sIPaddr = @Params.Length > 0 ? @Params[0] : "";
            string sFixDeny = @Params.Length > 1 ? @Params[3] : "";
            if (string.IsNullOrEmpty(sIPaddr)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            try {
                if (!string.IsNullOrEmpty(sFixDeny) && sFixDeny[0] == '1') {
                    //Settings.g_DenyIPAddrList.Add(sIPaddr, ((1) as Object));
                    M2Share.SaveDenyIPAddrList();
                    PlayObject.SysMsg(sIPaddr + "已加入禁止登录IP列表", MsgColor.Green, MsgType.Hint);
                }
                else {
                    //Settings.g_DenyIPAddrList.Add(sIPaddr, ((0) as Object));
                    PlayObject.SysMsg(sIPaddr + "已加入临时禁止登录IP列表", MsgColor.Green, MsgType.Hint);
                }
            }
            finally {
            }
        }
    }
}