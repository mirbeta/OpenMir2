using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    [GameCommand("DenyAccountLogon", "", "登录帐号 是否永久封(0,1)", 10)]
    public class DenyAccountLogonCommand : BaseCommond
    {
        [DefaultCommand]
        public void DenyAccountLogon(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sAccount = @Params.Length > 0 ? @Params[0] : "";
            var sFixDeny = @Params.Length > 1 ? @Params[1] : "";
            if (sAccount == "")
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            try
            {
                if (sFixDeny != "" && sFixDeny[0] == '1')
                {
                    //M2Share.g_DenyAccountList.Add(sAccount, ((1) as Object));
                    M2Share.SaveDenyAccountList();
                    PlayObject.SysMsg(sAccount + "已加入禁止登录帐号列表", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    //M2Share.g_DenyAccountList.Add(sAccount, ((0) as Object));
                    PlayObject.SysMsg(sAccount + "已加入临时禁止登录帐号列表", MsgColor.Green, MsgType.Hint);
                }
            }
            finally
            {
            }
        }
    }
}