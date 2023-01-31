using GameSvr.Maps;
using SystemModule.Data;

namespace GameSvr.Player
{
    public class PlayCloneObject : PlayObject
    {
        private readonly int m_dwRunTime;

        public PlayCloneObject(PlayObject PlayObject) : base()
        {
            this.m_dwRunTime = HUtil32.GetTickCount();
            this.ChrName = "Clone";
            this.CurrX = PlayObject.CurrX;
            this.CurrY = PlayObject.CurrY;
            this.Direction = this.GetBackDir(PlayObject.Direction);
            this.Envir = PlayObject.Envir;
            this.Gender = PlayObject.Gender;
            this.Hair = PlayObject.Hair;
            this.Envir.AddToMap(this.CurrX, this.CurrY, CellType.Play, this);
            this.SendRefMsg(Grobal2.RM_TURN, this.Direction, this.CurrX, this.CurrY, 0, this.ChrName);
        }

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }
    }
}

