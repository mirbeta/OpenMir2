using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 重新加载怪物爆率文件
    /// </summary>
    [Command("ReloadMonItems", "重新加载怪物爆率文件", 10)]
    public class ReloadMonItemsCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            try
            {
                //var keyList = M2Share.WorldEngine.MonsterList.Keys.ToList();
                //for (var i = 0; i < keyList.Count; i++) {
                //    var monster = M2Share.WorldEngine.MonsterList[keyList[i]];
                //    M2Share.LocalDb.LoadMonitems(monster.Name, ref monster.ItemList);
                //}
                PlayerActor.SysMsg("怪物爆物品列表重加载完成...", MsgColor.Green, MsgType.Hint);
            }
            catch
            {
                PlayerActor.SysMsg("怪物爆物品列表重加载失败!!!", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}