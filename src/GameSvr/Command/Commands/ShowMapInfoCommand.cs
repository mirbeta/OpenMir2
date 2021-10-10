using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    [GameCommand("ShowMapInfo", "显示当前地图信息", 0)]
    public class ShowMapInfoCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowMapInfo(TPlayObject PlayObject)
        {
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMapInfoMsg, PlayObject.m_PEnvir.sMapName, PlayObject.m_PEnvir.sMapDesc), TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMapInfoSizeMsg, PlayObject.m_PEnvir.wWidth, PlayObject.m_PEnvir.wHeight), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}