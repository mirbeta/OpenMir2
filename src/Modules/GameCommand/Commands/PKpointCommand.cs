using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 查看指定玩家PK值
    /// </summary>
    [Command("PKpoint", "查看指定玩家PK值", CommandHelp.GameCommandPKPointHelpMsg, 10)]
    public class PKpointCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (!string.IsNullOrEmpty(sHumanName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandPKPointMsg, sHumanName, mIPlayerActor.PkPoint), MsgColor.Green, MsgType.Hint);
        }
    }
}