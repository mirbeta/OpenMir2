using GameSrv.Actor;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 召唤指定怪物为宠物
    /// 格式:RECALLMOB 怪物名称 宝宝等级(最高为 7) 叛变时间(分钟) 是否自动变色（0、1）固定颜色（1-7）
    /// </summary>
    [Command("RecallMob", "召唤指定怪物为宠物", "怪物名称 数量 等级(0-7) 叛变时间(分钟) 是否自动变色（0、1）固定颜色（1-7）", 10)]
    public class RecallMobCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (Params == null) {
                return;
            }
            string sMonName = @Params.Length > 0 ? @Params[0] : "";
            int nCount = @Params.Length > 1 ? HUtil32.StrToInt(@Params[1],0) : 0;
            int nLevel = @Params.Length > 2 ? HUtil32.StrToInt(@Params[2],0) : 0;
            int nTick = @Params.Length > 3 ? HUtil32.StrToInt(@Params[3],0) : 86400000;
            int nAutoChangeColor = @Params.Length > 4 ? HUtil32.StrToInt(@Params[4],0) : 0;
            int nFixColor = @Params.Length > 5 ? HUtil32.StrToInt(@Params[5], 0) : 0;
            short nX = 0;
            short nY = 0;
            BaseObject mon;
            if (string.IsNullOrEmpty(sMonName) || !string.IsNullOrEmpty(sMonName) && sMonName[0] == '?')
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nLevel >= 10) {
                nLevel = 0;
            }
            if (nCount <= 0) {
                nCount = 1;
            }
            for (int i = 0; i < nCount; i++) {
                if (PlayObject.SlaveList.Count >= 20) {
                    break;
                }
                PlayObject.GetFrontPosition(ref nX, ref nY);
                mon = M2Share.WorldEngine.RegenMonsterByName(PlayObject.Envir.MapName, nX, nY, sMonName);
                if (mon != null) {
                    mon.Master = PlayObject;
                    mon.IsSlave = true;
                    mon.MasterRoyaltyTick = nTick;
                    mon.SlaveMakeLevel = 3;
                    mon.SlaveExpLevel = (byte)nLevel;
                    if (nAutoChangeColor == 1) {
                        mon.AutoChangeColor = true;
                    }
                    else if (nFixColor > 0) {
                        mon.FixColor = true;
                        mon.FixColorIdx = (byte)(nFixColor - 1);
                    }
                    mon.RecalcAbilitys();
                    mon.RefNameColor();
                    PlayObject.SlaveList.Add(mon);
                }
            }
        }
    }
}