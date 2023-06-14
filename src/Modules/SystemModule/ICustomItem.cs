namespace SystemModule
{
    public interface ICustomItem
    {
        bool AddCustomItemName(int nMakeIndex, int nItemIndex, string sItemName);
        void DelCustomItemName(int nMakeIndex, int nItemIndex);
        string GetCustomItemName(int nMakeIndex, int nItemIndex);
        void LoadCustomItemName();
        void SaveCustomItemName();
    }
}