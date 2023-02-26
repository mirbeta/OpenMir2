﻿using GameSrv.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 重新加载怪物爆率文件
    /// </summary>
    [Command("ReloadMonItems", "重新加载怪物爆率文件", 10)]
    public class ReloadMonItemsCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            try {
                List<string> keyList = M2Share.WorldEngine.MonsterList.Keys.ToList();
                for (int i = 0; i < keyList.Count; i++) {
                    MonsterInfo Monster = M2Share.WorldEngine.MonsterList[keyList[i]];
                    M2Share.LocalDb.LoadMonitems(Monster.Name, ref Monster.ItemList);
                }
                PlayObject.SysMsg("怪物爆物品列表重加载完成...", MsgColor.Green, MsgType.Hint);
            }
            catch {
                PlayObject.SysMsg("怪物爆物品列表重加载失败!!!", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}