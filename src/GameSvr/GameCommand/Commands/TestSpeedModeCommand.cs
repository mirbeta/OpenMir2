using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("TestSpeedMode", "", 10)]
    public class TestSpeedModeCommand : GameCommand
    {
        [ExecuteCommand]
        public static void TestSpeedMode(PlayObject PlayObject)
        {
            PlayObject.TestSpeedMode = !PlayObject.TestSpeedMode;
            if (PlayObject.TestSpeedMode)
            {
                PlayObject.SysMsg("开启速度测试模式", MsgColor.Red, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg("关闭速度测试模式", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}