using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 启用/禁止文字过滤功能
    /// </summary>
    [Command("DisableFilter", "启用/禁止文字过滤功能", 10)]
    public class DisableFilterCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            M2Share.FilterWord = !M2Share.FilterWord;
            if (M2Share.FilterWord) {
                playObject.SysMsg("已启用文字过滤。", MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg("已禁止文字过滤。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}