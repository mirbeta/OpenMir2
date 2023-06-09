using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 启用/禁止文字过滤功能
    /// </summary>
    [Command("DisableFilter", "启用/禁止文字过滤功能", 10)]
    public class DisableFilterCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            ModuleShare.FilterWord = !ModuleShare.FilterWord;
            if (ModuleShare.FilterWord) {
                PlayerActor.SysMsg("已启用文字过滤。", MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayerActor.SysMsg("已禁止文字过滤。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}