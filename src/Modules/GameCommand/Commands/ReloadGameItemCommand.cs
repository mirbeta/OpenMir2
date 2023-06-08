using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 重新加载物品数据库
    /// </summary>
    [Command("ReloadItemDB", "重新加载物品数据库", 10)]

    public class ReloadGameItemCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            M2Share.CommonDb.LoadItemsDB();
            playObject.SysMsg("物品数据库重新加载完成。", MsgColor.Green, MsgType.Hint);
        }
    }
}