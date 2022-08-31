using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家职业
    /// </summary>
    [GameCommand("ChangeJob", "调整指定玩家职业", GameCommandConst.g_sGameCommandChangeJobHelpMsg, 10)]
    public class ChangeJobCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeJob(string[] @params, TPlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sJobName = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || sJobName == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                if (string.Compare(sJobName, "Warr", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.m_btJob = PlayJob.Warr;
                }
                if (string.Compare(sJobName, "Wizard", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.m_btJob = PlayJob.Wizard;
                }
                if (string.Compare(sJobName, "Taos", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.m_btJob = PlayJob.Taos;
                }
                m_PlayObject.HasLevelUp(1);
                m_PlayObject.SysMsg(GameCommandConst.g_sGameCommandChangeJobHumanMsg, MsgColor.Green, MsgType.Hint);
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandChangeJobMsg, sHumanName), MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}