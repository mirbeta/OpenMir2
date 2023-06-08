using M2Server.Player;
using M2Server;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 进入/退出隐身模式(进入模式后别人看不到自己)(支持权限分配)
    /// </summary>
    [Command("ChangeObMode", "进入/退出隐身模式(进入模式后别人看不到自己)", 10)]
    public class ChangeObModeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            var boFlag = !playObject.ObMode;
            if (boFlag) {
                playObject.SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");// 发送刷新数据到客户端，解决隐身有影子问题
            }
            playObject.ObMode = boFlag;
            if (playObject.ObMode) {
                playObject.SysMsg(Settings.ObserverMode, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(Settings.ReleaseObserverMode, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}