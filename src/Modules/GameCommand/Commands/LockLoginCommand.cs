using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 锁定登录
    /// </summary>
    [Command("LockLogin", "锁定登录")]
    public class LockLoginCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (!SystemShare.Config.LockHumanLogin)
            {
                PlayerActor.SysMsg("本服务器还没有启用登录锁功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.IsLockLogon && !PlayerActor.IsLockLogoned)
            {
                PlayerActor.SysMsg("您还没有打开登录锁或还没有设置锁密码!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayerActor.IsLockLogon = !PlayerActor.IsLockLogon;
            if (PlayerActor.IsLockLogon)
            {
                PlayerActor.SysMsg("已开启登录锁", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg("已关闭登录锁", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}