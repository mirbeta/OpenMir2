using GameSvr.Maps;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Player
{
    public class TPlayCloneObject : TPlayObject
    {
        private readonly int m_dwRunTime = 0;

        public TPlayCloneObject(TPlayObject PlayObject) : base()
        {
            this.m_dwRunTime = HUtil32.GetTickCount();
            this.m_sCharName = "Clone";
            this.m_nCurrX = PlayObject.m_nCurrX;
            this.m_nCurrY = PlayObject.m_nCurrY;
            this.Direction = this.GetBackDir(PlayObject.Direction);
            this.m_PEnvir = PlayObject.m_PEnvir;
            this.Gender = PlayObject.Gender;
            this.m_btHair = PlayObject.m_btHair;
            this.m_PEnvir.AddToMap(this.m_nCurrX, this.m_nCurrY, CellType.MovingObject, this);
            this.SendRefMsg(Grobal2.RM_TURN, this.Direction, this.m_nCurrX, this.m_nCurrY, 0, this.m_sCharName);
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }
    }
}

