using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [Command("ShowDenyIPaddrLogon", "", 10)]
    public class ShowDenyIPaddrLogonCommand : Command
    {
        [ExecuteCommand]
        public void ShowDenyIPaddrLogon(PlayObject PlayObject)
        {
            int nCount;
            try
            {
                nCount = M2Share.g_DenyIPAddrList.Count;
                if (M2Share.g_DenyIPAddrList.Count <= 0)
                {
                    PlayObject.SysMsg("禁止登录角色列表为空。", MsgColor.Green, MsgType.Hint);
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
            }
        }
    }
}