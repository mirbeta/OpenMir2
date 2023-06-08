using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 重新加载机器人脚本
    /// </summary>
    [Command("ReloadRobot", "重新加载机器人脚本", 10)]
    public class ReloadRobotCommand : GameCommand {
        public void Execute(PlayObject playObject) {
            M2Share.RobotMgr.ReLoadRobot();
            playObject.SysMsg("重新加载机器人配置完成...", MsgColor.Green, MsgType.Hint);
        }
    }
}