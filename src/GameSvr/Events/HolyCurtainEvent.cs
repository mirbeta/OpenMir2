namespace GameSvr
{
    public class HolyCurtainEvent : Event
    {
        public HolyCurtainEvent(Envirnoment Envir, int nX, int nY, int nType, int nTime) : base(Envir, nX, nY, nType, nTime, true)
        {

        }
    }
}

