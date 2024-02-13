using MakePlayer.Cliens;
using MakePlayer.Scenes.Scene;
using NLog;

namespace MakePlayer.Scenes
{
    public class ScreenManager
    {
        
        private readonly PlayClient client;
        public SceneType Scenetype;
        public SceneBase CurrentScene = null;

        public ScreenManager(PlayClient robotClient)
        {
            this.client = robotClient;
            CurrentScene = new IntroScene();
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
                    //CurrentScene = robotClient.IntroScene;
                    //robotClient.IntroScene.m_dwStartTime = HUtil32.GetTickCount() + 2000;
                    break;
                case SceneType.Login:
                    Scenetype = SceneType.Login;
                    CurrentScene = client.LoginScene;
                    break;
                case SceneType.SelectCountry:
                    break;
                case SceneType.SelectChr:
                    Scenetype = SceneType.SelectChr;
                    CurrentScene = client.SelectChrScene;
                    break;
                case SceneType.NewChr:
                    break;
                case SceneType.Loading:
                    break;
                case SceneType.PlayGame:
                    Scenetype = SceneType.PlayGame;
                    CurrentScene = client.PlayScene;
                    break;
            }
            if (CurrentScene != null)
            {
                CurrentScene.OpenScene();
            }
        }

        public void AddSysMsg(string msg)
        {
            LogService.Info(msg);
        }

        public static void AddChatBoardString(string str)
        {
            LogService.Info(str);
        }
    }
}