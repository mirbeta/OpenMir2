using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家经验值
    /// </summary>
    [Command("AdjuestExp", "调整指定人物的经验值", "物名称 经验值", 10)]
    public class AdjuestExpCommand : Command
    {
        [ExecuteCommand]
        public void AdjuestExp(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sExp = @Params.Length > 1 ? @Params[1] : "";
            if (string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var dwExp = HUtil32.StrToInt(sExp, 0);
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                var dwOExp = PlayObject.Abil.Exp;
                m_PlayObject.Abil.Exp = dwExp;
                m_PlayObject.HasLevelUp(m_PlayObject.Abil.Level - 1);
                PlayObject.SysMsg(sHumanName + " 经验调整完成。", MsgColor.Green, MsgType.Hint);
                if (M2Share.Config.ShowMakeItemMsg)
                {
                    M2Share.Log.Warn("[经验调整] " + PlayObject.ChrName + '(' + m_PlayObject.ChrName + ' ' + dwOExp + " -> " + m_PlayObject.Abil.Exp + ')');
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}