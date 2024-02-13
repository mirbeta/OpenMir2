using MediatR;
using SystemModule.ModuleEvent;

namespace RobotSystem.Event
{
    public class MessageEventHandler : INotificationHandler<UserSelectMessageEvent>
    {
        public Task Handle(UserSelectMessageEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("Message received: " + notification.Lable);


            return Task.CompletedTask;
        }
    }
}