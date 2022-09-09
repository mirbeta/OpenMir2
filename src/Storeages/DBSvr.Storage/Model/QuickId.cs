namespace DBSvr.Storage.Model
{
    public class QuickId
    {
        public int nSelectID;
        public string sAccount;
        public int nIndex;
        public string sChrName;

        public QuickId() { }

        public QuickId(string account, int index)
        {
            sAccount = account;
            nIndex = index;
        }
    }
}
