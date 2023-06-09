using M2Server.Player;
using NLog;
using SystemModule.Common;
using SystemModule.Enums;

namespace M2Server.Robots
{
    public class RobotObject : PlayObject {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly char[] LoadSriptSpitConst = new[] { ' ', '/', '\t' };
        private readonly IList<AutoRunInfo> _autoRunList;
        public string ScriptFileName = string.Empty;

        public RobotObject()
        {
            _autoRunList = new List<AutoRunInfo>(20);
            this.SuperMan = true;
            this.Race = ActorRace.RoBot;
        }

        ~RobotObject() {
            ClearScript();
        }

        private void AutoRun(AutoRunInfo autoRunInfo) {
            var currentTime = HUtil32.GetTimestamp();
            if (currentTime >= autoRunInfo.RunTimeTick)
            {
                switch (autoRunInfo.Moethod)
                {
                    //case Robot.nRODAY:
                    //    autoRunInfo.RunTimeTick = DateTimeOffset.Now.AddDays(1).ToUnixTimeMilliseconds();
                    //    autoRunInfo.RunTick = HUtil32.GetTimestamp();
                    //    GameShare.ScriptEngine.GotoLable(this, M2Share.RobotNPC.ActorId, autoRunInfo.sParam2, false);
                    //    break;
                    //case Robot.nROHOUR:
                    //    autoRunInfo.RunTimeTick = DateTimeOffset.Now.AddHours(1).ToUnixTimeMilliseconds();
                    //    autoRunInfo.RunTick = HUtil32.GetTimestamp();
                    //    GameShare.ScriptEngine.GotoLable(this, M2Share.RobotNPC.ActorId, autoRunInfo.sParam2, false);
                    //    break;
                    //case Robot.nROMIN:
                    //    autoRunInfo.RunTimeTick = DateTimeOffset.Now.AddMinutes(1).ToUnixTimeMilliseconds();
                    //    autoRunInfo.RunTick = HUtil32.GetTimestamp();
                    //    GameShare.ScriptEngine.GotoLable(this, M2Share.RobotNPC.ActorId, autoRunInfo.sParam2, false);
                    //    break;
                    //case Robot.nROSEC:
                    //    autoRunInfo.RunTimeTick = DateTimeOffset.Now.AddSeconds(1).ToUnixTimeMilliseconds();
                    //    autoRunInfo.RunTick = HUtil32.GetTimestamp();
                    //    GameShare.ScriptEngine.GotoLable(this, M2Share.RobotNPC.ActorId, autoRunInfo.sParam2, false);
                    //    break;
                    case Robot.nRUNONWEEK:
                        GetWeekTime(autoRunInfo.sParam1, ref autoRunInfo.RunTimeTick);
                        autoRunInfo.RunTick = HUtil32.GetTimestamp();
                        AutoRunOfOnWeek(autoRunInfo);
                        break;
                    case Robot.nRUNONDAY:
                        GetDayTime(autoRunInfo.sParam1, ref autoRunInfo.RunTimeTick);
                        autoRunInfo.RunTick = HUtil32.GetTimestamp();
                        AutoRunOfOnDay(autoRunInfo);
                        break;
                    case Robot.nRUNONHOUR:
                        GetHourTime(autoRunInfo.sParam1, ref autoRunInfo.RunTimeTick);
                        autoRunInfo.RunTick = HUtil32.GetTimestamp();
                        AutoRunOfOnHour(autoRunInfo);
                        break;
                    case Robot.nRUNONMIN:
                        GetMinuteTime(autoRunInfo.sParam1, ref autoRunInfo.RunTimeTick);
                        autoRunInfo.RunTick = HUtil32.GetTimestamp();
                        AutoRunOfOnMin(autoRunInfo);
                        break;
                    case Robot.nRUNONSEC:
                        GetSecondTime(autoRunInfo.sParam1, ref autoRunInfo.RunTimeTick);
                        autoRunInfo.RunTick = HUtil32.GetTimestamp();
                        AutoRunOfOnSec(autoRunInfo);
                        break;
                }
            }
        }

