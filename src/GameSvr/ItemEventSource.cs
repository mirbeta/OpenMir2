using System.Diagnostics.Tracing;

namespace GameSvr
{
    [EventSource(Name = "itemProvider")]
    public class ItemEventSource : EventSource
    {
        public void AddGameLog(string log)
        {
            WriteEvent(1, log);
        }
    }
}