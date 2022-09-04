using GameSvr.Player;
using SystemModule;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整自己的等级
    /// </summary>
    [GameCommand("ChangeLevel", "调整自己的等级", "等级(1-65535)", 10)]
    public class ChangeLevelCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeLevel(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sParam1 = @Params.Length > 0 ? @Params[0] : "";
            var nLevel = HUtil32.Str_ToInt(sParam1, 1);
            int nOLevel = PlayObject.Abil.Level;
            PlayObject.Abil.Level = (byte)HUtil32._MIN(M2Share.MAXUPLEVEL, nLevel);
            PlayObject.HasLevelUp(1);// 等级调整记录日志
            M2Share.AddGameDataLog("17" + "\09" + PlayObject.MapName + "\09" + PlayObject.CurrX + "\09" + PlayObject.CurrY
                + "\09" + PlayObject.CharName + "\09" + PlayObject.Abil.Level + "\09" + "0" + "\09" + "=(" + nLevel + ")" + "\09" + "0");
            if (M2Share.g_Config.boShowMakeItemMsg)
            {
                M2Share.LogSystem.Warn(string.Format(GameCommandConst.g_sGameCommandLevelConsoleMsg, PlayObject.CharName, nOLevel, PlayObject.Abil.Level));
            }
        }
    }
}