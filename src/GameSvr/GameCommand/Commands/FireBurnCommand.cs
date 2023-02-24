using GameSvr.Event.Events;
using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    [Command("FireBurn", "", 10)]
    public class FireBurnCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            int nInt = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            int nTime = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            int nN = @Params.Length > 2 ? int.Parse(@Params[2]) : 0;
            if (PlayObject.Permission < 6) {
                return;
            }
            if (nInt == 0 || nTime == 0 || nN == 0) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            FireBurnEvent FireBurnEvent = new FireBurnEvent(PlayObject, PlayObject.CurrX, PlayObject.CurrY, (byte)nInt, nTime, nN);
            M2Share.EventMgr.AddEvent(FireBurnEvent);
        }
    }
}