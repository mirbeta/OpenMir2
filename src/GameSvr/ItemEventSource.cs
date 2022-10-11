using System.Diagnostics.Tracing;

namespace GameSvr
{
    [EventSource(Name = "GameProvider")]
    public class GameEventSource : EventSource
    {
        public void AddEventLog(string log)
        {
            WriteEvent(1, log);
        }
    }
}