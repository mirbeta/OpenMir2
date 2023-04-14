using GameSrv.DataSource;
using GameSrv.Player;
using GameSrv.World;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 重新加载管理员列表
    /// </summary>
    [Command("ReLoadAdmin", "重新加载管理员列表", 10)]
    public class ReLoadAdminCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            LocalDb.LoadAdminList();
            WorldServer.SendServerGroupMsg(213, M2Share.ServerIndex, "");
            playObject.SysMsg("管理员列表重新加载成功...", MsgColor.Green, MsgType.Hint);
        }
    }
}