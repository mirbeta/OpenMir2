using M2Server.Items;
using M2Server.Monster;
using M2Server.Monster.Monsters;
using M2Server.Player;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Events;
using SystemModule.Packets.ClientPackets;

namespace M2Server.Actor
{
    public partial class BaseObject : ActorEntity, IActor
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string ChrName { get; set; }
        /// <summary>
        /// 人物名字的颜色
        /// </summary>        
        public byte NameColor { get; set; }
        /// <summary>
        /// 所在座标X
        /// </summary>
        public short CurrX { get; set; }
        /// <summary>
        /// 所在座标Y
        /// </summary>
        public short CurrY { get; set; }
        /// <summary>
        /// 所在方向
        /// </summary>
        public byte Dir { get; set; }
        /// <summary>
        /// 所在地图名称
        /// </summary>
        public string MapName { get; set; }
        /// <summary>
        /// 地图文件名称
        /// </summary>
        public string MapFileName { get; set; }
        /// <summary>
        /// 人物金币数
        /// </summary>
        public int Gold { get; set; }
        /// <summary>
        /// 状态值
        /// </summary>
        public int CharStatus { get; set; }
        public int CharStatusEx { get; set; }
        public MonGenInfo MonGen { get; set; }
        /// <summary>
        /// 基本属性
        /// </summary>
        public Ability Abil { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public Ability WAbil { get; set; }
        /// <summary>
        /// 附加属性
        /// </summary>
        public AddAbility AddAbil { get; set; }
        /// <summary>
        /// 视觉范围大小
        /// </summary>
        public byte ViewRange { get; set; }
        /// <summary>
        /// 状态属性值结束时间
        /// 0-绿毒(减HP) 1-红毒(减MP) 2-防、魔防为0(唯我独尊3级) 3-不能跑动(中蛛网)
        /// 4-不能移动(中战连击) 5-麻痹(石化) 6-减血，被连击技能万剑归宗击中后掉血
        /// 7-冰冻(不能跑动，不能魔法) 8-隐身 9-防御力(神圣战甲术) 10-魔御力(幽灵盾) 11-魔法盾
        /// </summary>
        public ushort[] StatusTimeArr { get; set; }
        /// <summary>
        /// 状态持续的开始时间
        /// </summary>
        public int[] StatusArrTick { get; set; }
        /// <summary>
        /// 防麻痹
        /// </summary>
        public bool UnParalysis { get; set; }
        /// <summary>
        /// 外观代码
        /// </summary>
        public ushort Appr { get; set; }
        /// <summary>
        /// 种族类型
        /// </summary>
        public byte Race { get; set; }
        /// <summary>
        /// 在地图上的类型
        /// </summary>
        public CellType CellType { get; set; }
        /// <summary>
        /// 角色外形
        /// </summary>
        public byte RaceImg { get; set; }
        /// <summary>
        /// 人物攻击准确度
        /// </summary>
        public byte HitPoint { get; set; }
        /// <summary>
        /// 中毒躲避
        /// </summary>
        public byte AntiPoison { get; set; }
        /// <summary>
        /// 魔法躲避
        /// </summary>
        public ushort AntiMagic { get; set; }
        /// <summary>
        /// 中绿毒降HP点数
        /// </summary>
        public byte GreenPoisoningPoint { get; set; }
        /// <summary>
        /// 敏捷度
        /// </summary>
        public byte SpeedPoint { get; set; }
        /// <summary>
        /// 否可以看到隐身人物(视线范围) 
        /// </summary>
        public byte CoolEyeCode { get; set; }
        /// <summary>
        /// 是否可以看到隐身人物
        /// </summary>
        public bool CoolEye { get; set; }
        /// <summary>
        /// 是否被召唤(主人)
        /// </summary>
        public IActor Master { get; set; }
        /// <summary>
        /// 不死系,1为不死系
        /// </summary>
        public byte LifeAttrib { get; set; }
        /// <summary>
        /// 下属列表
        /// </summary>        
        public IList<IActor> SlaveList { get; set; }
        /// <summary>
        /// 宝宝攻击状态(休息/攻击)
        /// </summary>
        public bool SlaveRelax { get; set; }
        /// <summary>
        /// 亮度
        /// </summary>
        public byte Light { get; set; }
        /// <summary>
        /// 所属城堡
        /// </summary>
        public IUserCastle Castle { get; set; }
        /// <summary>
        /// 无敌模式
        /// </summary>
        public bool SuperMan { get; set; }
        /// <summary>
        /// 是否是动物
        /// </summary>
        public bool Animal { get; set; }
        /// <summary>
        /// 死亡是否不掉物品
        /// </summary>
        public bool NoItem { get; set; }
        /// <summary>
        /// 固定隐身模式
        /// </summary>
        public bool FixedHideMode { get; set; }
        /// <summary>
        /// 不能冲撞模式(即敌人不能使用野蛮冲撞技能攻击)
        /// </summary>
        public bool StickMode { get; set; }
        /// <summary>
        /// 被打到是否减慢行走速度,等级小于50的怪 false-减慢 true-不减慢
        /// </summary>
        public bool RushMode { get; set; }
        public bool NoTame { get; set; }
        /// <summary>
        /// 尸体
        /// </summary>
        public bool Skeleton { get; set; }
        /// <summary>
        /// 身体坚韧性
        /// </summary>
        public byte BodyLeathery { get; set; }
        /// <summary>
        /// 心灵启示
        /// </summary>
        public bool ShowHp { get; set; }
        /// <summary>
        /// 心灵启示检查时间
        /// </summary>
        public int ShowHpTick { get; set; }
        /// <summary>
        /// 心灵启示有效时长
        /// </summary>
        public int ShowHpInterval { get; set; }
        public IEnvirnoment Envir { get; set; }
        /// <summary>
        /// 尸体清除
        /// </summary>
        public bool Ghost { get; set; }
        /// <summary>
        /// 尸体清除时间
        /// </summary>
        public int GhostTick { get; set; }
        /// <summary>
        /// 死亡
        /// </summary>
        public bool Death { get; set; }
        /// <summary>
        /// 死亡时间
        /// </summary>
        public int DeathTick { get; set; }
        public bool Invisible { get; set; }
        /// <summary>
        /// 是否可以复活
        /// </summary>
        public bool CanReAlive { get; set; }
        /// <summary>
        /// 复活时间
        /// </summary>
        public int ReAliveTick { get; set; }
        /// <summary>
        /// 怪物所拿的武器
        /// </summary>
        public byte MonsterWeapon { get; set; }
        /// <summary>
        /// 受攻击间隔
        /// </summary>
        public int StruckTick { get; set; }
        /// <summary>
        /// 刷新消息
        /// </summary>
        public bool WantRefMsg { get; set; }
        /// <summary>
        /// 增加到地图是否成功
        /// </summary>
        public bool AddtoMapSuccess { get; set; }
        /// <summary>
        /// 换地图时，跑走不考虑坐标
        /// </summary>
        public bool SpaceMoved { get; set; }
        public bool Mission { get; set; }
        public short MissionX { get; set; }
        public short MissionY { get; set; }
        /// <summary>
        /// 隐身戒指
        /// </summary>
        public bool HideMode { get; set; }
        /// <summary>
        /// 石像化
        /// </summary>
        public bool StoneMode { get; set; }
        /// <summary>
        /// 魔法隐身(隐身术)
        /// </summary>
        public bool Transparent { get; set; }
        /// <summary>
        /// 管理模式
        /// </summary>
        public bool AdminMode { get; set; }
        /// <summary>
        /// 隐身模式（GM模式）
        /// </summary>
        public bool ObMode { get; set; }
        /// <summary>
        /// 视觉搜索时间间隔
        /// </summary>
        public int SearchTime { get; set; }
        /// <summary>
        /// 视觉搜索间隔
        /// </summary>
        public int SearchTick { get; set; }
        /// <summary>
        /// 上次运行时间
        /// </summary>
        public int RunTick { get; set; }
        /// <summary>
        /// 运行时间
        /// </summary>
        public int RunTime { get; set; }
        /// <summary>
        /// 特别指定为 此类型  加血间隔
        /// </summary>
        public int HealthTick { get; set; }
        public int SpellTick { get; set; }
        public IActor TargetCret { get; set; }
        public int TargetFocusTick { get; set; }
        /// <summary>
        /// 被对方杀害时对方对象
        /// </summary>
        public IActor LastHiter { get; set; }
        public int LastHiterTick { get; set; }
        public IActor ExpHitter { get; set; }
        public int ExpHitterTick { get; set; }
        /// <summary>
        /// 中毒处理间隔时间
        /// </summary>
        public int PoisoningTick { get; set; }
        public int VerifyTick { get; set; }
        /// <summary>
        /// 恢复血量和魔法间隔
        /// </summary>
        public int AutoRecoveryTick { get; set; }
        /// <summary>
        /// 可视范围内的人物列表
        /// </summary>
        public IList<int> VisibleHumanList { get; set; }
        /// <summary>
        /// 是否在可视范围内有人物,及宝宝
        /// </summary>
        public bool IsVisibleActive { get; set; }
        /// <summary>
        /// 可视范围内的精灵列表
        /// </summary>
        public IList<VisibleBaseObject> VisibleActors { get; set; }
        /// <summary>
        /// 玩家包裹物品列表或怪物物品掉落列表
        /// </summary>
        public IList<UserItem> ItemList { get; set; }
        public int SendRefMsgTick { get; set; }
        /// <summary>
        /// 攻击间隔
        /// </summary>
        public int AttackTick { get; set; }
        /// <summary>
        /// 走路间隔
        /// </summary>
        public int WalkTick { get; set; }
        /// <summary>
        /// 走路速度
        /// </summary>
        public int WalkSpeed { get; set; }
        /// <summary>
        /// 下次攻击时间
        /// </summary>
        public int NextHitTime { get; set; }
        /// <summary>
        /// 是否刷新在地图上信息
        /// </summary>
        public bool DenyRefStatus { get; set; }
        /// <summary>
        /// 是否增加地图计数
        /// </summary>
        public bool AddToMaped { get; set; }
        /// <summary>
        /// 是否从地图中删除计数
        /// </summary>
        public bool DelFormMaped { get; set; }
        public bool AutoChangeColor { get; set; }
        public int AutoChangeColorTick { get; set; }
        public byte AutoChangeIdx { get; set; }
        /// <summary>
        /// 固定颜色
        /// </summary>
        public bool FixColor { get; set; }
        public byte FixColorIdx { get; set; }
        public int FixStatus { get; set; }
        /// <summary>
        /// 快速麻痹，受攻击后麻痹立即消失
        /// </summary>
        public bool FastParalysis { get; set; }
        public bool NastyMode { get; set; }
        /// <summary>
        /// 是否机器人
        /// </summary>
        public bool IsRobot { get; set; }
        public byte SlaveExpLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected BaseObject()
        {
            Ghost = false;
            GhostTick = 0;
            Death = false;
            DeathTick = 0;
            SendRefMsgTick = HUtil32.GetTickCount();
            Dir = 4;
            Race = ActorRace.Animal;
            RaceImg = 0;
            Gold = 0;
            Appr = 0;
            ViewRange = 5;
            Light = 0;
            NameColor = 255;
            HitPoint = 5;
            SpeedPoint = 15;
            LifeAttrib = 0;
            AntiPoison = 0;
            AntiMagic = 0;
            GreenPoisoningPoint = 0;
            CharStatus = 0;
            CharStatusEx = 0;
            StatusTimeArr = new ushort[15];
            StatusArrTick = new int[15];
            SuperMan = false;
            Skeleton = false;
            RushMode = false;
            ShowHp = false;
            Animal = false;
            NoItem = false;
            BodyLeathery = 50;
            FixedHideMode = false;
            StickMode = false;
            NoTame = false;
            AddAbil = new AddAbility();
            VisibleHumanList = new List<int>();
            VisibleActors = new List<VisibleBaseObject>();
            IsVisibleActive = false;
            Castle = null;
            Master = null;
            WAbil = new Ability();
            AddAbil = new AddAbility();
            Abil = new Ability();
            Abil = new Ability
            {
                Level = 1,
                AC = 0,
                MAC = 0,
                DC = (ushort)HUtil32.MakeLong(1, 4),
                MC = (ushort)HUtil32.MakeLong(1, 2),
                SC = (ushort)HUtil32.MakeLong(1, 2),
                HP = 15,
                MP = 15,
                MaxHP = 15,
                MaxMP = 15,
                Exp = 0,
                MaxExp = 50,
                Weight = 0,
                MaxWeight = 100
            };
            WantRefMsg = false;
            Mission = false;
            HideMode = false;
            StoneMode = false;
            CoolEye = false;
            Transparent = false;
            AdminMode = false;
            ObMode = false;
            RunTick = HUtil32.GetTickCount() + M2Share.RandomNumber.Random(1500);
            RunTime = 250;
            SearchTime = M2Share.RandomNumber.Random(2000) + 2000;
            SearchTick = HUtil32.GetTickCount();
            PoisoningTick = HUtil32.GetTickCount();
            VerifyTick = HUtil32.GetTickCount();
            AutoRecoveryTick = HUtil32.GetTickCount();
            WalkSpeed = 1400;
            NextHitTime = 2000;
            HealthTick = 0;
            SpellTick = 0;
            TargetCret = null;
            LastHiter = null;
            ExpHitter = null;
            DenyRefStatus = false;
            AddToMaped = true;
            AutoChangeColor = false;
            AutoChangeColorTick = HUtil32.GetTickCount();
            AutoChangeIdx = 0;
            FixColor = false;
            FixColorIdx = 0;
            FixStatus = -1;
            FastParalysis = false;
            NastyMode = false;
            M2Share.ActorMgr.Add(this);
        }

        /// <summary>
        /// 获取物品掉落位置
        /// </summary>
        /// <returns></returns>
        private bool GetDropPosition(short nOrgX, short nOrgY, int nRange, ref short pX, ref short pY)
        {
            bool result = false;
            int nItemCount = 0;
            int n24 = 999;
            short n28 = 0;
            short n2C = 0;
            for (int i = 0; i < nRange; i++)
            {
                for (int ii = -i; ii <= i; ii++)
                {
                    for (int iii = -i; iii <= i; iii++)
                    {
                        pX = (short)(nOrgX + iii + 1);
                        pY = (short)(nOrgY + ii + 1);
                        if (Envir.GetItemEx(pX, pY, ref nItemCount) == 0)
                        {
                            if (Envir.ChFlag)
                            {
                                result = true;
                                break;
                            }
                        }
                        else
                        {
                            if (Envir.ChFlag && n24 > nItemCount)
                            {
                                n24 = nItemCount;
                                n28 = pX;
                                n2C = pY;
                            }
                        }
                    }
                    if (result)
                    {
                        break;
                    }
                }
                if (result)
                {
                    break;
                }
            }
            if (!result)
            {
                if (n24 < 8)
                {
                    pX = n28;
                    pY = n2C;
                }
                else
                {
                    pX = nOrgX;
                    pY = nOrgY;
                }
            }
            return result;
        }

        public bool DropItemDown(UserItem userItem, int nScatterRange, bool boDieDrop, int itemOfCreat, int dropCreat)
        {
            if (userItem == null)
            {
                return false;
            }
            bool result = false;
            short dx = 0;
            short dy = 0;
            StdItem stdItem = ModuleShare.ItemSystem.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                if (stdItem.StdMode == 40)
                {
                    ushort idura = userItem.Dura;
                    idura = (ushort)(idura - 2000);
                    if (idura <= 0)
                    {
                        idura = 0;
                    }
                    userItem.Dura = idura;
                }
                MapItem mapItem = new MapItem
                {
                    UserItem = new UserItem(userItem),
                    Name = CustomItem.GetItemName(userItem),// 取自定义物品名称
                    Looks = stdItem.Looks
                };
                if (stdItem.StdMode == 45)
                {
                    mapItem.Looks = (ushort)M2Share.GetRandomLook(mapItem.Looks, stdItem.Shape);
                }
                mapItem.AniCount = stdItem.AniCount;
                mapItem.Reserved = 0;
                mapItem.Count = 1;
                mapItem.OfBaseObject = itemOfCreat;
                mapItem.CanPickUpTick = HUtil32.GetTickCount();
                mapItem.DropBaseObject = dropCreat;
                GetDropPosition(CurrX, CurrY, nScatterRange, ref dx, ref dy);
                if (Envir.AddItemToMap(dx, dy, mapItem))
                {
                    SendRefMsg(Messages.RM_ITEMSHOW, mapItem.Looks, mapItem.ItemId, dx, dy, mapItem.Name);
                    int logcap = boDieDrop ? 15 : 7;
                    if (!M2Share.IsCheapStuff(stdItem.StdMode))
                    {
                        if (stdItem.NeedIdentify == 1)
                        {
                            // M2Share.EventSource.AddEventLog(logcap, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" +HUtil32.BoolToIntStr(Race == ActorRace.Play) + "\t" + '0');
                        }
                    }
                    result = true;
                }
            }
            return result;
        }

