using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands
{
    /// <summary>
    /// 复活指定玩家
    /// </summary>
    [Command("ReAlive", "复活指定玩家", CommandHelp.GameCommandPrvMsgHelpMsg, 10)]
    public class ReAliveCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName))
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null)
            {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            mPlayObject.ReAlive();
            mPlayObject.WAbil.HP = mPlayObject.WAbil.MaxHP;
            mPlayObject.SendMsg(playObject, Messages.RM_ABILITY, 0, 0, 0, 0);
            playObject.SysMsg(string.Format(CommandHelp.GameCommandReAliveMsg, sHumanName), MsgColor.Green, MsgType.Hint);
            playObject.SysMsg(sHumanName + " 已获重生。", MsgColor.Green, MsgType.Hint);
        }
    }
}