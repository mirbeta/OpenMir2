using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 添加IP地址到禁止登录列表
    /// </summary>
    [Command("DenyIPaddrLogon", "添加IP地址到禁止登录列表", "IP地址 是否永久封(0,1)", 10)]
    public class DenyIPaddrLogonCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sIPaddr = @params.Length > 0 ? @params[0] : "";
            string sFixDeny = @params.Length > 1 ? @params[3] : "";
            if (string.IsNullOrEmpty(sIPaddr)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            try {
                if (!string.IsNullOrEmpty(sFixDeny) && sFixDeny[0] == '1') {
                    //Settings.g_DenyIPAddrList.Add(sIPaddr, ((1) as Object));
                    M2Share.SaveDenyIPAddrList();
                    playObject.SysMsg(sIPaddr + "已加入禁止登录IP列表", MsgColor.Green, MsgType.Hint);
                }
                else {
                    //Settings.g_DenyIPAddrList.Add(sIPaddr, ((0) as Object));
                    playObject.SysMsg(sIPaddr + "已加入临时禁止登录IP列表", MsgColor.Green, MsgType.Hint);
                }
            }
            finally {
            }
        }
    }
}