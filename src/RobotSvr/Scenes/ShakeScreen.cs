using System.Drawing;

namespace RobotSvr;

public class ShakeScreen
{
    public bool BoShakeX = false;
    public bool BoShakeY = false;
    public int NShakeCntX = 0;
    public int NShakeCntY = 0;
    public int NShakeLoopCntX = 0;
    public int NShakeLoopCntY = 0;
    public int NShakeRangeX = 0;
    public int NShakeRangeY = 0;
    public long DwShakeTimeX = 0;
    public long DwShakeTickX = 0;
    public long DwShakeTimeY = 0;
    public long DwShakeTickY = 0;
    // 地图特效动作
    //Constructor  Create()
    public ShakeScreen()
    {
        BoShakeX = false;
        NShakeCntX = 0;
        BoShakeY = false;
        NShakeCntY = 0;
        NShakeLoopCntX = 0;
        NShakeLoopCntY = 0;
        DwShakeTimeX = 0;
        DwShakeTimeY = 0;
        NShakeRangeX = 5;
        NShakeRangeY = 5;
    }
    public void SetScrShake_X(int cnt)
    {
        if (BoShakeX || !MShare.g_gcGeneral[10])
        {
            return;
        }
        BoShakeX = true;
        DwShakeTickX = MShare.GetTickCount();
        NShakeCntX = 0;
        NShakeLoopCntX = cnt;
        NShakeRangeX = cnt;
    }

    public void SetScrShake_Y(int cnt)
    {
        if (BoShakeY || !MShare.g_gcGeneral[10])
        {
            return;
        }
        BoShakeY = true;
        DwShakeTickY = MShare.GetTickCount();
        NShakeCntY = 0;
        NShakeLoopCntY = cnt;
        NShakeRangeY = cnt;
    }

    public Rectangle GetShakeRect(long tick)
    {
        Rectangle result;
        int i;
        result.Left = 0;
        result.Top = 0;
        result.Right = MShare.SCREENWIDTH - Units.PlayScn.SOFFX * 2;
        result.Bottom = MShare.SCREENHEIGHT;
        if (BoShakeX)
        {
            if (NShakeLoopCntX > 0)
            {
                if (NShakeCntX < NShakeRangeX)
                {
                    if (tick - DwShakeTickX > DwShakeTimeX)
                    {
                        DwShakeTickX = tick;
                        i = NShakeRangeX;
                        i -= NShakeCntX;
                        result.Left = i;
                        i += MShare.SCREENWIDTH - Units.PlayScn.SOFFX * 2;
                        result.Right = i;
                        NShakeCntX ++;
                    }
                }
                else
                {
                    if (NShakeRangeX > 1)
                    {
                        NShakeRangeX -= 1;
                    }
                    NShakeCntX = 0;
                    NShakeLoopCntX -= 1;
                    if (NShakeLoopCntX <= 0)
                    {
                        BoShakeX = false;
                    }
                }
            }
        }
        if (BoShakeY)
        {
            if (NShakeLoopCntY > 0)
            {
                if (NShakeCntY < NShakeRangeY)
                {
                    if (tick - DwShakeTickY > DwShakeTimeY)
                    {
                        DwShakeTickY = tick;
                        i = NShakeRangeY;
                        i -= NShakeCntY;
                        result.Top = i;
                        i += MShare.SCREENHEIGHT;
                        result.Bottom = i;
                        NShakeCntY ++;
                    }
                }
                else
                {
                    if (NShakeRangeY > 1)
                    {
                        NShakeRangeY -= 1;
                    }
                    NShakeCntY = 0;
                    NShakeLoopCntY -= 1;
                    if (NShakeLoopCntY <= 0)
                    {
                        BoShakeY = false;
                    }
                }
            }
        }
        return result;
    }

}