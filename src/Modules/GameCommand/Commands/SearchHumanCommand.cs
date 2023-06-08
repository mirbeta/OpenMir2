using M2Server.Player;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 搜索指定玩家所在地图XY坐标
    /// </summary>
    [Command("SearchHuman", "搜索指定玩家所在地图XY坐标", "人物名称")]
    public class SearchHumanCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (playObject.ProbeNecklace || playObject.Permission >= 6) {
                if (string.IsNullOrEmpty(sHumanName)) {
                    playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (HUtil32.GetTickCount() - playObject.ProbeTick > 10000 || playObject.Permission >= 3) {
                    playObject.ProbeTick = HUtil32.GetTickCount();
                    var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
                    if (mPlayObject != null) {
                        playObject.SysMsg(sHumanName + " 现在位于 " + mPlayObject.Envir.MapDesc + '(' + mPlayObject.Envir.MapName + ") " + mPlayObject.CurrX + ':'
                            + playObject.CurrY, MsgColor.Blue, MsgType.Hint);
                    }
                    else {
                        playObject.SysMsg(sHumanName + " 现在不在线，或位于其它服务器上!!!", MsgColor.Red, MsgType.Hint);
                    }
                }
                else {
                    playObject.SysMsg((HUtil32.GetTickCount() - playObject.ProbeTick) / 1000 - 10 + " 秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}