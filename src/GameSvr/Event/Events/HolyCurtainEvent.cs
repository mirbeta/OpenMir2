using GameSvr.Maps;

namespace GameSvr.Event.Events
{
    public class HolyCurtainEvent : EventInfo
    {
        public HolyCurtainEvent(Envirnoment Envir, int nX, int nY, int nType, int nTime) : base(Envir, nX, nY, nType, nTime, true)
        {

        }
    }
}

