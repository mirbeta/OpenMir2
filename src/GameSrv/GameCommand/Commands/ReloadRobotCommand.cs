using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 重新加载机器人脚本
    /// </summary>
    [Command("ReloadRobot", "重新加载机器人脚本", 10)]
    public class ReloadRobotCommand : GameCommand {
        public void Execute(PlayObject playObject) {
            GameShare.RobotMgr.ReLoadRobot();
            playObject.SysMsg("重新加载机器人配置完成...", MsgColor.Green, MsgType.Hint);
        }
    }
}