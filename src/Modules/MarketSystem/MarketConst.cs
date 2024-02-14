namespace MarketSystem
{
    public static class MarketConst
    {
        public const int MAKET_ITEMCOUNT_PER_PAGE = 10;
        public const int MAKET_MAX_PAGE = 15;
        public const int MAKET_MAX_ITEM_COUNT = MAKET_ITEMCOUNT_PER_PAGE * MAKET_MAX_PAGE;
        public const int MARKET_CHARGE_MONEY = 1000;
        public const int MARKET_ALLOW_LEVEL = 1;
        public const int MARKET_COMMISION = 10;
        public const int MARKET_MAX_TRUST_MONEY = 50000000;
        public const int MARKET_MAX_SELL_COUNT = 5;
        public const int MAKET_STATE_EMPTY = 0;
        public const int MAKET_STATE_LOADING = 1;
        public const int MAKET_STATE_LOADED = 2;

        public const byte USERMARKET_TYPE_ALL = 0;
        public const byte USERMARKET_TYPE_WEAPON = 1;
        public const byte USERMARKET_TYPE_NECKLACE = 2;
        public const byte USERMARKET_TYPE_RING = 3;
        public const byte USERMARKET_TYPE_BRACELET = 4;
        public const byte USERMARKET_TYPE_CHARM = 5;
        public const byte USERMARKET_TYPE_HELMET = 6;
        public const byte USERMARKET_TYPE_BELT = 7;
        public const byte USERMARKET_TYPE_SHOES = 8;
        public const byte USERMARKET_TYPE_ARMOR = 9;
        public const byte USERMARKET_TYPE_DRINK = 10;
        public const byte USERMARKET_TYPE_JEWEL = 11;
        public const byte USERMARKET_TYPE_BOOK = 12;
        public const byte USERMARKET_TYPE_MINERAL = 13;
        public const byte USERMARKET_TYPE_QUEST = 14;
        public const byte USERMARKET_TYPE_ETC = 15;
        public const byte USERMARKET_TYPE_ITEMNAME = 16;
        public const byte USERMARKET_TYPE_SET = 100;
        public const byte USERMARKET_TYPE_MINE = 200;
        public const short USERMARKET_TYPE_OTHER = 300;

        public const byte USERMARKET_MODE_NULL = 0;
        public const byte USERMARKET_MODE_BUY = 1;
        public const byte USERMARKET_MODE_INQUIRY = 2;
        public const byte USERMARKET_MODE_SELL = 3;

        public const byte UMRESULT_SUCCESS = 0;
        public const byte UMResult_Fail = 1;
        public const byte UMResult_ReadFail = 2;
        public const byte UMResult_WriteFail = 3;
        public const byte UMResult_ReadyToSell = 4;
        public const byte UMResult_OverSellCount = 5;
        public const byte UMResult_LessMoney = 6;
        public const byte UMResult_LessLevel = 7;
        public const byte UMResult_MaxBagItemCount = 8;
        public const byte UMResult_NoItem = 9;
        public const byte UMResult_DontSell = 10;
        public const byte UMResult_DontBuy = 11;
        public const byte UMResult_DontGetMoney = 12;
        public const byte UMResult_MarketNotReady = 13;
        public const byte UMResult_LessTrustMoney = 14;
        public const byte UMResult_MaxTrustMoney = 15;
        public const byte UMResult_CancelFail = 16;
        public const byte UMResult_OverMoney = 17;
        public const byte UMResult_SellOK = 18;
        public const byte UMResult_BuyOK = 19;
        public const byte UMResult_CancelOK = 20;
        public const byte UMResult_GetPayOK = 21;
    }
}