using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家经验值
    /// </summary>
    [Command("AdjuestExp", "调整指定人物的经验值", "物名称 经验值", 10)]
    public class AdjuestExpCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sExp = @Params.Length > 1 ? @Params[1] : "";
            if (string.IsNullOrEmpty(sHumanName)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            int dwExp = HUtil32.StrToInt(sExp, 0);
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null) {
                int dwOExp = PlayObject.Abil.Exp;
                m_PlayObject.Abil.Exp = dwExp;
                m_PlayObject.HasLevelUp(m_PlayObject.Abil.Level - 1);
                PlayObject.SysMsg(sHumanName + " 经验调整完成。", MsgColor.Green, MsgType.Hint);
                if (M2Share.Config.ShowMakeItemMsg) {
                    M2Share.Logger.Warn("[经验调整] " + PlayObject.ChrName + '(' + m_PlayObject.ChrName + ' ' + dwOExp + " -> " + m_PlayObject.Abil.Exp + ')');
                }
            }
            else {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}