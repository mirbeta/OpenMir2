using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家等级
    /// </summary>
    [Command("AdjuestLevel", "调整指定玩家等级", "人物名称 等级", 10)]
    public class AdjuestLevelCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            int nLevel = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (string.IsNullOrEmpty(sHumanName)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null) {
                int nOLevel = m_PlayObject.Abil.Level;
                m_PlayObject.HasLevelUp(1);
                M2Share.EventSource.AddEventLog(17, m_PlayObject.MapName + "\09" + m_PlayObject.CurrX + "\09" + m_PlayObject.CurrY + "\09"
                                                    + m_PlayObject.ChrName + "\09" + m_PlayObject.Abil.Level + "\09" + PlayObject.ChrName + "\09" + "+(" + nLevel + ")" + "\09" + "0");
                PlayObject.SysMsg(sHumanName + " 等级调整完成。", MsgColor.Green, MsgType.Hint);
                if (M2Share.Config.ShowMakeItemMsg) {
                    M2Share.Logger.Warn("[等级调整] " + PlayObject.ChrName + "(" + m_PlayObject.ChrName + " " + nOLevel + " -> " + m_PlayObject.Abil.Level + ")");
                }
            }
            else {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, new object[]
                {
                    sHumanName
                }), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}