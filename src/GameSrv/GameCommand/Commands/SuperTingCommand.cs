using GameSrv.Actor;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 随机传送一个指定玩家和他身边的人
    /// </summary>
    [Command("SuperTing", "随机传送一个指定玩家和他身边的人", CommandHelp.GameCommandSuperTingHelpMsg, 10)]
    public class SuperTingCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sRange = @params.Length > 1 ? @params[1] : "";
            PlayObject moveHuman;
            IList<BaseObject> humanList;
            if (string.IsNullOrEmpty(sRange) || string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?') {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var nRange = HUtil32._MAX(10, HUtil32.StrToInt(sRange, 2));
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null) {
                humanList = new List<BaseObject>();
                M2Share.WorldEngine.GetMapRageHuman(mPlayObject.Envir, mPlayObject.CurrX, mPlayObject.CurrY, nRange, ref humanList);
                for (var i = 0; i < humanList.Count; i++) {
                    moveHuman = humanList[i] as PlayObject;
                    if (moveHuman != playObject) {
                        moveHuman.MapRandomMove(moveHuman.HomeMap, 0);
                    }
                }
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}