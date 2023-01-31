using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 重新加载物品数据库
    /// </summary>
    [Command("ReloadMagicDB", "重新加载技能数据库", 10)]

    public class ReloadMagicDBCommand : Command
    {
        [ExecuteCommand]
        public void ReloadMonItems(PlayObject PlayObject)
        {
            M2Share.CommonDb.LoadMagicDB();
            PlayObject.SysMsg("魔法数据库重新加载完成。", MsgColor.Green, MsgType.Hint);
        }
    }
}