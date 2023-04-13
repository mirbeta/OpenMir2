using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("DelDenyAccountLogon", "", "登录帐号", 10)]
    public class DelDenyAccountLogonCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sAccount = @params.Length > 0 ? @params[0] : "";
            string sFixDeny = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sAccount)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            bool boDelete = false;
            for (int i = 0; i < M2Share.DenyAccountList.Count; i++) {
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
            if (!boDelete) {
                playObject.SysMsg(sAccount + "没有被禁止登录。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}