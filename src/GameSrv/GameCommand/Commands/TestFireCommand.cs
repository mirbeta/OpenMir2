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
            var nRange = @params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0;
            var nType = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            var nTime = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            var nPoint = @params.Length > 3 ? HUtil32.StrToInt(@params[3], 0) : 0;

            FireBurnEvent fireBurnEvent;
            var nMinX = playObject.CurrX - nRange;
            var nMaxX = playObject.CurrX + nRange;
            var nMinY = playObject.CurrY - nRange;
            var nMaxY = playObject.CurrY + nRange;
            for (var nX = nMinX; nX <= nMaxX; nX++)
            {
                for (var nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX || nY == nMaxY)
                    {
                        fireBurnEvent = new FireBurnEvent(playObject, (short)nX, (short)nY, (byte)nType, nTime * 1000, nPoint);
                        GameShare.EventMgr.AddEvent(fireBurnEvent);
                    }
                }
            }
        }
    }
}