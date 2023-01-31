using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("ShowMapInfo", "显示当前地图信息", 0)]
    public class ShowMapInfoCommand : Command
    {
        [ExecuteCommand]
        public static void ShowMapInfo(PlayObject PlayObject)
        {
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMapInfoMsg, PlayObject.Envir.MapName, PlayObject.Envir.MapDesc), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMapInfoSizeMsg, PlayObject.Envir.Width, PlayObject.Envir.Height), MsgColor.Green, MsgType.Hint);
        }
    }
}