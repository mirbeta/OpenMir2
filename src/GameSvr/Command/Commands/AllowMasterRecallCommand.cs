using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 此命令用于允许或禁止师徒传送
    /// </summary>
    [GameCommand("AllowMasterRecall", "此命令用于允许或禁止师徒传送", 0)]
    public class AllowMasterRecallCommand : BaseCommond
    {
        [DefaultCommand]
        public void AllowMasterRecall(PlayObject PlayObject)
        {
            PlayObject.m_boCanMasterRecall = !PlayObject.m_boCanMasterRecall;
            if (PlayObject.m_boCanMasterRecall)
            {
                PlayObject.SysMsg(M2Share.g_sEnableMasterRecall, MsgColor.Blue, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(M2Share.g_sDisableMasterRecall, MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}