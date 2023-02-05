using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("DisableSendMsgList", "", 10)]
    public class DisableSendMsgListCommand : GameCommand
    {
        [ExecuteCommand]
        public static void DisableSendMsgList(PlayObject PlayObject)
        {
            if (M2Share.DisableSendMsgList.Count <= 0)
            {
                PlayObject.SysMsg("禁言列表为空!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg("禁言列表:", MsgColor.Blue, MsgType.Hint);
            for (int i = 0; i < M2Share.DisableSendMsgList.Count; i++)
            {
                //PlayObject.SysMsg(Settings.g_DisableSendMsgList[i], TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}