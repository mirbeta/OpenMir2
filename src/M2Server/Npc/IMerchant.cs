using SystemModule.Packets.ClientPackets;

namespace M2Server.Npc
{
    public class IMerchant : NormNpc
    {
        /// <summary>
        /// 脚本路径
        /// </summary>
        public string ScriptName = string.Empty;
        /// <summary>
        /// 物品价格倍率 默认为 100%
        /// </summary>
        public int PriceRate;
        /// <summary>
        /// 沙巴克城堡商人
        /// </summary>
        public bool CastleMerchant;
        /// <summary>
        /// 刷新在售商品时间
        /// </summary>
        protected int RefillGoodsTick;
        /// <summary>
        /// 清理武器升级过期时间
        /// </summary>
        protected int ClearExpreUpgradeTick;
        /// <summary>
        /// NPC买卖物品类型列表，脚本中前面的 +1 +30 之类的
        /// </summary>
        public IList<int> ItemTypeList;
        public IList<Goods> RefillGoodsList;
        /// <summary>
        /// 商品列表
        /// </summary>
        protected IList<IList<UserItem>> GoodsList;
        /// <summary>
        /// 物品价格列表
        /// </summary>
        protected IList<ItemPrice> ItemPriceList;
        /// <summary>
        /// 物品升级列表
        /// </summary>
        protected IList<WeaponUpgradeInfo> UpgradeWeaponList;
        public bool BoCanMove = false;
        public int MoveTime = 0;
        public int MoveTick;
        /// <summary>
        /// 是否购买物品
        /// </summary>
        public bool IsBuy;
        /// <summary>
        /// 是否交易物品
        /// </summary>
        public bool IsSell;
        public bool IsMakeDrug;
        public bool IsPrices;
        public bool IsStorage;
        public bool IsGetback;
        public bool IsUpgradenow;
        public bool IsGetBackupgnow;
        public bool IsRepair;
        public bool IsSupRepair;
        public bool IsSendMsg = false;
        public bool IsGetMarry;
        public bool IsGetMaster;
        public bool IsUseItemName;
        public bool IsOffLineMsg = false;
        public bool IsYbDeal = false;
        public bool CanItemMarket = false;
    }
}