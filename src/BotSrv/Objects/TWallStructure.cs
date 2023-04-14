using BotSrv.Player;

namespace BotSrv.Objects
{
    public class TWallStructure : Actor
    {
        private bool bomarkpos;

        public TWallStructure(RobotPlayer robotClient) : base(robotClient)
        {
            m_btDir = 0;
            bomarkpos = false;
        }

        public override void Run()
        {
            if (Death)
            {
                if (bomarkpos)
                {
                    robotClient.Map.MarkCanWalk(CurrX, CurrY, true);
                    bomarkpos = false;
                }
            }
            else
            {
                if (!bomarkpos)
                {
                    robotClient.Map.MarkCanWalk(CurrX, CurrY, false);
                    bomarkpos = true;
                }
            }
            robotClient.PlayScene.SetActorDrawLevel(this, 0);
            base.Run();
        }
    }
}