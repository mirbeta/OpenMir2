using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家等级
    /// </summary>
    [GameCommand("AdjuestLevel", "调整指定玩家等级", "人物名称 等级", 10)]
    public class AdjuestLevelCommand : BaseCommond
    {
        [DefaultCommand]
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
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                int nOLevel = m_PlayObject.Abil.Level;
                m_PlayObject.HasLevelUp(1);
                M2Share.AddGameDataLog("17" + "\09" + m_PlayObject.MapName + "\09" + m_PlayObject.CurrX + "\09" + m_PlayObject.CurrY + "\09"
                    + m_PlayObject.CharName + "\09" + m_PlayObject.Abil.Level + "\09" + PlayObject.CharName + "\09" + "+(" + nLevel + ")" + "\09" + "0");
                PlayObject.SysMsg(sHumanName + " 等级调整完成。", MsgColor.Green, MsgType.Hint);
                if (M2Share.Config.boShowMakeItemMsg)
                {
                    M2Share.LogSystem.Warn("[等级调整] " + PlayObject.CharName + "(" + m_PlayObject.CharName + " " + nOLevel + " -> " + m_PlayObject.Abil.Level + ")");
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sNowNotOnLineOrOnOtherServer, new object[] { sHumanName }), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}