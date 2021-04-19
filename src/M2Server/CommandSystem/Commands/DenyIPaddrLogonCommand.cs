using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 添加IP地址到禁止登录列表
    /// </summary>
    [GameCommand("DenyIPaddrLogon", "添加IP地址到禁止登录列表", 10)]
    public class DenyIPaddrLogonCommand : BaseCommond
    {
        [DefaultCommand]
        public void DenyIPaddrLogon(string[] @Params, TPlayObject PlayObject)
        {
            var sIPaddr = @Params.Length > 0 ? @Params[0] : "";
            var sFixDeny = @Params.Length > 1 ? @Params[3] : "";

            if (sIPaddr == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " IP地址 是否永久封(0,1)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.g_DenyIPAddrList.__Lock();
            try
            {
                if ((sFixDeny != "") && (sFixDeny[0] == '1'))
                {
                    //M2Share.g_DenyIPAddrList.Add(sIPaddr, ((1) as Object));
                    M2Share.SaveDenyIPAddrList();
                    PlayObject.SysMsg(sIPaddr + "已加入禁止登录IP列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    //M2Share.g_DenyIPAddrList.Add(sIPaddr, ((0) as Object));
                    PlayObject.SysMsg(sIPaddr + "已加入临时禁止登录IP列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            finally
            {
                M2Share.g_DenyIPAddrList.UnLock();
            }
        }
    }
}