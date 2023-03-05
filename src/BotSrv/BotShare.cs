using NLog;

namespace BotSrv
{
    public static class BotShare
    {
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static ClientManager ClientMgr;
    }
}