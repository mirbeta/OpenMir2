using GameSvr.Maps;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Player
{
    public class TPlayCloneObject : PlayObject
    {
        private readonly int m_dwRunTime = 0;

        public TPlayCloneObject(PlayObject PlayObject) : base()
        {
            this.m_dwRunTime = HUtil32.GetTickCount();
            this.CharName = "Clone";
            this.CurrX = PlayObject.CurrX;
            this.CurrY = PlayObject.CurrY;
            this.Direction = this.GetBackDir(PlayObject.Direction);
            this.m_PEnvir = PlayObject.m_PEnvir;
            this.Gender = PlayObject.Gender;
            this.Hair = PlayObject.Hair;
            this.m_PEnvir.AddToMap(this.CurrX, this.CurrY, CellType.MovingObject, this);
            this.SendRefMsg(Grobal2.RM_TURN, this.Direction, this.CurrX, this.CurrY, 0, this.CharName);
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }
    }
}