        private void AutoRunOfOnDay(AutoRunInfo autoRunInfo) {
            var sMin = string.Empty;
            var sHour = string.Empty;
            var sLineText = autoRunInfo.sParam1;
            sLineText = HUtil32.GetValidStr3(sLineText, ref sHour, ':');
            sLineText = HUtil32.GetValidStr3(sLineText, ref sMin, ':');
            var nHour = HUtil32.StrToInt(sHour, -1);
            var nMin = HUtil32.StrToInt(sMin, -1);
            var wHour = DateTime.Now.Hour;
            var wMin = DateTime.Now.Minute;
            if (nHour >= 0 && nHour <= 24 && nMin >= 0 && nMin <= 60) {
                if (wHour == nHour) {
                    if (wMin == nMin) {
                        if (autoRunInfo.boStatus) return;
                       // GameShare.ScriptEngine.GotoLable(this, M2Share.RobotNPC.ActorId, autoRunInfo.sParam2, false);
                        autoRunInfo.boStatus = true;
                    }
                    else {
                        autoRunInfo.boStatus = false;
                    }
                }
            }
        }

        private static void AutoRunOfOnHour(AutoRunInfo autoRunInfo) {
        }

        private void AutoRunOfOnMin(AutoRunInfo autoRunInfo) {
            var sMin = string.Empty;
            var sLineText = autoRunInfo.sParam1;
            sLineText = HUtil32.GetValidStr3(sLineText, ref sMin, ':');
            var nMin = HUtil32.StrToInt(sMin, -1);
            if (nMin >= 0 && nMin <= 60) {
                var wMin = DateTime.Now.Minute;
                if (wMin == nMin) {
                    if (autoRunInfo.boStatus) return;
                  //  GameShare.ScriptEngine.GotoLable(this, M2Share.RobotNPC.ActorId, autoRunInfo.sParam2, false);
                    autoRunInfo.boStatus = true;
                }
                else {
                    autoRunInfo.boStatus = false;
                }
            }
        }

        private static void AutoRunOfOnSec(AutoRunInfo autoRunInfo) {
        }

        private void AutoRunOfOnWeek(AutoRunInfo autoRunInfo) {
            var sMin = string.Empty;
            var sHour = string.Empty;
            var sWeek = string.Empty;
            var sLineText = autoRunInfo.sParam1;
            sLineText = HUtil32.GetValidStr3(sLineText, ref sWeek, ':');
            sLineText = HUtil32.GetValidStr3(sLineText, ref sHour, ':');
            sLineText = HUtil32.GetValidStr3(sLineText, ref sMin, ':');
            var nWeek = HUtil32.StrToInt(sWeek, -1);
            var nHour = HUtil32.StrToInt(sHour, -1);
            var nMin = HUtil32.StrToInt(sMin, -1);
            if (nWeek >= 1 && nWeek <= 7 && nHour >= 0 && nHour <= 24 && nMin >= 0 && nMin <= 60) {
                var wHour = DateTime.Now.Hour;
                var wMin = DateTime.Now.Minute;
                var wWeek = DateTime.Now.DayOfWeek;
                if ((int)wWeek == nWeek && wHour == nHour) {
                    if (wMin == nMin) {
                        if (autoRunInfo.boStatus) return;
                       // GameShare.ScriptEngine.GotoLable(this, M2Share.RobotNPC.ActorId, autoRunInfo.sParam2, false);
                        autoRunInfo.boStatus = true;
                    }
                    else {
                        autoRunInfo.boStatus = false;
                    }
                }
            }
        }

        private void ClearScript() {
            for (var i = 0; i < _autoRunList.Count; i++) {
                _autoRunList[i] = null;
            }
            _autoRunList.Clear();
        }

