using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 复活指定玩家
    /// </summary>
    [Command("ReAlive", "复活指定玩家", CommandHelp.GameCommandPrvMsgHelpMsg, 10)]
    public class ReAliveCommand : Command
    {
        [ExecuteCommand]
        public void ReAlive(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            m_PlayObject.ReAlive();
            m_PlayObject.WAbil.HP = m_PlayObject.WAbil.MaxHP;
            m_PlayObject.SendMsg(m_PlayObject, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandReAliveMsg, sHumanName), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(sHumanName + " 已获重生。", MsgColor.Green, MsgType.Hint);
        }
    }
}