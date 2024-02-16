using OpenMir2.Data;
using OpenMir2.Packets.ClientPackets;

namespace SystemModule.SubSystem
{
    public interface IEquipmentSystem
    {
        void AddItem(StdItem stdItem);

        void Clear();

        int ItemCount { get; }

        string GetStdItemName(int itemIdx);

        ushort GetStdItemIdx(string itemName);

        bool CopyToUserItemFromName(string sItemName, ref UserItem item);

        StdItem GetStdItem(string sItemName);

        StdItem GetStdItem(ushort nItemIdx);

        int GetStdItemWeight(int nItemIdx);

        int GetUpgrade2(int x, int a);

        /// <summary>
        /// 升级游戏物品
        /// </summary>
        /// <returns></returns>
        int GetUpgradeStdItem(StdItem stdItem, UserItem userItem, ref ClientItem clientItem);

        void RandomSetUnknownItem(StdItem stdItem, UserItem pu);

        void RandomUpgradeItem(StdItem stdItem, UserItem pu);

        int RealAttackSpeed(short wAtkSpd);
    }
}