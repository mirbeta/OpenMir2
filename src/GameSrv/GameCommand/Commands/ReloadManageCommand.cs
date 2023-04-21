using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("ReloadManage", "重新加载脚本", 10)]
    public class ReloadManageCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (GameShare.ManageNPC != null) {
                GameShare.ManageNPC.ClearScript();
                GameShare.ManageNPC.LoadNPCScript();
                playObject.SysMsg("重新加载登录脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg("重新加载登录脚本失败...", MsgColor.Green, MsgType.Hint);
            }
            if (GameShare.FunctionNPC != null) {
                GameShare.FunctionNPC.ClearScript();
                GameShare.FunctionNPC.LoadNPCScript();
                playObject.SysMsg("重新加载功能脚本完成...", MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg("重新加载功能脚本失败...", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}