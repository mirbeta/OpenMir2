using GameSvr.Actor;
using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 将指定地图所有玩家随机移动
    /// </summary>
    [Command("MapMoveHuman", "将指定地图所有玩家随机移动", CommandHelp.GameCommandMapMoveHelpMsg, 10)]
    public class MapMoveHumanCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sSrcMap = @Params.Length > 0 ? @Params[0] : "";
            string sDenMap = @Params.Length > 1 ? @Params[1] : "";
            if (sDenMap == "" || sSrcMap == "" || !string.IsNullOrEmpty(sSrcMap) && sSrcMap[0] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            Maps.Envirnoment SrcEnvir = M2Share.MapMgr.FindMap(sSrcMap);
            Maps.Envirnoment DenEnvir = M2Share.MapMgr.FindMap(sDenMap);
            if (SrcEnvir == null) {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMapMoveMapNotFound, sSrcMap), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (DenEnvir == null) {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMapMoveMapNotFound, sDenMap), MsgColor.Red, MsgType.Hint);
                return;
            }
            IList<BaseObject> HumanList = new List<BaseObject>();
            M2Share.WorldEngine.GetMapRageHuman(SrcEnvir, SrcEnvir.Width / 2, SrcEnvir.Height / 2, 1000, HumanList);
            for (int i = 0; i < HumanList.Count; i++) {
                PlayObject MoveHuman = (PlayObject)HumanList[i];
                if (MoveHuman != PlayObject) {
                    MoveHuman.MapRandomMove(sDenMap, 0);
                }
            }
        }
    }
}