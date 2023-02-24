using GameSvr.Player;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 调整物品属性
    /// </summary>
    [Command("RefineWeapon", "调整身上武器属性", "攻击力 魔法力 道术 准确度", 10)]
    public class RefineWeaponCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            int nDc = @Params.Length > 0 ? Convert.ToInt32(@Params[0]) : 0;
            int nMc = @Params.Length > 1 ? Convert.ToInt32(@Params[1]) : 0;
            int nSc = @Params.Length > 2 ? Convert.ToInt32(@Params[2]) : 0;
            int nHit = @Params.Length > 3 ? Convert.ToInt32(@Params[3]) : 0;
            if (nDc + nMc + nSc > 10) {
                return;
            }
            if (PlayObject.UseItems[Grobal2.U_WEAPON] == null || PlayObject.UseItems[Grobal2.U_WEAPON].Index <= 0) {
                return;
            }
            PlayObject.UseItems[Grobal2.U_WEAPON].Desc[0] = (byte)nDc;
            PlayObject.UseItems[Grobal2.U_WEAPON].Desc[1] = (byte)nMc;
            PlayObject.UseItems[Grobal2.U_WEAPON].Desc[2] = (byte)nSc;
            PlayObject.UseItems[Grobal2.U_WEAPON].Desc[5] = (byte)nHit;
            PlayObject.SendUpdateItem(PlayObject.UseItems[Grobal2.U_WEAPON]);
            PlayObject.RecalcAbilitys();
            PlayObject.SendMsg(PlayObject, Messages.RM_ABILITY, 0, 0, 0, 0, "");
            PlayObject.SendMsg(PlayObject, Messages.RM_SUBABILITY, 0, 0, 0, 0, "");
            M2Share.Logger.Warn("[武器调整]" + PlayObject.ChrName + " DC:" + nDc + " MC" + nMc + " SC" + nSc + " HIT:" + nHit);
        }
    }
}