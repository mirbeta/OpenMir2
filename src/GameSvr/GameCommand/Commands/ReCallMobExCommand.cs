using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 召唤指定怪物为宠物，宝宝等级直接为1级
    /// </summary>
    [Command("ReCallMobEx", "召唤宝宝", "怪物名称 名字颜色 X Y", 10)]
    public class ReCallMobExCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sMonName = @Params.Length > 0 ? @Params[0] : "";
            int nNameColor = @Params.Length > 0 ? Convert.ToInt32(@Params[1]) : 0;
            short nX = @Params.Length > 0 ? Convert.ToInt16(@Params[2]) : (short)0;
            short nY = @Params.Length > 0 ? Convert.ToInt16(@Params[3]) : (short)0;
            if (sMonName == "" || !string.IsNullOrEmpty(sMonName) && sMonName[0] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nX < 0) {
                nX = 0;
            }
            if (nY < 0) {
                nY = 0;
            }
            if (nNameColor < 0) {
                nNameColor = 0;
            }
            if (nNameColor > 255) {
                nNameColor = 255;
            }
            Actor.BaseObject mon = M2Share.WorldEngine.RegenMonsterByName(PlayObject.Envir.MapName, nX, nY, sMonName);
            if (mon != null) {
                mon.Master = PlayObject;
                mon.MasterRoyaltyTick = 86400000;// 24 * 60 * 60 * 1000
                mon.SlaveMakeLevel = 3;
                mon.SlaveExpLevel = 1;
                mon.NameColor = (byte)nNameColor;
                mon.RecalcAbilitys();
                mon.RefNameColor();
                PlayObject.SlaveList.Add(mon);
            }
        }
    }
}