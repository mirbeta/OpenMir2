using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 启用/禁止文字过滤功能
    /// </summary>
    [Command("DisableFilter", "启用/禁止文字过滤功能", 10)]
    public class DisableFilterCommand : Command
    {
        [ExecuteCommand]
        public static void DisableFilter(PlayObject PlayObject)
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