using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 进入/退出隐身模式(进入模式后别人看不到自己)(支持权限分配)
    /// </summary>
    [GameCommand("ChangeObMode", "进入/退出隐身模式(进入模式后别人看不到自己)", 10)]
    public class ChangeObModeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeObMode(PlayObject PlayObject)
        {
            var boFlag = !PlayObject.m_boObMode;
            if (boFlag)
            {
                PlayObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");// 发送刷新数据到客户端，解决隐身有影子问题
            }
            PlayObject.m_boObMode = boFlag;
            if (PlayObject.m_boObMode)
            {
                PlayObject.SysMsg(M2Share.sObserverMode, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(M2Share.g_sReleaseObserverMode, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}