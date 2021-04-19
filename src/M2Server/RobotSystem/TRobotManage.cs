using mSystemModule;
using System;
using System.Collections.Generic;
using System.IO;

namespace M2Server
{
    public class RobotManage
    {
        private IList<RobotObject> _robotHumanList = null;

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
            StringList LoadList;
            var sRobotName = string.Empty;
            var sScriptFileName = string.Empty;
            var sFileName = M2Share.g_Config.sEnvirDir + "Robot.txt";
            if (!File.Exists(sFileName)) return;
            LoadList = new StringList();
            LoadList.LoadFromFile(sFileName);
            for (var i = 0; i < LoadList.Count; i++)
            {
                var sLineText = LoadList[i];
                if ((sLineText == "") || (sLineText[0] == ';')) continue;
                sLineText = HUtil32.GetValidStr3(sLineText, ref sRobotName, new string[] { " ", "/", "\t" });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sScriptFileName, new string[] { " ", "/", "\t" });
                if ((sRobotName == "") || (sScriptFileName == "")) continue;
                var RobotHuman = new RobotObject();
                RobotHuman.m_sCharName = sRobotName;
                RobotHuman.m_sScriptFileName = sScriptFileName;
                RobotHuman.LoadScript();
                _robotHumanList.Add(RobotHuman);
            }
            LoadList = null;
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
                M2Share.MainOutMessage(sExceptionMsg, MessageType.Error);
                M2Share.MainOutMessage(e.Message, MessageType.Error);
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

