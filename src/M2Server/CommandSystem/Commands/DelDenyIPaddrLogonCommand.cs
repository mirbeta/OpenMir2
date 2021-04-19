﻿using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("DelDenyIPaddrLogon", "", 10)]
    public class DelDenyIPaddrLogonCommand : BaseCommond
    {
        [DefaultCommand]
        public void DelDenyIPaddrLogon(string[] @Params, TPlayObject PlayObject)
        {
            var sIPaddr = @Params.Length > 0 ? @Params[0] : "";

            if (sIPaddr == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " IP地址", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var boDelete = false;
            M2Share.g_DenyIPAddrList.__Lock();
            try
            {
                for (var i = M2Share.g_DenyIPAddrList.Count - 1; i >= 0; i--)
                {
                    if (M2Share.g_DenyIPAddrList.Count <= 0)
                    {
                        break;
                    }
                    //if ((sIPaddr).ToLower().CompareTo((M2Share.g_DenyIPAddrList[i]).ToLower()) == 0)
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
                M2Share.g_DenyIPAddrList.UnLock();
            }
            if (!boDelete)
            {
                PlayObject.SysMsg(sIPaddr + "没有被禁止登录。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}