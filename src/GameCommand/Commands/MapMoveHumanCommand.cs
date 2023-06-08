using M2Server.Actor;
using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands
{
    /// <summary>
    /// 将指定地图所有玩家随机移动
    /// </summary>
    [Command("MapMoveHuman", "将指定地图所有玩家随机移动", CommandHelp.GameCommandMapMoveHelpMsg, 10)]
    public class MapMoveHumanCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            var sSrcMap = @params.Length > 0 ? @params[0] : "";
            var sDenMap = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sDenMap) || string.IsNullOrEmpty(sSrcMap) ||
                !string.IsNullOrEmpty(sSrcMap) && sSrcMap[0] == '?')
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var srcEnvir = M2Share.MapMgr.FindMap(sSrcMap);
            var denEnvir = M2Share.MapMgr.FindMap(sDenMap);
            if (srcEnvir == null)
            {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandMapMoveMapNotFound, sSrcMap), MsgColor.Red,
                    MsgType.Hint);
                return;
            }
            if (denEnvir == null)
            {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandMapMoveMapNotFound, sDenMap), MsgColor.Red,
                    MsgType.Hint);
                return;
            }
            IList<BaseObject> humanList = new List<BaseObject>();
            M2Share.WorldEngine.GetMapRageHuman(srcEnvir, srcEnvir.Width / 2, srcEnvir.Height / 2, 1000, ref humanList, true);
            for (var i = 0; i < humanList.Count; i++)
            {
                var moveHuman = (PlayObject)humanList[i];
                if (moveHuman != playObject)
                {
                    moveHuman.MapRandomMove(sDenMap, 0);
                }
            }
        }
    }
}