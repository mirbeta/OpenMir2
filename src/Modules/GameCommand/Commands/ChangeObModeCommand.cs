using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 进入/退出隐身模式(进入模式后别人看不到自己)(支持权限分配)
    /// </summary>
    [Command("ChangeObMode", "进入/退出隐身模式(进入模式后别人看不到自己)", 10)]
    public class ChangeObModeCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            var boFlag = !PlayerActor.ObMode;
            if (boFlag)
            {
                PlayerActor.SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");// 发送刷新数据到客户端，解决隐身有影子问题
            }
            PlayerActor.ObMode = boFlag;
            if (PlayerActor.ObMode)
            {
                PlayerActor.SysMsg(Settings.ObserverMode, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(Settings.ReleaseObserverMode, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}