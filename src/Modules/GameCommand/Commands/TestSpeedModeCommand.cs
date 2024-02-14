using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("TestSpeedMode", "", 10)]
    public class TestSpeedModeCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.TestSpeedMode = !PlayerActor.TestSpeedMode;
            if (PlayerActor.TestSpeedMode)
            {
                PlayerActor.SysMsg("开启速度测试模式", MsgColor.Red, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg("关闭速度测试模式", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}