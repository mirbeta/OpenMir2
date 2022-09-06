using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 重新加载怪物爆率文件
    /// </summary>
    [GameCommand("ReloadMonItems", "重新加载怪物爆率文件", 10)]
    public class ReloadMonItemsCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadMonItems(PlayObject PlayObject)
        {
            try
            {
                for (var i = 0; i < M2Share.UserEngine.MonsterList.Count; i++)
                {
                    var Monster = M2Share.UserEngine.MonsterList[i];
                    M2Share.LocalDb.LoadMonitems(Monster.sName, ref Monster.ItemList);
                }
                PlayObject.SysMsg("怪物爆物品列表重加载完成...", MsgColor.Green, MsgType.Hint);
            }
            catch
            {
                PlayObject.SysMsg("怪物爆物品列表重加载失败!!!", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}