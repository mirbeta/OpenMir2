using EventLogSystem.Services;
using MediatR;
using SystemModule.ModuleEvent;

namespace EventLogSystem
{
    public class MessageEventHandler : INotificationHandler<GameMessageEvent>
    {
        private IGameEventSource _gameEventSource;

        public MessageEventHandler(IGameEventSource gameEventSource)
        {
            _gameEventSource = gameEventSource;
        }

        public Task Handle(GameMessageEvent notification, CancellationToken cancellationToken)
        {
            //todo 解析游戏事件
            
            //_gameEventSource.AddEventLog();
            return Task.CompletedTask;
        }
    }
}