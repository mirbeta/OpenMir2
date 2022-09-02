using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 查看指定玩家信息
    /// </summary>
    [GameCommand("HumanInfo", "查看指定玩家信息", GameCommandConst.g_sGameCommandHumanLocalHelpMsg, 10)]
    public class HumanInfoCommand : BaseCommond
    {
        [DefaultCommand]
        public void HumanInfo(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg(PlayObject.GeTBaseObjectInfo(), MsgColor.Green, MsgType.Hint);
        }
    }
}