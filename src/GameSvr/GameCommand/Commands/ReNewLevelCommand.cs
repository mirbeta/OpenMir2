using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家转生等级
    /// </summary>
    [Command("ReNewLevel", "调整指定玩家转生等级", "人物名称 点数(为空则查看)", 10)]
    public class ReNewLevelCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sLevel = @Params.Length > 1 ? @Params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            int nLevel = HUtil32.StrToInt(sLevel, -1);
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null) {
                if (nLevel >= 0 && nLevel <= 255) {
                    m_PlayObject.ReLevel = (byte)nLevel;
                    m_PlayObject.RefShowName();
                }
                PlayObject.SysMsg(sHumanName + " 的转生等级为 " + PlayObject.ReLevel, MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayObject.SysMsg(sHumanName + " 没在线上!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}