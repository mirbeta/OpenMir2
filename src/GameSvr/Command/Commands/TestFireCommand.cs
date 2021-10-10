using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("TestFire", "", 10)]
    public class TestFireCommand : BaseCommond
    {
        [DefaultCommand]
        public void TestFire(string[] @params, TPlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            var nRange = @params.Length > 0 ? int.Parse(@params[0]) : 0;
            var nType = @params.Length > 1 ? int.Parse(@params[1]) : 0;
            var nTime = @params.Length > 2 ? int.Parse(@params[2]) : 0;
            var nPoint = @params.Length > 3 ? int.Parse(@params[3]) : 0;

            TFireBurnEvent FireBurnEvent;
            var nMinX = PlayObject.m_nCurrX - nRange;
            var nMaxX = PlayObject.m_nCurrX + nRange;
            var nMinY = PlayObject.m_nCurrY - nRange;
            var nMaxY = PlayObject.m_nCurrY + nRange;
            for (var nX = nMinX; nX <= nMaxX; nX++)
            {
                for (var nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX || nY == nMaxY)
                    {
                        FireBurnEvent = new TFireBurnEvent(PlayObject, nX, nY, nType, nTime * 1000, nPoint);
                        M2Share.EventManager.AddEvent(FireBurnEvent);
                    }
                }
            }
        }
    }
}