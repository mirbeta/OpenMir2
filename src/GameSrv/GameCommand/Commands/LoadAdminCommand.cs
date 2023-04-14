using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 重新加载管理员列表
    /// </summary>
    [Command("LoadAdmin", "重新加载管理员列表", 10)]
    public class LoadAdminCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (playObject.Permission < 6) {
                return;
            }
            //LocalDB.GetInstance().LoadAdminList();
            // UserEngine.SendServerGroupMsg(213, nServerIndex, '');
            playObject.SysMsg("管理员列表重新加载成功...", MsgColor.Green, MsgType.Hint);
        }
    }
}