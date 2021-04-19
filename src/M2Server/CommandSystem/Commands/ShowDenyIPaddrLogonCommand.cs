using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("ShowDenyIPaddrLogon", "", 10)]
    public class ShowDenyIPaddrLogonCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowDenyIPaddrLogon(string[] @Params, TPlayObject PlayObject)
        {
            int nCount;
            M2Share.g_DenyIPAddrList.__Lock();
            try
            {
                nCount = M2Share.g_DenyIPAddrList.Count;
                if (M2Share.g_DenyIPAddrList.Count <= 0)
                {
                    PlayObject.SysMsg("禁止登录角色列表为空。", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                if (nCount > 0)
                {
                    for (var i = 0; i < M2Share.g_DenyIPAddrList.Count; i++)
                    {
                        //PlayObject.SysMsg(M2Share.g_DenyIPAddrList[i], TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                }
            }
            finally
            {
                M2Share.g_DenyIPAddrList.UnLock();
            }
        }
    }
}