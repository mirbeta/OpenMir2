using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.MagicEvent.Events;

namespace CommandModule.Commands
{
    [Command("TestFire", "", 10)]
    public class TestFireCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
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
            int nMinX = PlayerActor.CurrX - nRange;
            int nMaxX = PlayerActor.CurrX + nRange;
            int nMinY = PlayerActor.CurrY - nRange;
            int nMaxY = PlayerActor.CurrY + nRange;
            for (int nX = nMinX; nX <= nMaxX; nX++)
            {
                for (int nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX || nY == nMaxY)
                    {
                        fireBurnEvent = new FireBurnEvent(PlayerActor, (short)nX, (short)nY, (byte)nType, nTime * 1000, nPoint);
                        SystemShare.EventMgr.AddEvent(fireBurnEvent);
                    }
                }
            }
        }
    }
}