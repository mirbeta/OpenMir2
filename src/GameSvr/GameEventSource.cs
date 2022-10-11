using System.Diagnostics.Tracing;

namespace GameSvr
{
    [EventSource(Name = "GameProvider")]
    public class GameEventSource : EventSource
    {
        public void AddEventLog(int eventType, string meesage)
        {
            //todo eventType需整理归类
            WriteEvent(eventType, meesage);
        }
        
        public void AddEventLog(GameEventLogType eventType, string meesage)
        {
            WriteEvent((int)eventType, meesage);
        }
    }

    public enum GameEventLogType : int
    {
        /// <summary>
        /// 沙巴克每日收入统计
        /// </summary>
        CastleTodayIncome = 23,
        /// <summary>
        /// 沙巴克存入金币
        /// </summary>
        CastleReceiptGolds = 24
    }
}