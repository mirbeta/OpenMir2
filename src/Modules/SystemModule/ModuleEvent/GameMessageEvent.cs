using MediatR;

namespace SystemModule.ModuleEvent
{
    /// <summary>
    /// 游戏消息事件
    /// </summary>
    public class GameMessageEvent : INotification
    {
        public byte EventType { get; set; }
        public string Event { get; set; }
    }
}
