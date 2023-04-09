using GameSrv.World.Managers;
using SystemModule.Data;
using SystemModule.Packets.ServerPackets;

namespace GameSrv.Player
{
    public partial class PlayObject
    {
        public void SendUserMarketList(int nextPage, MarketDataMessage marketData)
        {
            if (marketData.TotalCount <= 0)
            {
                return;
            }
            var cnt = 0;
            var page = 0;
            var bFirstSend = 0;
            if (nextPage == 0)
            {
                bFirstSend = 1;
                page = 1;
            }
            else
            {
                bFirstSend = 0;
            }
            if (nextPage == 1)
            {
                page = MarketUser.CurrPage + 1;
            }
            MarketUser.CurrPage = page;
            var maxpage = (int)Math.Ceiling(marketData.TotalCount / (double)MarketConst.MAKET_ITEMCOUNT_PER_PAGE);
            var marketItems = marketData.List.Skip((page - 1) * MarketConst.MAKET_ITEMCOUNT_PER_PAGE).Take(MarketConst.MAKET_ITEMCOUNT_PER_PAGE).ToList();
            var buffer = string.Empty;
            if (marketItems.Count > 0)
            {
                for (int i = 0; i < marketItems.Count; i++)
                {
                    cnt++;
                    buffer = buffer + EDCode.EncodeBuffer(marketData.List[i]) + '/';
                }
            }
            buffer = cnt + '/' + page + '/' + maxpage + '/' + buffer;
            SendMsg(this, Messages.RM_MARKET_LIST, 0, MarketUser.UserMode, MarketUser.ItemType, bFirstSend, buffer);
        }

        public void ReadyToSellUserMarket(MarkerUserLoadMessage readyItem)
        {
            if (readyItem.IsBusy != MarketConst.UMRESULT_SUCCESS) return;
            if (readyItem.SellCount < MarketConst.MARKET_MAX_SELL_COUNT)
            {
                SendMsg(this, Messages.RM_MARKET_RESULT, 0, readyItem.MarketNPC, MarketConst.UMResult_ReadyToSell, 0, "");
            }
            else
            {
                SendMsg(this, Messages.RM_MARKET_RESULT, 0, 0, MarketConst.UMResult_OverSellCount, 0, "");
            }
            FlagReadyToSellCheck = true;
        }
    }
}