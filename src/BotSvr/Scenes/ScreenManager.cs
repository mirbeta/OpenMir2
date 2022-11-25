using System;

namespace BotSvr.Scenes
{
    public class ScreenManager
    {
        private readonly RobotClient robotClient;
        public SceneBase CurrentScene = null;

        public ScreenManager(RobotClient robotClient)
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
                case SceneType.stIntro:
                    CurrentScene = robotClient.IntroScene;
                    robotClient.IntroScene.m_dwStartTime = MShare.GetTickCount() + 2000;
                    break;
                case SceneType.stLogin:
                    CurrentScene = robotClient.LoginScene;
                    break;
                case SceneType.stSelectCountry:
                    break;
                case SceneType.stSelectChr:
                    CurrentScene = robotClient.SelectChrScene;
                    break;
                case SceneType.stNewChr:
                    break;
                case SceneType.stLoading:
                    break;
                case SceneType.stPlayGame:
                    CurrentScene = robotClient.g_PlayScene;
                    break;
            }
            if (CurrentScene != null)
            {
                CurrentScene.OpenScene();
            }
        }

        public void AddSysMsg(string msg)
        {

        }

        public void AddChatBoardString(string str, int fcolor, ConsoleColor bcolor = ConsoleColor.White)
        {
            Console.BackgroundColor = bcolor;
            Console.ForegroundColor = (ConsoleColor)fcolor;
            Console.WriteLine(str);
            Console.ResetColor();
        }

        public void AddChatBoardString(string str, ConsoleColor fcolor, ConsoleColor bcolor = ConsoleColor.White)
        {
            Console.BackgroundColor = bcolor;
            Console.ForegroundColor = fcolor;
            Console.WriteLine(str);
            Console.ResetColor();
        }

        public void AddChatBoardString(string str, int fcolor, int bcolor)
        {
            Console.WriteLine(str);
        }
    }
}

