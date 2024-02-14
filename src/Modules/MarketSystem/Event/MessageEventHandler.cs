using MarketSystem.Services;
using MediatR;
using SystemModule.ModuleEvent;

namespace MarketSystem.Event
{
    public class MessageEventHandler : INotificationHandler<UserSelectMessageEvent>
    {
        private readonly IMarketService _marketService;

        public MessageEventHandler(IMarketService marketService)
        {
            _marketService = marketService;
        }

        public Task Handle(UserSelectMessageEvent messageEvent, CancellationToken cancellationToken)
        {
            if (string.Compare(messageEvent.Lable, "market_0", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _marketService.SendUserMarket(messageEvent.NormNpc, messageEvent.Actor, MarketConst.USERMARKET_TYPE_ALL, MarketConst.USERMARKET_MODE_BUY);
            }
            else if (string.Compare(messageEvent.Lable, "@market_sell", StringComparison.OrdinalIgnoreCase) == 0)
            {
                _marketService.SendUserMarket(messageEvent.NormNpc, messageEvent.Actor, MarketConst.USERMARKET_TYPE_ALL, MarketConst.USERMARKET_MODE_SELL);
            }
            return Task.CompletedTask;
        }
    }
}