using GameSrv.Event.Events;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands
{
    [Command("FireBurn", "", 10)]
    public class FireBurnCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            int nInt = @params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0;
            int nTime = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            int nN = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            if (playObject.Permission < 6)
            {
                return;
            }
            if (nInt == 0 || nTime == 0 || nN == 0)
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            FireBurnEvent fireBurnEvent = new FireBurnEvent(playObject, playObject.CurrX, playObject.CurrY, (byte)nInt, nTime, nN);
            M2Share.EventMgr.AddEvent(fireBurnEvent);
        }
    }
}