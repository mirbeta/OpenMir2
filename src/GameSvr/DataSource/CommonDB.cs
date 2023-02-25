using GameSvr.GameCommand;
using GameSvr.Items;
using GameSvr.Npc;
using MySqlConnector;
using System.Data;
using System.Text.Json;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Extensions;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.DataSource {
    public class CommonDB {
        private IDbConnection _dbConnection;

        public int LoadItemsDB() {
            int result = -1;
            int Idx;
            StdItem Item;
            const string sSQLString = "SELECT * FROM stditems";
            try {
                HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
                for (int i = 0; i < M2Share.WorldEngine.StdItemList.Count; i++) {
                    M2Share.WorldEngine.StdItemList[i] = null;
                }
                M2Share.WorldEngine.StdItemList.Clear();
                result = -1;
                if (!Open()) {
                    return result;
                }
                using (IDataReader dr = Query(sSQLString)) {
                    while (dr.Read()) {
                        Item = new StdItem();
                        Idx = dr.GetInt32("ID");
                        Item.Name = dr.GetString("NAME");
                        Item.StdMode = dr.GetByte("StdMode");
                        Item.Shape = dr.GetByte("SHAPE");
                        Item.Weight = dr.GetByte("WEIGHT");
                        Item.AniCount = dr.GetByte("ANICOUNT");
                        Item.SpecialPwr = dr.GetSByte("SOURCE");
                        Item.ItemDesc = dr.GetByte("RESERVED");
                        Item.Looks = dr.GetUInt16("IMGINDEX");
                        Item.DuraMax = dr.GetUInt16("DURAMAX");
                        Item.AC = HUtil32.MakeWord(dr.GetUInt16("AC"), dr.GetUInt16("ACMAX"));
                        Item.MAC = HUtil32.MakeWord(dr.GetUInt16("MAC"), dr.GetUInt16("MACMAX"));
                        Item.DC = HUtil32.MakeWord(dr.GetUInt16("DC"), dr.GetUInt16("DCMAX"));
                        Item.MC = HUtil32.MakeWord(dr.GetUInt16("MC"), dr.GetUInt16("MCMAX"));
                        Item.SC = HUtil32.MakeWord(dr.GetUInt16("SC"), dr.GetUInt16("SCMAX"));
                        Item.Need = dr.GetByte("NEED");
                        Item.NeedLevel = dr.GetByte("NEEDLEVEL");
                        Item.NeedIdentify = 0;
                        Item.Price = dr.GetInt32("PRICE");
                        Item.Stock = dr.GetInt32("STOCK");
                        Item.AtkSpd = dr.GetByte("ATKSPD");
                        Item.Agility = dr.GetByte("AGILITY");
                        Item.Accurate = dr.GetByte("ACCURATE");
                        Item.MgAvoid = dr.GetByte("MGAVOID");
                        Item.Strong = dr.GetByte("STRONG");
                        Item.Undead = dr.GetByte("UNDEAD");
                        Item.HpAdd = dr.GetInt32("HPADD");
                        Item.MpAdd = dr.GetInt32("MPADD");
                        Item.ExpAdd = dr.GetInt32("EXPADD");
                        Item.EffType1 = dr.GetByte("EFFTYPE1");
                        Item.EffRate1 = dr.GetByte("EFFRATE1");
                        Item.EffValue1 = dr.GetByte("EFFVALUE1");
                        Item.EffType2 = dr.GetByte("EFFTYPE2");
                        Item.EffRate2 = dr.GetByte("EFFRATE2");
                        Item.EffValue2 = dr.GetByte("EFFVALUE2");
                        Item.Slowdown = dr.GetByte("SLOWDOWN");
                        Item.Tox = dr.GetByte("TOX");
                        Item.ToxAvoid = dr.GetByte("TOXAVOID");
                        Item.UniqueItem = dr.GetByte("UNIQUEITEM");
                        Item.OverlapItem = dr.GetByte("OVERLAPITEM");
                        Item.Light = dr.GetByte("LIGHT");
                        Item.ItemType = dr.GetByte("ITEMTYPE");
                        Item.ItemSet = dr.GetUInt16("ITEMSET");
                        Item.Reference = dr.GetString("REFERENCE");
                        if (M2Share.WorldEngine.StdItemList.Count <= Idx) {
                            M2Share.WorldEngine.StdItemList.Add(Item);
                            result = 1;
                        }
                        else {
                            M2Share.Logger.Error($"加载物品(Idx:{Idx} Name:{Item.Name})数据失败!!!");
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
            catch (Exception ex) {
                M2Share.Logger.Error(ex.StackTrace);
                return result;
            }
            finally {
                Close();
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
            return result;
        }

        public int LoadMagicDB() {
            MagicInfo Magic;
            const string sSQLString = "select * from magics";
            int result = -1;
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try {
                M2Share.WorldEngine.SwitchMagicList();
                if (!Open()) {
                    return result;
                }
                using IDataReader dr = Query(sSQLString);
                while (dr.Read()) {
                    Magic = new MagicInfo {
                        MagicId = dr.GetUInt16("MagId"),
                        MagicName = dr.GetString("MagName"),
                        EffectType = (byte)dr.GetInt32("EffectType"),
                        Effect = (byte)dr.GetInt32("Effect"),
                        Spell = dr.GetUInt16("Spell"),
                        Power = dr.GetUInt16("Power"),
                        MaxPower = dr.GetUInt16("MaxPower"),
                        Job = (byte)dr.GetInt32("Job")
                    };
                    Magic.TrainLevel[0] = (byte)dr.GetInt32("NeedL1");
                    Magic.TrainLevel[1] = (byte)dr.GetInt32("NeedL2");
                    Magic.TrainLevel[2] = (byte)dr.GetInt32("NeedL3");
                    Magic.TrainLevel[3] = (byte)dr.GetInt32("NeedL3");
                    Magic.MaxTrain[0] = dr.GetInt32("L1Train");
                    Magic.MaxTrain[1] = dr.GetInt32("L2Train");
                    Magic.MaxTrain[2] = dr.GetInt32("L3Train");
                    Magic.MaxTrain[3] = Magic.MaxTrain[2];
                    Magic.TrainLv = 3;
                    Magic.DelayTime = dr.GetInt32("Delay");
                    Magic.DefSpell = (byte)dr.GetInt32("DefSpell");
                    Magic.DefPower = (byte)dr.GetInt32("DefPower");
                    Magic.DefMaxPower = (byte)dr.GetInt32("DefMaxPower");
                    Magic.Desc = dr.GetString("Descr");
                    if (Magic.MagicId > 0) {
                        M2Share.WorldEngine.MagicList.Add(Magic);
                    }
                    else {
                        Magic = null;
                    }
                    result = 1;
                }
            }
            catch (Exception ex) {
                M2Share.Logger.Error(ex.StackTrace);
            }
            finally {
                Close();
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
            return result;
        }

        public int LoadMonsterDB() {
            int result = 0;
            MonsterInfo Monster;
            const string sSQLString = "select * from monsters";
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try {
                M2Share.WorldEngine.MonsterList.Clear();
                if (!Open()) {
                    return result;
                }
                using IDataReader dr = Query(sSQLString);
                while (dr.Read()) {
                    Monster = new MonsterInfo {
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
                    if (Monster.Race == ActorRace.SabukWall || Monster.Race == ActorRace.SabukDoor) {
                        // 如果为城墙或城门由HP不加倍
                        Monster.HP = dr.GetUInt16("HP");
                    }
                    else {
                        Monster.HP = (ushort)HUtil32.Round(dr.GetInt32("HP") * (M2Share.Config.MonsterPowerRate / 10));
                    }
                    Monster.MP = (ushort)HUtil32.Round(dr.GetInt32("MP") * (M2Share.Config.MonsterPowerRate / 10));
                    Monster.AC = (ushort)HUtil32.Round(dr.GetInt32("AC") * (M2Share.Config.MonsterPowerRate / 10));
                    Monster.MAC = (ushort)HUtil32.Round(dr.GetInt32("MAC") * (M2Share.Config.MonsterPowerRate / 10));
                    Monster.DC = (ushort)HUtil32.Round(dr.GetInt32("DC") * (M2Share.Config.MonsterPowerRate / 10));
                    Monster.MaxDC = (ushort)HUtil32.Round(dr.GetInt32("DCMAX") * (M2Share.Config.MonsterPowerRate / 10));
                    Monster.MC = (ushort)HUtil32.Round(dr.GetInt32("MC") * (M2Share.Config.MonsterPowerRate / 10));
                    Monster.SC = (ushort)HUtil32.Round(dr.GetInt32("SC") * (M2Share.Config.MonsterPowerRate / 10));
                    Monster.Speed = dr.GetByte("SPEED");
                    Monster.HitPoint = dr.GetByte("HIT");
                    Monster.WalkSpeed = (ushort)HUtil32._MAX(200, dr.GetInt32("WALK_SPD"));
                    Monster.WalkStep = (ushort)HUtil32._MAX(1, dr.GetInt32("WalkStep"));
                    Monster.WalkWait = (ushort)dr.GetInt32("WalkWait");
                    Monster.AttackSpeed = (ushort)dr.GetInt32("ATTACK_SPD");
                    if (Monster.WalkSpeed < 200) {
                        Monster.WalkSpeed = 200;
                    }
                    if (Monster.AttackSpeed < 200) {
                        Monster.AttackSpeed = 200;
                    }
                    Monster.ItemList = null;
                    M2Share.LocalDb.LoadMonitems(Monster.Name, ref Monster.ItemList);
                    if (M2Share.WorldEngine.MonsterList.ContainsKey(Monster.Name)) {
                        M2Share.Logger.Error($"怪物名称[{Monster.Name}]重复,请确认数据是否正常.");
                        continue;
                    }
                    M2Share.WorldEngine.MonsterList.Add(Monster.Name, Monster);
                    result = 1;
                }
            }
            finally {
                Close();
                HUtil32.LeaveCriticalSection(M2Share.ProcessHumanCriticalSection);
            }
            return result;
        }

        /// <summary>
        /// 加载寄售系统数据
        /// </summary>
        public void LoadSellOffItemList() {
            if (!Open()) {
                M2Share.Logger.Error("读取物品寄售列表失败.");
                return;
            }
            try {
                DealOffInfo DealOffInfo;
                const string sSQLString = "select * from goldsales";
                using IDataReader dr = Query(sSQLString);
                while (dr.Read()) {
                    string sDealChrName = dr.GetString("DealChrName");
                    string sBuyChrName = dr.GetString("BuyChrName");
                    DateTime dSellDateTime = dr.GetDateTime("SellDateTime");
                    byte nState = dr.GetByte("State");
                    short nSellGold = dr.GetInt16("SellGold");
                    string sUseItems = dr.GetString("UseItems");
                    if ((!string.IsNullOrEmpty(sDealChrName)) && (!string.IsNullOrEmpty(sBuyChrName)) && (nState < 4)) {
                        DealOffInfo = new DealOffInfo();
                        DealOffInfo.sDealChrName = sDealChrName;
                        DealOffInfo.sBuyChrName = sBuyChrName;
                        DealOffInfo.dSellDateTime = dSellDateTime;
                        DealOffInfo.nSellGold = nSellGold;
                        DealOffInfo.UseItems = JsonSerializer.Deserialize<UserItem[]>(sUseItems);
                        DealOffInfo.Flag = nState;
                        M2Share.SellOffItemList.Add(DealOffInfo);
                    }
                }
            }
            catch (Exception) {
                throw;
            }
            finally {
                Close();
            }
        }

        private const string SaveSellItemSql = "INSERT INTO goldsales (DealChrName, BuyChrName, SellDateTime, State, SellGold,UseItems) values (@DealChrName, @BuyChrName, @SellDateTime, @State, @SellGold,@UseItems))";

        /// <summary>
        /// 保存寄售系统数据
        /// </summary>
        public void SaveSellOffItemList() {
            if (!Open()) {
                M2Share.Logger.Error("保存物品寄售数据失败.");
                return;
            }
            DealOffInfo DealOffInfo;
            const string sSQLString = "delete from goldsales";
            try {
                if (M2Share.SellOffItemList.Count > 0) {
                    Execute(sSQLString);
                    for (int i = 0; i < M2Share.SellOffItemList.Count; i++) {
                        DealOffInfo = M2Share.SellOffItemList[i];
                        if (DealOffInfo != null) {
                            MySqlCommand command = new MySqlCommand();
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
            }
            catch (Exception) {
                throw;
            }
            finally {
                Close();
            }
        }

        public static int LoadUpgradeWeaponRecord(string sNPCName, IList<WeaponUpgradeInfo> DataList) {
            //todo 加载武器升级数据
            return -1;
        }

        public static int SaveUpgradeWeaponRecord(string sNPCName, IList<WeaponUpgradeInfo> DataList) {
            //todo 保存武器升级数据
            return -1;
        }

        private IDataReader Query(string sSQLString) {
            MySqlCommand command = new MySqlCommand();
            command.Connection = (MySqlConnection)_dbConnection;
            command.CommandText = sSQLString;
            return command.ExecuteReader();
        }

        private int Execute(string sSQLString) {
            MySqlCommand command = new MySqlCommand();
            command.Connection = (MySqlConnection)_dbConnection;
            command.CommandText = sSQLString;
            return command.ExecuteNonQuery();
        }

        private bool Open() {
            if (_dbConnection == null) {
                try {
                    _dbConnection = new MySqlConnection(M2Share.Config.ConnctionString);
                    _dbConnection.Open();
                    return true;
                }
                catch (Exception e) {
                    Console.WriteLine(M2Share.Config.ConnctionString);
                    M2Share.Logger.Error(e.StackTrace);
                    return false;
                }
            }
            else if (_dbConnection.State == ConnectionState.Closed) {
                _dbConnection = new MySqlConnection(M2Share.Config.ConnctionString);
                _dbConnection.Open();
            }
            return true;
        }

        private void Close() {
            if (_dbConnection != null) {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }
        }
    }
}