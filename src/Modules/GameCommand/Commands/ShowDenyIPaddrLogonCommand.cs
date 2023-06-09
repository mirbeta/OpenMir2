using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("ShowDenyIPaddrLogon", "", 10)]
    public class ShowDenyIPaddrLogonCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            int nCount;
            try
            {
                nCount = ModuleShare.DenyIPAddrList.Count;
                if (ModuleShare.DenyIPAddrList.Count <= 0)
                {
                    PlayerActor.SysMsg("禁止登录角色列表为空。", MsgColor.Green, MsgType.Hint);
                }
                if (nCount > 0)
                {
                    for (var i = 0; i < ModuleShare.DenyIPAddrList.Count; i++)
                    {
                        //PlayerActor.SysMsg(Settings.g_DenyIPAddrList[i], MsgColor.c_Green, MsgType.t_Hint);
                    }
                }
            }
            finally
            {
            }
        }
    }
}