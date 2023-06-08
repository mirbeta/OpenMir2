using NLog;
using SystemModule;
using SystemModule.Packets.ClientPackets;

namespace MakePlayer.Scenes
{
    public class SceneBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public SceneType Scenetype;
        public Action? NotifyEvent;
        public int NotifyEventTick = 0;
        /// <summary>
        /// 当前游戏网络连接步骤
        /// </summary>
        public ConnectionStatus ConnectionStatus;

        public virtual void Initialize()
        {

        }

        public virtual void OpenScene()
        {

        }

        public virtual void CloseScene()
        {

        }

        public virtual void OpeningScene()
        {

        }

        public virtual void PlayScene()
        {

        }

        protected void SetNotifyEvent(Action notifyEvent, int nTime)
        {
            NotifyEventTick = HUtil32.GetTickCount() + nTime;
            NotifyEvent = notifyEvent;
        }

        public void DoNotifyEvent()
        {
            if (NotifyEvent != null)
            {
                if (HUtil32.GetTickCount() > NotifyEventTick)
                {
                    NotifyEvent();
                    NotifyEvent = null;
                }
            }
        }

        internal virtual void ProcessPacket(CommandMessage command, string sBody)
        {

        }

        protected void MainOutMessage(string msg)
        {
            Console.WriteLine($"{msg}");
        }
    }
}