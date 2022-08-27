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
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMapInfoMsg, PlayObject.m_PEnvir.SMapName, PlayObject.m_PEnvir.SMapDesc), MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMapInfoSizeMsg, PlayObject.m_PEnvir.WWidth, PlayObject.m_PEnvir.WHeight), MsgColor.Green, MsgType.Hint);
        }
    }
}