using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 移动地图指定座标(需要戴传送装备)
    /// </summary>
    [GameCommand("UserMoveXY", "移动地图指定座标(需要戴传送装备)", 10)]
    public class UserMoveXYCommand : BaseCommond
    {
        [DefaultCommand]
        public unsafe void UserMoveXY(string[] @Params, TPlayObject PlayObject)
        {
            var sX = @Params.Length > 0 ? @Params[0] : "";
            var sY = @Params.Length > 1 ? @Params[1] : "";
            //if (PlayObject.m_boTeleport)
            //{
            //    int nX = HUtil32.Str_ToInt(sX, -1);
            //    int nY = HUtil32.Str_ToInt(sY, -1);
            //    if (!PlayObject.m_PEnvir.m_boNOPOSITIONMOVE)
            //    {
            //        if (PlayObject.m_PEnvir.CanWalkOfItem(nX, nY, M2Share.g_Config.boUserMoveCanDupObj, M2Share.g_Config.boUserMoveCanOnItem))
            //        {
            //            if ((HUtil32.GetTickCount() - PlayObject.m_dwTeleportTick) > M2Share.g_Config.dwUserMoveTime * 1000)
            //            {
            //                PlayObject.m_dwTeleportTick = HUtil32.GetTickCount();
            //                if ((PlayObject.m_UseItems[Grobal2.U_BUJUK].wIndex > 0) && (PlayObject.m_UseItems[Grobal2.U_BUJUK].Dura > 0))// 增加传送符功能
            //                {
            //                    if (PlayObject.m_UseItems[Grobal2.U_BUJUK].Dura > 100)
            //                    {
            //                        PlayObject.m_UseItems[Grobal2.U_BUJUK].Dura -= 100;
            //                    }
            //                    else
            //                    {
            //                        PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_BUJUK]);  // 如果使用完，则删除物品
            //                        PlayObject.m_UseItems[Grobal2.U_BUJUK].Dura = 0;
            //                    }
            //                    PlayObject.SendMsg(PlayObject, Grobal2.RM_DURACHANGE, Grobal2.U_BUJUK, PlayObject.m_UseItems[Grobal2.U_BUJUK].Dura,
            //                        PlayObject.m_UseItems[Grobal2.U_BUJUK].DuraMax, 0, "");
            //                    PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
            //                    if ((nX < 0) || (nY < 0))
            //                    {
            //                        PlayObject.RandomMove();
            //                    }
            //                    else
            //                    {
            //                        if (PlayObject.m_PEnvir.CanWalk(nX, nY, false))
            //                        {
            //                            PlayObject.SpaceMove(PlayObject.m_sMapName, nX, nY, 0);
            //                        }
            //                    }
            //                    return;
            //                }
            //                PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
            //                if ((nX < 0) || (nY < 0))
            //                {
            //                    PlayObject.RandomMove();
            //                }
            //                else
            //                {
            //                    if (PlayObject.m_PEnvir.CanWalk(nX, nY, false))
            //                    {
            //                        PlayObject.SpaceMove(PlayObject.m_sMapName, nX, nY, 0);
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                PlayObject.SysMsg(M2Share.g_Config.dwUserMoveTime - (HUtil32.GetTickCount() - PlayObject.m_dwTeleportTick) / 1000
            //                    + "秒之后才可以再使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            //            }
            //        }
            //        else
            //        {
            //            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandPositionMoveCanotMoveToMap, PlayObject.m_sMapName, sX, sY), TMsgColor.c_Green, TMsgType.t_Hint);
            //        }
            //    }
            //    else
            //    {
            //        PlayObject.SysMsg("此地图禁止使用此命令！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            //    }
            //}
            //else
            //{
            //    PlayObject.SysMsg("您现在还无法使用此功能！！！", TMsgColor.c_Red, TMsgType.t_Hint);
            //}
        }
    }
}