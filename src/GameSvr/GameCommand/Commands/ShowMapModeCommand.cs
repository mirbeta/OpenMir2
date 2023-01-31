using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 显示指定地图信息
    /// </summary>
    [Command("ShowMapMode", "显示指定地图信息", "地图号", 10)]
    public class ShowMapModeCommand : Command
    {
        [ExecuteCommand]
        public void ShowMapMode(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMapName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sMapName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Envir = M2Share.MapMgr.FindMap(sMapName);
            if (Envir == null)
            {
                PlayObject.SysMsg(sMapName + " 不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var sMsg = "地图模式: " + Envir.GetEnvirInfo();
            PlayObject.SysMsg(sMsg, MsgColor.Blue, MsgType.Hint);
        }
    }
}