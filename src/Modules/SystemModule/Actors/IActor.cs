using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

namespace SystemModule
{
    public interface IActor
    {
        int ActorId { get; }
        /// <summary>
        /// 名称
        /// </summary>
        string ChrName { get; set; }
        /// <summary>
        /// 人物名字的颜色
        /// </summary>        
        byte NameColor { get; set; }
        /// <summary>
        /// 所在座标X
        /// </summary>
        short CurrX { get; set; }
        /// <summary>
        /// 所在座标Y
        /// </summary>
        short CurrY { get; set; }
        /// <summary>
        /// 所在方向
        /// </summary>
        byte Dir { get; set; }
        /// <summary>
        /// 所在地图名称
        /// </summary>
        string MapName { get; set; }
        /// <summary>
        /// 地图文件名称
        /// </summary>
        string MapFileName { get; set; }
        /// <summary>
        /// 人物金币数
        /// </summary>
        int Gold { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        int CharStatus { get; set; }
        int CharStatusEx { get; set; }
        MonGenInfo MonGen { get; set; }
        /// <summary>
        /// 基本属性
        /// </summary>
        Ability Abil { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        Ability WAbil { get; set; }
        /// <summary>
        /// 附加属性
        /// </summary>
        AddAbility AddAbil { get; set; }
        /// <summary>
        /// 视觉范围大小
        /// </summary>
        byte ViewRange { get; set; }
        /// <summary>
        /// 状态属性值结束时间
        /// 0-绿毒(减HP) 1-红毒(减MP) 2-防、魔防为0(唯我独尊3级) 3-不能跑动(中蛛网)
        /// 4-不能移动(中战连击) 5-麻痹(石化) 6-减血，被连击技能万剑归宗击中后掉血
        /// 7-冰冻(不能跑动，不能魔法) 8-隐身 9-防御力(神圣战甲术) 10-魔御力(幽灵盾) 11-魔法盾
        /// </summary>
        ushort[] StatusTimeArr { get; set; }
        /// <summary>
        /// 状态持续的开始时间
        /// </summary>
        int[] StatusArrTick { get; set; }
        /// <summary>
        /// 防麻痹
        /// </summary>
        bool UnParalysis { get; set; }
        /// <summary>
        /// 外观代码
        /// </summary>
        ushort Appr { get; set; }
        /// <summary>
        /// 种族类型
        /// </summary>
        byte Race { get; set; }
        /// <summary>
        /// 在地图上的类型
        /// </summary>
        CellType CellType { get; set; }
        /// <summary>
        /// 角色外形
        /// </summary>
        byte RaceImg { get; set; }
        /// <summary>
        /// 人物攻击准确度
        /// </summary>
        byte HitPoint { get; set; }
        /// <summary>
        /// 中毒躲避
        /// </summary>
        byte AntiPoison { get; set; }
        /// <summary>
        /// 魔法躲避
        /// </summary>
        ushort AntiMagic { get; set; }
        /// <summary>
        /// 中绿毒降HP点数
        /// </summary>
        byte GreenPoisoningPoint { get; set; }
        /// <summary>
        /// 敏捷度
        /// </summary>
        byte SpeedPoint { get; set; }
        /// <summary>
        /// 否可以看到隐身人物(视线范围) 
        /// </summary>
        byte CoolEyeCode { get; set; }
        /// <summary>
        /// 是否可以看到隐身人物
        /// </summary>
        bool CoolEye { get; set; }
        /// <summary>
        /// 是否被召唤(主人)
        /// </summary>
        IActor Master { get; set; }
        IActor GetMaster();
        /// <summary>
        /// 不死系,1为不死系
        /// </summary>
        byte LifeAttrib { get; set; }
        /// <summary>
        /// 下属列表
        /// </summary>        
        IList<IMonsterActor> SlaveList { get; set; }
        /// <summary>
        /// 宝宝攻击状态(休息/攻击)
        /// </summary>
        bool SlaveRelax { get; set; }
        /// <summary>
        /// 亮度
        /// </summary>
        byte Light { get; set; }
        /// <summary>
        /// 所属城堡
        /// </summary>
        IUserCastle Castle { get; set; }
        /// <summary>
        /// 无敌模式
        /// </summary>
        bool SuperMan { get; set; }
        /// <summary>
        /// 是否是动物
        /// </summary>
        bool Animal { get; set; }
        /// <summary>
        /// 死亡是否不掉物品
        /// </summary>
        bool NoItem { get; set; }
        /// <summary>
        /// 固定隐身模式
        /// </summary>
        bool FixedHideMode { get; set; }
        /// <summary>
        /// 不能冲撞模式(即敌人不能使用野蛮冲撞技能攻击)
        /// </summary>
        bool StickMode { get; set; }
        /// <summary>
        /// 被打到是否减慢行走速度,等级小于50的怪 false-减慢 true-不减慢
        /// </summary>
        bool RushMode { get; set; }
        bool NoTame { get; set; }
        /// <summary>
        /// 尸体
        /// </summary>
        bool Skeleton { get; set; }
        /// <summary>
        /// 身体坚韧性
        /// </summary>
        byte BodyLeathery { get; set; }
        /// <summary>
        /// 心灵启示
        /// </summary>
        bool ShowHp { get; set; }
        /// <summary>
        /// 心灵启示检查时间
        /// </summary>
        int ShowHpTick { get; set; }
        /// <summary>
        /// 心灵启示有效时长
        /// </summary>
        int ShowHpInterval { get; set; }
        IEnvirnoment Envir { get; set; }
        /// <summary>
        /// 尸体清除
        /// </summary>
        bool Ghost { get; set; }
        /// <summary>
        /// 尸体清除时间
        /// </summary>
        int GhostTick { get; set; }
        /// <summary>
        /// 死亡
        /// </summary>
        bool Death { get; set; }
        /// <summary>
        /// 死亡时间
        /// </summary>
        int DeathTick { get; set; }
        bool Invisible { get; set; }
        /// <summary>
        /// 是否可以复活
        /// </summary>
        bool CanReAlive { get; set; }
        /// <summary>
        /// 复活时间
        /// </summary>
        int ReAliveTick { get; set; }
        /// <summary>
        /// 怪物所拿的武器
        /// </summary>
        byte MonsterWeapon { get; set; }
        /// <summary>
        /// 受攻击间隔
        /// </summary>
        int StruckTick { get; set; }
        /// <summary>
        /// 刷新消息
        /// </summary>
        bool WantRefMsg { get; set; }
        /// <summary>
        /// 增加到地图是否成功
        /// </summary>
        bool AddtoMapSuccess { get; set; }
        /// <summary>
        /// 换地图时，跑走不考虑坐标
        /// </summary>
        bool SpaceMoved { get; set; }
        bool Mission { get; set; }
        short MissionX { get; set; }
        short MissionY { get; set; }
        /// <summary>
        /// 隐身戒指
        /// </summary>
        bool HideMode { get; set; }
        /// <summary>
        /// 石像化
        /// </summary>
        bool StoneMode { get; set; }
        /// <summary>
        /// 魔法隐身(隐身术)
        /// </summary>
        bool Transparent { get; set; }
        /// <summary>
        /// 管理模式
        /// </summary>
        bool AdminMode { get; set; }
        /// <summary>
        /// 隐身模式（GM模式）
        /// </summary>
        bool ObMode { get; set; }
        /// <summary>
        /// 视觉搜索时间间隔
        /// </summary>
        int SearchTime { get; set; }
        /// <summary>
        /// 视觉搜索间隔
        /// </summary>
        int SearchTick { get; set; }
        /// <summary>
        /// 上次运行时间
        /// </summary>
        int RunTick { get; set; }
        /// <summary>
        /// 运行时间
        /// </summary>
        int RunTime { get; set; }
        /// <summary>
        /// 特别指定为 此类型  加血间隔
        /// </summary>
        int HealthTick { get; set; }
        int SpellTick { get; set; }
        IActor TargetCret { get; set; }
        int TargetFocusTick { get; set; }
        /// <summary>
        /// 被对方杀害时对方对象
        /// </summary>
        IActor LastHiter { get; set; }
        int LastHiterTick { get; set; }
        IActor ExpHitter { get; set; }
        int ExpHitterTick { get; set; }
        /// <summary>
        /// 中毒处理间隔时间
        /// </summary>
        int PoisoningTick { get; set; }
        int VerifyTick { get; set; }
        /// <summary>
        /// 恢复血量和魔法间隔
        /// </summary>
        int AutoRecoveryTick { get; set; }
        /// <summary>
        /// 可视范围内的人物列表
        /// </summary>
        IList<int> VisibleHumanList { get; set; }
        /// <summary>
        /// 是否在可视范围内有人物,及宝宝
        /// </summary>
        bool IsVisibleActive { get; set; }
        /// <summary>
        /// 可视范围内的精灵列表
        /// </summary>
        IList<VisibleBaseObject> VisibleActors { get; set; }
        /// <summary>
        /// 玩家包裹物品列表或怪物物品掉落列表
        /// </summary>
        IList<UserItem> ItemList { get; set; }
        int SendRefMsgTick { get; set; }
        /// <summary>
        /// 攻击间隔
        /// </summary>
        int AttackTick { get; set; }
        /// <summary>
        /// 走路间隔
        /// </summary>
        int WalkTick { get; set; }
        /// <summary>
        /// 走路速度
        /// </summary>
        int WalkSpeed { get; set; }
        /// <summary>
        /// 下次攻击时间
        /// </summary>
        int NextHitTime { get; set; }
        /// <summary>
        /// 是否刷新在地图上信息
        /// </summary>
        bool DenyRefStatus { get; set; }
        /// <summary>
        /// 是否增加地图计数
        /// </summary>
        bool AddToMaped { get; set; }
        /// <summary>
        /// 是否从地图中删除计数
        /// </summary>
        bool DelFormMaped { get; set; }
        bool AutoChangeColor { get; set; }
        int AutoChangeColorTick { get; set; }
        byte AutoChangeIdx { get; set; }
        /// <summary>
        /// 固定颜色
        /// </summary>
        bool FixColor { get; set; }
        byte FixColorIdx { get; set; }
        int FixStatus { get; set; }
        /// <summary>
        /// 快速麻痹，受攻击后麻痹立即消失
        /// </summary>
        bool FastParalysis { get; set; }
        bool NastyMode { get; set; }
        /// <summary>
        /// 是否机器人
        /// </summary>
        bool IsRobot { get; set; }

        void Die();

        void MakeGhost();

        void SendSelfDelayMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay);

        void SendRefMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg);

