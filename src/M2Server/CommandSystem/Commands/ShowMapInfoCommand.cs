using M2Server.CommandSystem;
using SystemModule;

namespace M2Server
{
    [GameCommand("ShowMapInfo", "显示当前地图信息", 0)]
    public class ShowMapInfoCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowMapInfo(string[] @Params, TPlayObject PlayObject)
        {
            var sParam1 = @Params.Length > 0 ? @Params[0] : "";

            if (sParam1 != "" && sParam1[0] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMapInfoMsg, PlayObject.m_PEnvir.sMapName, PlayObject.m_PEnvir.sMapDesc), TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMapInfoSizeMsg, PlayObject.m_PEnvir.wWidth, PlayObject.m_PEnvir.wHeight), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}