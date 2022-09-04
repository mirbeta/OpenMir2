using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 重新加载机器人脚本
    /// </summary>
    [GameCommand("ReloadRobot", "重新加载机器人脚本", 10)]
    public class ReloadRobotCommand : BaseCommond
    {
        public void ReloadRobot(PlayObject PlayObject)
        {
            M2Share.RobotMgr.ReLoadRobot();
            PlayObject.SysMsg("重新加载机器人配置完成...", MsgColor.Green, MsgType.Hint);
        }
    }
}