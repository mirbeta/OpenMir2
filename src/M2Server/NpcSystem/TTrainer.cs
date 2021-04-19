namespace M2Server
{
    public class TTrainer: TNormNpc
    {
        public int n564 = 0;
        public int m_dw568 = 0;
        public int n56C = 0;
        public int n570 = 0;

        public TTrainer() : base()
        {
            m_dw568 = HUtil32.GetTickCount();
            n56C = 0;
            n570 = 0;
        }

        ~TTrainer()
        {

        }

        public override bool Operate(TProcessMessage ProcessMsg)
        {
            var result = false;
            if (ProcessMsg.wIdent == grobal2.RM_STRUCK || ProcessMsg.wIdent == grobal2.RM_MAGSTRUCK)
            {
                if (ProcessMsg.BaseObject == this.ObjectId)
                {
                    n56C += ProcessMsg.wParam;
                    m_dw568 = HUtil32.GetTickCount();
                    n570 ++;
                    this.ProcessSayMsg("破坏力为 " + ProcessMsg.wParam + ",平均值为 " + n56C / n570);
                }
            }
            if (ProcessMsg.wIdent == grobal2.RM_MAGSTRUCK)
            {
                result = base.Operate(ProcessMsg);
            }
            return result;
        }

        public override void Run()
        {
            if (n570 > 0)
            {
                if (HUtil32.GetTickCount() - m_dw568 > 3 * 1000)
                {
                    this.ProcessSayMsg("总破坏力为  " + n56C + ",平均值为 " + n56C / n570);
                    n570 = 0;
                    n56C = 0;
                }
            }
            base.Run();
        }
    } 
}

