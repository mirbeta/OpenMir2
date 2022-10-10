using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 重新加载物品数据库
    /// </summary>
    [Command("ReloadMagicDB", "重新加载技能数据库", 10)]

    public class ReloadMagicDBCommand : Commond
    {
        [ExecuteCommand]
        public void ReloadMonItems(PlayObject PlayObject)
        {
            M2Share.CommonDb.LoadMagicDB();
            PlayObject.SysMsg("魔法数据库重新加载完成。", MsgColor.Green, MsgType.Hint);
        }
    }
}