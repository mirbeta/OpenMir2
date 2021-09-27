using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 移动地图指定座标(需要戴传送装备)
    /// </summary>
    [GameCommand("UserMoveXY", "移动地图指定座标(需要戴传送装备)", 0)]
    public class UserMoveXYCommand : BaseCommond
    {
        [DefaultCommand]
        public void UserMoveXY(string[] @Params, TPlayObject PlayObject)
        {
            var sX = @Params.Length > 0 ? @Params[0] : "";
            var sY = @Params.Length > 1 ? @Params[1] : "";
            if (PlayObject.m_boTeleport)
            {
                short nX = (short)HUtil32.Str_ToInt(sX, -1);
                short nY = (short)HUtil32.Str_ToInt(sY, -1);
                if (!PlayObject.m_PEnvir.Flag.boNOPOSITIONMOVE)
                {
                    if (PlayObject.m_PEnvir.CanWalkOfItem(nX, nY, M2Share.g_Config.boUserMoveCanDupObj, M2Share.g_Config.boUserMoveCanOnItem))
                    {
                        // 10000
                        if (HUtil32.GetTickCount() - PlayObject.m_dwTeleportTick > M2Share.g_Config.dwUserMoveTime * 1000)
                        {
                            PlayObject.m_dwTeleportTick = HUtil32.GetTickCount();
                            PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                            PlayObject.SpaceMove(PlayObject.m_sMapName, nX, nY, 0);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.g_Config.dwUserMoveTime - (HUtil32.GetTickCount() - PlayObject.m_dwTeleportTick) / 1000 + "秒之后才可以再使用此功能!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                    }
                    else
                    {
                        PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandPositionMoveCanotMoveToMap, PlayObject.m_sMapName, sX, sY), TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("此地图禁止使用此命令!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}