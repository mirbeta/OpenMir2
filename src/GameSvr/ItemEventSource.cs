using System.Diagnostics.Tracing;

namespace GameSvr
{
    [EventSource(Name = "itemProvider")]
    public class ItemEventSource : EventSource
    {
        public void TestEvent(int id) => WriteEvent(1, id);
    }
}