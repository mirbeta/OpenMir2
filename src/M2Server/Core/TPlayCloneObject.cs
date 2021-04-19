namespace M2Server
{
    public class TPlayCloneObject: TPlayObject
    {
        public int m_dwRunTime = 0;
        public int m_dwRunNextTick = 0;

        public TPlayCloneObject(TPlayObject PlayObject) : base()
        {
            m_dwRunTime = HUtil32.GetTickCount();
            m_dwRunNextTick = 5000;
            this.m_sCharName = "Clone";
            this.m_nCurrX = PlayObject.m_nCurrX;
            this.m_nCurrY = PlayObject.m_nCurrY;
            this.m_btDirection = this.GetBackDir(PlayObject.m_btDirection);
            this.m_PEnvir = PlayObject.m_PEnvir;
            this.m_btGender = PlayObject.m_btGender;
            this.m_btHair = PlayObject.m_btHair;
            this.m_PEnvir.AddToMap(this.m_nCurrX, this.m_nCurrY, grobal2.OS_MOVINGOBJECT, this);
            this.SendRefMsg(grobal2.RM_TURN, this.m_btDirection, this.m_nCurrX, this.m_nCurrY, 0, this.m_sCharName);
        }

        public override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }
    }
}

