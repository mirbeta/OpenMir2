using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整服务器最高上线人数
    /// </summary>
    [Command("ChangeUserFull", "调整服务器最高上限人数", "人数", 10)]
    public class ChangeUserFullCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sUserCount = @params.Length > 0 ? @params[0] : "";
            var nCount = HUtil32.StrToInt(sUserCount, -1);
            if (string.IsNullOrEmpty(sUserCount) || nCount < 1 || !string.IsNullOrEmpty(sUserCount))
            {
                PlayerActor.SysMsg("设置服务器最高上线人数。", MsgColor.Red, MsgType.Hint);
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            SystemShare.Config.UserFull = nCount;
            PlayerActor.SysMsg($"服务器上线人数限制: {nCount}", MsgColor.Green, MsgType.Hint);
        }
    }
}