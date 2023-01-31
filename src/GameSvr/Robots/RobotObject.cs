using GameSvr.Player;
using NLog;
using SystemModule.Common;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Robots
{
    public class RobotObject : PlayObject
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public string ScriptFileName = string.Empty;
        private readonly char[] LoadSriptSpitConst = new[] { ' ', '/', '\t' };
        private readonly IList<AutoRunInfo> AutoRunList;

        public RobotObject() : base()
        {
            AutoRunList = new List<AutoRunInfo>();
            this.SuperMan = true;
        }

        ~RobotObject()
        {
            ClearScript();
        }

        private void AutoRun(AutoRunInfo AutoRunInfo)
        {
            if (M2Share.RobotNPC == null)
            {
                return;
            }
            long currentTime = HUtil32.GetTimestamp();
            if (currentTime >= AutoRunInfo.RunTimeTick)
            {
                switch (AutoRunInfo.RunCmd)
                {
                    case Robot.nRONPCLABLEJMP:
                        switch (AutoRunInfo.Moethod)
                        {
                            case Robot.nRODAY:
                                AutoRunInfo.RunTimeTick = DateTimeOffset.Now.AddDays(1).ToUnixTimeSeconds();
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                M2Share.RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                break;
                            case Robot.nROHOUR:
                                AutoRunInfo.RunTimeTick = DateTimeOffset.Now.AddHours(1).ToUnixTimeSeconds();
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                M2Share.RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                break;
                            case Robot.nROMIN:
                                AutoRunInfo.RunTimeTick = DateTimeOffset.Now.AddMinutes(1).ToUnixTimeSeconds();
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                M2Share.RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                break;
                            case Robot.nROSEC:
                                AutoRunInfo.RunTimeTick = DateTimeOffset.Now.AddSeconds(1).ToUnixTimeSeconds();
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                M2Share.RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                break;
                            case Robot.nRUNONWEEK:
                                GetWeekTime(AutoRunInfo.sParam1, ref AutoRunInfo.RunTimeTick);
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                AutoRunOfOnWeek(AutoRunInfo);
                                break;
                            case Robot.nRUNONDAY:
                                GetDayTime(AutoRunInfo.sParam1, ref AutoRunInfo.RunTimeTick);
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                AutoRunOfOnDay(AutoRunInfo);
                                break;
                            case Robot.nRUNONHOUR:
                                GetHourTime(AutoRunInfo.sParam1, ref AutoRunInfo.RunTimeTick);
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                AutoRunOfOnHour(AutoRunInfo);
                                break;
                            case Robot.nRUNONMIN:
                                GetMinuteTime(AutoRunInfo.sParam1, ref AutoRunInfo.RunTimeTick);
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                AutoRunOfOnMin(AutoRunInfo);
                                break;
                            case Robot.nRUNONSEC:
                                GetSecondTime(AutoRunInfo.sParam1, ref AutoRunInfo.RunTimeTick);
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
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
            string sMin = string.Empty;
            string sHour = string.Empty;
            string sLineText = AutoRunInfo.sParam1;
            sLineText = HUtil32.GetValidStr3(sLineText, ref sHour, ':');
            sLineText = HUtil32.GetValidStr3(sLineText, ref sMin, ':');
            int nHour = HUtil32.StrToInt(sHour, -1);
            int nMin = HUtil32.StrToInt(sMin, -1);
            int wHour = DateTime.Now.Hour;
            int wMin = DateTime.Now.Minute;
            if (nHour >= 0 && nHour <= 24 && nMin >= 0 && nMin <= 60)
            {
                if (wHour == nHour)
                {
                    if (wMin == nMin)
                    {
                        if (AutoRunInfo.boStatus) return;
                        M2Share.RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                        AutoRunInfo.boStatus = true;
                    }
                    else
                    {
                        AutoRunInfo.boStatus = false;
                    }
                }
            }
        }

        private static void AutoRunOfOnHour(AutoRunInfo AutoRunInfo)
        {
        }

        private static void AutoRunOfOnMin(AutoRunInfo AutoRunInfo)
        {
        }

        private static void AutoRunOfOnSec(AutoRunInfo AutoRunInfo)
        {
        }

        private void AutoRunOfOnWeek(AutoRunInfo AutoRunInfo)
        {
            string sMin = string.Empty;
            string sHour = string.Empty;
            string sWeek = string.Empty;
            string sLineText = AutoRunInfo.sParam1;
            sLineText = HUtil32.GetValidStr3(sLineText, ref sWeek, ':');
            sLineText = HUtil32.GetValidStr3(sLineText, ref sHour, ':');
            sLineText = HUtil32.GetValidStr3(sLineText, ref sMin, ':');
            int nWeek = HUtil32.StrToInt(sWeek, -1);
            int nHour = HUtil32.StrToInt(sHour, -1);
            int nMin = HUtil32.StrToInt(sMin, -1);
            if (nWeek >= 1 && nWeek <= 7 && nHour >= 0 && nHour <= 24 && nMin >= 0 && nMin <= 60)
            {
                int wHour = DateTime.Now.Hour;
                int wMin = DateTime.Now.Minute;
                DayOfWeek wWeek = DateTime.Now.DayOfWeek;
                if ((int)wWeek == nWeek && wHour == nHour)
                {
                    if (wMin == nMin)
                    {
                        if (AutoRunInfo.boStatus) return;
                        M2Share.RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
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
            for (int i = 0; i < AutoRunList.Count; i++)
            {
                AutoRunList[i] = null;
            }
            AutoRunList.Clear();
        }

        public void LoadScript()
        {
            string sLineText;
            string sActionType = string.Empty;
            string sRunCmd = string.Empty;
            string sMoethod = string.Empty;
            string sParam1 = string.Empty;
            string sParam2 = string.Empty;
            string sParam3 = string.Empty;
            string sParam4 = string.Empty;
            string sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "Robot_def", $"{ScriptFileName}.txt");
            if (File.Exists(sFileName))
            {
                StringList LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (int i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if (!string.IsNullOrEmpty(sLineText) && sLineText[0] != ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sActionType, LoadSriptSpitConst);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sRunCmd, LoadSriptSpitConst);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMoethod, LoadSriptSpitConst);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sParam1, LoadSriptSpitConst);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sParam2, LoadSriptSpitConst);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sParam3, LoadSriptSpitConst);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sParam4, LoadSriptSpitConst);
                        if (string.Compare(sActionType, Robot.sROAUTORUN, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (string.Compare(sRunCmd, Robot.sRONPCLABLEJMP, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                AutoRunInfo AutoRunInfo = new AutoRunInfo();
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                AutoRunInfo.RunTimeTick = 0;
                                AutoRunInfo.boStatus = false;
                                AutoRunInfo.RunCmd = Robot.nRONPCLABLEJMP;
                                if (string.Compare(sMoethod, Robot.sRODAY, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nRODAY;
                                }
                                if (string.Compare(sMoethod, Robot.sROHOUR, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nROHOUR;
                                }
                                if (string.Compare(sMoethod, Robot.sROMIN, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nROMIN;
                                }
                                if (string.Compare(sMoethod, Robot.sROSEC, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nROSEC;
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONWEEK, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nRUNONWEEK;
                                    if (!GetWeekTime(sParam1, ref AutoRunInfo.RunTimeTick))
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONDAY, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nRUNONDAY;
                                    if (!GetDayTime(sParam1, ref AutoRunInfo.RunTimeTick))
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONHOUR, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nRUNONHOUR;
                                    if (!GetHourTime(sParam1, ref AutoRunInfo.RunTimeTick))
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONMIN, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nRUNONMIN;
                                    if (!GetMinuteTime(sParam1, ref AutoRunInfo.RunTimeTick))
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONSEC, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nRUNONSEC;
                                    if (!GetSecondTime(sParam1, ref AutoRunInfo.RunTimeTick))
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                AutoRunInfo.sParam1 = sParam1;
                                AutoRunInfo.sParam2 = sParam2;
                                AutoRunInfo.sParam3 = sParam3;
                                AutoRunInfo.sParam4 = sParam4;
                                AutoRunInfo.nParam1 = HUtil32.StrToInt(sParam1, 1);
                                AutoRunList.Add(AutoRunInfo);
                            }
                        }
                    }
                }
            }
        }

        private bool GetWeekTime(string param, ref long runTime)
        {
            if (!DateTimeOffset.TryParse(param, out DateTimeOffset runWeekTime)) return false;
            runTime = GetSundayDate(runWeekTime).ToUnixTimeMilliseconds();
            return true;
        }

        private static bool GetDayTime(string param, ref long runTime)
        {
            if (!DateTimeOffset.TryParse(param, out DateTimeOffset runDayTime)) return false;
            runTime = runDayTime.ToUnixTimeMilliseconds();
            return true;
        }

        private static bool GetHourTime(string param, ref long runTime)
        {
            if (!int.TryParse(param, out int runHour)) return false;
            runTime = DateTimeOffset.Now.AddHours(runHour).ToUnixTimeMilliseconds();
            return true;
        }

        private static bool GetMinuteTime(string param, ref long runTime)
        {
            if (!int.TryParse(param, out int runHour)) return false;
            runTime = DateTimeOffset.Now.AddMinutes(runHour).ToUnixTimeMilliseconds();
            return true;
        }

        private static bool GetSecondTime(string param, ref long runTime)
        {
            if (!int.TryParse(param, out int runHour)) return false;
            runTime = DateTimeOffset.Now.AddSeconds(runHour).ToUnixTimeMilliseconds();
            return true;
        }

        private void OutErrorMessage(string sActionType, string sRunCmd, string sParam1, string sParam2, string sParam3, string sParam4)
        {
            _logger.Error($"机器人脚本错误 ActionType:{sActionType} RunCmd:{sRunCmd} Params1:{sParam1} Params2:{sParam2} Params3:{sParam3} Params4:{sParam4}");
        }

        /// <summary>
        /// 计算某日结束日期（礼拜日的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>
        private static DateTimeOffset GetSundayDate(DateTimeOffset someDate)
        {
            int i = (7 - (int)someDate.DayOfWeek);
            return someDate.Add(new TimeSpan(i, 0, 0, 0));
        }

        private void ProcessAutoRun()
        {
            for (int i = AutoRunList.Count - 1; i >= 0; i--)
            {
                AutoRun(AutoRunList[i]);
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

        internal override void SendSocket(CommandPacket DefMsg, string sMsg)
        {

        }
    }
}