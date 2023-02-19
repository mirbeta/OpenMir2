using GameSvr.Actor;

namespace GameSvr.Event.Events
{
    /// <summary>
    /// 火墙
    /// </summary>
    public class FireBurnEvent : EventInfo
    {
        /// <summary>
        /// 火墙运行时间
        /// </summary>
        protected int FireRunTick;

        public FireBurnEvent(BaseObject creat, short nX, short nY, byte nType, int nTime, int nDamage) : base(creat.Envir, nX, nY, nType, nTime, true)
        {
            Damage = nDamage;
            OwnBaseObject = creat;
        }

        public override void Run()
        {
            if ((HUtil32.GetTickCount() - FireRunTick) > 3000)
            {
                FireRunTick = HUtil32.GetTickCount();
                IList<BaseObject> BaseObjectList = new List<BaseObject>();
                if (Envir != null)
                {
                    Envir.GetBaseObjects(nX, nY, true, BaseObjectList);
                    for (int i = 0; i < BaseObjectList.Count; i++)
                    {
                        BaseObject targetBaseObject = BaseObjectList[i];
                        if (targetBaseObject != null && OwnBaseObject != null && OwnBaseObject.IsProperTarget(targetBaseObject))
                        {
                            targetBaseObject.SendMsg(OwnBaseObject, Messages.RM_MAGSTRUCK_MINE, 0, Damage, 0, 0, "");
                        }
                    }
                }
                BaseObjectList.Clear();
            }
            base.Run();
        }
    }
}

