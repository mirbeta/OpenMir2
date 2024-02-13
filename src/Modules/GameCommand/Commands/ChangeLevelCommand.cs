using OpenMir2;
using SystemModule;
using SystemModule.Actors;

namespace CommandModule.Commands
{
    /// <summary>
    /// 调整自己的等级
    /// </summary>
    [Command("ChangeLevel", "调整自己的等级", "等级(1-65535)", 10)]
    public class ChangeLevelCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sParam1 = @params.Length > 0 ? @params[0] : "";
            var nLevel = HUtil32.StrToInt(sParam1, 1);
            int nOLevel = PlayerActor.Abil.Level;
            PlayerActor.Abil.Level = (byte)HUtil32._MIN(MessageSettings.MAXUPLEVEL, nLevel);
            PlayerActor.HasLevelUp(1);// 等级调整记录日志
            //   M2Share.EventSource.AddEventLog(17, PlayerActor.MapName + "\09" + PlayerActor.CurrX + "\09" + PlayerActor.CurrY+ "\09" + PlayerActor.ChrName + "\09" + PlayerActor.Abil.Level + "\09" + "0" + "\09" + "=(" + nLevel + ")" + "\09" + "0");
            if (SystemShare.Config.ShowMakeItemMsg)
            {
                LogService.Warn(string.Format(CommandHelp.GameCommandLevelConsoleMsg, PlayerActor.ChrName, nOLevel, PlayerActor.Abil.Level));
            }
        }
    }
}