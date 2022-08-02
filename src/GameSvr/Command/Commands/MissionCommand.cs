using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 设置怪物集中目标
    /// </summary>
    [GameCommand("Mission", "设置怪物集中目标", " X Y", 10)]
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
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            nX = (short)HUtil32.Str_ToInt(sX, 0);
            nY = (short)HUtil32.Str_ToInt(sY, 0);
            M2Share.g_boMission = true;
            M2Share.g_sMissionMap = PlayObject.m_sMapName;
            M2Share.g_nMissionX = nX;
            M2Share.g_nMissionY = nY;
            PlayObject.SysMsg("怪物集中目标已设定为: " + PlayObject.m_sMapName + '(' + M2Share.g_nMissionX + ':' + M2Share.g_nMissionY + ')', MsgColor.Green, MsgType.Hint);
        }
    }
}