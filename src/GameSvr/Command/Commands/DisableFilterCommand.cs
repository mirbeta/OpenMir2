using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 启用/禁止文字过滤功能
    /// </summary>
    [GameCommand("DisableFilter", "启用/禁止文字过滤功能", 10)]
    public class DisableFilterCommand : BaseCommond
    {
        [DefaultCommand]
        public void DisableFilter(TPlayObject PlayObject)
        {
            M2Share.boFilterWord = !M2Share.boFilterWord;
            if (M2Share.boFilterWord)
            {
                PlayObject.SysMsg("已启用文字过滤。", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg("已禁止文字过滤。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}