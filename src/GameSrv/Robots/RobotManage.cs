namespace GameSrv.Robots
{
    public class RobotManage : IAutoBotSystem
    {

        private readonly IList<RobotObject> RobotHumanList;

        public RobotManage()
        {
            RobotHumanList = new List<RobotObject>();
            LoadRobot();
        }

        ~RobotManage()
        {
            UnLoadRobot();
        }

        public IList<RobotObject> Robots => RobotHumanList;

        private void LoadRobot()
        {
            string sRobotName = string.Empty;
            string sScriptFileName = string.Empty;
            string sFileName = M2Share.GetEnvirFilePath("Robot.txt");
            if (!File.Exists(sFileName))
            {
                return;
            }

            using StringList LoadList = new StringList();
            LoadList.LoadFromFile(sFileName);
            for (int i = 0; i < LoadList.Count; i++)
            {
                string sLineText = LoadList[i];
                if (string.IsNullOrEmpty(sLineText) || sLineText[0] == ';')
                {
                    continue;
                }

                sLineText = HUtil32.GetValidStr3(sLineText, ref sRobotName, new[] { ' ', '/', '\t' });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sScriptFileName, new[] { ' ', '/', '\t' });
                if (string.IsNullOrEmpty(sRobotName) || string.IsNullOrEmpty(sScriptFileName))
                {
                    continue;
                }

                RobotObject robotHuman = new RobotObject();
                robotHuman.ChrName = sRobotName;
                robotHuman.ScriptFileName = sScriptFileName;
                robotHuman.LoadScript();
                RobotHumanList.Add(robotHuman);
            }
        }

        public void ReLoadRobot()
        {
            UnLoadRobot();
            LoadRobot();
        }

        private void UnLoadRobot()
        {
            for (int i = 0; i < RobotHumanList.Count; i++)
            {
                RobotHumanList[i] = null;
            }
            RobotHumanList.Clear();
        }

        public void Run()
        {
            for (int i = 0; i < RobotHumanList.Count; i++)
            {
                RobotHumanList[i].Run();
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < RobotHumanList.Count; i++)
            {
                RobotHumanList[i].Initialize();
            }
        }
    }
}