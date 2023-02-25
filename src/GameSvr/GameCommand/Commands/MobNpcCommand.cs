using GameSvr.Npc;
using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 在当前XY坐标创建NPC
    /// </summary>
    [Command("MobNpc", "在当前XY坐标创建NPC", CommandHelp.GameCommandMobNpcHelpMsg, 10)]
    public class MobNpcCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sParam1 = @Params.Length > 0 ? @Params[0] : "";
            string sParam2 = @Params.Length > 1 ? @Params[1] : "";
            string sParam3 = @Params.Length > 2 ? @Params[2] : "";
            string sParam4 = @Params.Length > 3 ? @Params[3] : "";
            if (string.IsNullOrEmpty(sParam1) || string.IsNullOrEmpty(sParam2) || (!string.IsNullOrEmpty(sParam1)) && sParam1[0] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            int nAppr = HUtil32.StrToInt(sParam3, 0);
            bool boIsCastle = HUtil32.StrToInt(sParam4, 0) == 1;
            if (string.IsNullOrEmpty(sParam1)) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            short nX = 0;
            short nY = 0;
            Merchant Merchant = new Merchant();
            Merchant.ChrName = sParam1;
            Merchant.MapName = PlayObject.MapName;
            Merchant.Envir = PlayObject.Envir;
            Merchant.Appr = (ushort)nAppr;
            Merchant.NpcFlag = 0;
            Merchant.CastleMerchant = boIsCastle;
            Merchant.ScriptName = sParam2;
            PlayObject.GetFrontPosition(ref nX, ref nY);
            Merchant.CurrX = nX;
            Merchant.CurrY = nY;
            Merchant.Initialize();
            Merchant.OnEnvirnomentChanged();
            M2Share.WorldEngine.AddMerchant(Merchant);
        }
    }
}