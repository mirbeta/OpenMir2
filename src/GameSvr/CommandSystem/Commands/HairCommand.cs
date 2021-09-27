using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("Hair", "改完玩家发型", 10)]
    public class HairCommand : BaseCommond
    {
        [DefaultCommand]
        public void Hair(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var nHair = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (string.IsNullOrEmpty(sHumanName) || nHair < 0)
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称 类型值", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                m_PlayObject.m_btHair = (byte)nHair;
                m_PlayObject.FeatureChanged();
                PlayObject.SysMsg(sHumanName + " 的头发已改变。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}