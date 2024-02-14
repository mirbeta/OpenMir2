using BotSrv.Player;

namespace BotSrv.Objects;

public class TCowFaceKing : TGasKuDeGi
{
    public TCowFaceKing(RobotPlayer robotClient) : base(robotClient)
    {
    }

    public override int light()
    {
        int result;
        int L = m_nChrLight;
        if (L < 2)
        {
            if (m_boUseEffect)
            {
                L = 2;
            }
        }

        result = L;
        return result;
    }
}