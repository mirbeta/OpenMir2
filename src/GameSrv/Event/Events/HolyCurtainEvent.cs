using GameSrv.Maps;

namespace GameSrv.Event.Events
{
    /// <summary>
    /// 困魔咒
    /// </summary>
    public class HolyCurtainEvent : MapEvent
    {
        public HolyCurtainEvent(Envirnoment envir, short nX, short nY, byte nType, int nTime) : base(envir, nX, nY, nType, nTime, true)
        {

        }
    }
}