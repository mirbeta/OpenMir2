using MediatR;
using SystemModule.Actors;

namespace SystemModule.ModuleEvent
{
    /// <summary>
    /// 脚本选择消息事件
    /// </summary>
    public class UserSelectMessageEvent : INotification
    {
        public IPlayerActor Actor { get; set; }
        public INormNpc NormNpc { get; set; }
        public string Lable { get; set; }
    }
}
