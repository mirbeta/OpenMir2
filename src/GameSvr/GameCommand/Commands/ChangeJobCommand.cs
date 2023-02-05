using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家职业
    /// </summary>
    [Command("ChangeJob", "调整指定玩家职业", CommandHelp.GameCommandChangeJobHelpMsg, 10)]
    public class ChangeJobCommand : GameCommand
    {
        [ExecuteCommand]
        public void ChangeJob(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            string sJobName = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || sJobName == "")
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                if (string.Compare(sJobName, "Warr", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.Job = PlayJob.Warrior;
                }
                if (string.Compare(sJobName, "Wizard", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.Job = PlayJob.Wizard;
                }
                if (string.Compare(sJobName, "Taos", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.Job = PlayJob.Taoist;
                }
                m_PlayObject.HasLevelUp(1);
                m_PlayObject.SysMsg(CommandHelp.GameCommandChangeJobHumanMsg, MsgColor.Green, MsgType.Hint);
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandChangeJobMsg, sHumanName), MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}