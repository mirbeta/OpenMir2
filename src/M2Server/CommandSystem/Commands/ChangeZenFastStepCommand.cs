using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 设置怪物行动速度
    /// </summary>
    [GameCommand("ChangeZenFastStep", "设置怪物行动速度", 10)]
    public class ChangeZenFastStepCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeZenFastStep(string[] @Params, TPlayObject PlayObject)
        {
            var sFastStep = @Params.Length > 0 ? @Params[0] : "";

            int nFastStep;
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
            nFastStep = HUtil32.Str_ToInt(sFastStep, -1);
            if ((sFastStep == "") || (nFastStep < 1) || ((sFastStep != "") && (sFastStep[0] == '?')))
            {
                PlayObject.SysMsg("设置怪物行动速度。", TMsgColor.c_Red, TMsgType.t_Hint);
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 速度", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.g_Config.nZenFastStep = nFastStep;
            PlayObject.SysMsg(string.Format("怪物行动速度: {0}", nFastStep), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}