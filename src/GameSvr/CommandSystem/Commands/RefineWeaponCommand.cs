using System;
using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 调整物品属性
    /// </summary>
    [GameCommand("RefineWeapon", "调整物品属性", 10)]
    public class RefineWeaponCommand : BaseCommond
    {
        [DefaultCommand]
        public void RefineWeapon(string[] @Params, TPlayObject PlayObject)
        {
            var nDc = @Params.Length > 0 ? Convert.ToInt32(@Params[0]) : 0;
            var nMc = @Params.Length > 1 ? Convert.ToInt32(@Params[1]) : 0;
            var nSc = @Params.Length > 2 ? Convert.ToInt32(@Params[2]) : 0;
            var nHit = @Params.Length > 3 ? Convert.ToInt32(@Params[3]) : 0;
            if (nDc + nMc + nSc > 10)
            {
                return;
            }
            if (PlayObject.m_UseItems[Grobal2.U_WEAPON] == null && PlayObject.m_UseItems[Grobal2.U_WEAPON].wIndex <= 0)
            {
                return;
            }
            PlayObject.m_UseItems[Grobal2.U_WEAPON].btValue[0] = (byte)nDc;
            PlayObject.m_UseItems[Grobal2.U_WEAPON].btValue[1] = (byte)nMc;
            PlayObject.m_UseItems[Grobal2.U_WEAPON].btValue[2] = (byte)nSc;
            PlayObject.m_UseItems[Grobal2.U_WEAPON].btValue[5] = (byte)nHit;
            PlayObject.SendUpdateItem(PlayObject.m_UseItems[Grobal2.U_WEAPON]);
            PlayObject.RecalcAbilitys();
            PlayObject.SendMsg(PlayObject, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            PlayObject.SendMsg(PlayObject, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
            M2Share.MainOutMessage("[武器调整]" + PlayObject.m_sCharName + " DC:" + nDc + " MC" + nMc + " SC" + nSc
                + " HIT:" + nHit);
        }
    }
}