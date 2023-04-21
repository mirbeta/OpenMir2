using NLog;
using SystemModule.Common;

namespace GameSrv.Robots
{
    public class RobotManage
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
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
            var sRobotName = string.Empty;
            var sScriptFileName = string.Empty;
            var sFileName = GameShare.GetEnvirFilePath("Robot.txt");
            if (!File.Exists(sFileName)) return;
            using var LoadList = new StringList();
            LoadList.LoadFromFile(sFileName);
            for (var i = 0; i < LoadList.Count; i++)
            {
                var sLineText = LoadList[i];
                if (string.IsNullOrEmpty(sLineText) || sLineText[0] == ';') continue;
                sLineText = HUtil32.GetValidStr3(sLineText, ref sRobotName, new[] { ' ', '/', '\t' });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sScriptFileName, new[] { ' ', '/', '\t' });
                if (string.IsNullOrEmpty(sRobotName) || string.IsNullOrEmpty(sScriptFileName)) continue;
                var robotHuman = new RobotObject();
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
            for (var i = 0; i < RobotHumanList.Count; i++)
            {
                RobotHumanList[i] = null;
            }
            RobotHumanList.Clear();
        }
    }
}