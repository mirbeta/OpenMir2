using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 清除指定玩家PK值
    /// </summary>
    [Command("FreePenalty", "清除指定玩家PK值", "人物名称", 10)]
    public class FreePenaltyCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (!string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
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
            mIPlayerActor.PkPoint = 0;
            mIPlayerActor.RefNameColor();
            mIPlayerActor.SysMsg(CommandHelp.GameCommandFreePKHumanMsg, MsgColor.Green, MsgType.Hint);
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandFreePKMsg, sHumanName), MsgColor.Green, MsgType.Hint);
        }
    }
}