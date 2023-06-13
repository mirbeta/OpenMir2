using MediatR;
using SystemModule.ModuleEvent;

namespace EventLogSystem
{
    public class MessageEventHandler : INotificationHandler<GameMessageEvent>
    {
        public Task Handle(GameMessageEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}