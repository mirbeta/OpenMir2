using System;
using SystemModule;

namespace BotSvr.Scenes
{
    public class SceneBase
    {
        public SceneType Scenetype;
        public RobotClient robotClient;
        public Action? FNotifyEvent = null;
        public int m_dwNotifyEventTick = 0;
        /// <summary>
        /// 当前游戏网络连接步骤
        /// </summary>
        public TConnectionStep m_ConnectionStep;

        public SceneBase(SceneType scenetype, RobotClient robotClient)
        {
            Scenetype = scenetype;
            this.robotClient = robotClient;
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

        public void SetNotifyEvent(Action ANotifyEvent, int nTime)
        {
            m_dwNotifyEventTick = HUtil32.GetTickCount() + nTime;
            FNotifyEvent = ANotifyEvent;
        }

        public void DoNotifyEvent()
        {
            if (FNotifyEvent != null)
            {
                if (HUtil32.GetTickCount() > m_dwNotifyEventTick)
                {
                    FNotifyEvent();
                    FNotifyEvent = null;
                }
            }
        }

        public void MainOutMessage(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}