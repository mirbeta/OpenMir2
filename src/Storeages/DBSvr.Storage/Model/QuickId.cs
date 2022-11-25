namespace DBSvr.Storage.Model
{
    public class PlayQuick
    {
        public int SelectID;
        public string Account;
        public int Index;
        public string ChrName;

        public PlayQuick() { }

        public PlayQuick(string account, int index)
        {
            Account = account;
            Index = index;
        }
    }
}
