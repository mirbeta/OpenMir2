using SystemModule;

namespace M2Server
{
    /// <summary>
    /// 精灵状态系统
    /// </summary>
    public class ActorStateSystem
    {
        public IList<IActor> Actors = new List<IActor>();

        public void Add(IActor actor, byte stateType, int stateval)
        {

        }
    }
}