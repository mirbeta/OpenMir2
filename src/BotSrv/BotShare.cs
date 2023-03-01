using System;
using NLog;

namespace BotSrv
{
    public static class BotShare
    {
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static ClientManager ClientMgr;


        public static int GetCodeMsgSize(float len)
        {
            if ((int)len < len)
            {
                return (int)(Math.Truncate(len) + 1);
            }
            return (int)Math.Truncate(len);
        }
    }
}