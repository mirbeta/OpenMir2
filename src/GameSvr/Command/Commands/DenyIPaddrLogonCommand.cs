using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 添加IP地址到禁止登录列表
    /// </summary>
    [Command("DenyIPaddrLogon", "添加IP地址到禁止登录列表", "IP地址 是否永久封(0,1)", 10)]
    public class DenyIPaddrLogonCommand : Commond
    {
        [ExecuteCommand]
        public void DenyIPaddrLogon(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sIPaddr = @Params.Length > 0 ? @Params[0] : "";
            var sFixDeny = @Params.Length > 1 ? @Params[3] : "";
            if (sIPaddr == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            try
            {
                if (sFixDeny != "" && sFixDeny[0] == '1')
                {
                    //M2Share.g_DenyIPAddrList.Add(sIPaddr, ((1) as Object));
                    M2Share.SaveDenyIPAddrList();
                    PlayObject.SysMsg(sIPaddr + "已加入禁止登录IP列表", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    //M2Share.g_DenyIPAddrList.Add(sIPaddr, ((0) as Object));
                    PlayObject.SysMsg(sIPaddr + "已加入临时禁止登录IP列表", MsgColor.Green, MsgType.Hint);
                }
            }
            finally
            {
            }
        }
    }
}