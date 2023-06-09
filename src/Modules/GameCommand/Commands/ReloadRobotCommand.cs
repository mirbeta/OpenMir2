using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 重新加载机器人脚本
    /// </summary>
    [Command("ReloadRobot", "重新加载机器人脚本", 10)]
    public class ReloadRobotCommand : GameCommand
    {
        public void Execute(IPlayerActor PlayerActor)
        {
            //M2Share.RobotMgr.ReLoadRobot();
            PlayerActor.SysMsg("重新加载机器人配置完成...", MsgColor.Green, MsgType.Hint);
        }
    }
}