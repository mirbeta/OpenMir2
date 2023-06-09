using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 召唤指定怪物为宠物
    /// 格式:RECALLMOB 怪物名称 宝宝等级(最高为 7) 叛变时间(分钟) 是否自动变色（0、1）固定颜色（1-7）
    /// </summary>
    [Command("RecallMob", "召唤指定怪物为宠物", "怪物名称 数量 等级(0-7) 叛变时间(分钟) 是否自动变色（0、1）固定颜色（1-7）", 10)]
    public class RecallMobCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sMonName = @params.Length > 0 ? @params[0] : "";
            var nCount = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            var nLevel = @params.Length > 2 ? HUtil32.StrToInt(@params[2], 0) : 0;
            var nTick = @params.Length > 3 ? HUtil32.StrToInt(@params[3], 0) : 86400000;
            var nAutoChangeColor = @params.Length > 4 ? HUtil32.StrToInt(@params[4], 0) : 0;
            var nFixColor = @params.Length > 5 ? HUtil32.StrToInt(@params[5], 0) : 0;
            short nX = 0;
            short nY = 0;
            if (string.IsNullOrEmpty(sMonName) || !string.IsNullOrEmpty(sMonName) && sMonName[0] == '?')
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nLevel >= 10)
            {
                nLevel = 0;
            }
            if (nCount <= 0)
            {
                nCount = 1;
            }
            for (var i = 0; i < nCount; i++)
            {
                if (PlayerActor.SlaveList.Count >= 20)
                {
                    break;
                }
                PlayerActor.GetFrontPosition(ref nX, ref nY);
                IActor mon = ModuleShare.WorldEngine.RegenMonsterByName(PlayerActor.Envir.MapName, nX, nY, sMonName);
                if (mon != null)
                {
                    mon.Master = PlayerActor;
                    //mon.IsSlave = true;
                    //mon.MasterRoyaltyTick = nTick;
                    //mon.SlaveMakeLevel = 3;
                    mon.SlaveExpLevel = (byte)nLevel;
                    if (nAutoChangeColor == 1)
                    {
                        mon.AutoChangeColor = true;
                    }
                    else if (nFixColor > 0)
                    {
                        mon.FixColor = true;
                        mon.FixColorIdx = (byte)(nFixColor - 1);
                    }
                    mon.RecalcAbilitys();
                    mon.RefNameColor();
                    PlayerActor.SlaveList.Add(mon);
                }
            }
        }
    }
}