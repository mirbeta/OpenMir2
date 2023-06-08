using M2Server.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 重新加载怪物爆率文件
    /// </summary>
    [Command("ReloadMonItems", "重新加载怪物爆率文件", 10)]
    public class ReloadMonItemsCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            try {
                var keyList = M2Share.WorldEngine.MonsterList.Keys.ToList();
                for (var i = 0; i < keyList.Count; i++) {
                    var monster = M2Share.WorldEngine.MonsterList[keyList[i]];
                    M2Share.LocalDb.LoadMonitems(monster.Name, ref monster.ItemList);
                }
                playObject.SysMsg("怪物爆物品列表重加载完成...", MsgColor.Green, MsgType.Hint);
            }
            catch {
                playObject.SysMsg("怪物爆物品列表重加载失败!!!", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}