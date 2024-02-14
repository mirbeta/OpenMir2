using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;
using SystemModule.MagicEvent.Events;

namespace CommandModule.Commands
{
    [Command("FireBurn", "", 10)]
    public class FireBurnCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            int nInt = @params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0;
            int nTime = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            int nN = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            if (PlayerActor.Permission < 6)
            {
                return;
            }
            if (nInt == 0 || nTime == 0 || nN == 0)
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            FireBurnEvent fireBurnEvent = new FireBurnEvent(PlayerActor, PlayerActor.CurrX, PlayerActor.CurrY, (byte)nInt, nTime, nN);
            SystemShare.EventMgr.AddEvent(fireBurnEvent);
        }
    }
}