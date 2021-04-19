using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("DelDenyAccountLogon", "", 10)]
    public class DelDenyAccountLogonCommand : BaseCommond
    {
        [DefaultCommand]
        public void DelDenyAccountLogon(string[] @Params, TPlayObject PlayObject)
        {
            var sAccount = @Params.Length > 0 ? @Params[0] : "";
            var sFixDeny = @Params.Length > 1 ? @Params[1] : "";
            if (sAccount == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 登录帐号", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var boDelete = false;
            for (var i = 0; i < M2Share.g_DenyAccountList.Count; i++)
            {
                //if ((sAccount).ToLower().CompareTo((M2Share.g_DenyAccountList[i]).ToLower()) == 0)
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
                PlayObject.SysMsg(sAccount + "没有被禁止登录。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}