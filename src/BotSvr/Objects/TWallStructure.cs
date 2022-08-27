namespace BotSvr.Objects
{
    public class TWallStructure : TActor
    {
        private const int V = 0;
        private readonly int ax = 0;
        private readonly int ay = 0;
        private readonly int bx = 0;
        private readonly int by = 0;
        private bool bomarkpos;

        public TWallStructure(RobotClient robotClient) : base(robotClient)
        {
            m_btDir = 0;
            bomarkpos = false;
        }

        public override void Run()
        {
            if (m_boDeath)
            {
                if (bomarkpos)
                {
                    robotClient.Map.MarkCanWalk(m_nCurrX, m_nCurrY, true);
                    bomarkpos = false;
                }
            }
            else
            {
                if (!bomarkpos)
                {
                    robotClient.Map.MarkCanWalk(m_nCurrX, m_nCurrY, false);
                    bomarkpos = true;
                }
            }

            robotClient.g_PlayScene.SetActorDrawLevel(this, 0);
            base.Run();
        }
    }
}