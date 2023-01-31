using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 拒绝发言
    /// </summary>
    [Command("PrvMsg", "拒绝发言", CommandHelp.GameCommandPrvMsgHelpMsg, 10)]
    public class PrvMsgCommand : Command
    {
        [ExecuteCommand]
        public void PrvMsg(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            for (int i = PlayObject.LockWhisperList.Count - 1; i >= 0; i--)
            {
                if (PlayObject.LockWhisperList.Count <= 0)
                {
                    break;
                }
                //if ((PlayObject.m_BlockWhisperList[i]).CompareTo((sHumanName)) == 0)
                //{
                //    PlayObject.m_BlockWhisperList.RemoveAt(i);
                //    PlayObject.SysMsg(string.Format(Settings.GameCommandPrvMsgUnLimitMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
                //    return;
                //}
            }
            PlayObject.LockWhisperList.Add(sHumanName);
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandPrvMsgLimitMsg, sHumanName), MsgColor.Green, MsgType.Hint);
        }
    }
}