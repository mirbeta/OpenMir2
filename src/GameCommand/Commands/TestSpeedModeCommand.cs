using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    [Command("TestSpeedMode", "", 10)]
    public class TestSpeedModeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.TestSpeedMode = !playObject.TestSpeedMode;
            if (playObject.TestSpeedMode) {
                playObject.SysMsg("开启速度测试模式", MsgColor.Red, MsgType.Hint);
            }
            else {
                playObject.SysMsg("关闭速度测试模式", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}