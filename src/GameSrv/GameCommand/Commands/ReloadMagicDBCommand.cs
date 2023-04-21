using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 重新加载物品数据库
    /// </summary>
    [Command("ReloadMagicDB", "重新加载技能数据库", 10)]

    public class ReloadMagicDbCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            GameShare.CommonDb.LoadMagicDB();
            playObject.SysMsg("魔法数据库重新加载完成。", MsgColor.Green, MsgType.Hint);
        }
    }
}