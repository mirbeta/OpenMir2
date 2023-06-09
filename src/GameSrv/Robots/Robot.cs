namespace GameSrv.Robots
{
    /*;AutoRunRobot.txt 
    #AutoRun NPC SEC 10 @SendRedMsg
    SEC：按秒运行
    MIN：按分运行
    HOUR：按小时运行
    DAY：按天运行
    RUNONWEEK：按星期几及时间运行
    #AutoRun NPC RUNONWEEK 5:15:55 @SendRedMsg
    星期五15点55分运行 
    RUNONMONTH: 按每月几日及时间运行
    ;#AutoRun NPC RUNONMONTH 16:23:52 @SendRedMsg
    ;每月16号23点52分运行
    RUNONHOURTOMIN: 按每小时几分运行
    ;#AutoRun NPC RUNONHOURTOMIN 30 @SendRedMsg
    ;每小时30分运行
    RUNODAYTIME:指定时间段内按指定间隔运行(结束时间不能小于开始时间)
    ;每天15:09启动,15:30结束,每次30秒执行触发@30秒刷新排行榜
    ;#AutoRun NPC RUNODAYTIME 15:09 15:30 30 @30秒刷新排行榜*/
    public static class Robot
    {
        public const string sROAUTORUN = "#AUTORUN";
        public const string sRONPCLABLEJMP = "NPC";
        /// <summary>
        /// 每天运行
        /// </summary>
        public const string sRODAY = "DAY";
        public const int nRODAY = 200;
        /// <summary>
        /// 每小时运行
        /// </summary>
        public const string sROHOUR = "HOUR";
        public const int nROHOUR = 201;
        /// <summary>
        /// 每分运行
        /// </summary>
        public const string sROMIN = "MIN";
        public const int nROMIN = 202;
        /// <summary>
        /// 每秒运行
        /// </summary>
        public const string sROSEC = "SEC";
        public const int nROSEC = 203;
        /// <summary>
        /// 每周运行
        /// </summary>
        public const string sRUNONWEEK = "RUNONWEEK";
        public const int nRUNONWEEK = 300;
        /// <summary>
        /// 每天运行
        /// </summary>
        public const string sRUNONDAY = "RUNONDAY";
        public const int nRUNONDAY = 301;
        /// <summary>
        /// 每小时运行
        /// </summary>
        public const string sRUNONHOUR = "RUNONHOUR";
        public const int nRUNONHOUR = 302;
        /// <summary>
        /// 每分钟运行
        /// </summary>
        public const string sRUNONMIN = "RUNONMIN";
        public const int nRUNONMIN = 303;
        /// <summary>
        /// 每秒运行
        /// </summary>
        public const string sRUNONSEC = "RUNONSEC";
        public const int nRUNONSEC = 304;
    }
}


