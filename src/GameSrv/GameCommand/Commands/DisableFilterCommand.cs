using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 启用/禁止文字过滤功能
    /// </summary>
    [Command("DisableFilter", "启用/禁止文字过滤功能", 10)]
    public class DisableFilterCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            GameShare.FilterWord = !GameShare.FilterWord;
            if (GameShare.FilterWord) {
                playObject.SysMsg("已启用文字过滤。", MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg("已禁止文字过滤。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}