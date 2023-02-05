using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 显示指定地图信息
    /// </summary>
    [Command("ShowMapMode", "显示指定地图信息", "地图号", 10)]
    public class ShowMapModeCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sMapName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sMapName))
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            Maps.Envirnoment Envir = M2Share.MapMgr.FindMap(sMapName);
            if (Envir == null)
            {
                PlayObject.SysMsg(sMapName + " 不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            string sMsg = "地图模式: " + Envir.GetEnvirInfo();
            PlayObject.SysMsg(sMsg, MsgColor.Blue, MsgType.Hint);
        }
    }
}