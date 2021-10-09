using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 进入/退出隐身模式(进入模式后别人看不到自己)(支持权限分配)
    /// </summary>
    [GameCommand("ChangeObMode", "进入/退出隐身模式(进入模式后别人看不到自己)", 10)]
    public class ChangeObModeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeObMode(string[] @Params, TPlayObject PlayObject)
        {
            if (Params != null && Params.Length > 0)
            {
                var sParam1 = @Params.Length > 0 ? @Params[0] : "";
                if (sParam1 != "" && sParam1[0] == '?')
                {
                    PlayObject.SysMsg(CommandAttribute.CommandHelp(), TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
            }
            var boFlag = !PlayObject.m_boObMode;
            if (boFlag)
            {
                PlayObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");// 发送刷新数据到客户端，解决GM登录隐身有影子问题
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