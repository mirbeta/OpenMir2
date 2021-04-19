using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 进入/退出隐身模式(进入模式后别人看不到自己)(支持权限分配)
    /// </summary>
    [GameCommand("ChangeObMode", "进入/退出隐身模式(进入模式后别人看不到自己)(支持权限分配)", 10)]
    public class ChangeObModeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeObMode(string[] @Params, TPlayObject PlayObject)
        {
            var nPermission = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            var sParam1 = @Params.Length > 1 ? @Params[1] : "";
            var boFlag = @Params.Length > 2 ? bool.Parse(@Params[2]) : false;

            if ((sParam1 != "") && (sParam1[0] == '?'))
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (boFlag)
            {
                PlayObject.SendRefMsg(grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");// 01/21 强行发送刷新数据到客户端，解决GM登录隐身有影子问题
            }
            PlayObject.m_boObMode = boFlag;
            if (PlayObject.m_boObMode)
            {
                PlayObject.SysMsg(M2Share.sObserverMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                PlayObject.SysMsg(M2Share.g_sReleaseObserverMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}