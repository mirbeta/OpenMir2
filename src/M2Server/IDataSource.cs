using SystemModule.Data;

namespace M2Server
{
    public interface IDataSource
    {
        /// <summary>
        /// 读取物品数据
        /// </summary>
        /// <returns></returns>
        public int LoadItemsDB();
        
        /// <summary>
        /// 读取技能数据
        /// </summary>
        /// <returns></returns>
        public int LoadMagicDB();

        /// <summary>
        /// 读取怪物数据
        /// </summary>
        /// <returns></returns>
        int LoadMonsterDB();

        /// <summary>
        /// 加载寄售系统数据
        /// </summary>
        void LoadSellOffItemList();

        /// <summary>
        /// 保存寄售系统数据
        /// </summary>
        void SaveSellOffItemList();

        int LoadUpgradeWeaponRecord(string sNPCName, IList<WeaponUpgradeInfo> DataList);

        int SaveUpgradeWeaponRecord(string sNPCName, IList<WeaponUpgradeInfo> DataList);
    }
}
