using MakePlayer.Option;
using MakePlayer.Scenes;
using MakePlayer.Scenes.Scene;
using OpenMir2;
using SystemModule;

namespace MakePlayer.Cliens
{
    public class PlayClient
    {
        public string SessionId;
        public int RunTick;
        public string ChrName;
        /// <summary>
        /// 当前场景步骤
        /// </summary>
        public ConnectionStep ConnectionStep;
        /// <summary>
        /// 当前游戏网络连接步骤
        /// </summary>
        public ConnectionStatus ConnectionStatus;
        public string ServerName = string.Empty;
        public bool CreateAccount;
        public bool IsLogin;
        public long SayTick;
        public string SelChrAddr;
        public int SelChrPort;
        public int Certification;
        public string LoginId;
        public string LoginPasswd;
        public string RunServerAddr;
        public int RunServerPort;
        public ScreenManager DScreen;
        public LoginScene LoginScene;
        public SelectChrScene SelectChrScene;
        public GameScene PlayScene;

        public PlayClient(MakePlayOptions playOptions)
        {
            SessionId = string.Empty;
            DScreen = new ScreenManager(this);
            LoginScene = new LoginScene(this, playOptions.Address, playOptions.Port);
            SelectChrScene = new SelectChrScene(this);
            PlayScene = new GameScene(this);
            Certification = 0;
            ConnectionStep = ConnectionStep.Connect;
            ConnectionStatus = ConnectionStatus.Success;
            IsLogin = false;
            CreateAccount = false;
            LoginId = string.Empty;
            LoginPasswd = string.Empty;
            RunServerAddr = string.Empty;
            SelChrAddr = string.Empty;
            ChrName = string.Empty;
            RunTick = HUtil32.GetTickCount();
        }

        public void ProcessPacket(byte[] reviceBuffer)
        {
            var sockText = HUtil32.GetString(reviceBuffer, 0, reviceBuffer.Length);
            if (!string.IsNullOrEmpty(sockText))
            {
                while (sockText.Length >= 2)
                {
                    if (sockText.IndexOf("!", StringComparison.OrdinalIgnoreCase) <= 0)
                    {
                        break;
                    }
                    var sData = string.Empty;
                    sockText = HUtil32.ArrestStringEx(sockText, "#", "!", ref sData);
                    if (string.IsNullOrEmpty(sData))
                    {
                        break;
                    }
                    DecodeMessagePacket(sData);
                }
            }
        }

        private void DecodeMessagePacket(string sDataBlock)
        {
            if (sDataBlock[0] == '+')
            {
                return;
            }
            if (sDataBlock.Length < Messages.DefBlockSize)
            {
                return;
            }
            var sDefMsg = sDataBlock[..Messages.DefBlockSize];
            var sBody = sDataBlock.Substring(Messages.DefBlockSize, sDataBlock.Length - Messages.DefBlockSize);
            var defMsg = EDCode.DecodePacket(sDefMsg);
            DScreen.CurrentScene.ProcessPacket(defMsg, sBody);
        }

        public void Run()
        {
            if (HUtil32.GetTickCount() - RunTick > 1000)
            {
                RunTick = HUtil32.GetTickCount();
                if (DScreen.Scenetype == SceneType.Intro)
                {
                    DScreen.ChangeScene(SceneType.Login);
                    return;
                }
                DScreen.CurrentScene.DoNotifyEvent();
                switch (DScreen.Scenetype)
                {
                    case SceneType.Intro:
                        break;
                    case SceneType.Login:
                        break;
                    case SceneType.SelectCountry:
                        break;
                    case SceneType.SelectChr:
                        break;
                    case SceneType.NewChr:
                        break;
                    case SceneType.Loading:
                        break;
                    case SceneType.PlayGame:
                        PlayScene.PlayScene();
                        break;
                }
            }
        }
    }
}