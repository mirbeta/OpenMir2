using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("DelDenyAccountLogon", "", "登录帐号", 10)]
    public class DelDenyAccountLogonCommand : GameCommand
    {
        [ExecuteCommand]
        public void DelDenyAccountLogon(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sAccount = @Params.Length > 0 ? @Params[0] : "";
            string sFixDeny = @Params.Length > 1 ? @Params[1] : "";
            if (sAccount == "")
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            bool boDelete = false;
            for (int i = 0; i < M2Share.DenyAccountList.Count; i++)
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