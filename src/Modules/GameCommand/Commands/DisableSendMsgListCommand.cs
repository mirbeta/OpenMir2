using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("DisableSendMsgList", "", 10)]
    public class DisableSendMsgListCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (SystemShare.DisableSendMsgList.Count <= 0)
            {
                PlayerActor.SysMsg("禁言列表为空!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayerActor.SysMsg("禁言列表:", MsgColor.Blue, MsgType.Hint);
            for (int i = 0; i < SystemShare.DisableSendMsgList.Count; i++)
            {
                //PlayerActor.SysMsg(Settings.g_DisableSendMsgList[i], MsgColor.c_Green, MsgType.t_Hint);
            }
        }
    }
}