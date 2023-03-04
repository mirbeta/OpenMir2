using BotSrv.Player;
using NLog;

namespace BotSrv.Scenes
{
    public class ScreenManager
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly RobotPlayer robotClient;
        public SceneBase CurrentScene = null;

        public ScreenManager(RobotPlayer robotClient)
        {
            this.robotClient = robotClient;
            CurrentScene = null;
        }

        public void ChangeScene(SceneType scenetype)
        {
            if (CurrentScene != null)
            {
                CurrentScene.CloseScene();
            }
            switch (scenetype)
            {
                case SceneType.Intro:
                    CurrentScene = robotClient.IntroScene;
                    robotClient.IntroScene.m_dwStartTime = MShare.GetTickCount() + 2000;
                    break;
                case SceneType.Login:
                    CurrentScene = robotClient.LoginScene;
                    break;
                case SceneType.SelectCountry:
                    break;
                case SceneType.SelectChr:
                    CurrentScene = robotClient.SelectChrScene;
                    break;
                case SceneType.NewChr:
                    break;
                case SceneType.Loading:
                    break;
                case SceneType.PlayGame:
                    CurrentScene = robotClient.PlayScene;
                    break;
            }
            if (CurrentScene != null)
            {
                CurrentScene.OpenScene();
            }
        }

        public void AddSysMsg(string msg)
        {
            logger.Info(msg);
        }

        public static void AddChatBoardString(string str)
        {
            logger.Info(str);
        }
    }
}