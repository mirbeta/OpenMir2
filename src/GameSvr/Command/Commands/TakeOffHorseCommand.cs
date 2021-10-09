using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("TakeOffHorse", desc: "下马命令，在骑马状态输入此命令下马。", 10)]
    public class TakeOffHorseCommand : BaseCommond
    {
        [DefaultCommand]
        public void TakeOffHorse(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sParam = @Params.Length > 0 ? @Params[0] : "";
            if (sParam != "" && sParam[0] == '?')
            {
                PlayObject.SysMsg("下马命令，在骑马状态输入此命令下马。", TMsgColor.c_Red, TMsgType.t_Hint);
                PlayObject.SysMsg($"命令格式: @{this.CommandAttribute.Name}", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (!PlayObject.m_boOnHorse)
            {
                return;
            }
            PlayObject.m_boOnHorse = false;
            PlayObject.FeatureChanged();
        }
    }
}