using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 启用/禁止文字过滤功能
    /// </summary>
    [GameCommand("DisableFilter", "启用/禁止文字过滤功能", 10)]
    public class DisableFilterCommand : BaseCommond
    {
        [DefaultCommand]
        public void DisableFilter(string[] @Params, TPlayObject PlayObject)
        {
            var sParam1 = @Params.Length > 0 ? @Params[0] : "";
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
            if (sParam1 != "" && sParam1[0] == '?')
            {
                PlayObject.SysMsg("启用/禁止文字过滤功能。", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.boFilterWord = !M2Share.boFilterWord;
            if (M2Share.boFilterWord)
            {
                PlayObject.SysMsg("已启用文字过滤。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                PlayObject.SysMsg("已禁止文字过滤。", TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}