        public void LoadScript()
        {
            var sActionType = string.Empty;
            var sRunCmd = string.Empty;
            var sMoethod = string.Empty;
            var sParam1 = string.Empty;
            var sParam2 = string.Empty;
            var sParam3 = string.Empty;
            var sParam4 = string.Empty;
            var sFileName = M2Share.GetEnvirFilePath("Robot_def", $"{ScriptFileName}.txt");
            if (File.Exists(sFileName))
            {
                using var loadList = new StringList();
                loadList.LoadFromFile(sFileName);
                for (var i = 0; i < loadList.Count; i++)
                {
                    var sLineText = loadList[i];
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
                                var autoRunInfo = new AutoRunInfo();
                                autoRunInfo.RunTick = HUtil32.GetTimestamp();
                                autoRunInfo.RunTimeTick = 0;
                                autoRunInfo.boStatus = false;
                                if (string.Compare(sMoethod, Robot.sRODAY, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    autoRunInfo.Moethod = Robot.nRODAY;
                                }
                                if (string.Compare(sMoethod, Robot.sROHOUR, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    autoRunInfo.Moethod = Robot.nROHOUR;
                                }
                                if (string.Compare(sMoethod, Robot.sROMIN, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    autoRunInfo.Moethod = Robot.nROMIN;
                                }
                                if (string.Compare(sMoethod, Robot.sROSEC, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    autoRunInfo.Moethod = Robot.nROSEC;
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONWEEK, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    autoRunInfo.Moethod = Robot.nRUNONWEEK;
                                    if (!GetWeekTime(sParam1, ref autoRunInfo.RunTimeTick))
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONDAY, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    autoRunInfo.Moethod = Robot.nRUNONDAY;
                                    if (!GetDayTime(sParam1, ref autoRunInfo.RunTimeTick))
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONHOUR, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    autoRunInfo.Moethod = Robot.nRUNONHOUR;
                                    if (!GetHourTime(sParam1, ref autoRunInfo.RunTimeTick))
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONMIN, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    autoRunInfo.Moethod = Robot.nRUNONMIN;
                                    if (!GetMinuteTime(sParam1, ref autoRunInfo.RunTimeTick))
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                if (string.Compare(sMoethod, Robot.sRUNONSEC, StringComparison.OrdinalIgnoreCase) == 0)
                                {
                                    autoRunInfo.Moethod = Robot.nRUNONSEC;
                                    if (!GetSecondTime(sParam1, ref autoRunInfo.RunTimeTick))
                                    {
                                        OutErrorMessage(sActionType, sRunCmd, sParam1, sParam2, sParam3, sParam4);
                                    }
                                }
                                autoRunInfo.sParam1 = sParam1;
                                autoRunInfo.sParam2 = sParam2;
                                autoRunInfo.sParam3 = sParam3;
                                autoRunInfo.sParam4 = sParam4;
                                autoRunInfo.nParam1 = HUtil32.StrToInt(sParam1, 1);
                                _autoRunList.Add(autoRunInfo);
                            }
                        }
                    }
                }
            }
        }

        private bool GetWeekTime(string param, ref long runTime) {
            if (!DateTimeOffset.TryParse(param, out var runWeekTime)) return false;
            runTime = GetSundayDate(runWeekTime).ToUnixTimeMilliseconds();
            return true;
        }

        private static bool GetDayTime(string param, ref long runTime) {
            if (!DateTimeOffset.TryParse(param, out var runDayTime)) return false;
            runTime = runDayTime.ToUnixTimeMilliseconds();
            return true;
        }

        private static bool GetHourTime(string param, ref long runTime) {
            if (!int.TryParse(param, out var runHour)) return false;
            runTime = DateTimeOffset.Now.AddHours(runHour).ToUnixTimeMilliseconds();
            return true;
        }

        private static bool GetMinuteTime(string param, ref long runTime) {
            if (!int.TryParse(param, out var runHour)) return false;
            runTime = DateTimeOffset.Now.AddMinutes(runHour).ToUnixTimeMilliseconds();
            return true;
        }

        private static bool GetSecondTime(string param, ref long runTime) {
            if (!int.TryParse(param, out var runHour)) return false;
            runTime = DateTimeOffset.Now.AddSeconds(runHour).ToUnixTimeMilliseconds();
            return true;
        }

        private void OutErrorMessage(string sActionType, string sRunCmd, string sParam1, string sParam2, string sParam3, string sParam4) {
            _logger.Error($"机器人脚本错误 ActionType:{sActionType} RunCmd:{sRunCmd} Params1:{sParam1} Params2:{sParam2} Params3:{sParam3} Params4:{sParam4}");
        }

        /// <summary>
        /// 计算某日结束日期（礼拜日的日期）
        /// </summary>
        /// <param name="someDate">该周中任意一天</param>
        /// <returns>返回礼拜日日期，后面的具体时、分、秒和传入值相等</returns>
        private static DateTimeOffset GetSundayDate(DateTimeOffset someDate) {
            var i = (7 - (int)someDate.DayOfWeek);
            return someDate.Add(new TimeSpan(i, 0, 0, 0));
        }

        public void ReloadScript() {
            ClearScript();
            LoadScript();
        }

        public override void Run()
        {
            if (M2Share.RobotNPC == null)
            {
                return;
            }
            for (var i = _autoRunList.Count - 1; i >= 0; i--)
            {
                AutoRun(_autoRunList[i]);
            }
        }

        /*internal override void SendSocket(CommandMessage defMsg, string sMsg) {

        }*/
    }
}