using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("DelDenyIPaddrLogon", "", "IP地址", 10)]
    public class DelDenyIPaddrLogonCommand : BaseCommond
    {
        [DefaultCommand]
        public void DelDenyIPaddrLogon(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sIPaddr = @Params.Length > 0 ? @Params[0] : "";
            if (sIPaddr == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var boDelete = false;
            try
            {
                for (var i = M2Share.g_DenyIPAddrList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.g_DenyIPAddrList.Count <= 0)
                    {
                        break;
                    }
                    //if ((sIPaddr).CompareTo((M2Share.g_DenyIPAddrList[i])) == 0)
                    //{
                    //    //if (((int)M2Share.g_DenyIPAddrList[i]) != 0)
                    //    //{
                    //    //    M2Share.SaveDenyIPAddrList();
                    //    //}
                    //    M2Share.g_DenyIPAddrList.RemoveAt(i);
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