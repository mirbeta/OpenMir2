using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

namespace SystemModule
{
    public interface IMerchant : INormNpc
    {
        /// <summary>
        /// 脚本路径
        /// </summary>
        string ScriptName { get; set; }
        /// <summary>
        /// 物品价格倍率 默认为 100%
        /// </summary>
        int PriceRate { get; set; }
        /// <summary>
        /// 沙巴克城堡商人
        /// </summary>
        bool CastleMerchant { get; set; }
        /// <summary>
        /// 刷新在售商品时间
        /// </summary>
        int RefillGoodsTick { get; set; }
        /// <summary>
        /// 清理武器升级过期时间
        /// </summary>
        int ClearExpreUpgradeTick { get; set; }
        /// <summary>
        /// NPC买卖物品类型列表，脚本中前面的 +1 +30 之类的
        /// </summary>
        IList<int> ItemTypeList { get; set; }
        /// <summary>
        /// 补充商品列表
        /// </summary>
        IList<Goods> RefillGoodsList { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        IList<IList<UserItem>> GoodsList { get; set; }
        /// <summary>
        /// 物品价格列表
        /// </summary>
        IList<ItemPrice> ItemPriceList { get; set; }
        /// <summary>
        /// 物品升级列表
        /// </summary>
        IList<WeaponUpgradeInfo> UpgradeWeaponList { get; set; }
        bool BoCanMove { get; set; }
        int MoveTime { get; set; }
        int MoveTick { get; set; }
        /// <summary>
        /// 是否购买物品
        /// </summary>
        bool IsBuy { get; set; }
        /// <summary>
        /// 是否交易物品
        /// </summary>
        bool IsSell { get; set; }
        bool IsMakeDrug { get; set; }
        bool IsPrices { get; set; }
        bool IsStorage { get; set; }
        bool IsGetback { get; set; }
        bool IsUpgradenow { get; set; }
        bool IsGetBackupgnow { get; set; }
        bool IsRepair { get; set; }
        bool IsSupRepair { get; set; }
        bool IsSendMsg { get; set; }
        bool IsGetMarry { get; set; }
        bool IsGetMaster { get; set; }
        bool IsUseItemName { get; set; }
        bool IsOffLineMsg { get; set; }
        bool IsYbDeal { get; set; }
        bool CanItemMarket { get; set; }
        short NpcFlag { get; set; }

        void Initialize();

        void LoadMerchantScript();

        void LoadNpcData();

        void ClearData();

        void Run();
    }
}