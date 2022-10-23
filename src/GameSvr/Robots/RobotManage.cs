using NLog;
using SystemModule;
using SystemModule.Common;

namespace GameSvr.Robots
{
    public class RobotManage
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private IList<RobotObject> RobotHumanList;

        public RobotManage()
        {
            RobotHumanList = new List<RobotObject>();
            LoadRobot();
        }

        ~RobotManage()
        {
            UnLoadRobot();
            RobotHumanList = null;
        }

        private void LoadRobot()
        {
            var sRobotName = string.Empty;
            var sScriptFileName = string.Empty;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "Robot.txt");
            if (!File.Exists(sFileName)) return;
            using var LoadList = new StringList();
            LoadList.LoadFromFile(sFileName);
            for (var i = 0; i < LoadList.Count; i++)
            {
                var sLineText = LoadList[i];
                if (sLineText == "" || sLineText[0] == ';') continue;
                sLineText = HUtil32.GetValidStr3(sLineText, ref sRobotName, new[] { " ", "/", "\t" });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sScriptFileName, new[] { " ", "/", "\t" });
                if (sRobotName == "" || sScriptFileName == "") continue;
                var RobotHuman = new RobotObject();
                RobotHuman.ChrName = sRobotName;
                RobotHuman.ScriptFileName = sScriptFileName;
                RobotHuman.LoadScript();
                RobotHumanList.Add(RobotHuman);
            }
        }

        public void ReLoadRobot()
        {
            UnLoadRobot();
            LoadRobot();
        }

        public void Run()
        {
            const string sExceptionMsg = "[Exception] TRobotManage::Run";
            try
            {
                for (var i = RobotHumanList.Count - 1; i >= 0; i--)
                {
                    RobotHumanList[i].Run();
                }
            }
            catch (Exception e)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.Message);
            }
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

