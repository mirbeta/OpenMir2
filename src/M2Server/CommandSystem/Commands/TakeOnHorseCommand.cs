using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("TakeOnHorse", "", 10)]
    public class TakeOnHorseCommand : BaseCommond
    {
        [DefaultCommand]
        public void TakeOnHorse(string[] @Params, TPlayObject PlayObject)
        {
            var sParam = @Params.Length > 0 ? @Params[0] : "";
            if (sParam != "" && sParam[1] == '?')
            {
                PlayObject.SysMsg("上马命令，在戴好马牌后输入此命令就可以骑上马。", TMsgColor.c_Red, TMsgType.t_Hint);
                PlayObject.SysMsg(string.Format("命令格式: @%s", this.Attributes.Name), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (PlayObject.m_boOnHorse)
            {
                return;
            }
            if (PlayObject.m_btHorseType == 0)
            {
                PlayObject.SysMsg("骑马必须先戴上马牌！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.m_boOnHorse = true;
            PlayObject.FeatureChanged();
            if (PlayObject.m_boOnHorse)
            {
                try
                {
                    // M2Share.g_FunctionNPC.GotoLable(PlayObject, "@OnHorse", false);
                }
                catch
                {
                }
            }
        }
    }
}