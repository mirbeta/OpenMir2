using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("ShowMapInfo", "显示当前地图信息", 0)]
    public class ShowMapInfoCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMapInfoMsg, PlayObject.Envir.MapName, PlayObject.Envir.MapDesc), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMapInfoSizeMsg, PlayObject.Envir.Width, PlayObject.Envir.Height), MsgColor.Green, MsgType.Hint);
        }
    }
}