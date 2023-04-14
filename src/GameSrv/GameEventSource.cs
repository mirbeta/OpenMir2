using System.Diagnostics.Tracing;

namespace GameSrv {
    [EventSource(Name = "UserLogProvider")]
    public class GameEventSource : EventSource {
        public void AddEventLog(int eventType, string meesage) {
            //todo eventType需整理归类
            WriteEvent(eventType, meesage);
        }

        public void AddEventLog(GameEventLogType eventType, string meesage) {
            WriteEvent((int)eventType, meesage);
        }
    }

    public enum GameEventLogType : int {
        /// <summary>
        /// 取回物品
        /// </summary>
        TakeItem = 0,
        /// <summary>
        /// 存放物品
        /// </summary>
        StorageItem = 1,
        /// <summary>
        /// 炼制药品
        /// </summary>
        RefiningItem = 2,
        /// <summary>
        /// 持久消失
        /// </summary>
        ItemDur = 3,
        /// <summary>
        /// 捡起物品
        /// </summary>
        PickUpItem = 4,
        /// <summary>
        /// 制造物品
        /// </summary>
        MakeItem = 5,
        /// <summary>
        /// 毁掉物品
        /// </summary>
        DestroyItem = 6,
        /// <summary>
        /// 扔掉物品
        /// </summary>
        ThrowItem = 7,
        /// <summary>
        /// 交易物品
        /// </summary>
        DealItem = 8,
        /// <summary>
        /// 购买物品
        /// </summary>
        BuyItem = 9,
        /// <summary>
        /// 出售物品
        /// </summary>
        SellItem = 10,
        /// <summary>
        /// 使用物品
        /// </summary>
        UseItem = 11,
        /// <summary>
        /// 人物升级
        /// </summary>
        LevelUp = 12,
        /// <summary>
        /// 减少金币
        /// </summary>
        GoldDesc = 13,
        /// <summary>
        /// 增加金币
        /// </summary>
        GoldInc = 14,
        /// <summary>
        /// 死亡掉落物品
        /// </summary>
        GhostDropItem = 15,
        /// <summary>
        /// 掉落物品
        /// </summary>
        DropItem = 16,
        /// <summary>
        /// 等级调整
        /// </summary>
        ChangeLevel = 17,
        /// <summary>
        /// 无效代码
        /// </summary>
        Unknown = 18,
        /// <summary>
        /// 人物死亡
        /// </summary>
        PlayDie = 19,
        /// <summary>
        /// 武器升级成功
        /// </summary>
        WeaponUpgradeSuccess = 20,
        /// <summary>
        /// 武器升级失败
        /// </summary>
        WeaponUpgradeFail = 21,
        /// <summary>
        /// 沙巴克存入金币
        /// </summary>
        CastleReceiptGolds = 22,//城堡取钱
        /// <summary>
        /// 城堡存钱
        /// </summary>
        CastleTodayIncome = 23,
        /// <summary>
        /// 武器升级取回
        /// </summary>
        GetBackupgWeapon = 24,
        /// <summary>
        /// 开始沙巴克武器升级
        /// </summary>
        UpgradeWapon = 25,
        /// <summary>
        /// 背包减少
        /// </summary>
        BagItemChangge = 26,
        /// <summary>
        /// 改变城主
        /// </summary>
        ChangeCastleMaster = 27,
        /// <summary>
        /// 商铺购买物品
        /// </summary>
        BuyShopItem = 30,
        /// <summary>
        /// 装备升级
        /// </summary>
        ItemUpgradeSuccess = 31,
        /// <summary>
        /// 个人商店
        /// </summary>
        UserStall = 34
    }
}