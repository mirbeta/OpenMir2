using NLog;
using SystemModule;
using SystemModule.Common;

namespace GameSvr.Robots
{
    public class RobotManage
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private IList<RobotObject> _robotHumanList;

        public RobotManage()
        {
            _robotHumanList = new List<RobotObject>();
            LoadRobot();
        }

        ~RobotManage()
        {
            UnLoadRobot();
            _robotHumanList = null;
        }

        private void LoadRobot()
        {
            var sRobotName = string.Empty;
            var sScriptFileName = string.Empty;
            var sFileName = Path.Combine(M2Share.sConfigPath, M2Share.Config.sEnvirDir, "Robot.txt");
            if (!File.Exists(sFileName)) return;
            using var LoadList = new StringList();
            LoadList.LoadFromFile(sFileName);
            for (var i = 0; i < LoadList.Count; i++)
            {
                var sLineText = LoadList[i];
                if (sLineText == "" || sLineText[0] == ';') continue;
                sLineText = HUtil32.GetValidStr3(sLineText, ref sRobotName, new string[] { " ", "/", "\t" });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sScriptFileName, new string[] { " ", "/", "\t" });
                if (sRobotName == "" || sScriptFileName == "") continue;
                var RobotHuman = new RobotObject();
                RobotHuman.CharName = sRobotName;
                RobotHuman.m_sScriptFileName = sScriptFileName;
                RobotHuman.LoadScript();
                _robotHumanList.Add(RobotHuman);
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
                for (var i = _robotHumanList.Count - 1; i >= 0; i--)
                {
                    _robotHumanList[i].Run();
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
            for (var i = 0; i < _robotHumanList.Count; i++)
            {
                _robotHumanList[i] = null;
            }
            _robotHumanList.Clear();
        }
    }
}

