using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 重新加载物品数据库
    /// </summary>
    [Command("ReloadItemDB", "重新加载物品数据库", 10)]

    public class ReloadGameItemCommand : GameCommand
    {
        [ExecuteCommand]
        public static void ReloadMonItems(PlayObject PlayObject)
        {
            M2Share.CommonDb.LoadItemsDB();
            PlayObject.SysMsg("物品数据库重新加载完成。", MsgColor.Green, MsgType.Hint);
        }
    }
}