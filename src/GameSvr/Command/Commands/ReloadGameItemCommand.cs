using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 重新加载物品数据库
    /// </summary>
    [GameCommand("ReloadItemDB", "重新加载物品数据库", 10)]

    public class ReloadGameItemCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadMonItems(TPlayObject PlayObject)
        {
            M2Share.CommonDB.LoadItemsDB();
            PlayObject.SysMsg("物品数据库重新加载完成。", MsgColor.Green, MsgType.Hint);
        }
    }
}