using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 设置怪物行动速度
    /// </summary>
    [GameCommand("ChangeZenFastStep", "设置怪物行动速度", "速度", 10)]
    public class ChangeZenFastStepCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeZenFastStep(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sFastStep = @Params.Length > 0 ? @Params[0] : "";
            var nFastStep = HUtil32.Str_ToInt(sFastStep, -1);
            if (sFastStep == "" || nFastStep < 1 || sFastStep != "")
            {
                PlayObject.SysMsg("设置怪物行动速度。", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            M2Share.Config.nZenFastStep = nFastStep;
            PlayObject.SysMsg($"怪物行动速度: {nFastStep}", MsgColor.Green, MsgType.Hint);
        }
    }
}