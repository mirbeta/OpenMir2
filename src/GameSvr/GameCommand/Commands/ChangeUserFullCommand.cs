using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整服务器最高上线人数
    /// </summary>
    [Command("ChangeUserFull", "调整服务器最高上限人数", "人数", 10)]
    public class ChangeUserFullCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sUserCount = @Params.Length > 0 ? @Params[0] : "";
            int nCount = HUtil32.StrToInt(sUserCount, -1);
            if (sUserCount == "" || nCount < 1 || !string.IsNullOrEmpty(sUserCount))
            {
                PlayObject.SysMsg("设置服务器最高上线人数。", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            M2Share.Config.UserFull = nCount;
            PlayObject.SysMsg($"服务器上线人数限制: {nCount}", MsgColor.Green, MsgType.Hint);
        }
    }
}