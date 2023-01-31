using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("DenyAccountLogon", "", "登录帐号 是否永久封(0,1)", 10)]
    public class DenyAccountLogonCommand : Command
    {
        [ExecuteCommand]
        public void DenyAccountLogon(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sAccount = @Params.Length > 0 ? @Params[0] : "";
            var sFixDeny = @Params.Length > 1 ? @Params[1] : "";
            if (sAccount == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            try
            {
                if (sFixDeny != "" && sFixDeny[0] == '1')
                {
                    //Settings.g_DenyAccountList.Add(sAccount, ((1) as Object));
                    M2Share.SaveDenyAccountList();
                    PlayObject.SysMsg(sAccount + "已加入禁止登录帐号列表", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    //Settings.g_DenyAccountList.Add(sAccount, ((0) as Object));
                    PlayObject.SysMsg(sAccount + "已加入临时禁止登录帐号列表", MsgColor.Green, MsgType.Hint);
                }
            }
            finally
            {
            }
        }
    }
}