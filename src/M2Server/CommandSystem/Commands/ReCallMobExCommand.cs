using System;
using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 召唤宝宝，宝宝等级直接为1级
    /// </summary>
    [GameCommand("ReCallMobEx", "改完玩家发型", 10)]
    public class ReCallMobExCommand : BaseCommond
    {
        [DefaultCommand]
        public unsafe void ReCallMobEx(string[] @Params, TPlayObject PlayObject)
        {
            var sMonName = @Params.Length > 0 ? @Params[0] : "";
            var nNameColor = @Params.Length > 0 ? Convert.ToInt32(@Params[1]) : 0;
            var nX = @Params.Length > 0 ? Convert.ToInt16(@Params[2]) : (short)0;
            var nY = @Params.Length > 0 ? Convert.ToInt16(@Params[3]) : (short)0;
            TBaseObject mon;
            if (PlayObject.m_btPermission < this.Attributes.nPermissionMin)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if ((sMonName == "") || ((sMonName != "") && (sMonName[0] == '?')))
            {
                //PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandRecallMobExHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (nX < 0)
            {
                nX = 0;
            }
            if (nY < 0)
            {
                nY = 0;
            }
            if (nNameColor < 0)
            {
                nNameColor = 0;
            }
            if (nNameColor > 255)
            {
                nNameColor = 255;
            }
            mon = M2Share.UserEngine.RegenMonsterByName(PlayObject.m_PEnvir.sMapName, nX, nY, sMonName);
            if (mon != null)
            {
                mon.m_Master = PlayObject;
                mon.m_dwMasterRoyaltyTick = 86400000;// 24 * 60 * 60 * 1000
                mon.m_btSlaveMakeLevel = 3;
                mon.m_btSlaveExpLevel = 1;
                mon.m_btNameColor = (byte)nNameColor;
                mon.RecalcAbilitys();
                mon.RefNameColor();
                PlayObject.m_SlaveList.Add(mon);
            }
        }
    }
}