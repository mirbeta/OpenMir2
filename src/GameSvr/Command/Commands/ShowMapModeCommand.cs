using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 显示指定地图信息
    /// </summary>
    [GameCommand("ShowMapMode", "显示指定地图信息", "地图号", 10)]
    public class ShowMapModeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowMapMode(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMapName = @Params.Length > 0 ? @Params[0] : "";
            string sMsg;
            if (string.IsNullOrEmpty(sMapName))
            {
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), MsgColor.Red, MsgType.Hint);
                return;
            }
            var Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir == null)
            {
                PlayObject.SysMsg(sMapName + " 不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            sMsg = "地图模式: " + Envir.GetEnvirInfo();
            PlayObject.SysMsg(sMsg, MsgColor.Blue, MsgType.Hint);
        }
    }
}