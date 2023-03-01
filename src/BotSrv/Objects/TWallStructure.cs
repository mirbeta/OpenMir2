using BotSrv.Player;

namespace BotSrv.Objects
{
    public class TWallStructure : Actor
    {
        private const int V = 0;
        private readonly int ax = 0;
        private readonly int ay = 0;
        private readonly int bx = 0;
        private readonly int by = 0;
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