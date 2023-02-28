using BotSrv.Player;

namespace BotSrv.Scenes.Scene
{
    public class IntroScene : SceneBase
    {
        public long m_dwStartTime = 0;

        public IntroScene(RobotPlayer robotClient) : base(SceneType.Intro, robotClient)
        {

        }

        public override void OpenScene()
        {
            m_dwStartTime = MShare.GetTickCount() + 2 * 1000;
        }

        public override void CloseScene()
        {

        }

        public override void PlayScene()
        {
            if (MShare.GetTickCount() > m_dwStartTime)
            {
                RobotClient.DScreen.ChangeScene(SceneType.Login);
            }
        }
    }
}