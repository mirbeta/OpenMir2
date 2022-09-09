using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 拒绝发言
    /// </summary>
    [GameCommand("PrvMsg", "拒绝发言", GameCommandConst.GameCommandPrvMsgHelpMsg, 10)]
    public class PrvMsgCommand : BaseCommond
    {
        [DefaultCommand]
        public void PrvMsg(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = PlayObject.LockWhisperList.Count - 1; i >= 0; i--)
            {
                if (PlayObject.LockWhisperList.Count <= 0)
                {
                    break;
                }
                //if ((PlayObject.m_BlockWhisperList[i]).CompareTo((sHumanName)) == 0)
                //{
                //    PlayObject.m_BlockWhisperList.RemoveAt(i);
                //    PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandPrvMsgUnLimitMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
                //    return;
                //}
            }
            PlayObject.LockWhisperList.Add(sHumanName);
            PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandPrvMsgLimitMsg, sHumanName), MsgColor.Green, MsgType.Hint);
        }
    }
}