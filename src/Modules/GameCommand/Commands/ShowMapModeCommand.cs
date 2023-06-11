using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 显示指定地图信息
    /// </summary>
    [Command("ShowMapMode", "显示指定地图信息", "地图号", 10)]
    public class ShowMapModeCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }

            var sMapName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sMapName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }

            var envir = SystemShare.MapMgr.FindMap(sMapName);
            if (envir == null)
            {
                PlayerActor.SysMsg(sMapName + " 不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }

            var sMsg = "地图模式: " + envir.GetEnvirInfo();
            PlayerActor.SysMsg(sMsg, MsgColor.Blue, MsgType.Hint);
        }
    }
}