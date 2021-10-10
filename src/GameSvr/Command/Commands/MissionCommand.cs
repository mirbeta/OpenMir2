using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 设置怪物集中目标
    /// </summary>
    [GameCommand("Mission", "设置怪物集中目标", 10)]
    public class MissionCommand : BaseCommond
    {
        [DefaultCommand]
        public void Mission(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sX = @Params.Length > 0 ? @Params[0] : "";
            var sY = @Params.Length > 1 ? @Params[1] : "";
            short nX;
            short nY;
            if (sX == "" || sY == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.CommandAttribute.Name + " X  Y", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nX = (short)HUtil32.Str_ToInt(sX, 0);
            nY = (short)HUtil32.Str_ToInt(sY, 0);
            M2Share.g_boMission = true;
            M2Share.g_sMissionMap = PlayObject.m_sMapName;
            M2Share.g_nMissionX = nX;
            M2Share.g_nMissionY = nY;
            PlayObject.SysMsg("怪物集中目标已设定为: " + PlayObject.m_sMapName + '(' + M2Share.g_nMissionX + ':' + M2Share.g_nMissionY + ')', TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}