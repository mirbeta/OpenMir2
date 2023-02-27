using GameSrv.Event.Events;
using GameSrv.Player;

namespace GameSrv.GameCommand.Commands
{
    [Command("TestFire", "", 10)]
    public class TestFireCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            int nRange = @params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0;
            int nType = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            int nTime = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            int nPoint = @params.Length > 3 ? HUtil32.StrToInt(@params[3], 0) : 0;

            FireBurnEvent FireBurnEvent;
            int nMinX = PlayObject.CurrX - nRange;
            int nMaxX = PlayObject.CurrX + nRange;
            int nMinY = PlayObject.CurrY - nRange;
            int nMaxY = PlayObject.CurrY + nRange;
            for (int nX = nMinX; nX <= nMaxX; nX++)
            {
                for (int nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX || nY == nMaxY)
                    {
                        FireBurnEvent = new FireBurnEvent(PlayObject, (short)nX, (short)nY, (byte)nType, nTime * 1000, nPoint);
                        M2Share.EventMgr.AddEvent(FireBurnEvent);
                    }
                }
            }
        }
    }
}