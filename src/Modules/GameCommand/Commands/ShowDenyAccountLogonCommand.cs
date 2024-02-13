using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("ShowDenyAccountLogon", "", 10)]
    public class ShowDenyAccountLogonCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (PlayerActor.Permission < 6)
            {
                return;
            }
            try
            {
                if (SystemShare.DenyAccountList.Count <= 0)
                {
                    PlayerActor.SysMsg("禁止登录帐号列表为空。", MsgColor.Green, MsgType.Hint);
                    return;
                }
                for (var i = 0; i < SystemShare.DenyAccountList.Count; i++)
                {
                    //PlayerActor.SysMsg(Settings.g_DenyAccountList[i], MsgColor.c_Green, MsgType.t_Hint);
                }
            }
            finally
            {
            }
        }
    }
}