using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("DelDenyIPaddrLogon", "", "IP地址", 10)]
    public class DelDenyIPaddrLogonCommand : GameCommand
    {
        [ExecuteCommand]
        public void DelDenyIPaddrLogon(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sIPaddr = @Params.Length > 0 ? @Params[0] : "";
            if (sIPaddr == "")
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            bool boDelete = false;
            try
            {
                for (int i = M2Share.DenyIPAddrList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.DenyIPAddrList.Count <= 0)
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
                    //    PlayObject.SysMsg(sIPaddr + "已从禁止登录IP列表中删除。", TMsgColor.c_Green, TMsgType.t_Hint);
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
                PlayObject.SysMsg(sIPaddr + "没有被禁止登录。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}