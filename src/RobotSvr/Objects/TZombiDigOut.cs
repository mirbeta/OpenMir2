using SystemModule;

namespace RobotSvr
{
    public class TZombiDigOut: TSkeletonOma
    {
        // ============================= TZombiDigOut =============================
        public override void RunFrameAction(int frame)
        {
            TClEvent clEvent;
            if (this.m_nCurrentAction == Grobal2.SM_DIGUP)
            {
                if (frame == 6)
                {
                    clEvent = new TClEvent(this.m_nCurrentEvent, this.m_nCurrX, this.m_nCurrY, Grobal2.ET_DIGOUTZOMBI);
                    clEvent.m_nDir = this.m_btDir;
                   ClMain.EventMan.AddEvent(clEvent);
                }
            }
        }

    } // end TZombiDigOut

    } // end TBanyaGuardMon

