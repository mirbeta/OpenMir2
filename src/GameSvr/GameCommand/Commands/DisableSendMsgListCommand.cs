using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("DisableSendMsgList", "", 10)]
    public class DisableSendMsgListCommand : Command
    {
        [ExecuteCommand]
        public static void DisableSendMsgList(PlayObject PlayObject)
        {
            if (M2Share.g_DisableSendMsgList.Count <= 0)
            {
                PlayObject.SysMsg("禁言列表为空!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg("禁言列表:", MsgColor.Blue, MsgType.Hint);
            for (var i = 0; i < M2Share.g_DisableSendMsgList.Count; i++)
            {
                //PlayObject.SysMsg(M2Share.g_DisableSendMsgList[i], TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}