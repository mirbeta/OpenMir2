using SystemModule;
using SystemModule.MagicEvent;
using SystemModule.Maps;

namespace M2Server.Event.Events
{
    /// <summary>
    /// 困魔咒
    /// </summary>
    public class HolyCurtainEvent : MapEvent
    {
        public HolyCurtainEvent(IEnvirnoment envir, short nX, short nY, byte nType, int nTime) : base(envir, nX, nY, nType, nTime, true)
        {

        }
    }
}