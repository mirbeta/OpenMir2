using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 设置怪物行动速度
    /// </summary>
    [GameCommand("ChangeZenFastStep", "设置怪物行动速度", "速度", 10)]
    public class ChangeZenFastStepCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeZenFastStep(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sFastStep = @Params.Length > 0 ? @Params[0] : "";
            var nFastStep = HUtil32.Str_ToInt(sFastStep, -1);
            if (sFastStep == "" || nFastStep < 1 || sFastStep != "")
            {
                PlayObject.SysMsg("设置怪物行动速度。", TMsgColor.c_Red, TMsgType.t_Hint);
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.g_Config.nZenFastStep = nFastStep;
            PlayObject.SysMsg($"怪物行动速度: {nFastStep}", TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}