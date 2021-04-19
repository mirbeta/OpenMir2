using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("TestSpeedMode", "", 10)]
    public class TestSpeedModeCommand : BaseCommond
    {
        [DefaultCommand]
        public void TestSpeedMode(string[] @Params, TPlayObject PlayObject)
        {
            PlayObject.m_boTestSpeedMode = !PlayObject.m_boTestSpeedMode;
            if (PlayObject.m_boTestSpeedMode)
            {
                PlayObject.SysMsg("开启速度测试模式", TMsgColor.c_Red, TMsgType.t_Hint);
            }
            else
            {
                PlayObject.SysMsg("关闭速度测试模式", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}