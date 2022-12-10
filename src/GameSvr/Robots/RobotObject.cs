using GameSvr.Player;
using NLog;
using SystemModule;
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
            if (M2Share.g_RobotNPC == null)
            {
                return;
            }
            var currentTime = HUtil32.GetTimestamp();
            if (currentTime >= AutoRunInfo.RunTimeTick)
            {
                switch (AutoRunInfo.RunCmd)
                {
                    case Robot.nRONPCLABLEJMP:
                        switch (AutoRunInfo.Moethod)
                        {
                            case Robot.nRODAY:
                                if ((currentTime - AutoRunInfo.RunTick) > (24 * 60 * 60 * 1000 * AutoRunInfo.nParam1))
                                {
                                    AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                    M2Share.g_RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                }
                                break;
                            case Robot.nROHOUR:
                                if ((currentTime - AutoRunInfo.RunTick) > (60 * 60 * 1000 * AutoRunInfo.nParam1))
                                {
                                    AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                    M2Share.g_RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                }
                                break;
                            case Robot.nROMIN:
                                if ((currentTime - AutoRunInfo.RunTick) > (60 * 1000 * AutoRunInfo.nParam1))
                                {
                                    AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                    M2Share.g_RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                }
                                break;
                            case Robot.nROSEC:
                                if ((currentTime - AutoRunInfo.RunTick) > (1000 * AutoRunInfo.nParam1))
                                {
                                    AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                    M2Share.g_RobotNPC.GotoLable(this, AutoRunInfo.sParam2, false);
                                }
                                break;
                            case Robot.nRUNONWEEK:
                                AutoRunInfo.RunTimeTick = HUtil32.GetTimestamp(TimeSpan.FromDays(7));
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                AutoRunOfOnWeek(AutoRunInfo);
                                break;
                            case Robot.nRUNONDAY:
                                AutoRunInfo.RunTimeTick = HUtil32.GetTimestamp(TimeSpan.FromDays(1));
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                AutoRunOfOnDay(AutoRunInfo);
                                break;
                            case Robot.nRUNONHOUR:
                                AutoRunInfo.RunTimeTick = HUtil32.GetTimestamp(TimeSpan.FromHours(1));
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                AutoRunOfOnHour(AutoRunInfo);
                                break;
                            case Robot.nRUNONMIN:
                                AutoRunInfo.RunTimeTick = HUtil32.GetTimestamp(TimeSpan.FromMinutes(1));
                                AutoRunInfo.RunTick = HUtil32.GetTimestamp();
                                AutoRunOfOnMin(AutoRunInfo);
                                break;
                            case Robot.nRUNONSEC:
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
            sLineText = HUtil32.GetValidStr3(sLineText, ref sHour, ':');
            sLineText = HUtil32.GetValidStr3(sLineText, ref sMin, ':');
            var nHour = HUtil32.StrToInt(sHour, -1);
            var nMin = HUtil32.StrToInt(sMin, -1);
            var wHour = DateTime.Now.Hour;
            var wMin = DateTime.Now.Minute;
            if (nHour >= 0 && nHour <= 24 && nMin >= 0 && nMin <= 60)
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
            sLineText = HUtil32.GetValidStr3(sLineText, ref sWeek, ':');
            sLineText = HUtil32.GetValidStr3(sLineText, ref sHour, ':');
            sLineText = HUtil32.GetValidStr3(sLineText, ref sMin, ':');
            var nWeek = HUtil32.StrToInt(sWeek, -1);
            var nHour = HUtil32.StrToInt(sHour, -1);
            var nMin = HUtil32.StrToInt(sMin, -1);
            if (nWeek >= 1 && nWeek <= 7 && nHour >= 0 && nHour <= 24 && nMin >= 0 && nMin <= 60)
            {
                var wHour = DateTime.Now.Hour;
                var wMin = DateTime.Now.Minute;
                var wWeek = DateTime.Now.DayOfWeek;
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
            for (var i = 0; i < AutoRunList.Count; i++)
            {
                AutoRunList[i] = null;
            }
            AutoRunList.Clear();
        }

        public void LoadScript()
        {
            string sLineText;
            var sActionType = string.Empty;
            var sRunCmd = string.Empty;
            var sMoethod = string.Empty;
            var sParam1 = string.Empty;
            var sParam2 = string.Empty;
            var sParam3 = string.Empty;
            var sParam4 = string.Empty;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "Robot_def", $"{ScriptFileName}.txt");
            if (File.Exists(sFileName))
            {
                var LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
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
                                var AutoRunInfo = new AutoRunInfo();
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
                                    if (DateTimeOffset.TryParse(sParam1, out var runWeekTime))
                                    {
                                        AutoRunInfo.RunTimeTick = GetSundayDate(runWeekTime).ToUnixTimeMilliseconds();
                                    }
                                    else
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONDAY, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nRUNONDAY;
                                    if (DateTimeOffset.TryParse(sParam1, out var runDayTime))
                                    {
                                        AutoRunInfo.RunTimeTick = runDayTime.ToUnixTimeMilliseconds();
                                    }
                                    else
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONHOUR, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nRUNONHOUR;
                                    if (int.TryParse(sParam1,out var runHour))
                                    {
                                        AutoRunInfo.RunTimeTick = DateTimeOffset.Now.AddHours(runHour).ToUnixTimeMilliseconds();
                                    }
                                    else
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONMIN, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nRUNONMIN;
                                    if (int.TryParse(sParam1,out var runMin))
                                    {
                                        AutoRunInfo.RunTimeTick = DateTimeOffset.Now.AddMinutes(runMin).ToUnixTimeMilliseconds();
                                    }
                                    else
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONSEC, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    AutoRunInfo.Moethod = Robot.nRUNONSEC;
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

        private void OutErrorMessage(string sActionType, string sRunCmd, string sParam1, string sParam2, string sParam3, string sParam4)
        {
            _logger.Error($"机器人脚本错误 ActionType:{sActionType} RunCmd:{sRunCmd} Params1:{sParam1} Params2:{sParam2} Params3:{sParam3} Params4:{sParam4}");
        }

        /// <summary>
        /// 计算某日结束日期（礼拜日的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>
        private DateTimeOffset GetSundayDate(DateTimeOffset someDate)
        {
            var i = (7 - (int)someDate.DayOfWeek);
            return someDate.Add(new TimeSpan(i, 0, 0, 0));
        }

        private void ProcessAutoRun()
        {
            for (var i = AutoRunList.Count - 1; i >= 0; i--)
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

        internal override void SendSocket(ClientMesaagePacket DefMsg, string sMsg)
        {

        }
    }
}