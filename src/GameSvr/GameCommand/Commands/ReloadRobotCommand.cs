using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 重新加载机器人脚本
    /// </summary>
    [Command("ReloadRobot", "重新加载机器人脚本", 10)]
    public class ReloadRobotCommand : Command
    {
        public static void ReloadRobot(PlayObject PlayObject)
        {
            M2Share.RobotMgr.ReLoadRobot();
            PlayObject.SysMsg("重新加载机器人配置完成...", MsgColor.Green, MsgType.Hint);
        }
    }
}