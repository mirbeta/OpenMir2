using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整物品属性
    /// </summary>
    [Command("RefineWeapon", "调整身上武器属性", "攻击力 魔法力 道术 准确度", 10)]
    public class RefineWeaponCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var nDc = @params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0;
            var nMc = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            var nSc = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            var nHit = @params.Length > 3 ? HUtil32.StrToInt(@params[3], 0) : 0;
            if (nDc + nMc + nSc > 10)
            {
                return;
            }
            if (PlayerActor.UseItems[ItemLocation.Weapon] == null || PlayerActor.UseItems[ItemLocation.Weapon].Index <= 0)
            {
                return;
            }
            PlayerActor.UseItems[ItemLocation.Weapon].Desc[0] = (byte)nDc;
            PlayerActor.UseItems[ItemLocation.Weapon].Desc[1] = (byte)nMc;
            PlayerActor.UseItems[ItemLocation.Weapon].Desc[2] = (byte)nSc;
            PlayerActor.UseItems[ItemLocation.Weapon].Desc[5] = (byte)nHit;
            PlayerActor.SendUpdateItem(PlayerActor.UseItems[ItemLocation.Weapon]);
            PlayerActor.RecalcAbilitys();
            PlayerActor.SendMsg(PlayerActor, Messages.RM_ABILITY, 0, 0, 0, 0);
            PlayerActor.SendMsg(PlayerActor, Messages.RM_SUBABILITY, 0, 0, 0, 0);
            SystemShare.Logger.Warn("[武器调整]" + PlayerActor.ChrName + " DC:" + nDc + " MC" + nMc + " SC" + nSc + " HIT:" + nHit);
        }
    }
}