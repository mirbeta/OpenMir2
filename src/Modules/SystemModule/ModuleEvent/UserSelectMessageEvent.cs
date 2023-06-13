using MediatR;

namespace SystemModule.ModuleEvent
{
    public class UserSelectMessageEvent : INotification
    {
        public IPlayerActor Actor { get; set; }
        public string Lable { get; set; }
    }
}
