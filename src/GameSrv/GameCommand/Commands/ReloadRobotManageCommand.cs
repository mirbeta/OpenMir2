using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("ReloadRobotManage", "重新加载机器人管理列表", 10)]
    public class ReloadRobotManageCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (GameShare.RobotNPC != null) {
                GameShare.RobotNPC.ClearScript();
                GameShare.RobotNPC.LoadNPCScript();
                playObject.SysMsg("重新加载机器人专用脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg("重新加载机器人专用脚本失败...", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}