using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("TestSpeedMode", "", 10)]
    public class TestSpeedModeCommand : BaseCommond
    {
        [DefaultCommand]
        public void TestSpeedMode(TPlayObject PlayObject)
        {
            PlayObject.m_boTestSpeedMode = !PlayObject.m_boTestSpeedMode;
            if (PlayObject.m_boTestSpeedMode)
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