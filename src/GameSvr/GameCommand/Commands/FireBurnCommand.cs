using GameSvr.Event.Events;
using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [Command("FireBurn", "", 10)]
    public class FireBurnCommand : Command
    {
        [ExecuteCommand]
        public void FireBurn(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var nInt = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            var nTime = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            var nN = @Params.Length > 2 ? int.Parse(@Params[2]) : 0;
            if (PlayObject.Permission < 6)
            {
                return;
            }
            if (nInt == 0 || nTime == 0 || nN == 0)
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var FireBurnEvent = new FireBurnEvent(PlayObject, PlayObject.CurrX, PlayObject.CurrY, nInt, nTime, nN);
            M2Share.EventMgr.AddEvent(FireBurnEvent);
        }
    }
}