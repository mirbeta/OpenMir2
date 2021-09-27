using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 设定怪物集中点
    /// </summary>
    [GameCommand("MobPlace", "设定怪物集中点", 10)]
    public class MobPlaceCommand : BaseCommond
    {
        [DefaultCommand]
        public void MobPlace(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sX = @Params.Length > 0 ? @Params[0] : "";
            var sY = @Params.Length > 1 ? @Params[1] : "";
            var sMonName = @Params.Length > 2 ? @Params[2] : "";
            var sCount = @Params.Length > 3 ? @Params[3] : "";
            var nCount = HUtil32._MIN(500, HUtil32.Str_ToInt(sCount, 0));
            var nX = (short)HUtil32.Str_ToInt(sX, 0);
            var nY = (short)HUtil32.Str_ToInt(sY, 0);
            TEnvirnoment MEnvir;
            TBaseObject mon = null;
            byte nCode;
            nCode = 0;
            try
            {
                nCount = HUtil32._MIN(500, HUtil32.Str_ToInt(sCount, 0));
                nX = (short)HUtil32.Str_ToInt(sX, 0);
                nY = (short)HUtil32.Str_ToInt(sY, 0);
                if (nX <= 0 || nY <= 0 || sMonName == "" || nCount <= 0)
                {
                    PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " X  Y 怪物名称 怪物数量", TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                nCode = 1;
                MEnvir = M2Share.g_MapManager.FindMap(M2Share.g_sMissionMap);
                if (!M2Share.g_boMission || MEnvir == null)
                {
                    PlayObject.SysMsg("还没有设定怪物集中点!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                    PlayObject.SysMsg("请先用命令" + this.Attributes.Name + "设置怪物的集中点。", TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                nCode = 2;
                for (var i = 0; i < nCount; i++)
                {
                    nCode = 3;
                    mon = M2Share.UserEngine.RegenMonsterByName(M2Share.g_sMissionMap, nX, nY, sMonName);
                    nCode = 4;
                    if (mon != null)
                    {
                        nCode = 5;
                        mon.m_boMission = true;
                        mon.m_nMissionX = M2Share.g_nMissionX;
                        mon.m_nMissionY = M2Share.g_nMissionY;
                    }
                    else
                    {
                        break;
                    }
                }
                nCode = 6;
                if (mon.m_btRaceServer != 136)
                {
                    PlayObject.SysMsg(nCount + " 只 " + sMonName + " 已正在往地图 " + M2Share.g_sMissionMap + " " +
                        M2Share.g_nMissionX + ":" + M2Share.g_nMissionY + " 集中。", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            catch
            {
                M2Share.MainOutMessage("[异常] TPlayObject.CmdMobPlace Code:" + nCode);
            }
        }
    }
}