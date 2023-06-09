using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    [Command("ShowMapInfo", "显示当前地图信息")]
    public class ShowMapInfoCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandMapInfoMsg, PlayerActor.Envir.MapName, PlayerActor.Envir.MapDesc), MsgColor.Green, MsgType.Hint);
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandMapInfoSizeMsg, PlayerActor.Envir.Width, PlayerActor.Envir.Height), MsgColor.Green, MsgType.Hint);
        }
    }
}