using GameSvr.Command;
using GameSvr.Items;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json;
using SystemModule;
using SystemModule.Data;
using SystemModule.Extensions;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.DataStores
{
    /// <summary>
    /// 数据库查询类
    /// </summary>
    public class CommonDB
    {
        private IDbConnection _dbConnection;

        public int LoadItemsDB()
        {
            int result = -1;
            int Idx;
            StdItem Item;
            const string sSQLString = "SELECT * FROM TBL_StdItems";
            try
            {
                HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
                for (var i = 0; i < M2Share.UserEngine.StdItemList.Count; i++)
                {
                    M2Share.UserEngine.StdItemList[i] = null;
                }
                M2Share.UserEngine.StdItemList.Clear();
                result = -1;
                if (!Open())
                {
                    return result;
                }
                using (var dr = Query(sSQLString))
                {
                    while (dr.Read())
                    {
                        Item = new StdItem();
                        Idx = dr.GetInt32("Idx");// 序号
                        Item.Name = dr.GetString("Name");// 名称
                        Item.StdMode = (byte)dr.GetInt32("StdMode");// 分类号
                        Item.Shape = (byte)dr.GetInt32("Shape");// 装备外观
                        Item.Weight = (byte)dr.GetInt32("Weight");// 重量
                        Item.AniCount = (byte)dr.GetInt32("AniCount");
                        Item.Source = dr.GetInt16("Source");
                        Item.Reserved = (byte)dr.GetInt32("Reserved");// 保留
                        Item.Looks = dr.GetUInt16("Looks");// 物品外观
                        Item.DuraMax = (ushort)dr.GetInt32("DuraMax");// 持久
                        Item.Ac = (ushort)HUtil32.Round(dr.GetInt32("AC") * (M2Share.Config.ItemsACPowerRate / 10));
                        Item.Ac2 = (ushort)HUtil32.Round(dr.GetInt32("AC2") * (M2Share.Config.ItemsACPowerRate / 10));
                        Item.Mac = (ushort)HUtil32.Round(dr.GetInt32("MAC") * (M2Share.Config.ItemsACPowerRate / 10));
                        Item.Mac2 = (ushort)HUtil32.Round(dr.GetInt32("MAC2") * (M2Share.Config.ItemsACPowerRate / 10));
                        Item.Dc = (ushort)HUtil32.Round(dr.GetInt32("DC") * (M2Share.Config.ItemsPowerRate / 10));
                        Item.Dc2 = (ushort)HUtil32.Round(dr.GetInt32("DC2") * (M2Share.Config.ItemsPowerRate / 10));
                        Item.Mc = (ushort)HUtil32.Round(dr.GetInt32("MC") * (M2Share.Config.ItemsPowerRate / 10));
                        Item.Mc2 = (ushort)HUtil32.Round(dr.GetInt32("MC2") * (M2Share.Config.ItemsPowerRate / 10));
                        Item.Sc = (ushort)HUtil32.Round(dr.GetInt32("SC") * (M2Share.Config.ItemsPowerRate / 10));
                        Item.Sc2 = (ushort)HUtil32.Round(dr.GetInt32("SC2") * (M2Share.Config.ItemsPowerRate / 10));
                        Item.Need = dr.GetByte("Need");// 附加条件
                        Item.NeedLevel = dr.GetByte("NeedLevel");// 需要等级
                        Item.Price = dr.GetInt32("Price");// 价格
                        Item.NeedIdentify = M2Share.GetGameLogItemNameList(Item.Name);
                        switch (Item.StdMode)
                        {
                            case 0:
                            case 55:
                            case 58: // 药品
                                Item.ItemType = GoodType.ITEM_LEECHDOM;
                                break;
                            case 5:
                            case 6: // 武器
                                Item.ItemType = GoodType.ITEM_WEAPON;
                                break;
                            case 10:
                            case 11: // 衣服
                                Item.ItemType = GoodType.ITEM_ARMOR;
                                break;
                            case 15:
                            case 19:
                            case 20:
                            case 21:
                            case 22:
                            case 23:
                            case 24:
                            case 26:
                            case 51:
                            case 52:
                            case 53:
                            case 54:
                            case 62:
                            case 63:
                            case 64:
                            case 30: // 辅助物品
                                Item.ItemType = GoodType.ITEM_ACCESSORY;
                                break;
                            default: // 其它物品
                                Item.ItemType = GoodType.ITEM_ETC;
                                break;
                        }
                        if (M2Share.UserEngine.StdItemList.Count <= Idx)
                        {
                            M2Share.UserEngine.StdItemList.Add(Item);
                            result = 1;
                        }
                        else
                        {
                            M2Share.Log.Error(string.Format("加载物品(Idx:{0} Name:{1})数据失败!!!", new object[] { Idx, Item.Name }));
                            result = -100;
                            return result;
                        }
                    }
                }
                M2Share.g_boGameLogGold = M2Share.GetGameLogItemNameList(Grobal2.sSTRING_GOLDNAME) == 1;
                M2Share.g_boGameLogHumanDie = M2Share.GetGameLogItemNameList(GameCommandConst.g_sHumanDieEvent) == 1;
                M2Share.g_boGameLogGameGold = M2Share.GetGameLogItemNameList(M2Share.Config.GameGoldName) == 1;
                M2Share.g_boGameLogGamePoint = M2Share.GetGameLogItemNameList(M2Share.Config.GamePointName) == 1;
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(ex.StackTrace);
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
            TMagic Magic;
            const string sSQLString = "select * from TBL_Magics";
            var result = -1;
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                M2Share.UserEngine.SwitchMagicList();
                if (!Open())
                {
                    return result;
                }
                using (var dr = Query(sSQLString))
                {
                    while (dr.Read())
                    {
                        Magic = new TMagic
                        {
                            wMagicID = dr.GetUInt16("MagId"),
                            sMagicName = dr.GetString("MagName"),
                            btEffectType = (byte)dr.GetInt32("EffectType"),
                            btEffect = (byte)dr.GetInt32("Effect"),
                            wSpell = dr.GetUInt16("Spell"),
                            wPower = dr.GetUInt16("Power"),
                            wMaxPower = dr.GetUInt16("MaxPower"),
                            btJob = (byte)dr.GetInt32("Job")
                        };
                        Magic.TrainLevel[0] = (byte)dr.GetInt32("NeedL1");
                        Magic.TrainLevel[1] = (byte)dr.GetInt32("NeedL2");
                        Magic.TrainLevel[2] = (byte)dr.GetInt32("NeedL3");
                        Magic.TrainLevel[3] = (byte)dr.GetInt32("NeedL3");
                        Magic.MaxTrain[0] = dr.GetInt32("L1Train");
                        Magic.MaxTrain[1] = dr.GetInt32("L2Train");
                        Magic.MaxTrain[2] = dr.GetInt32("L3Train");
                        Magic.MaxTrain[3] = Magic.MaxTrain[2];
                        Magic.btTrainLv = 3;
                        Magic.dwDelayTime = dr.GetInt32("Delay");
                        Magic.btDefSpell = (byte)dr.GetInt32("DefSpell");
                        Magic.btDefPower = (byte)dr.GetInt32("DefPower");
                        Magic.btDefMaxPower = (byte)dr.GetInt32("DefMaxPower");
                        Magic.sDescr = dr.GetString("Descr");
                        if (Magic.wMagicID > 0)
                        {
                            M2Share.UserEngine.MagicList.Add(Magic);
                        }
                        else
                        {
                            Magic = null;
                        }
                        result = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(ex.StackTrace);
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
            TMonInfo Monster;
            const string sSQLString = "select * from TBL_Monsters";
            HUtil32.EnterCriticalSection(M2Share.ProcessHumanCriticalSection);
            try
            {
                M2Share.UserEngine.MonsterList.Clear();
                if (!Open())
                {
                    return result;
                }
                using (var dr = Query(sSQLString))
                {
                    while (dr.Read())
                    {
                        Monster = new TMonInfo
                        {
                            ItemList = new List<TMonItem>(),
                            sName = dr.GetString("NAME").Trim(),
                            btRace = (byte)dr.GetInt32("Race"),
                            btRaceImg = (byte)dr.GetInt32("RaceImg"),
                            wAppr = dr.GetUInt16("Appr"),
                            wLevel = dr.GetUInt16("Lvl"),
                            btLifeAttrib = (byte)dr.GetInt32("Undead"),
                            wCoolEye = dr.GetInt16("CoolEye"),
                            dwExp = dr.GetInt32("Exp")
                        };
                        // 城门或城墙的状态跟HP值有关，如果HP异常，将导致城墙显示不了
                        if (Monster.btRace == 110 || Monster.btRace == 111)
                        {
                            // 如果为城墙或城门由HP不加倍
                            Monster.wHP = dr.GetUInt16("HP");
                        }
                        else
                        {
                            Monster.wHP = (ushort)HUtil32.Round(dr.GetInt32("HP") * (M2Share.Config.MonsterPowerRate / 10));
                        }
                        Monster.wMP = (ushort)HUtil32.Round(dr.GetInt32("MP") * (M2Share.Config.MonsterPowerRate / 10));
                        Monster.wAC = (ushort)HUtil32.Round(dr.GetInt32("AC") * (M2Share.Config.MonsterPowerRate / 10));
                        Monster.wMAC = (ushort)HUtil32.Round(dr.GetInt32("MAC") * (M2Share.Config.MonsterPowerRate / 10));
                        Monster.wDC = (ushort)HUtil32.Round(dr.GetInt32("DC") * (M2Share.Config.MonsterPowerRate / 10));
                        Monster.wMaxDC = (ushort)HUtil32.Round(dr.GetInt32("DCMAX") * (M2Share.Config.MonsterPowerRate / 10));
                        Monster.wMC = (ushort)HUtil32.Round(dr.GetInt32("MC") * (M2Share.Config.MonsterPowerRate / 10));
                        Monster.wSC = (ushort)HUtil32.Round(dr.GetInt32("SC") * (M2Share.Config.MonsterPowerRate / 10));
                        Monster.wSpeed = dr.GetUInt16("SPEED");
                        Monster.wHitPoint = dr.GetUInt16("HIT");
                        Monster.wWalkSpeed = (ushort)HUtil32._MAX(200, dr.GetInt32("WALK_SPD"));
                        Monster.wWalkStep = (ushort)HUtil32._MAX(1, dr.GetInt32("WalkStep"));
                        Monster.wWalkWait = (ushort)dr.GetInt32("WalkWait");
                        Monster.wAttackSpeed = (ushort)dr.GetInt32("ATTACK_SPD");
                        if (Monster.wWalkSpeed < 200)
                        {
                            Monster.wWalkSpeed = 200;
                        }
                        if (Monster.wAttackSpeed < 200)
                        {
                            Monster.wAttackSpeed = 200;
                        }
                        Monster.ItemList = null;
                        M2Share.LocalDb.LoadMonitems(Monster.sName, ref Monster.ItemList);
                        M2Share.UserEngine.MonsterList.Add(Monster);
                        result = 1;
                    }
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
                M2Share.Log.Error("读取物品寄售列表失败.");
                return;
            }
            try
            {
                TDealOffInfo DealOffInfo;
                const string sSQLString = "select * from TBL_GOLDSALES";
                using (var dr = Query(sSQLString))
                {
                    while (dr.Read())
                    {
                        var sDealCharName = dr.GetString("DealCharName");
                        var sBuyCharName = dr.GetString("BuyCharName");
                        var dSellDateTime = dr.GetDateTime("SellDateTime");
                        var nState = dr.GetByte("State");
                        var nSellGold = dr.GetInt16("SellGold");
                        var sUseItems = dr.GetString("UseItems");
                        if ((sDealCharName != "") && (sBuyCharName != "") && (nState < 4))
                        {
                            DealOffInfo = new TDealOffInfo();
                            DealOffInfo.sDealCharName = sDealCharName;
                            DealOffInfo.sBuyCharName = sBuyCharName;
                            DealOffInfo.dSellDateTime = dSellDateTime;
                            DealOffInfo.nSellGold = nSellGold;
                            DealOffInfo.UseItems = JsonSerializer.Deserialize<TUserItem[]>(sUseItems);
                            DealOffInfo.N = nState;
                            M2Share.sSellOffItemList.Add(DealOffInfo);
                        }
                    }
                }
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

        /// <summary>
        /// 保存寄售系统数据
        /// </summary>
        public void SaveSellOffItemList()
        {
            if (!Open())
            {
                M2Share.Log.Error("保存物品寄售数据失败.");
                return;
            }
            TDealOffInfo DealOffInfo;
            const string sSQLString = "delete from TBL_GOLDSALES";
            try
            {
                if (M2Share.sSellOffItemList.Count > 0)
                {
                    Execute(sSQLString);

                    for (var i = 0; i < M2Share.sSellOffItemList.Count; i++)
                    {
                        DealOffInfo = M2Share.sSellOffItemList[i];
                        if (DealOffInfo != null)
                        {
                            string InsertSql = "INSERT INTO sales (DealCharName, BuyCharName, SellDateTime, State, SellGold,UseItems) values " +
                                "(" + DealOffInfo.sDealCharName + "," + DealOffInfo.sBuyCharName + "," + DealOffInfo.dSellDateTime + "," + DealOffInfo.N + ","
                                + DealOffInfo.nSellGold + "," + JsonSerializer.Serialize(DealOffInfo.UseItems) + ")";
                            Execute(InsertSql);
                        }
                    }
                }
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    M2Share.Log.Error(e.StackTrace);
                    Console.ResetColor();
                    return false;
                }
            }
            else if (_dbConnection.State == ConnectionState.Closed)
            {
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