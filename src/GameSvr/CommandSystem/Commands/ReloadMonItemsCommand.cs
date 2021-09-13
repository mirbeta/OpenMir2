using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 重新加载怪物爆率文件
    /// </summary>
    [GameCommand("ReloadMonItems", "重新加载怪物爆率文件", 10)]
    public class ReloadMonItemsCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadMonItems(string[] @Params, TPlayObject PlayObject)
        {
            TMonInfo Monster;
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
            try
            {
                for (var i = 0; i < M2Share.UserEngine.MonsterList.Count; i++)
                {
                    Monster = M2Share.UserEngine.MonsterList[i];
                    //LocalDB.GetInstance().LoadMonitems(Monster.sName, ref Monster.ItemList);
                }
                PlayObject.SysMsg("怪物爆物品列表重加载完成...", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            catch
            {
                PlayObject.SysMsg("怪物爆物品列表重加载失败！！！", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}