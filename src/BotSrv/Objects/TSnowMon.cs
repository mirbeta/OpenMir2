using BotSrv.Player;
using OpenMir2;
using SystemModule;

namespace BotSrv.Objects;

public class TSnowMon : Actor
{
    protected int ax = 0;
    protected int ay = 0;
    protected bool BoUseDieEffect;
    protected int bx = 0;
    protected int by = 0;
    protected int fire16dir = 0;
    protected int firedir;
    protected bool m_bowChrEffect;

    public TSnowMon(RobotPlayer robotClient) : base(robotClient)
    {
        m_boUseEffect = false;
        BoUseDieEffect = false;
        m_bowChrEffect = false;
    }

    public override void Run()
    {
        long dwEffectFrameTimetime;
        base.Run();
        if (m_boUseEffect)
        {
            if (m_boMsgMuch)
                dwEffectFrameTimetime = HUtil32.Round(m_dwEffectFrameTime * 2 / 3);
            else
                dwEffectFrameTimetime = m_dwEffectFrameTime;
            if (MShare.GetTickCount() - m_dwEffectStartTime > dwEffectFrameTimetime)
            {
                m_dwEffectStartTime = MShare.GetTickCount();
                if (m_nEffectFrame < m_nEffectEnd)
                    m_nEffectFrame++;
                else
                    m_boUseEffect = false;
            }
        }
    }
}