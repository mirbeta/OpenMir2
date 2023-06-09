using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

namespace SystemModule
{
    public interface IItemSystem
    {
        string GetStdItemName(int itemIdx);

        StdItem GetStdItem(int index);

        int GetStdItemIdx(string itemName);

        bool CopyToUserItemFromName(string sItemName, ref UserItem item);
    }
}