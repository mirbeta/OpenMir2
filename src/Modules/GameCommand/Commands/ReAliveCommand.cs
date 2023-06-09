using SystemModule;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    /// <summary>
    /// 复活指定玩家
    /// </summary>
    [Command("ReAlive", "复活指定玩家", CommandHelp.GameCommandPrvMsgHelpMsg, 10)]
    public class ReAliveCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName))
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
            mIPlayerActor.ReAlive();
            mIPlayerActor.WAbil.HP = mIPlayerActor.WAbil.MaxHP;
            mIPlayerActor.SendMsg(IPlayerActor, Messages.RM_ABILITY, 0, 0, 0, 0);
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandReAliveMsg, sHumanName), MsgColor.Green, MsgType.Hint);
            PlayerActor.SysMsg(sHumanName + " 已获重生。", MsgColor.Green, MsgType.Hint);
        }
    }
}