using NLog;
using SystemModule.Common;

namespace GameSrv.Robots {
    public class RobotManage {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private IList<RobotObject> RobotHumanList;

        public RobotManage() {
            RobotHumanList = new List<RobotObject>();
            LoadRobot();
        }

        ~RobotManage() {
            UnLoadRobot();
            RobotHumanList = null;
        }

        private void LoadRobot() {
            string sRobotName = string.Empty;
            string sScriptFileName = string.Empty;
            string sFileName = M2Share.GetEnvirFilePath("Robot.txt");
            if (!File.Exists(sFileName)) return;
            using StringList LoadList = new StringList();
            LoadList.LoadFromFile(sFileName);
            for (int i = 0; i < LoadList.Count; i++) {
                string sLineText = LoadList[i];
                if (string.IsNullOrEmpty(sLineText) || sLineText[0] == ';') continue;
                sLineText = HUtil32.GetValidStr3(sLineText, ref sRobotName, new[] { ' ', '/', '\t' });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sScriptFileName, new[] { ' ', '/', '\t' });
                if (string.IsNullOrEmpty(sRobotName) || string.IsNullOrEmpty(sScriptFileName)) continue;
                RobotObject RobotHuman = new RobotObject();
                RobotHuman.ChrName = sRobotName;
                RobotHuman.ScriptFileName = sScriptFileName;
                RobotHuman.LoadScript();
                RobotHumanList.Add(RobotHuman);
            }
        }

        public void ReLoadRobot() {
            UnLoadRobot();
            LoadRobot();
        }

        public void Run() {
            const string sExceptionMsg = "[Exception] TRobotManage::Run";
            try {
                for (int i = RobotHumanList.Count - 1; i >= 0; i--) {
                    RobotHumanList[i].Run();
                }
            }
            catch (Exception e) {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.Message);
            }
        }

        private void UnLoadRobot() {
            for (int i = 0; i < RobotHumanList.Count; i++) {
                RobotHumanList[i] = null;
            }
            RobotHumanList.Clear();
        }
    }
}

