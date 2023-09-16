using SystemModule.Data;

namespace SystemModule
{
    public interface IUserCastle
    {
        /// <summary>
        /// 守卫列表
        /// </summary>
        ArcherUnit[] Archers { get; set; }
        /// <summary>
        /// 攻城行会列表
        /// </summary>
        IList<IGuild> AttackGuildList { get; set; }
        /// <summary>
        /// 攻城列表
        /// </summary>
        IList<AttackerInfo> AttackWarList { get; set; }
        /// <summary>
        /// 是否显示攻城战役结束消息
        /// </summary>
        bool ShowOverMsg { get; set; }
        /// <summary>
        /// 是否开始攻城
        /// </summary>
        bool IsStartWar { get; set; }
        /// <summary>
        /// 正在沙巴克战役
        /// </summary>
        bool UnderWar { get; set; }
        ArcherUnit CenterWall { get; set; }
        DateTime ChangeDate { get; set; }
        /// <summary>
        /// 城门状态
        /// </summary>
        DoorStatus DoorStatus { get; set; }
        /// <summary>
        /// 是否已显示攻城结束信息
        /// </summary>
        int SaveTick { get; set; }
        int StartCastleWarTick { get; set; }
        IList<string> EnvirList { get; set; }
        ArcherUnit[] Guards { get; set; }
        DateTime IncomeToday { get; set; }
        ArcherUnit LeftWall { get; set; }
        ArcherUnit MainDoor { get; set; }
        /// <summary>
        /// 城堡地图
        /// </summary>
        IEnvirnoment CastleEnvir { get; set; }
        /// <summary>
        /// 皇宫地图
        /// </summary>
        IEnvirnoment PalaceEnvir { get; set; }
        /// <summary>
        /// 密道地图
        /// </summary>
        IEnvirnoment SecretEnvir { get; set; }
        /// <summary>
        /// 所属行会名称
        /// </summary>
        IGuild MasterGuild { get; set; }
        /// <summary>
        /// 行会回城点X
        /// </summary>
        int HomeX { get; set; }
        /// <summary>
        /// 行会回城点Y
        /// </summary>
        int HomeY { get; set; }
        int PalaceDoorX { get; set; }
        int PalaceDoorY { get; set; }
        /// <summary>
        /// 今日收入
        /// </summary>
        int TodayIncome { get; set; }
        /// <summary>
        /// 收入多少金币
        /// </summary>
        int TotalGold { get; set; }
        int WarRangeX { get; set; }
        int WarRangeY { get; set; }
        /// <summary>
        /// 皇宫右城墙
        /// </summary>
        ArcherUnit RightWall { get; set; }
        /// <summary>
        /// 皇宫门状态
        /// </summary>
        string ConfigDir { get; set; }
        /// <summary>
        /// 行会回城点地图
        /// </summary>
        string HomeMap { get; set; }
        /// <summary>
        /// 城堡所在地图名
        /// </summary>
        string MapName { get; set; }
        /// <summary>
        /// 城堡名称
        /// </summary>
        string sName { get; set; }
        /// <summary>
        /// 所属行会
        /// </summary>
        string OwnGuild { get; set; }
        /// <summary>
        /// 皇宫地图名称
        /// </summary>
        string PalaceMap { get; set; }
        /// <summary>
        /// 密道地图名称
        /// </summary>
        string SecretMap { get; set; }
        /// <summary>
        /// 攻城日期
        /// </summary>
        DateTime WarDate { get; set; }
        int Power { get; set; }
        int TechLevel { get; set; }

        bool AddAttackerInfo(IGuild Guild);
        bool CanGetCastle(IGuild guild);
        bool CheckInPalace(int nX, int nY);
        bool CheckInPalace(int nX, int nY, IPlayerActor targetObject);
        string GetAttackWarList();
        void GetCastle(IGuild Guild);
        short GetHomeX();
        short GetHomeY();
        string GetMapName();
        string GetWarDate();
        bool InCastleWarArea(IEnvirnoment envir, int nX, int nY);
        void IncRateGold(int nGold);
        void Initialize();
        int InPalaceGuildCount();
        bool IsAttackAllyGuild(IGuild Guild);
        bool IsAttackGuild(IGuild Guild);
        bool IsDefenseAllyGuild(IGuild guild);
        bool IsDefenseGuild(IGuild guild);
        bool IsMasterGuild(IGuild guild);
        bool IsMember(IPlayerActor member);
        void MainDoorControl(bool boClose);
        int ReceiptGolds(IPlayerActor PlayObject, int nGold);
        bool RepairDoor();
        bool RepairWall(int nWallIndex);
        void Run();
        void Save();
        void StartWallconquestWar();
        void StopWallconquestWar();
        int WithDrawalGolds(IPlayerActor PlayObject, int nGold);
    }
}