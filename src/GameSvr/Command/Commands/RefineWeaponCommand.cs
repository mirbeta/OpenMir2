using GameSvr.Player;
using SystemModule;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整物品属性
    /// </summary>
    [GameCommand("RefineWeapon", "调整身上武器属性", "攻击力 魔法力 道术 准确度", 10)]
    public class RefineWeaponCommand : BaseCommond
    {
        [DefaultCommand]
        public void RefineWeapon(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var nDc = @Params.Length > 0 ? Convert.ToInt32(@Params[0]) : 0;
            var nMc = @Params.Length > 1 ? Convert.ToInt32(@Params[1]) : 0;
            var nSc = @Params.Length > 2 ? Convert.ToInt32(@Params[2]) : 0;
            var nHit = @Params.Length > 3 ? Convert.ToInt32(@Params[3]) : 0;
            if (nDc + nMc + nSc > 10)
            {
                return;
            }
            if (PlayObject.UseItems[Grobal2.U_WEAPON] == null || PlayObject.UseItems[Grobal2.U_WEAPON].wIndex <= 0)
            {
                return;
            }
            PlayObject.UseItems[Grobal2.U_WEAPON].btValue[0] = (byte)nDc;
            PlayObject.UseItems[Grobal2.U_WEAPON].btValue[1] = (byte)nMc;
            PlayObject.UseItems[Grobal2.U_WEAPON].btValue[2] = (byte)nSc;
            PlayObject.UseItems[Grobal2.U_WEAPON].btValue[5] = (byte)nHit;
            PlayObject.SendUpdateItem(PlayObject.UseItems[Grobal2.U_WEAPON]);
            PlayObject.RecalcAbilitys();
            PlayObject.SendMsg(PlayObject, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            PlayObject.SendMsg(PlayObject, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
            M2Share.LogSystem.Warn("[武器调整]" + PlayObject.CharName + " DC:" + nDc + " MC" + nMc + " SC" + nSc + " HIT:" + nHit);
        }
    }
}