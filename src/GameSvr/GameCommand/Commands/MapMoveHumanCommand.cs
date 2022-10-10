using GameSvr.Actor;
using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 将指定地图所有玩家随机移动
    /// </summary>
    [Command("MapMoveHuman", "将指定地图所有玩家随机移动", CommandHelp.GameCommandMapMoveHelpMsg, 10)]
    public class MapMoveHumanCommand : Command
    {
        [ExecuteCommand]
        public void MapMoveHuman(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sSrcMap = @Params.Length > 0 ? @Params[0] : "";
            var sDenMap = @Params.Length > 1 ? @Params[1] : "";
            PlayObject MoveHuman;
            if (sDenMap == "" || sSrcMap == "" || sSrcMap != "" && sSrcMap[0] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var SrcEnvir = M2Share.MapMgr.FindMap(sSrcMap);
            var DenEnvir = M2Share.MapMgr.FindMap(sDenMap);
            if (SrcEnvir == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMapMoveMapNotFound, sSrcMap), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (DenEnvir == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMapMoveMapNotFound, sDenMap), MsgColor.Red, MsgType.Hint);
                return;
            }
            IList<BaseObject> HumanList = new List<BaseObject>();
            M2Share.WorldEngine.GetMapRageHuman(SrcEnvir, SrcEnvir.Width / 2, SrcEnvir.Height / 2, 1000, HumanList);
            for (var i = 0; i < HumanList.Count; i++)
            {
                MoveHuman = (PlayObject)HumanList[i];
                if (MoveHuman != PlayObject)
                {
                    MoveHuman.MapRandomMove(sDenMap, 0);
                }
            }
        }
    }
}