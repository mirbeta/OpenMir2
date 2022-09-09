using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("ShowMapInfo", "显示当前地图信息", 0)]
    public class ShowMapInfoCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowMapInfo(PlayObject PlayObject)
        {
            PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandMapInfoMsg, PlayObject.Envir.MapName, PlayObject.Envir.MapDesc), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandMapInfoSizeMsg, PlayObject.Envir.Width, PlayObject.Envir.Height), MsgColor.Green, MsgType.Hint);
        }
    }
}