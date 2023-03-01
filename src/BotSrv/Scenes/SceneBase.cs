using System;
using BotSrv.Player;
using SystemModule;

namespace BotSrv.Scenes
{
    public class SceneBase
    {
        public SceneType Scenetype;
        public RobotPlayer RobotClient;
        public Action NotifyEvent = null;
        public int NotifyEventTick = 0;
        /// <summary>
        /// 当前游戏网络连接步骤
        /// </summary>
        public ConnectionStep ConnectionStep;

        public SceneBase(SceneType scenetype, RobotPlayer robotClient)
        {
            Scenetype = scenetype;
            this.RobotClient = robotClient;
        }

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

        protected void SetNotifyEvent(Action ANotifyEvent, int nTime)
        {
            NotifyEventTick = HUtil32.GetTickCount() + nTime;
            NotifyEvent = ANotifyEvent;
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

        protected void MainOutMessage(string msg)
        {
            BotShare.logger.Info(msg);
        }
    }
}