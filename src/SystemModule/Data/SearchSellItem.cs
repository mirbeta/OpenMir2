using System.Collections.Generic;

namespace SystemModule.Data
{
    public class SearchSellItem
    {
        public string MarketName;
        public string Who;
        public string ItemName;
        public int MakeIndex;
        public int ItemType;
        public int ItemSet;
        public int SellIndex;
        public int CheckType;
        public int IsOK;
        public int UserMode;
        public IList<MarketItem> pList;
    }
}