        void SendMsg(int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg = "");

        void SpaceMove(string sMap, short nX, short nY, int nInt);

        void SendStruckDelayMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay);

        void SendStruckDelayMsg(int actorId, int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg, int dwDelay);

        void SendMsg(IActor baseObject, int wIdent, int wParam, int nParam1, int nParam2, int nParam3, string sMsg = "");

        void SendActionMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg);

        void SendUpdateMsg(int wIdent, int wParam, int lParam1, int lParam2, int lParam3, string sMsg);

        bool IsProperTarget(IActor actor);

        bool InSafeZone();

        void ScatterBagItems(int itemOfCreat);

        void DropUseItems(int baseObject);

        int GetFeature(IActor baseObject);

        int GetFeatureToLong();

        void ScatterGolds(int goldOfCreat);

        void StruckDamage(int nDamage);

        string GetShowName();

        void RefNameColor();

        void RefShowName();

        byte GetNameColor();

        void RecalcAbilitys();

        bool GetBackPosition(ref short nX, ref short nY);

        int CharPushed(byte nDir, byte nPushCount);

        ushort GetHitStruckDamage(IActor target, int nDamage);

        int GetMagStruckDamage(IActor baseObject, int nDamage);

        bool DefenceUp(int nSec);

        bool MagDefenceUp(int nSec);

        ushort GetFeatureEx();

        void WinExp(int exp);

        void ApplyMeatQuality();

        bool MakePosion(int nType, ushort nTime, int nPoint);

        void SetLastHiter(IActor actor);

        void SetTargetCreat(IActor actor);

        IActor GetPoseCreate();

        void OnEnvirnomentChanged();

        void AddMessage(SendMessage sendMessage);
    }
}