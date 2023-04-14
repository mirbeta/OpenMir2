using GameSrv.Maps;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 显示指定地图信息
    /// </summary>
    [Command("ShowMapMode", "显示指定地图信息", "地图号", 10)]
    public class ShowMapModeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sMapName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sMapName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            Envirnoment envir = M2Share.MapMgr.FindMap(sMapName);
            if (envir == null) {
                playObject.SysMsg(sMapName + " 不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            string sMsg = "地图模式: " + envir.GetEnvirInfo();
            playObject.SysMsg(sMsg, MsgColor.Blue, MsgType.Hint);
        }
    }
}