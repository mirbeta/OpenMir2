using SystemModule;
using SystemModule.Actors;
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
                nCount = SystemShare.DenyIPAddrList.Count;
                if (SystemShare.DenyIPAddrList.Count <= 0)
                {
                    PlayerActor.SysMsg("禁止登录角色列表为空。", MsgColor.Green, MsgType.Hint);
                }
                if (nCount > 0)
                {
                    for (int i = 0; i < SystemShare.DenyIPAddrList.Count; i++)
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