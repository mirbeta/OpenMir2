using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands
{
    /// <summary>
    /// 调整物品属性
    /// </summary>
    [Command("RefineWeapon", "调整身上武器属性", "攻击力 魔法力 道术 准确度", 10)]
    public class RefineWeaponCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
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
            if (playObject.UseItems[ItemLocation.Weapon] == null || playObject.UseItems[ItemLocation.Weapon].Index <= 0)
            {
                return;
            }
            playObject.UseItems[ItemLocation.Weapon].Desc[0] = (byte)nDc;
            playObject.UseItems[ItemLocation.Weapon].Desc[1] = (byte)nMc;
            playObject.UseItems[ItemLocation.Weapon].Desc[2] = (byte)nSc;
            playObject.UseItems[ItemLocation.Weapon].Desc[5] = (byte)nHit;
            playObject.SendUpdateItem(playObject.UseItems[ItemLocation.Weapon]);
            playObject.RecalcAbilitys();
            playObject.SendMsg(playObject, Messages.RM_ABILITY, 0, 0, 0, 0);
            playObject.SendMsg(playObject, Messages.RM_SUBABILITY, 0, 0, 0, 0);
            GameShare.Logger.Warn("[武器调整]" + playObject.ChrName + " DC:" + nDc + " MC" + nMc + " SC" + nSc + " HIT:" + nHit);
        }
    }
}