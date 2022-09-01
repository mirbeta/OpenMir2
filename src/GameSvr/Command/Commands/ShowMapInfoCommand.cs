using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("ShowMapInfo", "显示当前地图信息", 0)]
    public class ShowMapInfoCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowMapInfo(TPlayObject PlayObject)
        {
            PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandMapInfoMsg, PlayObject.m_PEnvir.MapName, PlayObject.m_PEnvir.MapDesc), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandMapInfoSizeMsg, PlayObject.m_PEnvir.Width, PlayObject.m_PEnvir.Height), MsgColor.Green, MsgType.Hint);
        }
    }
}