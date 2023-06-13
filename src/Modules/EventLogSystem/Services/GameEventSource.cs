using System.Diagnostics.Tracing;
using System.Runtime.Versioning;

namespace EventLogSystem.Services
{
    [SupportedOSPlatform("Windows")]
    [EventSource(Name = "UserLogProvider")]
    public class GameEventSource : EventSource, IGameEventSource
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
}