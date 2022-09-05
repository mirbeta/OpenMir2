using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 此命令用于允许或禁止编组传送功能
    /// </summary>
    [GameCommand("AllowGroupReCall", "此命令用于允许或禁止编组传送功能", 0)]
    public class AllowGroupReCallCommand : BaseCommond
    {
        [DefaultCommand]
        public void AllowGroupReCall(PlayObject PlayObject)
        {
            PlayObject.MBoAllowGroupReCall = !PlayObject.MBoAllowGroupReCall;
            if (PlayObject.MBoAllowGroupReCall)
            {
                PlayObject.SysMsg(M2Share.g_sEnableGroupRecall, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(M2Share.g_sDisableGroupRecall, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}