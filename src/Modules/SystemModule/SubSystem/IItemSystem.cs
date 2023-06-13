using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

namespace SystemModule
{
    public interface IItemSystem
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

        int GetUpgradeStdItem(StdItem stdItem, UserItem userItem, ref ClientItem clientItem);

        void RandomSetUnknownItem(StdItem stdItem, UserItem pu);

        void RandomUpgradeItem(StdItem stdItem, UserItem pu);

        int RealAttackSpeed(short wAtkSpd);
    }
}