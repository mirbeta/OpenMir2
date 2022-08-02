using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 重新加载物品数据库
    /// </summary>
    [GameCommand("ReloadMagicDB", "重新加载技能数据库", 10)]

    public class ReloadMagicDBCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadMonItems(TPlayObject PlayObject)
        {
            M2Share.CommonDB.LoadMagicDB();
            PlayObject.SysMsg("魔法数据库重新加载完成。", MsgColor.Green, MsgType.Hint);
        }
    }
}