using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家转生等级
    /// </summary>
    [GameCommand("ReNewLevel", "调整指定玩家转生等级", "人物名称 点数(为空则查看)", 10)]
    public class ReNewLevelCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReNewLevel(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sLevel = @Params.Length > 1 ? @Params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var nLevel = HUtil32.StrToInt(sLevel, -1);
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                if (nLevel >= 0 && nLevel <= 255)
                {
                    m_PlayObject.m_btReLevel = (byte)nLevel;
                    m_PlayObject.RefShowName();
                }
                PlayObject.SysMsg(sHumanName + " 的转生等级为 " + PlayObject.m_btReLevel, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(sHumanName + " 没在线上!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}