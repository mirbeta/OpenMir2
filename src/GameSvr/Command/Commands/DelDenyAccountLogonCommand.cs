using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("DelDenyAccountLogon", "", "登录帐号", 10)]
    public class DelDenyAccountLogonCommand : BaseCommond
    {
        [DefaultCommand]
        public void DelDenyAccountLogon(string[] @Params, PlayObject PlayObject)
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
            var boDelete = false;
            for (var i = 0; i < M2Share.g_DenyAccountList.Count; i++)
            {
                //if ((sAccount).CompareTo((M2Share.g_DenyAccountList[i])) == 0)
                //{
                //    //if (((int)M2Share.g_DenyAccountList[i]) != 0)
                //    //{
                //    //    M2Share.SaveDenyAccountList();
                //    //}
                //    M2Share.g_DenyAccountList.RemoveAt(i);
                //    PlayObject.SysMsg(sAccount + "已从禁止登录帐号列表中删除。", TMsgColor.c_Green, TMsgType.t_Hint);
                //    boDelete = true;
                //    break;
                //}
            }
            if (!boDelete)
            {
                PlayObject.SysMsg(sAccount + "没有被禁止登录。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}