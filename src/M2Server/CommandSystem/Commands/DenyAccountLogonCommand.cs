using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("DenyAccountLogon", "", 10)]
    public class DenyAccountLogonCommand : BaseCommond
    {
        [DefaultCommand]
        public void DenyAccountLogon(string[] @Params, TPlayObject PlayObject)
        {
            var sAccount = @Params.Length > 0 ? @Params[0] : "";
            var sFixDeny = @Params.Length > 1 ? @Params[1] : "";

            if (sAccount == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 登录帐号 是否永久封(0,1)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            try
            {
                if (sFixDeny != "" && sFixDeny[0] == '1')
                {
                    //M2Share.g_DenyAccountList.Add(sAccount, ((1) as Object));
                    M2Share.SaveDenyAccountList();
                    PlayObject.SysMsg(sAccount + "已加入禁止登录帐号列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    //M2Share.g_DenyAccountList.Add(sAccount, ((0) as Object));
                    PlayObject.SysMsg(sAccount + "已加入临时禁止登录帐号列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            finally
            {
            }
        }
    }
}