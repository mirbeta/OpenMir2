using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家等级
    /// </summary>
    [Command("AdjuestLevel", "调整指定玩家等级", "人物名称 等级", 10)]
    public class AdjuestLevelCommand : Command
    {
        [ExecuteCommand]
        public void AdjuestLevel(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var nLevel = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                int nOLevel = m_PlayObject.Abil.Level;
                m_PlayObject.HasLevelUp(1);
                M2Share.EventSource.AddEventLog(17, m_PlayObject.MapName + "\09" + m_PlayObject.CurrX + "\09" + m_PlayObject.CurrY + "\09"
                                                    + m_PlayObject.ChrName + "\09" + m_PlayObject.Abil.Level + "\09" + PlayObject.ChrName + "\09" + "+(" + nLevel + ")" + "\09" + "0");
                PlayObject.SysMsg(sHumanName + " 等级调整完成。", MsgColor.Green, MsgType.Hint);
                if (M2Share.Config.ShowMakeItemMsg)
                {
                     M2Share.Log.Warn("[等级调整] " + PlayObject.ChrName + "(" + m_PlayObject.ChrName + " " + nOLevel + " -> " + m_PlayObject.Abil.Level + ")");
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, new object[]
                {
                    sHumanName
                }), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}