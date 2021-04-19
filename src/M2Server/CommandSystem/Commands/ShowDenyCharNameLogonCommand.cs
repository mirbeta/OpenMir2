using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("ShowDenyCharNameLogon", "", 10)]
    public class ShowDenyCharNameLogonCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowDenyCharNameLogon(string[] @Params, TPlayObject PlayObject)
        {
            M2Share.g_DenyChrNameList.__Lock();
            try
            {
                if (M2Share.g_DenyChrNameList.Count <= 0)
                {
                    PlayObject.SysMsg("禁止登录角色列表为空。", TMsgColor.c_Green, TMsgType.t_Hint);
                    return;
                }
                for (var i = 0; i < M2Share.g_DenyChrNameList.Count; i++)
                {
                    //PlayObject.SysMsg(M2Share.g_DenyChrNameList[i], TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            finally
            {
                M2Share.g_DenyChrNameList.UnLock();
            }
        }
    }
}