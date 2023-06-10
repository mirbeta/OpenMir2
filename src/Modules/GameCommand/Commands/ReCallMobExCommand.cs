using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 召唤指定怪物为宠物，宝宝等级直接为1级
    /// </summary>
    [Command("ReCallMobEx", "召唤宝宝", "怪物名称 名字颜色 X Y", 10)]
    public class ReCallMobExCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sMonName = @params.Length > 0 ? @params[0] : "";
            var nNameColor = @params.Length > 0 ? HUtil32.StrToInt(@params[1], 0) : 0;
            var nX = (short)(@params.Length > 0 ? HUtil32.StrToInt(@params[2], 0) : 0);
            var nY = (short)(@params.Length > 0 ? HUtil32.StrToInt(@params[3], 0) : 0);
            if (string.IsNullOrEmpty(sMonName) || !string.IsNullOrEmpty(sMonName) && sMonName[0] == '?')
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
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
            var mon = SystemShare.WorldEngine.RegenMonsterByName(PlayerActor.Envir.MapName, nX, nY, sMonName);
            if (mon != null)
            {
                mon.Master = PlayerActor;
                //mon.MasterRoyaltyTick = 86400000;// 24 * 60 * 60 * 1000
                //mon.SlaveMakeLevel = 3;
                mon.SlaveExpLevel = 1;
                mon.NameColor = (byte)nNameColor;
                mon.RecalcAbilitys();
                mon.RefNameColor();
                PlayerActor.SlaveList.Add(mon);
            }
        }
    }
}