using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("ShowDenyAccountLogon", "", 10)]
    public class ShowDenyAccountLogonCommand : Command
    {
        [ExecuteCommand]
        public void ShowDenyAccountLogon(PlayObject PlayObject)
        {
            if (PlayObject.Permission < 6)
            {
                return;
            }
            try
            {
                if (M2Share.g_DenyAccountList.Count <= 0)
                {
                    PlayObject.SysMsg("禁止登录帐号列表为空。", MsgColor.Green, MsgType.Hint);
                    return;
                }
                for (var i = 0; i < M2Share.g_DenyAccountList.Count; i++)
                {
                    //PlayObject.SysMsg(M2Share.g_DenyAccountList[i], TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            finally
            {
            }
        }
    }
}