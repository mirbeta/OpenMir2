using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整服务器最高上线人数
    /// </summary>
    [Command("ChangeUserFull", "调整服务器最高上限人数", "人数", 10)]
    public class ChangeUserFullCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sUserCount = @params.Length > 0 ? @params[0] : "";
            int nCount = HUtil32.StrToInt(sUserCount, -1);
            if (string.IsNullOrEmpty(sUserCount) || nCount < 1 || !string.IsNullOrEmpty(sUserCount))
            {
                playObject.SysMsg("设置服务器最高上线人数。", MsgColor.Red, MsgType.Hint);
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            M2Share.Config.UserFull = nCount;
            playObject.SysMsg($"服务器上线人数限制: {nCount}", MsgColor.Green, MsgType.Hint);
        }
    }
}