using GameSrv.Event.Events;
using GameSrv.Player;

namespace GameSrv.GameCommand.Commands
{
    [Command("TestFire", "", 10)]
    public class TestFireCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            int nRange = @params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0;
            int nType = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            int nTime = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            int nPoint = @params.Length > 3 ? HUtil32.StrToInt(@params[3], 0) : 0;

            FireBurnEvent fireBurnEvent;
            int nMinX = playObject.CurrX - nRange;
            int nMaxX = playObject.CurrX + nRange;
            int nMinY = playObject.CurrY - nRange;
            int nMaxY = playObject.CurrY + nRange;
            for (int nX = nMinX; nX <= nMaxX; nX++)
            {
                for (int nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX || nY == nMaxY)
                    {
                        fireBurnEvent = new FireBurnEvent(playObject, (short)nX, (short)nY, (byte)nType, nTime * 1000, nPoint);
                        M2Share.EventMgr.AddEvent(fireBurnEvent);
                    }
                }
            }
        }
    }
}