using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("ShowMapInfo", "显示当前地图信息")]
    public class ShowMapInfoCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.SysMsg(string.Format(CommandHelp.GameCommandMapInfoMsg, playObject.Envir.MapName, playObject.Envir.MapDesc), MsgColor.Green, MsgType.Hint);
            playObject.SysMsg(string.Format(CommandHelp.GameCommandMapInfoSizeMsg, playObject.Envir.Width, playObject.Envir.Height), MsgColor.Green, MsgType.Hint);
        }
    }
}