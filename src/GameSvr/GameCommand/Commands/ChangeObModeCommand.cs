using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 进入/退出隐身模式(进入模式后别人看不到自己)(支持权限分配)
    /// </summary>
    [Command("ChangeObMode", "进入/退出隐身模式(进入模式后别人看不到自己)", 10)]
    public class ChangeObModeCommand : Command
    {
        [ExecuteCommand]
        public void ChangeObMode(PlayObject PlayObject)
        {
            var boFlag = !PlayObject.ObMode;
            if (boFlag)
            {
                PlayObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");// 发送刷新数据到客户端，解决隐身有影子问题
            }
            PlayObject.ObMode = boFlag;
            if (PlayObject.ObMode)
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