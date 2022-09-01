using GameSvr.Actor;
using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 将指定地图所有玩家随机移动
    /// </summary>
    [GameCommand("MapMoveHuman", "将指定地图所有玩家随机移动", GameCommandConst.g_sGameCommandMapMoveHelpMsg, 10)]
    public class MapMoveHumanCommand : BaseCommond
    {
        [DefaultCommand]
        public void MapMoveHuman(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sSrcMap = @Params.Length > 0 ? @Params[0] : "";
            var sDenMap = @Params.Length > 1 ? @Params[1] : "";
            TPlayObject MoveHuman;
            if (sDenMap == "" || sSrcMap == "" || sSrcMap != "" && sSrcMap[0] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var SrcEnvir = M2Share.MapManager.FindMap(sSrcMap);
            var DenEnvir = M2Share.MapManager.FindMap(sDenMap);
            if (SrcEnvir == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandMapMoveMapNotFound, sSrcMap), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (DenEnvir == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandMapMoveMapNotFound, sDenMap), MsgColor.Red, MsgType.Hint);
                return;
            }
            IList<TBaseObject> HumanList = new List<TBaseObject>();
            M2Share.UserEngine.GetMapRageHuman(SrcEnvir, SrcEnvir.Width / 2, SrcEnvir.Height / 2, 1000, HumanList);
            for (var i = 0; i < HumanList.Count; i++)
            {
                MoveHuman = (TPlayObject)HumanList[i];
                if (MoveHuman != PlayObject)
                {
                    MoveHuman.MapRandomMove(sDenMap, 0);
                }
            }
            HumanList = null;
        }
    }
}