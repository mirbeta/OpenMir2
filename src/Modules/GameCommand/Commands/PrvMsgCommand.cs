using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 拒绝发言
    /// </summary>
    [Command("PrvMsg", "拒绝发言", CommandHelp.GameCommandPrvMsgHelpMsg, 10)]
    public class PrvMsgCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = PlayerActor.LockWhisperList.Count - 1; i >= 0; i--)
            {
                if (PlayerActor.LockWhisperList.Count <= 0)
                {
                    break;
                }
                //if ((PlayerActor.SysMsgm_BlockWhisperList[i]).CompareTo((sHumanName)) == 0)
                //{
                //    PlayerActor.SysMsgm_BlockWhisperList.RemoveAt(i);
                //    PlayerActor.SysMsg(string.Format(Settings.GameCommandPrvMsgUnLimitMsg, sHumanName), MsgColor.c_Green, MsgType.t_Hint);
                //    return;
                //}
            }
            PlayerActor.LockWhisperList.Add(sHumanName);
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandPrvMsgLimitMsg, sHumanName), MsgColor.Green, MsgType.Hint);
        }
    }
}