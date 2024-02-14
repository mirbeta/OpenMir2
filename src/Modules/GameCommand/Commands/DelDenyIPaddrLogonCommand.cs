using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("DelDenyIPaddrLogon", "", "IP地址", 10)]
    public class DelDenyIPaddrLogonCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sIPaddr = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sIPaddr))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            bool boDelete = false;
            try
            {
                for (int i = SystemShare.DenyIPAddrList.Count - 1; i >= 0; i--)
                {
                    if (SystemShare.DenyIPAddrList.Count <= 0)
                    {
                        break;
                    }
                    //if ((sIPaddr).CompareTo((Settings.g_DenyIPAddrList[i])) == 0)
                    //{
                    //    //if (((int)Settings.g_DenyIPAddrList[i]) != 0)
                    //    //{
                    //    //    M2Share.SaveDenyIPAddrList();
                    //    //}
                    //    Settings.g_DenyIPAddrList.RemoveAt(i);
                    //    PlayerActor.SysMsg(sIPaddr + "已从禁止登录IP列表中删除。", MsgColor.c_Green, MsgType.t_Hint);
                    //    boDelete = true;
                    //    break;
                    //}
                }
            }
            finally
            {
            }
            if (!boDelete)
            {
                PlayerActor.SysMsg(sIPaddr + "没有被禁止登录。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}