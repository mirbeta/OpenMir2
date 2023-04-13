using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 锁定登录
    /// </summary>
    [Command("LockLogin", "锁定登录")]
    public class LockLoginCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (!M2Share.Config.LockHumanLogin) {
                playObject.SysMsg("本服务器还没有启用登录锁功能!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.IsLockLogon && !playObject.IsLockLogoned) {
                playObject.SysMsg("您还没有打开登录锁或还没有设置锁密码!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            playObject.IsLockLogon = !playObject.IsLockLogon;
            if (playObject.IsLockLogon) {
                playObject.SysMsg("已开启登录锁", MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg("已关闭登录锁", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}