        public void GoldChanged()
        {
            if (Race == ActorRace.Play)
            {
                SendUpdateMsg(Messages.RM_GOLDCHANGED, 0, 0, 0, 0, "");
            }
        }

        public void GameGoldChanged()
        {
            if (Race == ActorRace.Play)
            {
                SendUpdateMsg(Messages.RM_GAMEGOLDCHANGED, 0, 0, 0, 0, "");
            }
        }

        protected bool WalkTo(byte btDir, bool boFlag, bool fearFire = false)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::WalkTo";
            bool result = false;
            try
            {
                short oldX = CurrX;
                short oldY = CurrY;
                Dir = btDir;
                short newX = 0;
                short newY = 0;
                switch (btDir)
                {
                    case Direction.Up:
                        newX = CurrX;
                        newY = (short)(CurrY - 1);
                        break;
                    case Direction.UpRight:
                        newX = (short)(CurrX + 1);
                        newY = (short)(CurrY - 1);
                        break;
                    case Direction.Right:
                        newX = (short)(CurrX + 1);
                        newY = CurrY;
                        break;
                    case Direction.DownRight:
                        newX = (short)(CurrX + 1);
                        newY = (short)(CurrY + 1);
                        break;
                    case Direction.Down:
                        newX = CurrX;
                        newY = (short)(CurrY + 1);
                        break;
                    case Direction.DownLeft:
                        newX = (short)(CurrX - 1);
                        newY = (short)(CurrY + 1);
                        break;
                    case Direction.Left:
                        newX = (short)(CurrX - 1);
                        newY = CurrY;
                        break;
                    case Direction.UpLeft:
                        newX = (short)(CurrX - 1);
                        newY = (short)(CurrY - 1);
                        break;
                }
                if (newX >= 0 && Envir.Width - 1 >= newX && newY >= 0 && Envir.Height - 1 >= newY)
                {
                    var canWalk = true;
                    if (fearFire)//怪物不进入火墙才判断是否能走动
                    {
                        canWalk = !Envir.CanSafeWalk(newX, newY);
                    }
                    if (Master != null)
                    {
                        short n20 = 0;
                        short n24 = 0;
                        Master.Envir.GetNextPosition(Master.CurrX, Master.CurrY, Master.Dir, 1, ref n20, ref n24);
                        if (newX == 0 && newY == n24)
                        {
                            canWalk = false;
                        }
                    }
                    if (canWalk)
                    {
                        if (Envir.MoveToMovingObject(CurrX, CurrY, this, newX, newY, boFlag))
                        {
                            CurrX = newX;
                            CurrY = newY;
                        }
                    }
                }
                if (CurrX != oldX || CurrY != oldY)
                {
                    if (Walk(Messages.RM_WALK))
                    {
                        if (Transparent && HideMode)
                        {
                            StatusTimeArr[PoisonState.STATETRANSPARENT] = 1;
                        }
                        result = true;
                    }
                    else
                    {
                        Envir.DeleteFromMap(CurrX, CurrY, CellType, this.ActorId, this);
                        CurrX = oldX;
                        CurrY = oldY;
                        Envir.AddMapObject(CurrX, CurrY, CellType, this.ActorId, this);
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(ex.StackTrace);
            }
            return result;
        }

        protected void HealthSpellChanged()
        {
            if (Race == ActorRace.Play)
            {
                SendUpdateMsg(Messages.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
            }
            if (ShowHp)
            {
                SendRefMsg(Messages.RM_HEALTHSPELLCHANGED, 0, 0, 0, 0, "");
            }
        }

        internal int CalcGetExp(int nLevel, int nExp)
        {
            int result;
            if (ModuleShare.Config.HighLevelKillMonFixExp || (Abil.Level < (nLevel + 10)))
            {
                result = nExp;
            }
            else
            {
                result = nExp - HUtil32.Round(nExp / 15.0 * (Abil.Level - (nLevel + 10.0)));
            }
            if (result <= 0)
            {
                result = 1;
            }
            return result;
        }

        public void RefNameColor()
        {
            SendRefMsg(Messages.RM_CHANGENAMECOLOR, 0, 0, 0, 0, "");
        }

        protected bool DropGoldDown(int nGold, bool boFalg, int goldOfCreat, int dropGoldCreat)
        {
            bool result = false;
            short nX = 0;
            short nY = 0;
            MapItem mapItem = new MapItem
            {
                Name = Grobal2.StringGoldName,
                Count = nGold,
                Looks = M2Share.GetGoldShape(nGold),
                OfBaseObject = goldOfCreat,
                CanPickUpTick = HUtil32.GetTickCount(),
                DropBaseObject = dropGoldCreat
            };
            GetDropPosition(CurrX, CurrY, 3, ref nX, ref nY);
            if (Envir.AddItemToMap(nX, nY, mapItem))
            {
                SendRefMsg(Messages.RM_ITEMSHOW, mapItem.Looks, mapItem.ItemId, nX, nY, mapItem.Name);
                if (Race == ActorRace.Play)
                {
                    if (boFalg)
                    {
                    }
                    else
                    {
                    }
                    if (M2Share.GameLogGold)
                    {
                        //  M2Share.EventSource.AddEventLog(s20, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + Grobal2.StringGoldName + "\t" + nGold + "\t" + HUtil32.BoolToIntStr(Race == ActorRace.Play) + "\t" + '0');
                    }
                }
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 减少生命值
        /// </summary>
        /// <param name="nDamage"></param>
        internal void DamageHealth(int nDamage)
        {
            if ((LastHiter == null) || ((LastHiter.Race == ActorRace.Play) && !((PlayObject)LastHiter).UnMagicShield))
            {
                if (Race == ActorRace.Play && ((PlayObject)this).MagicShield && (nDamage > 0) && (WAbil.MP > 0))
                {
                    int nSpdam = HUtil32.Round(nDamage * 1.5);
                    if (WAbil.MP >= nSpdam)
                    {
                        WAbil.MP = (ushort)(WAbil.MP - nSpdam);
                        nSpdam = 0;
                    }
                    else
                    {
                        nSpdam = nSpdam - WAbil.MP;
                        WAbil.MP = 0;
                    }
                    nDamage = (ushort)HUtil32.Round(nSpdam / 1.5);
                    HealthSpellChanged();
                }
            }
            if (nDamage > 0)
            {
                if ((WAbil.HP - nDamage) > 0)
                {
                    WAbil.HP = (ushort)(WAbil.HP - nDamage);
                }
                else
                {
                    WAbil.HP = 0;
                }
            }
            else
            {
                if ((WAbil.HP - nDamage) < WAbil.MaxHP)
                {
                    WAbil.HP = (ushort)(WAbil.HP - nDamage);
                }
                else
                {
                    WAbil.HP = WAbil.MaxHP;
                }
            }
        }

        public static byte GetBackDir(byte nDir)
        {
            byte result = 0;
            switch (nDir)
            {
                case Direction.Up:
                    result = Direction.Down;
                    break;
                case Direction.Down:
                    result = Direction.Up;
                    break;
                case Direction.Left:
                    result = Direction.Right;
                    break;
                case Direction.Right:
                    result = Direction.Left;
                    break;
                case Direction.UpLeft:
                    result = Direction.DownRight;
                    break;
                case Direction.UpRight:
                    result = Direction.DownLeft;
                    break;
                case Direction.DownLeft:
                    result = Direction.UpRight;
                    break;
                case Direction.DownRight:
                    result = Direction.UpLeft;
                    break;
            }

            return result;
        }

        public int CharPushed(byte nDir, byte nPushCount)
        {
            short nx = 0;
            short ny = 0;
            int result = 0;
            byte olddir = Dir;
            Dir = nDir;
            byte nBackDir = GetBackDir(nDir);
            for (int i = 0; i < nPushCount; i++)
            {
                GetFrontPosition(ref nx, ref ny);
                if (Envir.CanWalk(nx, ny, false))
                {
                    if (Envir.MoveToMovingObject(CurrX, CurrY, this, nx, ny, false))
                    {
                        CurrX = nx;
                        CurrY = ny;
                        SendRefMsg(Messages.RM_PUSH, nBackDir, CurrX, CurrY, 0, "");
                        result++;
                        if (Race >= ActorRace.Animal)
                        {
                            WalkTick = WalkTick + 800;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            Dir = nBackDir;
            if (result == 0)
            {
                Dir = olddir;
            }
            return result;
        }

        public int MagPassThroughMagic(short sx, short sy, short tx, short ty, byte nDir, int magPwr, bool undeadAttack)
        {
            int tcount = 0;
            for (int i = 0; i < 12; i++)
            {
                IActor baseObject = Envir.GetMovingObject(sx, sy, true);
                if (baseObject != null)
                {
                    if (IsProperTarget(baseObject))
                    {
                        if (M2Share.RandomNumber.Random(10) >= baseObject.AntiMagic)
                        {
                            if (undeadAttack)
                            {
                                magPwr = HUtil32.Round(magPwr * 1.5);
                            }
                            baseObject.SendStruckDelayMsg(this.ActorId, Messages.RM_MAGSTRUCK, 0, magPwr, 0, 0, "", 600);
                            tcount++;
                        }
                    }
                }
                if (!((Math.Abs(sx - tx) <= 0) && (Math.Abs(sy - ty) <= 0)))
                {
                    nDir = M2Share.GetNextDirection(sx, sy, tx, ty);
                    if (!Envir.GetNextPosition(sx, sy, nDir, 1, ref sx, ref sy))
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            return tcount;
        }

        private void BreakOpenHealth()
        {
            if (ShowHp)
            {
                ShowHp = false;
                CharStatusEx = CharStatusEx ^ PoisonState.OPENHEATH;
                CharStatus = GetCharStatus();
                SendRefMsg(Messages.RM_CLOSEHEALTH, 0, 0, 0, 0, "");
            }
        }

        private void MakeOpenHealth()
        {
            ShowHp = true;
            CharStatusEx = CharStatusEx | PoisonState.OPENHEATH;
            CharStatus = GetCharStatus();
            SendRefMsg(Messages.RM_OPENHEALTH, 0, WAbil.HP, WAbil.MaxHP, 0, "");
        }

        protected void IncHealthSpell(int nHp, int nMp)
        {
            if ((nHp < 0) || (nMp < 0))
            {
                return;
            }
            if ((WAbil.HP + nHp) >= WAbil.MaxHP)
            {
                WAbil.HP = WAbil.MaxHP;
            }
            else
            {
                WAbil.HP += (ushort)nHp;
            }
            if ((WAbil.MP + nMp) >= WAbil.MaxMP)
            {
                WAbil.MP = WAbil.MaxMP;
            }
            else
            {
                WAbil.MP += (ushort)nMp;
            }
            HealthSpellChanged();
        }

        public bool GetFrontPosition(ref short nX, ref short nY)
        {
            IEnvirnoment envir = Envir;
            nX = CurrX;
            nY = CurrY;
            switch (Dir)
            {
                case Direction.Up:
                    if (nY > 0)
                    {
                        nY -= 1;
                    }
                    break;
                case Direction.UpRight:
                    if ((nX < (envir.Width - 1)) && (nY > 0))
                    {
                        nX++;
                        nY -= 1;
                    }
                    break;
                case Direction.Right:
                    if (nX < (envir.Width - 1))
                    {
                        nX++;
                    }
                    break;
                case Direction.DownRight:
                    if ((nX < (envir.Width - 1)) && (nY < (envir.Height - 1)))
                    {
                        nX++;
                        nY++;
                    }
                    break;
                case Direction.Down:
                    if (nY < (envir.Height - 1))
                    {
                        nY++;
                    }
                    break;
                case Direction.DownLeft:
                    if ((nX > 0) && (nY < (envir.Height - 1)))
                    {
                        nX -= 1;
                        nY++;
                    }
                    break;
                case Direction.Left:
                    if (nX > 0)
                    {
                        nX -= 1;
                    }
                    break;
                case Direction.UpLeft:
                    if ((nX > 0) && (nY > 0))
                    {
                        nX -= 1;
                        nY -= 1;
                    }
                    break;
            }
            return true;
        }

        private static bool SpaceMoveGetRandXY(IEnvirnoment envir, ref short nX, ref short nY)
        {
            int n14;
            short n18;
            int n1C;
            bool result = false;
            if (envir.Width < 80)
            {
                n18 = 3;
            }
            else
            {
                n18 = 10;
            }
            if (envir.Height < 150)
            {
                if (envir.Height < 50)
                {
                    n1C = 2;
                }
                else
                {
                    n1C = 15;
                }
            }
            else
            {
                n1C = 50;
            }
            n14 = 0;
            while (true)
            {
                if (envir.CanWalk(nX, nY, true))
                {
                    result = true;
                    break;
                }

                if (nX < (envir.Width - n1C - 1))
                {
                    nX += n18;
                }
                else
                {
                    nX = (short)M2Share.RandomNumber.Random(envir.Width);
                    if (nY < (envir.Height - n1C - 1))
                    {
                        nY += n18;
                    }
                    else
                    {
                        nY = (short)M2Share.RandomNumber.Random(envir.Height);
                    }
                }
                n14++;
                if (n14 >= 201)
                {
                    break;
                }
            }
            return result;
        }

        public void SpaceMove(string sMap, short nX, short nY, int nInt)
        {
            IEnvirnoment envir = ModuleShare.MapMgr.FindMap(sMap);
            if (envir != null)
            {
                if (M2Share.ServerIndex == envir.ServerIndex)
                {
                    IEnvirnoment oldEnvir = Envir;
                    short nOldX = CurrX;
                    short nOldY = CurrY;
                    bool moveSuccess = false;
                    Envir.DeleteFromMap(CurrX, CurrY, CellType, this.ActorId, this);
                    VisibleHumanList.Clear();
                    for (int i = 0; i < VisibleActors.Count; i++)
                    {
                        VisibleActors[i] = null;
                    }
                    VisibleActors.Clear();
                    Envir = envir;
                    MapName = envir.MapName;
                    MapFileName = envir.MapFileName;
                    CurrX = nX;
                    CurrY = nY;
                    short tempX = 0;
                    short tempY = 0;
                    if (SpaceMoveGetRandXY(Envir, ref tempX, ref tempY))
                    {
                        CurrX = tempX;
                        CurrY = tempY;
                        Envir.AddMapObject(CurrX, CurrY, CellType, this.ActorId, this);
                        SendMsg(Messages.RM_CLEAROBJECTS, 0, 0, 0, 0);
                        SendMsg(Messages.RM_CHANGEMAP, 0, 0, 0, 0, MapFileName);
                        if (nInt == 1)
                        {
                            SendRefMsg(Messages.RM_SPACEMOVE_SHOW2, Dir, CurrX, CurrY, 0, "");
                        }
                        else
                        {
                            SendRefMsg(Messages.RM_SPACEMOVE_SHOW, Dir, CurrX, CurrY, 0, "");
                        }
                        SpaceMoved = true;
                        moveSuccess = true;
                    }
                    if (!moveSuccess)
                    {
                        Envir = oldEnvir;
                        CurrX = nOldX;
                        CurrY = nOldY;
                        Envir.AddMapObject(CurrX, CurrY, CellType, this.ActorId, this);
                    }
                    OnEnvirnomentChanged();
                }
                else
                {
                    if (SpaceMoveGetRandXY(envir, ref nX, ref nY))
                    {
                        if (Race == ActorRace.Play)
                        {
                            DisappearA();
                            SpaceMoved = true;
                            ((PlayObject)this).ChangeSpaceMove(envir, nX, nY);
                        }
                        else
                        {
                            KickException();
                        }
                    }
                }
            }
        }

        public void RefShowName()
        {
            SendRefMsg(Messages.RM_USERNAME, 0, 0, 0, 0, GetShowName());
        }

        public BaseObject MakeSlave(string sMonName, int nMakeLevel, int nExpLevel, int nMaxMob, int dwRoyaltySec)
        {
            if (SlaveList == null)
            {
                SlaveList = new List<IActor>();
            }
            if (SlaveList.Count < nMaxMob)
            {
                short nX = 0;
                short nY = 0;
                GetFrontPosition(ref nX, ref nY);
                MonsterObject monObj = (MonsterObject)M2Share.WorldEngine.RegenMonsterByName(Envir.MapName, nX, nY, sMonName);
                if (monObj != null)
                {
                    monObj.Master = this;
                    monObj.IsSlave = true;
                    monObj.MasterRoyaltyTick = HUtil32.GetTickCount() + (dwRoyaltySec * 1000);
                    monObj.SlaveMakeLevel = (byte)nMakeLevel;
                    monObj.SlaveExpLevel = (byte)nExpLevel;
                    monObj.RecalcAbilitys();
                    if (monObj.WAbil.HP < monObj.WAbil.MaxHP)
                    {
                        monObj.WAbil.HP = (ushort)(monObj.WAbil.HP + (monObj.WAbil.MaxHP - monObj.WAbil.HP) / 2);
                    }
                    monObj.RefNameColor();
                    SlaveList.Add(monObj);
                    return monObj;
                }
            }
            return null;
        }

        /// <summary>
        /// 地图随机移动
        /// </summary>
        public void MapRandomMove(string sMapName, int nInt)
        {
            int nEgdey;
            IEnvirnoment envir = ModuleShare.MapMgr.FindMap(sMapName);
            if (envir != null)
            {
                if (envir.Height < 150)
                {
                    if (envir.Height < 30)
                    {
                        nEgdey = 2;
                    }
                    else
                    {
                        nEgdey = 20;
                    }
                }
                else
                {
                    nEgdey = 50;
                }
                short nX = (short)(M2Share.RandomNumber.Random(envir.Width - nEgdey - 1) + nEgdey);
                short nY = (short)(M2Share.RandomNumber.Random(envir.Height - nEgdey - 1) + nEgdey);
                SpaceMove(sMapName, nX, nY, nInt);
            }
        }

        public bool AddItemToBag(UserItem userItem)
        {
            if (ItemList.Count >= Grobal2.MaxBagItem)
                return false;
            ItemList.Add(userItem);
            WeightChanged();
            return true;
        }

        public IActor GetPoseCreate()
        {
            short nX = 0;
            short nY = 0;
            if (GetFrontPosition(ref nX, ref nY))
            {
                return Envir.GetMovingObject(nX, nY, true);
            }
            return null;
        }

        protected bool GetAttackDir(IActor targetObject, ref byte btDir)
        {
            bool result = false;
            if ((CurrX - 1 <= targetObject.CurrX) && (CurrX + 1 >= targetObject.CurrX) && (CurrY - 1 <= targetObject.CurrY) && (CurrY + 1 >= targetObject.CurrY) && ((CurrX != targetObject.CurrX) || (CurrY != targetObject.CurrY)))
            {
                result = true;
                if (((CurrX - 1) == targetObject.CurrX) && (CurrY == targetObject.CurrY))
                {
                    btDir = Direction.Left;
                    return true;
                }
                if (((CurrX + 1) == targetObject.CurrX) && (CurrY == targetObject.CurrY))
                {
                    btDir = Direction.Right;
                    return true;
                }
                if ((CurrX == targetObject.CurrX) && ((CurrY - 1) == targetObject.CurrY))
                {
                    btDir = Direction.Up;
                    return true;
                }
                if ((CurrX == targetObject.CurrX) && ((CurrY + 1) == targetObject.CurrY))
                {
                    btDir = Direction.Down;
                    return true;
                }
                if (((CurrX - 1) == targetObject.CurrX) && ((CurrY - 1) == targetObject.CurrY))
                {
                    btDir = Direction.UpLeft;
                    return true;
                }
                if (((CurrX + 1) == targetObject.CurrX) && ((CurrY - 1) == targetObject.CurrY))
                {
                    btDir = Direction.UpRight;
                    return true;
                }
                if (((CurrX - 1) == targetObject.CurrX) && ((CurrY + 1) == targetObject.CurrY))
                {
                    btDir = Direction.DownLeft;
                    return true;
                }
                if (((CurrX + 1) == targetObject.CurrX) && ((CurrY + 1) == targetObject.CurrY))
                {
                    btDir = Direction.DownRight;
                    return true;
                }
                btDir = 0;
            }
            return result;
        }

        protected bool GetAttackDir(IActor targetObject, int nRange, ref byte btDir)
        {
            short nX = 0;
            short nY = 0;
            btDir = M2Share.GetNextDirection(CurrX, CurrY, targetObject.CurrX, targetObject.CurrY);
            if (Envir.GetNextPosition(CurrX, CurrY, btDir, nRange, ref nX, ref nY))
            {
                return targetObject == Envir.GetMovingObject(nX, nY, true);
            }
            return false;
        }

        protected bool TargetInSpitRange(IActor targetObject, ref byte btDir)
        {
            bool result = false;
            if ((Math.Abs(targetObject.CurrX - CurrX) <= 2) && (Math.Abs(targetObject.CurrY - CurrY) <= 2))
            {
                var nX = targetObject.CurrX - CurrX;
                var nY = targetObject.CurrY - CurrY;
                if ((Math.Abs(nX) <= 1) && (Math.Abs(nY) <= 1))
                {
                    GetAttackDir(targetObject, ref btDir);
                    return true;
                }
                nX += 2;
                nY += 2;
                if ((nX >= 0) && (nX <= 4) && (nY >= 0) && (nY <= 4))
                {
                    btDir = M2Share.GetNextDirection(CurrX, CurrY, targetObject.CurrX, targetObject.CurrY);
                    if (ModuleShare.Config.SpitMap[btDir, nY, nX] == 1)
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 计算包裹物品总重量
        /// </summary>
        protected ushort RecalcBagWeight()
        {
            ushort result = 0;
            for (int i = 0; i < ItemList.Count; i++)
            {
                StdItem stdItem = ModuleShare.ItemSystem.GetStdItem(ItemList[i].Index);
                if (stdItem != null)
                {
                    result += stdItem.Weight;
                }
            }
            return result;
        }

        internal bool AddToMap()
        {
            var result = Envir.AddMapObject(CurrX, CurrY, CellType, this.ActorId, this);
            if (!FixedHideMode)
            {
                SendRefMsg(Messages.RM_TURN, Dir, CurrX, CurrY, 0, "");
            }
            return result;
        }

        /// <summary>
        /// 减少魔法值
        /// </summary>
        protected void DamageSpell(ushort nSpellPoint)
        {
            if (nSpellPoint > 0)
            {
                if ((WAbil.MP - nSpellPoint) > 0)
                {
                    WAbil.MP -= nSpellPoint;
                }
                else
                {
                    WAbil.MP = 0;
                }
            }
            else
            {
                if ((WAbil.MP - nSpellPoint) < WAbil.MaxMP)
                {
                    WAbil.MP -= nSpellPoint;
                }
                else
                {
                    WAbil.MP = WAbil.MaxMP;
                }
            }
        }

        protected static int GetLevelExp(int nLevel)
        {
            int result;
            if (nLevel <= Grobal2.MaxLevel)
            {
                result = ModuleShare.Config.NeedExps[nLevel];
            }
            else
            {
                result = ModuleShare.Config.NeedExps[ModuleShare.Config.NeedExps.Length];
            }
            return result;
        }

        public void HearMsg(string sMsg)
        {
            if (!string.IsNullOrEmpty(sMsg))
            {
                SendMsg(null, Messages.RM_HEAR, 0, ModuleShare.Config.btHearMsgFColor, ModuleShare.Config.btHearMsgBColor, 0, sMsg);
            }
        }

        protected bool InSafeArea()
        {
            if (Envir == null)
            {
                return false;
            }
            if (Envir.Flag.SafeArea)
            {
                return true;
            }
            bool result = false;
            for (int i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (string.Compare(M2Share.StartPointList[i].MapName, Envir.MapName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    short cX = M2Share.StartPointList[i].CurrX;
                    short cY = M2Share.StartPointList[i].CurrY;
                    if ((Math.Abs(CurrX - cX) <= 60) && (Math.Abs(CurrY - cY) <= 60))
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        private void MonsterRecalcAbilitys()
        {
            WAbil.DC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(WAbil.DC), HUtil32.HiWord(WAbil.DC));
            int maxHp = 0;
            if ((Race == ActorRace.MonsterWhiteskeleton) || (Race == ActorRace.MonsterElfmonster) || (Race == ActorRace.MonsterElfwarrior))
            {
                var slaveExpLevel = ((MonsterObject)this).SlaveExpLevel;
                WAbil.DC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(WAbil.DC), (ushort)HUtil32.Round((slaveExpLevel * 0.1 + 0.3) * 3.0 * slaveExpLevel + HUtil32.HiWord(WAbil.DC)));
                maxHp = maxHp + HUtil32.Round((slaveExpLevel * 0.1 + 0.3) * WAbil.MaxHP) * slaveExpLevel;
                maxHp = maxHp + WAbil.MaxHP;
                if (slaveExpLevel > 0)
                {
                    WAbil.MaxHP = (ushort)maxHp;
                }
                else
                {
                    WAbil.MaxHP = WAbil.MaxHP;
                }
            }
            else
            {
                var slaveExpLevel = ((MonsterObject)this).SlaveExpLevel;
                maxHp = WAbil.MaxHP;
                WAbil.DC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(WAbil.DC), (ushort)HUtil32.Round(slaveExpLevel * 2.0 + HUtil32.HiWord(WAbil.DC)));
                maxHp = maxHp + HUtil32.Round(WAbil.MaxHP * 0.15) * slaveExpLevel;
                WAbil.MaxHP = (ushort)HUtil32._MIN(HUtil32.Round(WAbil.MaxHP + slaveExpLevel * 60.0), maxHp);
            }
        }

        public int GetFeatureToLong()
        {
            return GetFeature(null);
        }

        public int GetCharStatus()
        {
            //0x80000000 指十六进制值，转成二进制则为10000000000000000000000000000000 然后Shr右移
            //例：I为3,右移3位，得到二进制值：10000000000000000000000000000
            //    I为6,右移6位，得到二进制值: 10000000000000000000000000
            //or 代表运算, 需要两个运算数，即两个数的位运算，只有其中一个是1就返回1; 都是0才返回0
            //and 表示 当对应位均为1时返回1，其余为0
            //从上面算法得到，最终 nStatus得到是1,
            int nStatus = 0;
            for (int i = 0; i < StatusTimeArr.Length; i++)
            {
                if (StatusTimeArr[i] > 0)
                {
                    nStatus = (int)(nStatus | (0x80000000 >> i));
                }
            }
            return nStatus | (CharStatusEx & 0x0000FFFF);
        }

        public void AbilCopyToWAbil()
        {
            WAbil = (Ability)Abil.Clone();
        }

        public void FeatureChanged()
        {
            SendRefMsg(Messages.RM_FEATURECHANGED, GetFeatureEx(), GetFeatureToLong(), 0, 0, "");
        }

        public void StatusChanged()
        {
            SendRefMsg(Messages.RM_CHARSTATUSCHANGED, 0, CharStatus, 0, 0, "");
        }

        protected void DisappearA()
        {
            Envir.DeleteFromMap(CurrX, CurrY, CellType, this.ActorId, this);
            SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");
        }

        protected virtual bool Walk(int nIdent)
        {
            const string sExceptionMsg = "[Exception] PlayObject::Walk {0} {1} {2}:{3}";
            bool result = true;
            if (Envir == null)
            {
                M2Share.Logger.Error("Walk nil PEnvir");
                return true;
            }
            try
            {
                if (!Envir.CellMatch(CurrX, CurrY))
                {
                    return true;
                }
                ref MapCellInfo cellInfo = ref Envir.GetCellInfo(CurrX, CurrY, out bool cellSuccess);
                if (cellSuccess && cellInfo.IsAvailable)
                {
                    for (int i = 0; i < cellInfo.ObjList.Count; i++)
                    {
                        CellObject cellObject = cellInfo.ObjList[i];
                        switch (cellObject.CellType)
                        {
                            case CellType.MapRoute:
                                result = false;
                                break;
                            case CellType.Event:
                                MapEvent mapEvent = null;
                                MapEvent owinEvent = M2Share.CellObjectMgr.Get<MapEvent>(cellObject.CellObjId);
                                if (owinEvent.OwnBaseObject != null)
                                {
                                    mapEvent = M2Share.CellObjectMgr.Get<MapEvent>(cellObject.CellObjId);
                                }
                                if (mapEvent != null)
                                {
                                    if (mapEvent.OwnBaseObject.IsProperTarget(this))
                                    {
                                        SendMsg(mapEvent.OwnBaseObject, Messages.RM_MAGSTRUCK_MINE, 0, mapEvent.Damage, 0, 0);
                                    }
                                }
                                break;
                        }
                    }
                }
                if (result)
                {
                    SendRefMsg(nIdent, Dir, CurrX, CurrY, 0, "");
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(Format(sExceptionMsg, ChrName, MapName, CurrX, CurrY));
                M2Share.Logger.Error(e.Message);
            }
            return result;
        }

        protected void TurnTo(byte nDir)
        {
            Dir = nDir;
            SendRefMsg(Messages.RM_TURN, nDir, CurrX, CurrY, 0, "");
        }

        public void SysMsg(string sMsg, MsgColor msgColor, MsgType msgType)
        {
            if (msgType == MsgType.Notice) // 公告
            {
                string str = string.Empty;
                string fColor = string.Empty;
                string bColor = string.Empty;
                string nTime = string.Empty;
                switch (sMsg[0])
                {
                    case '[':// 顶部滚动公告
                        {
                            sMsg = HUtil32.ArrestStringEx(sMsg, "[", "]", ref str);
                            bColor = HUtil32.GetValidStrCap(str, ref fColor, ',');
                            if (ModuleShare.Config.ShowPreFixMsg)
                            {
                                sMsg = ModuleShare.Config.LineNoticePreFix + sMsg;
                            }
                            SendMsg(Messages.RM_MOVEMESSAGE, 0, HUtil32.StrToUInt16(fColor, 255), HUtil32.StrToUInt16(bColor, 255), 0, sMsg);
                            break;
                        }
                    case '<':// 聊天框彩色公告
                        {
                            sMsg = HUtil32.ArrestStringEx(sMsg, "<", ">", ref str);
                            bColor = HUtil32.GetValidStrCap(str, ref fColor, ',');
                            if (ModuleShare.Config.ShowPreFixMsg)
                            {
                                sMsg = ModuleShare.Config.LineNoticePreFix + sMsg;
                            }
                            SendMsg(Messages.RM_SYSMESSAGE, 0, HUtil32.StrToUInt16(fColor, 255), HUtil32.StrToUInt16(bColor, 255), 0, sMsg);
                            break;
                        }
                    case '{': // 屏幕居中公告
                        {
                            sMsg = HUtil32.ArrestStringEx(sMsg, "{", "}", ref str);
                            str = HUtil32.GetValidStrCap(str, ref fColor, ',');
                            str = HUtil32.GetValidStrCap(str, ref bColor, ',');
                            str = HUtil32.GetValidStrCap(str, ref nTime, ',');
                            if (ModuleShare.Config.ShowPreFixMsg)
                            {
                                sMsg = ModuleShare.Config.LineNoticePreFix + sMsg;
                            }
                            SendMsg(Messages.RM_MOVEMESSAGE, 1, HUtil32.StrToUInt16(fColor, 255), HUtil32.StrToUInt16(bColor, 255), HUtil32.StrToUInt16(nTime, 0), sMsg);
                            break;
                        }
                    default:
                        switch (msgColor)
                        {
                            case MsgColor.Red: // 控制公告的颜色
                                if (ModuleShare.Config.ShowPreFixMsg)
                                {
                                    sMsg = ModuleShare.Config.LineNoticePreFix + sMsg;
                                }
                                SendMsg(Messages.RM_SYSMESSAGE, 0, ModuleShare.Config.RedMsgFColor, ModuleShare.Config.RedMsgBColor, 0, sMsg);
                                break;
                            case MsgColor.Green:
                                if (ModuleShare.Config.ShowPreFixMsg)
                                {
                                    sMsg = ModuleShare.Config.LineNoticePreFix + sMsg;
                                }
                                SendMsg(Messages.RM_SYSMESSAGE, 0, ModuleShare.Config.GreenMsgFColor, ModuleShare.Config.GreenMsgBColor, 0, sMsg);
                                break;
                            case MsgColor.Blue:
                                if (ModuleShare.Config.ShowPreFixMsg)
                                {
                                    sMsg = ModuleShare.Config.LineNoticePreFix + sMsg;
                                }
                                SendMsg(Messages.RM_SYSMESSAGE, 0, ModuleShare.Config.BlueMsgFColor, ModuleShare.Config.BlueMsgBColor, 0, sMsg);
                                break;
                        }
                        break;
                }
            }
            else
            {
                switch (msgColor)
                {
                    case MsgColor.Green:
                        SendMsg(Messages.RM_SYSMESSAGE, 0, ModuleShare.Config.GreenMsgFColor, ModuleShare.Config.GreenMsgBColor, 0, sMsg);
                        break;
                    case MsgColor.Blue:
                        SendMsg(Messages.RM_SYSMESSAGE, 0, ModuleShare.Config.BlueMsgFColor, ModuleShare.Config.BlueMsgBColor, 0, sMsg);
                        break;
                    default:
                        if (msgType == MsgType.Cust)
                        {
                            SendMsg(Messages.RM_SYSMESSAGE, 0, ModuleShare.Config.CustMsgFColor, ModuleShare.Config.CustMsgBColor, 0, sMsg);
                        }
                        else
                        {
                            SendMsg(Messages.RM_SYSMESSAGE, 0, ModuleShare.Config.RedMsgFColor, ModuleShare.Config.RedMsgBColor, 0, sMsg);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 设置肉的品质
        /// </summary>
        public void ApplyMeatQuality()
        {
            if (!Animal)
            {
                return;//不是动物无需设置肉的品质
            }
            for (int i = 0; i < ItemList.Count; i++)
            {
                StdItem stdItem = ModuleShare.ItemSystem.GetStdItem(ItemList[i].Index);
                if (stdItem != null)
                {
                    if (stdItem.StdMode == 40)
                    {
                        ItemList[i].Dura = ((AnimalObject)this).MeatQuality;
                    }
                }
            }
        }

        /// <summary>
        /// 散落金币
        /// </summary>
        /// <param name="goldOfCreat"></param>
        public void ScatterGolds(int goldOfCreat)
        {
            int I;
            int nGold;
            if (Gold > 0)
            {
                I = 0;
                while (true)
                {
                    if (Gold > ModuleShare.Config.MonOneDropGoldCount)
                    {
                        nGold = ModuleShare.Config.MonOneDropGoldCount;
                        Gold = Gold - ModuleShare.Config.MonOneDropGoldCount;
                    }
                    else
                    {
                        nGold = Gold;
                        Gold = 0;
                    }
                    if (nGold > 0)
                    {
                        if (!DropGoldDown(nGold, true, goldOfCreat, this.ActorId))
                        {
                            Gold = Gold + nGold;
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                    I++;
                    if (I >= 17)
                    {
                        break;
                    }
                }
                GoldChanged();
            }
        }

        public void SetLastHiter(IActor baseObject)
        {
            LastHiter = baseObject;
            LastHiterTick = HUtil32.GetTickCount();
            if (ExpHitter == null)
            {
                ExpHitter = baseObject;
                ExpHitterTick = HUtil32.GetTickCount();
            }
            else
            {
                if (ExpHitter == baseObject)
                {
                    ExpHitterTick = HUtil32.GetTickCount();
                }
            }
        }

        protected void WeightChanged()
        {
            WAbil.Weight = RecalcBagWeight();
            SendUpdateMsg(Messages.RM_WEIGHTCHANGED, 0, 0, 0, 0, "");
        }

        public bool InSafeZone()
        {
            if (Envir == null)
            {
                return true;
            }
            var result = Envir.Flag.SafeArea;
            if (result) //安全区
            {
                if ((Envir.MapName != ModuleShare.Config.RedHomeMap) || (Math.Abs(CurrX - ModuleShare.Config.RedHomeX) > ModuleShare.Config.SafeZoneSize) || (Math.Abs(CurrY - ModuleShare.Config.RedHomeY) > ModuleShare.Config.SafeZoneSize))
                {
                    for (int i = 0; i < M2Share.StartPointList.Count; i++)
                    {
                        if (string.Compare(M2Share.StartPointList[i].MapName, Envir.MapName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            short nSafeX = M2Share.StartPointList[i].CurrX;
                            short nSafeY = M2Share.StartPointList[i].CurrY;
                            if ((Math.Abs(CurrX - nSafeX) <= ModuleShare.Config.SafeZoneSize) && (Math.Abs(CurrY - nSafeY) <= ModuleShare.Config.SafeZoneSize))
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    result = true;
                }
            }
            return result;
        }

        public bool InSafeZone(IEnvirnoment envir, int nX, int nY)
        {
            if (Envir == null)
            {
                return true;
            }
            bool result = Envir.Flag.SafeArea;
            if (result)
            {
                return true;
            }
            if ((envir.MapName != ModuleShare.Config.RedHomeMap) ||
                (Math.Abs(nX - ModuleShare.Config.RedHomeX) > ModuleShare.Config.SafeZoneSize) ||
                (Math.Abs(nY - ModuleShare.Config.RedHomeY) > ModuleShare.Config.SafeZoneSize))
            {
                result = false;
            }
            else
            {
                return true;
            }
            for (int i = 0; i < M2Share.StartPointList.Count; i++)
            {
                if (M2Share.StartPointList[i].MapName == envir.MapName)
                {
                    short nSafeX = M2Share.StartPointList[i].CurrX;
                    short nSafeY = M2Share.StartPointList[i].CurrY;
                    if ((Math.Abs(nX - nSafeX) <= ModuleShare.Config.SafeZoneSize) && (Math.Abs(nY - nSafeY) <= ModuleShare.Config.SafeZoneSize))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 召唤属下
        /// </summary>
        /// <param name="sSlaveName"></param>
        public void RecallSlave(string sSlaveName)
        {
            short nX = 0;
            short nY = 0;
            int nFlag = -1;
            GetFrontPosition(ref nX, ref nY);
            if (string.Compare(sSlaveName, ModuleShare.Config.Dragon, StringComparison.OrdinalIgnoreCase) == 0)
            {
                nFlag = 1;
            }
            for (int i = SlaveList.Count - 1; i >= 0; i--)
            {
                if (nFlag == 1)
                {
                    if ((SlaveList[i].ChrName == ModuleShare.Config.Dragon) || (SlaveList[i].ChrName == ModuleShare.Config.Dragon1))
                    {
                        SlaveList[i].SpaceMove(Envir.MapName, nX, nY, 1);
                        break;
                    }
                }
                else if (SlaveList[i].ChrName == sSlaveName)
                {
                    SlaveList[i].SpaceMove(Envir.MapName, nX, nY, 1);
                    break;
                }
            }
        }

        public bool GetBackPosition(ref short nX, ref short nY)
        {
            IEnvirnoment envir = Envir;
            nX = CurrX;
            nY = CurrY;
            switch (Dir)
            {
                case Direction.Up:
                    if (nY < (envir.Height - 1))
                    {
                        nY++;
                    }
                    break;
                case Direction.Down:
                    if (nY > 0)
                    {
                        nY -= 1;
                    }
                    break;
                case Direction.Left:
                    if (nX < (envir.Width - 1))
                    {
                        nX++;
                    }
                    break;
                case Direction.Right:
                    if (nX > 0)
                    {
                        nX -= 1;
                    }
                    break;
                case Direction.UpLeft:
                    if ((nX < (envir.Width - 1)) && (nY < (envir.Height - 1)))
                    {
                        nX++;
                        nY++;
                    }
                    break;
                case Direction.UpRight:
                    if ((nX < (envir.Width - 1)) && (nY > 0))
                    {
                        nX -= 1;
                        nY++;
                    }
                    break;
                case Direction.DownLeft:
                    if ((nX > 0) && (nY < (envir.Height - 1)))
                    {
                        nX++;
                        nY -= 1;
                    }
                    break;
                case Direction.DownRight:
                    if ((nX > 0) && (nY > 0))
                    {
                        nX -= 1;
                        nY -= 1;
                    }
                    break;
            }
            return true;
        }

        public bool MakePosion(int nType, ushort nTime, int nPoint)
        {
            if (nType >= Grobal2.MAX_STATUS_ATTRIBUTE)
                return false;
            int nOldCharStatus = CharStatus;
            if (StatusTimeArr[nType] > 0)
            {
                if (StatusTimeArr[nType] < nTime)
                {
                    StatusTimeArr[nType] = nTime;
                }
            }
            else
            {
                StatusTimeArr[nType] = nTime;
            }
            StatusArrTick[nType] = HUtil32.GetTickCount();
            CharStatus = GetCharStatus();
            GreenPoisoningPoint = (byte)HUtil32._MAX(255, nPoint);
            if (nOldCharStatus != CharStatus)
            {
                StatusChanged();
            }
            if (Race == ActorRace.Play)
            {
                SysMsg(Format(Settings.YouPoisoned, nTime, nPoint), MsgColor.Red, MsgType.Hint);
            }
            return true;
        }

        /// <summary>
        /// 检查是否正有跨服数据
        /// </summary>
        /// <returns></returns>
        public bool CheckServerMakeSlave()
        {
            bool result = false;
            for (int i = 0; i < MsgQueue.Count; i++)
            {
                if (MsgQueue.TryPeek(out SendMessage sendMessage, out _))
                {
                    if (sendMessage.wIdent == Messages.RM_10401)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        protected bool GetRecallXy(short nX, short nY, int nRange, ref short nDx, ref short nDy)
        {
            bool result = false;
            if (Envir.GetMovingObject(nX, nY, true) == null)
            {
                result = true;
                nDx = nX;
                nDy = nY;
            }
            if (!result)
            {
                for (int i = 0; i < nRange; i++)
                {
                    for (int j = -i; j <= i; j++)
                    {
                        for (int k = -i; k <= i; k++)
                        {
                            nDx = (short)(nX + k);
                            nDy = (short)(nY + j);
                            if (Envir.GetMovingObject(nDx, nDy, true) == null)
                            {
                                result = true;
                                break;
                            }
                        }
                        if (result)
                        {
                            break;
                        }
                    }
                    if (result)
                    {
                        break;
                    }
                }
            }
            if (!result)
            {
                nDx = nX;
                nDy = nY;
            }
            return result;
        }

        /// <summary>
        /// 破魔法盾
        /// </summary>
        /// <param name="nInt"></param>
        internal void DamageBubbleDefence(int nInt)
        {
            if (StatusTimeArr[PoisonState.BubbleDefenceUP] > 0)
            {
                if (StatusTimeArr[PoisonState.BubbleDefenceUP] > 3)
                {
                    StatusTimeArr[PoisonState.BubbleDefenceUP] -= 3;
                }
                else
                {
                    StatusTimeArr[PoisonState.BubbleDefenceUP] = 1;
                }
            }
        }

        public bool MagCanHitTarget(short nX, short nY, IActor targetObject)
        {
            bool result = false;
            if (targetObject == null)
            {
                return false;
            }
            int n20 = Math.Abs(nX - targetObject.CurrX) + Math.Abs(nY - targetObject.CurrY);
            int n14 = 0;
            while (n14 < 13)
            {
                byte dir = M2Share.GetNextDirection(nX, nY, targetObject.CurrX, targetObject.CurrY);
                if (Envir.GetNextPosition(nX, nY, dir, 1, ref nX, ref nY) && Envir.IsValidCell(nX, nY))
                {
                    if ((nX == targetObject.CurrX) && (nY == targetObject.CurrY))
                    {
                        result = true;
                        break;
                    }
                    int n1C = Math.Abs(nX - targetObject.CurrX) + Math.Abs(nY - targetObject.CurrY);
                    if (n1C > n20)
                    {
                        result = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
                n14++;
            }
            return result;
        }

        public int MagMakeDefenceArea(int nX, int nY, int nRange, int nSec, byte btState)
        {
            int result = 0;
            int nStartX = nX - nRange;
            int nEndX = nX + nRange;
            int nStartY = nY - nRange;
            int nEndY = nY + nRange;
            for (int cX = nStartX; cX <= nEndX; cX++)
            {
                for (int cY = nStartY; cY <= nEndY; cY++)
                {
                    ref MapCellInfo cellInfo = ref Envir.GetCellInfo(nX, nY, out bool cellSuccess);
                    if (cellSuccess && cellInfo.IsAvailable)
                    {
                        for (int i = 0; i < cellInfo.ObjList.Count; i++)
                        {
                            CellObject cellObject = cellInfo.ObjList[i];
                            if ((cellObject.CellObjId > 0) && (cellObject.CellType == CellType.Play || cellObject.CellType == CellType.Monster))
                            {
                                IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                if ((baseObject != null) && (!baseObject.Ghost))
                                {
                                    if (IsProperFriend(baseObject))
                                    {
                                        if (btState == 0)
                                        {
                                            baseObject.DefenceUp(nSec);
                                        }
                                        else
                                        {
                                            baseObject.MagDefenceUp(nSec);
                                        }
                                        result++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool DefenceUp(int nSec)
        {
            bool result = false;
            if (StatusTimeArr[PoisonState.DefenceUP] > 0)
            {
                if (StatusTimeArr[PoisonState.DefenceUP] < nSec)
                {
                    StatusTimeArr[PoisonState.DefenceUP] = (ushort)nSec;
                    result = true;
                }
            }
            else
            {
                StatusTimeArr[PoisonState.DefenceUP] = (ushort)nSec;
                result = true;
            }
            StatusArrTick[PoisonState.DefenceUP] = HUtil32.GetTickCount();
            SysMsg(Format(Settings.DefenceUpTime, nSec), MsgColor.Green, MsgType.Hint);
            RecalcAbilitys();
            SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
            return result;
        }

        public bool MagDefenceUp(int nSec)
        {
            bool result = false;
            if (StatusTimeArr[PoisonState.MagDefenceUP] > 0)
            {
                if (StatusTimeArr[PoisonState.MagDefenceUP] < nSec)
                {
                    StatusTimeArr[PoisonState.MagDefenceUP] = (ushort)nSec;
                    result = true;
                }
            }
            else
            {
                StatusTimeArr[PoisonState.MagDefenceUP] = (ushort)nSec;
                result = true;
            }
            StatusArrTick[PoisonState.MagDefenceUP] = HUtil32.GetTickCount();
            SysMsg(Format(Settings.MagDefenceUpTime, nSec), MsgColor.Green, MsgType.Hint);
            RecalcAbilitys();
            SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
            return result;
        }

        public UserItem CheckItems(string sItemName)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem userItem = ItemList[i];
                if (userItem == null)
                {
                    continue;
                }
                if (string.Compare(ModuleShare.ItemSystem.GetStdItemName(userItem.Index), sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return userItem;
                }
            }
            return null;
        }

        protected void DelBagItem(int nIndex)
        {
            if ((nIndex < 0) || (nIndex >= ItemList.Count))
            {
                return;
            }
            Dispose(ItemList[nIndex]);
            ItemList.RemoveAt(nIndex);
        }

        public bool DelBagItem(int nItemIndex, string sItemName)
        {
            bool result = false;
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem userItem = ItemList[i];
                if ((userItem.MakeIndex == nItemIndex) &&
                    string.Compare(ModuleShare.ItemSystem.GetStdItemName(userItem.Index), sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Dispose(userItem);
                    ItemList.RemoveAt(i);
                    result = true;
                    break;
                }
            }
            if (result)
            {
                WeightChanged();
            }
            return result;
        }

        public bool CanMove(short nX, short nY, bool boFlag)
        {
            if (Math.Abs(CurrX - nX) <= 1 && Math.Abs(CurrX - nY) <= 1)
            {
                return Envir.CanWalkEx(nX, nY, boFlag);
            }
            return CanRun(nX, nY, boFlag);
        }

        protected bool CanMove(short nCurrX, short nCurrY, short nX, short nY, bool boFlag)
        {
            if ((Math.Abs(nCurrX - nX) <= 1) && (Math.Abs(nCurrY - nY) <= 1))
            {
                return Envir.CanWalkEx(nX, nY, boFlag);
            }
            return CanRun(nCurrX, nCurrY, nX, nY, boFlag);
        }

        private bool AdminCanRun()
        {
            if (Race == ActorRace.Play)
            {
                return ((((PlayObject)this).Permission > 9) && ModuleShare.Config.boGMRunAll);
            }
            return false;
        }

        public bool CanRun(short nCurrX, short nCurrY, short nX, short nY, bool boFlag)
        {
            byte btDir = M2Share.GetNextDirection(nCurrX, nCurrY, nX, nY);
            bool canWalk = (ModuleShare.Config.DiableHumanRun || AdminCanRun()) || (ModuleShare.Config.boSafeAreaLimited && InSafeZone());
            switch (btDir)
            {
                case Direction.Up:
                    if (nCurrY > 1)
                    {
                        if ((Envir.CanWalkEx(nCurrX, nCurrY - 1, canWalk)) && (Envir.CanWalkEx(nCurrX, nCurrY - 2, canWalk)))
                        {
                            return true;
                        }
                    }
                    break;
                case Direction.UpRight:
                    if (nCurrX < Envir.Width - 2 && nCurrY > 1)
                    {
                        if ((Envir.CanWalkEx(nCurrX + 1, nCurrY - 1, canWalk)) && (Envir.CanWalkEx(nCurrX + 2, nCurrY - 2, canWalk)))
                        {
                            return true;
                        }
                    }
                    break;
                case Direction.Right:
                    if (nCurrX < Envir.Width - 2)
                    {
                        if (Envir.CanWalkEx(nCurrX + 1, nCurrY, canWalk) && (Envir.CanWalkEx(nCurrX + 2, nCurrY, canWalk)))
                        {
                            return true;
                        }
                    }
                    break;
                case Direction.DownRight:
                    if ((nCurrX < Envir.Width - 2) && (nCurrY < Envir.Height - 2) && (Envir.CanWalkEx(nCurrX + 1, nCurrY + 1, canWalk) && (Envir.CanWalkEx(nCurrX + 2, nCurrY + 2, canWalk))))
                    {
                        return true;
                    }
                    break;
                case Direction.Down:
                    if ((nCurrY < Envir.Height - 2) && (Envir.CanWalkEx(nCurrX, nCurrY + 1, canWalk && (Envir.CanWalkEx(nCurrX, nCurrY + 2, canWalk)))))
                    {
                        return true;
                    }
                    break;
                case Direction.DownLeft:
                    if ((nCurrX > 1) && (nCurrY < Envir.Height - 2) && (Envir.CanWalkEx(nCurrX - 1, nCurrY + 1, canWalk)) && (Envir.CanWalkEx(nCurrX - 2, nCurrY + 2, canWalk)))
                    {
                        return true;
                    }
                    break;
                case Direction.Left:
                    if ((nCurrX > 1) && (Envir.CanWalkEx(nCurrX - 1, nCurrY, canWalk)) && (Envir.CanWalkEx(nCurrX - 2, nCurrY, canWalk)))
                    {
                        return true;
                    }
                    break;
                case Direction.UpLeft:
                    if ((nCurrX > 1) && (nCurrY > 1) && (Envir.CanWalkEx(nCurrX - 1, nCurrY - 1, canWalk)) && (Envir.CanWalkEx(nCurrX - 2, nCurrY - 2, canWalk)))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        private bool CanRun(short nX, short nY, bool boFlag)
        {
            byte btDir = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            bool canWalk = (ModuleShare.Config.DiableHumanRun || AdminCanRun()) || (ModuleShare.Config.boSafeAreaLimited && InSafeZone());
            switch (btDir)
            {
                case Direction.Up:
                    if (CurrY > 1)
                    {
                        if ((Envir.CanWalkEx(CurrX, CurrY - 1, canWalk)) && (Envir.CanWalkEx(CurrX, CurrY - 2, canWalk)))
                        {
                            return true;
                        }
                    }
                    break;
                case Direction.UpRight:
                    if (CurrX < Envir.Width - 2 && CurrY > 1)
                    {
                        if ((Envir.CanWalkEx(CurrX + 1, CurrY - 1, canWalk)) && (Envir.CanWalkEx(CurrX + 2, CurrY - 2, canWalk)))
                        {
                            return true;
                        }
                    }
                    break;
                case Direction.Right:
                    if (CurrX < Envir.Width - 2)
                    {
                        if (Envir.CanWalkEx(CurrX + 1, CurrY, canWalk && (Envir.CanWalkEx(CurrX + 2, CurrY, canWalk))))
                        {
                            return true;
                        }
                    }
                    break;
                case Direction.DownRight:
                    if ((CurrX < Envir.Width - 2) && (CurrY < Envir.Height - 2) && (Envir.CanWalkEx(CurrX + 1, CurrY + 1, canWalk) && (Envir.CanWalkEx(CurrX + 2, CurrY + 2, canWalk))))
                    {
                        return true;
                    }
                    break;
                case Direction.Down:
                    if ((CurrY < Envir.Height - 2)
                        && (Envir.CanWalkEx(CurrX, CurrY + 1, canWalk) && (Envir.CanWalkEx(CurrX, CurrY + 2, canWalk))))
                    {
                        return true;
                    }
                    break;
                case Direction.DownLeft:
                    if ((CurrX > 1) && (CurrY < Envir.Height - 2) && (Envir.CanWalkEx(CurrX - 1, CurrY + 1, canWalk)) && (Envir.CanWalkEx(CurrX - 2, CurrY + 2, canWalk)))
                    {
                        return true;
                    }
                    break;
                case Direction.Left:
                    if ((CurrX > 1) && (Envir.CanWalkEx(CurrX - 1, CurrY, canWalk)) && (Envir.CanWalkEx(CurrX - 2, CurrY, canWalk)))
                    {
                        return true;
                    }
                    break;
                case Direction.UpLeft:
                    if ((CurrX > 1) && (CurrY > 1) && (Envir.CanWalkEx(CurrX - 1, CurrY - 1, canWalk)) && (Envir.CanWalkEx(CurrX - 2, CurrY - 2, canWalk)))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        public IActor GetMaster()
        {
            if (Race != ActorRace.Play)
            {
                IActor masterObject = Master;
                if (masterObject != null)
                {
                    while (true)
                    {
                        if (masterObject.Master != null)
                        {
                            masterObject = masterObject.Master;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return masterObject;
            }
            return null;
        }

        private void KillFunc()
        {
            if (ExpHitter != null)
            {
                var fightExp = ((AnimalObject)this).FightExp; //后续移动到AnimalObject实现,减少一次转换
                if (ExpHitter.Master != null) //如果是角色下属杀死对象
                {
                    ExpHitter.Master.SendMsg(Messages.RM_PLAYERKILLMONSTER, this.ActorId, fightExp, 0, 0);
                    SendMsg(Messages.RM_DIEDROPITEM, ExpHitter.Master.ActorId, 0, 0, 0);
                }
                if (ExpHitter.Race == ActorRace.Play)
                {
                    ExpHitter.SendMsg(Messages.RM_PLAYERKILLMONSTER, this.ActorId, fightExp, 0, 0);
                    SendMsg(Messages.RM_DIEDROPITEM, ExpHitter.ActorId, 0, 0, 0);
                }
            }
        }

        /// <summary>
        /// 死亡掉落物品
        /// </summary>
        private void DieDropItems(int actorId)
        {
            if (!Envir.Flag.FightZone && !Envir.Flag.Fight3Zone && !this.Animal)
            {
                this.DropUseItems(actorId);
                if (Master == null && (!NoItem || !Envir.Flag.NoDropItem))
                {
                    this.ScatterBagItems(actorId);
                }
                if (this.Race >= ActorRace.Animal && Master == null && (!NoItem || !Envir.Flag.NoDropItem))
                {
                    this.ScatterGolds(actorId);
                }
            }
        }

        public bool ReAliveEx(MonGenInfo monGen)
        {
            WAbil = Abil;
            Gold = 0;
            NoItem = false;
            StoneMode = false;
            Skeleton = false;
            ShowHp = false;
            FixedHideMode = false;
            if (Race >= ActorRace.Animal)
            {
                ((MonsterObject)this).CrazyMode = false;
                ((AnimalObject)this).HolySeize = false;
            }
            if (this is CastleDoor)
            {
                ((CastleDoor)this).IsOpened = false;
                StickMode = true;
            }
            if (this is MagicMonster)
            {
                ((MagicMonster)this).DupMode = false;
            }
            if (this is MagicMonObject)
            {
                ((MagicMonObject)this).UseMagic = false;
            }
            if (this is RockManObject)
            {
                HideMode = false;
            }
            if (this is WallStructure)
            {
                ((WallStructure)this).SetMapFlaged = false;
            }
            if (this is SoccerBall)
            {
                ((SoccerBall)this).N550 = 0;
                ((SoccerBall)this).TargetX = -1;
            }
            if (this is FrostTiger)
            {
                //((TFrostTiger)(this)).m_boApproach = false;
            }
            if (this is CowKingMonster)
            {
                /*((TCowKingMonster)(this)).m_boCowKingMon = true;
                ((TCowKingMonster)(this)).m_nDangerLevel = 0;
                ((TCowKingMonster)(this)).m_boDanger = false;
                ((TCowKingMonster)(this)).m_boCrazy = false;*/
            }
            if (this is DigOutZombi)
            {
                FixedHideMode = true;
            }

            if (this is WhiteSkeleton)
            {
                ((WhiteSkeleton)this).BoIsFirst = true;
                FixedHideMode = true;
            }

            if (this is ScultureMonster)
            {
                FixedHideMode = true;
            }

            if (this is ScultureKingMonster)
            {
                StoneMode = true;
                CharStatusEx = PoisonState.STONEMODE;
            }

            if (this is ElfMonster)
            {
                FixedHideMode = true;
                ((ElfMonster)this).NoAttackMode = true;
                ((ElfMonster)this).BoIsFirst = true;
            }

            if (this is ElfWarriorMonster)
            {
                FixedHideMode = true;
                ((ElfWarriorMonster)this).BoIsFirst = true;
                ((ElfWarriorMonster)this).UsePoison = false;
            }

            if (this is ElectronicScolpionMon)
            {
                ((ElectronicScolpionMon)this).UseMagic = false;
                //((TElectronicScolpionMon)(this)).m_boApproach = false;
            }

            if (this is DoubleCriticalMonster)
            {
                //((TDoubleCriticalMonster)(this)).m_n7A0 = 0;
            }

            if (this is StickMonster)
            {
                SearchTick = HUtil32.GetTickCount();
                FixedHideMode = true;
                StickMode = true;
            }
            //m_nBodyLeathery = m_nPerBodyLeathery;
            //m_nPushedCount = 0;
            //m_nBodyState = 0;

            if (Animal)
            {
                ((AnimalObject)this).MeatQuality = (ushort)(M2Share.RandomNumber.Random(3500) + 3000);
            }

            switch (Race)
            {
                case 51:
                    ((AnimalObject)this).MeatQuality = (ushort)(M2Share.RandomNumber.Random(3500) + 3000);
                    BodyLeathery = 50;
                    break;
                case 52:
                    if (M2Share.RandomNumber.Random(30) == 0)
                    {
                        ((AnimalObject)this).MeatQuality = (ushort)(M2Share.RandomNumber.Random(20000) + 10000);
                        BodyLeathery = 150;
                    }
                    else
                    {
                        ((AnimalObject)this).MeatQuality = (ushort)(M2Share.RandomNumber.Random(8000) + 8000);
                        BodyLeathery = 150;
                    }
                    break;
                case 53:
                    ((AnimalObject)this).MeatQuality = (ushort)(M2Share.RandomNumber.Random(8000) + 8000);
                    BodyLeathery = 150;
                    break;
                case 54:
                    Animal = true;
                    break;
                case 95:
                    if (M2Share.RandomNumber.Random(2) == 0)
                    {
                        // m_boSafeWalk = true;
                    }
                    break;
                case 96:
                    if (M2Share.RandomNumber.Random(4) == 0)
                    {
                        // m_boSafeWalk = true;
                    }
                    break;
                case 97:
                    if (M2Share.RandomNumber.Random(2) == 0)
                    {
                        // m_boSafeWalk = true;
                    }
                    break;
                case 169:
                    StickMode = false;
                    break;
                case 170:
                    StickMode = true;
                    break;
            }

            //UseItems = new UserItem[13];
            if (ItemList != null)
            {
                for (int i = 0; i < ItemList.Count; i++)
                {
                    ItemList[i] = null;
                }
                ItemList.Clear();
            }

            OnEnvirnomentChanged();
            CharStatus = GetCharStatus();
            StatusChanged();
            if (Envir == null)
            {
                return false;
            }
            short nX = (short)(monGen.X - monGen.Range + M2Share.RandomNumber.Random(monGen.Range * 2 + 1));
            short nY = (short)(monGen.Y - monGen.Range + M2Share.RandomNumber.Random(monGen.Range * 2 + 1));
            bool mBoErrorOnInit = true;
            if (Envir.CanWalk(nX, nY, true))
            {
                CurrX = nX;
                CurrY = nY;
                if (AddToMap())
                {
                    mBoErrorOnInit = false;
                }
            }

            int nRange = 0;
            int nRange2 = 0;
            if (mBoErrorOnInit)
            {
                if (Envir.Width < 50)
                {
                    nRange = 2;
                }
                else
                {
                    nRange = 3;
                }

                if (Envir.Height < 250)
                {
                    if (Envir.Height < 30)
                    {
                        nRange2 = 2;
                    }
                    else
                    {
                        nRange2 = 20;
                    }
                }
                else
                {
                    nRange2 = 50;
                }
            }

            int nC = 0;
            bool addObj = false;
            short nX2 = CurrX;
            short nY2 = CurrY;
            while (true)
            {
                if (!Envir.CanWalk(nX, nY, false))
                {
                    if ((Envir.Width - nRange2 - 1) > nX)
                    {
                        nX = (short)(nX + nRange);
                    }
                    else
                    {
                        nX = (short)(M2Share.RandomNumber.Random(Envir.Width / 2) + nRange2);
                    }

                    if (Envir.Height - nRange2 - 1 > nY)
                    {
                        nY = (short)(nY + nRange);
                    }
                    else
                    {
                        nY = (short)(M2Share.RandomNumber.Random(Envir.Height / 2) + nRange2);
                    }
                }
                else
                {
                    CurrX = nX;
                    CurrY = nY;
                    addObj = Envir.AddMapObject(nX, nY, CellType, this.ActorId, this);
                    break;
                }
                nC++;
                if (nC > 46)
                {
                    break;
                }
            }
            if (!addObj)
            {
                CurrX = nX2;
                CurrY = nY2;
                Envir.AddMapObject(CurrX, CurrY, CellType, this.ActorId, this);
            }
            Abil.HP = Abil.MaxHP;
            Abil.MP = Abil.MaxMP;
            WAbil.HP = WAbil.MaxHP;
            WAbil.MP = WAbil.MaxMP;
            RecalcAbilitys();
            Death = false;
            Invisible = false;
            SendRefMsg(Messages.RM_TURN, Dir, CurrX, CurrY, GetFeatureToLong(), "");
            ((AnimalObject)this).MonsterSayMessage(null, MonStatus.MonGen);
            return true;
        }

        public void OnEnvirnomentChanged()
        {
            if (CanReAlive)
            {
                if ((MonGen != null) && (MonGen.Envir != Envir))
                {
                    CanReAlive = false;
                    if (MonGen.ActiveCount > 0)
                    {
                        MonGen.ActiveCount--;
                    }
                    MonGen = null;
                }
            }
            //if ((m_PEnvir != null))
            //{
            //    if (m_nLastMapSecret != m_PEnvir.Flag.nSecret)
            //    {
            //        if (m_btRaceServer == ActorRace.Play)
            //        {
            //            if ((m_btRaceServer = ActorRace.Play) && (m_nLastMapSecret != -1))
            //            {
            //                var i = GetFeatureToLong();
            //                var sSENDMSG = string.Empty;
            //                var nSafeX = GetTitleIndex();
            //                if (nSafeX > 0)
            //                {
            //                    var MessageBodyW = new TMessageBodyW();
            //                    MessageBodyW.Param1 = HUtil32.MakeWord(nSafeX, 0);
            //                    MessageBodyW.Param2 = 0;
            //                    MessageBodyW.Tag1 = 0;
            //                    MessageBodyW.Tag2 = 0;
            //                    sSENDMSG = EDcode.EncodeBuffer(@MessageBodyW);
            //                }
            //                ((PlayObject)(this)).m_DefMsg = EDCode.MakeDefaultMsg(Messages.SM_FEATURECHANGED, this.ObjectId, HUtil32.LoWord(i), HUtil32.HiWord(i), GetFeatureEx());
            //                ((PlayObject)(this)).SendSocket(((PlayObject)(this)).m_DefMsg, sSENDMSG);
            //                ((PlayObject)(this)).protectedPowerPointChanged();
            //                SendUpdateMsg(this, Messages.RM_USERNAME, 0, 0, 0, 0, GetShowName());
            //            }
            //            HealthSpellChanged();
            //        }
            //        m_nLastMapSecret = m_PEnvir.Flag.nSecret;
            //    }
            //}
            //m_nCurEnvirIdx = -1;
            //m_nCastleEnvirListIdx = -1;
            //m_CurSafeZoneList.Clear();
            //for (int i = 0; i < M2Share.StartPointList.Count; i++)
            //{
            //    var StartPointInfo = M2Share.StartPointList[i];
            //    if (StartPointInfo.MapName == m_PEnvir.sMapName)
            //    {
            //        m_CurSafeZoneList.Add(StartPointInfo);
            //    }
            //}
            //if ((m_btRaceServer == ActorRace.Play) && !((PlayObject)(this)).m_boOffLineFlag)
            //{
            //   ((PlayObject)(this)).CheckMapEvent(5, "");
            //}
        }

        public static void GetMapBaseObjects(IEnvirnoment envir, int nX, int nY, int nRage, ref IList<IActor> objectList)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::GetMapBaseObjects";
            try
            {
                int nStartX = nX - nRage;
                int nEndX = nX + nRage;
                int nStartY = nY - nRage;
                int nEndY = nY + nRage;
                for (int x = nStartX; x <= nEndX; x++)
                {
                    for (int y = nStartY; y <= nEndY; y++)
                    {
                        ref MapCellInfo cellInfo = ref envir.GetCellInfo(x, y, out bool cellSuccess);
                        if (cellSuccess && cellInfo.IsAvailable)
                        {
                            for (int i = 0; i < cellInfo.ObjList.Count; i++)
                            {
                                CellObject cellObject = cellInfo.ObjList[i];
                                if (cellObject.CellObjId > 0 && cellObject.ActorObject)
                                {
                                    IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                    if (baseObject != null && !baseObject.Death && !baseObject.Ghost)
                                    {
                                        objectList.Add(baseObject);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error(sExceptionMsg);
            }
        }

        protected static void Dispose(object obj)
        {
            obj = null;
        }

        protected static string Format(string str, params object[] par)
        {
            return string.Format(str, par);
        }

        public void WinExp(int exp)
        {
            throw new NotImplementedException();
        }
    }
}