using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 重新加载机器人脚本
    /// </summary>
    [GameCommand("ReloadRobot", "重新加载机器人脚本", 10)]
    public class ReloadRobotCommand : BaseCommond
    {
        public void ReloadRobot(string[] @params, TPlayObject PlayObject)
        {
            M2Share.RobotManage.ReLoadRobot();
            PlayObject.SysMsg("重新加载机器人配置完成...", TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}