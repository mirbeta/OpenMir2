using System;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;
using SystemModule.Packages;

namespace M2Server
{
    public class RobotObject: TPlayObject
    {
        public string m_sScriptFileName = string.Empty;
        private IList<AutoRunInfo> m_AutoRunList = null;

        private void AutoRun(AutoRunInfo AutoRunInfo)
        {
            if (M2Share.g_RobotNPC == null)
            {
                return;
            }
            if (HUtil32.GetTickCount()- AutoRunInfo.dwRunTick > AutoRunInfo.dwRunTimeLen)
            {
                switch(AutoRunInfo.nRunCmd)
                {
                    case ObjRobot.nRONPCLABLEJMP:
                        switch(AutoRunInfo.nMoethod)
                        {
                            case ObjRobot.nRODAY:
                                if (HUtil32.GetTickCount()- AutoRunInfo.dwRunTick > 24 * 60 * 60 * 1000 * AutoRunInfo.nParam1)
                                {
                                    AutoRunInfo.dwRunTick = HUtil32.GetTickCount();
                                    M2Share.g_RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                }
                                break;
                            case ObjRobot.nROHOUR:
                                if (HUtil32.GetTickCount()- AutoRunInfo.dwRunTick > 60 * 60 * 1000 * AutoRunInfo.nParam1)
                                {
                                    AutoRunInfo.dwRunTick = HUtil32.GetTickCount();
                                    M2Share.g_RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                }
                                break;
                            case ObjRobot.nROMIN:
                                if (HUtil32.GetTickCount()- AutoRunInfo.dwRunTick > 60 * 1000 * AutoRunInfo.nParam1)
                                {
                                    AutoRunInfo.dwRunTick = HUtil32.GetTickCount();
                                    M2Share.g_RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                }
                                break;
                            case ObjRobot.nROSEC:
                                if (HUtil32.GetTickCount()- AutoRunInfo.dwRunTick > 1000 * AutoRunInfo.nParam1)
                                {
                                    AutoRunInfo.dwRunTick = HUtil32.GetTickCount();
                                    M2Share.g_RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                }
                                break;
                            case ObjRobot.nRUNONWEEK:
                                AutoRunOfOnWeek(AutoRunInfo);
                                break;
                            case ObjRobot.nRUNONDAY:
                                AutoRunOfOnDay(AutoRunInfo);
                                break;
                            case ObjRobot.nRUNONHOUR:
                                AutoRunOfOnHour(AutoRunInfo);
                                break;
                            case ObjRobot.nRUNONMIN:
                                AutoRunOfOnMin(AutoRunInfo);
                                break;
                            case ObjRobot.nRUNONSEC:
                                AutoRunOfOnSec(AutoRunInfo);
                                break;
                        }
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }
        }

        private void AutoRunOfOnDay(AutoRunInfo AutoRunInfo)
        {
            var sMin = string.Empty;
            var sHour = string.Empty;
            var sLineText = AutoRunInfo.sParam1;
            sLineText = HUtil32.GetValidStr3(sLineText, ref sHour, ":");
            sLineText = HUtil32.GetValidStr3(sLineText, ref sMin, ":");
            var nHour = HUtil32.Str_ToInt(sHour, -1);
            var nMin = HUtil32.Str_ToInt(sMin, -1);
            var sLabel = AutoRunInfo.sParam2;
            var wHour = DateTime.Now.Hour;
            var wMin = DateTime.Now.Minute;
            var wSec = DateTime.Now.Second;
            var wMSec = DateTime.Now.Millisecond;
            if (nHour >= 0 && nHour<= 24 && nMin >= 0 && nMin<= 60)
            {
                if (wHour == nHour)
                {
                    if (wMin == nMin)
                    {
                        if (AutoRunInfo.boStatus) return;
                        M2Share.g_RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                        AutoRunInfo.boStatus = true;
                    }
                    else
                    {
                        AutoRunInfo.boStatus = false;
                    }
                }
            }
        }

        private void AutoRunOfOnHour(AutoRunInfo AutoRunInfo)
        {
        }

        private void AutoRunOfOnMin(AutoRunInfo AutoRunInfo)
        {
        }

        private void AutoRunOfOnSec(AutoRunInfo AutoRunInfo)
        {
        }

        private void AutoRunOfOnWeek(AutoRunInfo AutoRunInfo)
        {
            var sMin = string.Empty;
            var sHour = string.Empty;
            var sWeek = string.Empty;
            var sLineText = AutoRunInfo.sParam1;
            sLineText = HUtil32.GetValidStr3(sLineText, ref sWeek, ":");
            sLineText = HUtil32.GetValidStr3(sLineText, ref sHour, ":");
            sLineText = HUtil32.GetValidStr3(sLineText, ref sMin, ":");
            var nWeek = HUtil32.Str_ToInt(sWeek, -1);
            var nHour = HUtil32.Str_ToInt(sHour, -1);
            var nMin = HUtil32.Str_ToInt(sMin, -1);
            var sLabel = AutoRunInfo.sParam2;
            var wHour = DateTime.Now.Hour;
            var wMin = DateTime.Now.Minute;
            var wSec = DateTime.Now.Second;
            var wMSec = DateTime.Now.Millisecond;
            var wWeek = DateTime.Now.DayOfWeek;
            if (nWeek >= 1 && nWeek<= 7 && nHour >= 0 && nHour<= 24 && nMin >= 0 && nMin<= 60)
            {
                if ((int)wWeek == nWeek && wHour == nHour)
                {
                    if (wMin == nMin)
                    {
                        if (AutoRunInfo.boStatus) return;
                        M2Share.g_RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                        AutoRunInfo.boStatus = true;
                    }
                    else
                    {
                        AutoRunInfo.boStatus = false;
                    }
                }
            }
        }

        private void ClearScript()
        {
            for (var i = 0; i < m_AutoRunList.Count; i ++ )
            {
                m_AutoRunList[i] = null;
            }
            m_AutoRunList.Clear();
        }

        public RobotObject() : base()
        {
            m_AutoRunList = new List<AutoRunInfo>();
            this.m_boSuperMan = true;
        }

        ~RobotObject()
        {
            ClearScript();
            m_AutoRunList = null;
        }

        public void LoadScript()
        {
            StringList LoadList;
            string sLineText;
            var sActionType = string.Empty;
            var sRunCmd = string.Empty;
            var sMoethod = string.Empty;
            var sParam1 = string.Empty;
            var sParam2 = string.Empty;
            var sParam3 = string.Empty;
            var sParam4 = string.Empty;
            AutoRunInfo AutoRunInfo;
            var sFileName = Path.Combine(M2Share.g_Config.sEnvirDir, "Robot_def", $"{m_sScriptFileName}.txt");
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if (sLineText != "" && sLineText[0] != ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sActionType, new string[] { " ", "/", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sRunCmd, new string[] { " ", "/", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMoethod, new string[] { " ", "/", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sParam1, new string[] { " ", "/", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sParam2, new string[] { " ", "/", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sParam3, new string[] { " ", "/", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sParam4, new string[] { " ", "/", "\t" });
                        if (string.Compare(sActionType.ToLower(), ObjRobot.sROAUTORUN.ToLower(), StringComparison.Ordinal) == 0)
                        {
                            if (string.Compare(sRunCmd.ToLower(), ObjRobot.sRONPCLABLEJMP.ToLower(), StringComparison.Ordinal) == 0)
                            {
                                AutoRunInfo = new AutoRunInfo();
                                AutoRunInfo.dwRunTick = HUtil32.GetTickCount();
                                AutoRunInfo.dwRunTimeLen = 0;
                                AutoRunInfo.boStatus = false;
                                AutoRunInfo.nRunCmd = ObjRobot.nRONPCLABLEJMP;
                                if (string.Compare(sMoethod.ToLower(), ObjRobot.sRODAY.ToLower(), StringComparison.Ordinal) == 0)
                                {
                                    AutoRunInfo.nMoethod = ObjRobot.nRODAY;
                                }
                                if (string.Compare(sMoethod.ToLower(), ObjRobot.sROHOUR.ToLower(), StringComparison.Ordinal) == 0)
                                {
                                    AutoRunInfo.nMoethod = ObjRobot.nROHOUR;
                                }
                                if (string.Compare(sMoethod.ToLower(), ObjRobot.sROMIN.ToLower(), StringComparison.Ordinal) == 0)
                                {
                                    AutoRunInfo.nMoethod = ObjRobot.nROMIN;
                                }
                                if (string.Compare(sMoethod.ToLower(), ObjRobot.sROSEC.ToLower(), StringComparison.Ordinal) == 0)
                                {
                                    AutoRunInfo.nMoethod = ObjRobot.nROSEC;
                                }
                                if (string.Compare(sMoethod.ToLower(), ObjRobot.sRUNONWEEK.ToLower(), StringComparison.Ordinal) == 0)
                                {
                                    AutoRunInfo.nMoethod = ObjRobot.nRUNONWEEK;
                                }
                                if (string.Compare(sMoethod.ToLower(), ObjRobot.sRUNONDAY.ToLower(), StringComparison.Ordinal) == 0)
                                {
                                    AutoRunInfo.nMoethod = ObjRobot.nRUNONDAY;
                                }
                                if (string.Compare(sMoethod.ToLower(), ObjRobot.sRUNONHOUR.ToLower(), StringComparison.Ordinal) == 0)
                                {
                                    AutoRunInfo.nMoethod = ObjRobot.nRUNONHOUR;
                                }
                                if (string.Compare(sMoethod.ToLower(), ObjRobot.sRUNONMIN.ToLower(), StringComparison.Ordinal) == 0)
                                {
                                    AutoRunInfo.nMoethod = ObjRobot.nRUNONMIN;
                                }
                                if (string.Compare(sMoethod.ToLower(), ObjRobot.sRUNONSEC.ToLower(), StringComparison.Ordinal) == 0)
                                {
                                    AutoRunInfo.nMoethod = ObjRobot.nRUNONSEC;
                                }
                                AutoRunInfo.sParam1 = sParam1;
                                AutoRunInfo.sParam2 = sParam2;
                                AutoRunInfo.sParam3 = sParam3;
                                AutoRunInfo.sParam4 = sParam4;
                                AutoRunInfo.nParam1 = HUtil32.Str_ToInt(sParam1, 1);
                                m_AutoRunList.Add(AutoRunInfo);
                            }
                        }
                    }
                }
                LoadList = null;
            }
        }

        private void ProcessAutoRun()
        {
            AutoRunInfo AutoRunInfo;
            for (var i = m_AutoRunList.Count - 1; i >= 0; i--)
            {
                AutoRunInfo = m_AutoRunList[i];
                AutoRun(AutoRunInfo);
            }
        }

        public void ReloadScript()
        {
            ClearScript();
            LoadScript();
        }

        public override void Run()
        {
            ProcessAutoRun();
        }

        internal override void SendSocket(TDefaultMessage DefMsg, string sMsg)
        {
            
        }
    }
}

