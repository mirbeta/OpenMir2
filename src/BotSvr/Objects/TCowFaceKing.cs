namespace BotSvr.Objects;

public class TCowFaceKing : TGasKuDeGi
{
    public TCowFaceKing(RobotClient robotClient) : base(robotClient)
    {
    }

    public override int light()
    {
        int result;
        var L = m_nChrLight;
        if (L < 2)
            if (m_boUseEffect)
                L = 2;
        result = L;
        return result;
    }
}