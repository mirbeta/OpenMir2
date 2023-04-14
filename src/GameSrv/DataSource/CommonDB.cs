using System.Data;
using System.Text.Json;
using GameSrv.GameCommand;
using GameSrv.Items;
using GameSrv.Npc;
using MySqlConnector;
using NLog;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Extensions;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.DataSource
{
    public class CommonDB
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IDbConnection _dbConnection;

        public int LoadItemsDB()
        {
            var result = -1;
            const string sSQLString = "SELECT * FROM stditems";
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
                for (var i = 0; i < M2Share.WorldEngine.StdItemList.Count; i++)
                {
                    M2Share.WorldEngine.StdItemList[i] = null;
                }
                M2Share.WorldEngine.StdItemList.Clear();
                if (!Open())
                {
                    return result;
                }
                using (var dr = Query(sSQLString))
                {
                    while (dr.Read())
                    {
                        var stdItem = new StdItem();
                        var idx = dr.GetInt32("ID");
                        stdItem.Name = dr.GetString("NAME");
                        stdItem.StdMode = dr.GetByte("StdMode");
                        stdItem.Shape = dr.GetByte("SHAPE");
                        stdItem.Weight = dr.GetByte("WEIGHT");
                        stdItem.AniCount = dr.GetByte("ANICOUNT");
                        stdItem.SpecialPwr = dr.GetSByte("SOURCE");
                        stdItem.ItemDesc = dr.GetByte("RESERVED");
                        stdItem.Looks = dr.GetUInt16("IMGINDEX");
                        stdItem.DuraMax = dr.GetUInt16("DURAMAX");
                        stdItem.AC = HUtil32.MakeWord(dr.GetUInt16("AC"), dr.GetUInt16("ACMAX"));
                        stdItem.MAC = HUtil32.MakeWord(dr.GetUInt16("MAC"), dr.GetUInt16("MACMAX"));
                        stdItem.DC = HUtil32.MakeWord(dr.GetUInt16("DC"), dr.GetUInt16("DCMAX"));
                        stdItem.MC = HUtil32.MakeWord(dr.GetUInt16("MC"), dr.GetUInt16("MCMAX"));
                        stdItem.SC = HUtil32.MakeWord(dr.GetUInt16("SC"), dr.GetUInt16("SCMAX"));
                        stdItem.Need = dr.GetByte("NEED");
                        stdItem.NeedLevel = dr.GetByte("NEEDLEVEL");
                        stdItem.NeedIdentify = 0;
                        stdItem.Price = dr.GetInt32("PRICE");
                        stdItem.Stock = dr.GetInt32("STOCK");
                        stdItem.AtkSpd = dr.GetByte("ATKSPD");
                        stdItem.Agility = dr.GetByte("AGILITY");
                        stdItem.Accurate = dr.GetByte("ACCURATE");
                        stdItem.MgAvoid = dr.GetByte("MGAVOID");
                        stdItem.Strong = dr.GetByte("STRONG");
                        stdItem.Undead = dr.GetByte("UNDEAD");
                        stdItem.HpAdd = dr.GetInt32("HPADD");
                        stdItem.MpAdd = dr.GetInt32("MPADD");
                        stdItem.ExpAdd = dr.GetInt32("EXPADD");
                        stdItem.EffType1 = dr.GetByte("EFFTYPE1");
                        stdItem.EffRate1 = dr.GetByte("EFFRATE1");
                        stdItem.EffValue1 = dr.GetByte("EFFVALUE1");
                        stdItem.EffType2 = dr.GetByte("EFFTYPE2");
                        stdItem.EffRate2 = dr.GetByte("EFFRATE2");
                        stdItem.EffValue2 = dr.GetByte("EFFVALUE2");
                        stdItem.Slowdown = dr.GetByte("SLOWDOWN");
                        stdItem.Tox = dr.GetByte("TOX");
                        stdItem.ToxAvoid = dr.GetByte("TOXAVOID");
                        stdItem.UniqueItem = dr.GetByte("UNIQUEITEM");
                        stdItem.OverlapItem = dr.GetByte("OVERLAPITEM");
                        stdItem.Light = dr.GetByte("LIGHT");
                        stdItem.ItemType = dr.GetByte("ITEMTYPE");
                        stdItem.ItemSet = dr.GetUInt16("ITEMSET");
                        stdItem.Reference = dr.GetString("REFERENCE");
                        if (M2Share.WorldEngine.StdItemList.Count <= idx)
                        {
                            M2Share.WorldEngine.StdItemList.Add(stdItem);
                            result = 1;
                        }
                        else
                        {
                            logger.Error($"加载物品(Idx:{idx} Name:{stdItem.Name})数据失败!!!");
                            result = -100;
                            return result;
                        }
                    }
                }
                M2Share.GameLogGold = M2Share.GetGameLogItemNameList(Grobal2.StringGoldName) == 1;
                M2Share.GameLogHumanDie = M2Share.GetGameLogItemNameList(CommandHelp.HumanDieEvent) == 1;
                M2Share.GameLogGameGold = M2Share.GetGameLogItemNameList(M2Share.Config.GameGoldName) == 1;
                M2Share.GameLogGamePoint = M2Share.GetGameLogItemNameList(M2Share.Config.GamePointName) == 1;
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
                return result;
            }
            finally
            {
                Close();
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
            return result;
        }

        public int LoadMagicDB()
        {
            const string sSQLString = "select * from magics";
            var result = -1;
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                M2Share.WorldEngine.SwitchMagicList();
                if (!Open())
                {
                    return result;
                }
                using var dr = Query(sSQLString);
                while (dr.Read())
                {
                    var magic = new MagicInfo
                    {
                        MagicId = dr.GetUInt16("MagId"),
                        MagicName = dr.GetString("MagName"),
                        EffectType = (byte)dr.GetInt32("EffectType"),
                        Effect = (byte)dr.GetInt32("Effect"),
                        Spell = dr.GetUInt16("Spell"),
                        Power = dr.GetUInt16("Power"),
                        MaxPower = dr.GetUInt16("MaxPower"),
                        Job = (byte)dr.GetInt32("Job")
                    };
                    magic.TrainLevel[0] = (byte)dr.GetInt32("NeedL1");
                    magic.TrainLevel[1] = (byte)dr.GetInt32("NeedL2");
                    magic.TrainLevel[2] = (byte)dr.GetInt32("NeedL3");
                    magic.TrainLevel[3] = (byte)dr.GetInt32("NeedL3");
                    magic.MaxTrain[0] = dr.GetInt32("L1Train");
                    magic.MaxTrain[1] = dr.GetInt32("L2Train");
                    magic.MaxTrain[2] = dr.GetInt32("L3Train");
                    magic.MaxTrain[3] = magic.MaxTrain[2];
                    magic.TrainLv = 3;
                    magic.DelayTime = dr.GetInt32("Delay");
                    magic.DefSpell = (byte)dr.GetInt32("DefSpell");
                    magic.DefPower = (byte)dr.GetInt32("DefPower");
                    magic.DefMaxPower = (byte)dr.GetInt32("DefMaxPower");
                    magic.Desc = dr.GetString("Descr");
                    if (magic.MagicId > 0)
                    {
                        M2Share.WorldEngine.MagicList.Add(magic);
                    }
                    else
                    {
                        magic = null;
                    }
                    result = 1;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
            }
            finally
            {
                Close();
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
            return result;
        }

        public int LoadMonsterDB()
        {
            var result = 0;
            const string sSQLString = "select * from monsters";
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                M2Share.WorldEngine.MonsterList.Clear();
                if (!Open())
                {
                    return result;
                }
                using var dr = Query(sSQLString);
                while (dr.Read())
                {
                    var monster = new MonsterInfo
                    {
                        ItemList = new List<MonsterDropItem>(),
                        Name = dr.GetString("NAME").Trim(),
                        Race = (byte)dr.GetInt32("Race"),
                        RaceImg = (byte)dr.GetInt32("RaceImg"),
                        Appr = dr.GetUInt16("Appr"),
                        Level = dr.GetByte("Lvl"),
                        btLifeAttrib = (byte)dr.GetInt32("Undead"),
                        CoolEye = dr.GetByte("CoolEye"),
                        Exp = dr.GetInt32("Exp")
                    };
                    // 城门或城墙的状态跟HP值有关，如果HP异常，将导致城墙显示不了
                    if (monster.Race == ActorRace.SabukWall || monster.Race == ActorRace.SabukDoor)
                    {
                        monster.HP = dr.GetUInt16("HP");// 如果为城墙或城门由HP不加倍
                    }
                    else
                    {
                        monster.HP = (ushort)HUtil32.Round(dr.GetInt32("HP") * (M2Share.Config.MonsterPowerRate / 10.0));
                    }
                    monster.MP = (ushort)HUtil32.Round(dr.GetInt32("MP") * (M2Share.Config.MonsterPowerRate / 10.0));
                    monster.AC = (ushort)HUtil32.Round(dr.GetInt32("AC") * (M2Share.Config.MonsterPowerRate / 10.0));
                    monster.MAC = (ushort)HUtil32.Round(dr.GetInt32("MAC") * (M2Share.Config.MonsterPowerRate / 10.0));
                    monster.DC = (ushort)HUtil32.Round(dr.GetInt32("DC") * (M2Share.Config.MonsterPowerRate / 10.0));
                    monster.MaxDC = (ushort)HUtil32.Round(dr.GetInt32("DCMAX") * (M2Share.Config.MonsterPowerRate / 10.0));
                    monster.MC = (ushort)HUtil32.Round(dr.GetInt32("MC") * (M2Share.Config.MonsterPowerRate / 10.0));
                    monster.SC = (ushort)HUtil32.Round(dr.GetInt32("SC") * (M2Share.Config.MonsterPowerRate / 10.0));
                    monster.Speed = dr.GetByte("SPEED");
                    monster.HitPoint = dr.GetByte("HIT");
                    monster.WalkSpeed = (ushort)HUtil32._MAX(200, dr.GetInt32("WALK_SPD"));
                    monster.WalkStep = (ushort)HUtil32._MAX(1, dr.GetInt32("WalkStep"));
                    monster.WalkWait = (ushort)dr.GetInt32("WalkWait");
                    monster.AttackSpeed = (ushort)dr.GetInt32("ATTACK_SPD");
                    if (monster.WalkSpeed < 200)
                    {
                        monster.WalkSpeed = 200;
                    }
                    if (monster.AttackSpeed < 200)
                    {
                        monster.AttackSpeed = 200;
                    }
                    monster.ItemList = null;
                    M2Share.LocalDb.LoadMonitems(monster.Name, ref monster.ItemList);
                    if (M2Share.WorldEngine.MonsterList.ContainsKey(monster.Name))
                    {
                        logger.Warn($"怪物名称[{monster.Name}]重复,请确认数据是否正常.");
                        continue;
                    }
                    if (monster.ItemList == null || monster.ItemList.Count <= 0)
                    {
                        logger.Debug($"怪物[{monster.Name}]爆率文件为空.");
                        continue;
                    }
                    M2Share.WorldEngine.MonsterList.Add(monster.Name, monster);
                    result = 1;
                }
            }
            finally
            {
                Close();
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 加载寄售系统数据
        /// </summary>
        public void LoadSellOffItemList()
        {
            if (!Open())
            {
                logger.Error("读取物品寄售列表失败.");
                return;
            }
            try
            {
                const string sSQLString = "select * from goldsales";
                using var dr = Query(sSQLString);
                while (dr.Read())
                {
                    var sDealChrName = dr.GetString("DealChrName");
                    var sBuyChrName = dr.GetString("BuyChrName");
                    var dSellDateTime = dr.GetDateTime("SellDateTime");
                    var nState = dr.GetByte("State");
                    var nSellGold = dr.GetInt16("SellGold");
                    var sUseItems = dr.GetString("UseItems");
                    if ((!string.IsNullOrEmpty(sDealChrName)) && (!string.IsNullOrEmpty(sBuyChrName)) && (nState < 4))
                    {
                        var DealOffInfo = new DealOffInfo();
                        DealOffInfo.sDealChrName = sDealChrName;
                        DealOffInfo.sBuyChrName = sBuyChrName;
                        DealOffInfo.dSellDateTime = dSellDateTime;
                        DealOffInfo.nSellGold = nSellGold;
                        DealOffInfo.UseItems = JsonSerializer.Deserialize<UserItem[]>(sUseItems);
                        DealOffInfo.Flag = nState;
                        M2Share.SellOffItemList.Add(DealOffInfo);
                    }
                }
                logger.Info($"读取物品寄售列表成功...[{M2Share.SellOffItemList.Count}]");
            }
            finally
            {
                Close();
            }
        }

        private const string SaveSellItemSql = "INSERT INTO goldsales (DealChrName, BuyChrName, SellDateTime, State, SellGold,UseItems) values (@DealChrName, @BuyChrName, @SellDateTime, @State, @SellGold,@UseItems))";

        /// <summary>
        /// 保存寄售系统数据
        /// </summary>
        public void SaveSellOffItemList()
        {
            if (!Open())
            {
                logger.Error("保存物品寄售数据失败.");
                return;
            }
            DealOffInfo DealOffInfo;
            const string sSQLString = "delete from goldsales";
            try
            {
                if (M2Share.SellOffItemList.Count > 0)
                {
                    Execute(sSQLString);
                    for (var i = 0; i < M2Share.SellOffItemList.Count; i++)
                    {
                        DealOffInfo = M2Share.SellOffItemList[i];
                        if (DealOffInfo != null)
                        {
                            var command = new MySqlCommand();
                            command.Connection = (MySqlConnection)_dbConnection;
                            command.CommandText = SaveSellItemSql;
                            command.Parameters.AddWithValue("@DealChrName", DealOffInfo.sDealChrName);
                            command.Parameters.AddWithValue("@BuyChrName", DealOffInfo.sBuyChrName);
                            command.Parameters.AddWithValue("@SellDateTime", DealOffInfo.dSellDateTime);
                            command.Parameters.AddWithValue("@State", DealOffInfo.Flag);
                            command.Parameters.AddWithValue("@SellGold", DealOffInfo.nSellGold);
                            command.Parameters.AddWithValue("@UseItems", JsonSerializer.Serialize(DealOffInfo.UseItems));
                            command.ExecuteNonQuery();
                        }
                    }
                }
                logger.Info($"保存物品寄售列表成功...[{M2Share.SellOffItemList.Count}]");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Close();
            }
        }

        public static int LoadUpgradeWeaponRecord(string sNPCName, IList<WeaponUpgradeInfo> DataList)
        {
            //todo 加载武器升级数据
            return -1;
        }

        public static int SaveUpgradeWeaponRecord(string sNPCName, IList<WeaponUpgradeInfo> DataList)
        {
            //todo 保存武器升级数据
            return -1;
        }

        private IDataReader Query(string sSQLString)
        {
            var command = new MySqlCommand();
            command.Connection = (MySqlConnection)_dbConnection;
            command.CommandText = sSQLString;
            return command.ExecuteReader();
        }

        private int Execute(string sSQLString)
        {
            var command = new MySqlCommand();
            command.Connection = (MySqlConnection)_dbConnection;
            command.CommandText = sSQLString;
            return command.ExecuteNonQuery();
        }

        private bool Open()
        {
            if (_dbConnection == null)
            {
                try
                {
                    _dbConnection = new MySqlConnection(M2Share.Config.ConnctionString);
                    _dbConnection.Open();
                    return true;
                }
                catch (Exception e)
                {
                    logger.Error(M2Share.Config.ConnctionString);
                    logger.Error(e.StackTrace);
                    return false;
                }
            }
            else if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection = new MySqlConnection(M2Share.Config.ConnctionString);
                _dbConnection.Open();
            }
            return true;
        }

        private void Close()
        {
            if (_dbConnection != null)
            {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }
        }
    }
}