using System;
using System.Collections;
using System.Collections.Generic;
using SystemModule;

namespace DBSvr
{
    public class LocalDB
    {
        public static ArrayList StdItemList = null;
        public static IList<TMagic> m_MagicList = null;

        public static string magkeytostr(int Key)
        {
            string result = string.Empty;
            if (Key == 0)
            {
                result = "空";
                return result;
            }
            if (Key >= 49 && Key <= 56)
            {
                result = "F" + (Key - 48).ToString();
            }
            return result;
        }

        public static string GetStdItemName(int nItemIdx)
        {
            string result;
            result = "";
            nItemIdx -= 1;
            if ((nItemIdx >= 0) && (StdItemList.Count > nItemIdx))
            {
                result = ((TStdItem)(StdItemList[nItemIdx])).Name;
                if (result == "")
                {
                    result = "空";
                }
            }
            return result;
        }

        public static string FindMagicName(int nMagIdx)
        {
            string result = "";
            TMagic Magic;
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                Magic = m_MagicList[i];
                if (Magic.wMagicID == nMagIdx)
                {
                    result = Magic.sMagicName;
                    break;
                }
            }
            return result;
        }

        public static int LoadItemsDB()
        {
            int result = 0;
            int i;
            int Idx;
            TStdItem StdItem;
            const string sSQLString = "select * from StdItems";
            try
            {
                //for (i = 0; i < StdItemList.Count; i++)
                //{
                //    Dispose(((TStdItem)(StdItemList[i])));
                //}
                //StdItemList.Clear();
                //result = -1;
                //Query.SQL.Clear;
                //Query.SQL.Add(sSQLString);
                //try
                //{
                //    Query.Open;
                //}
                //finally
                //{
                //    result = -2;
                //}
                //for (i = 0; i < Query.RecordCount; i++)
                //{
                //    StdItem = new TStdItem();
                //    Idx = Query.FieldByName("Idx").AsInteger;
                //    StdItem.Name = Query.FieldByName("Name").AsString;
                //    StdItem.StdMode = Query.FieldByName("StdMode").AsInteger;
                //    StdItem.Shape = Query.FieldByName("Shape").AsInteger;
                //    StdItem.Weight = Query.FieldByName("Weight").AsInteger;
                //    StdItem.AniCount = Query.FieldByName("AniCount").AsInteger;
                //    StdItem.Source = Query.FieldByName("Source").AsInteger;
                //    StdItem.reserved = Query.FieldByName("Reserved").AsInteger;
                //    StdItem.Looks = Query.FieldByName("Looks").AsInteger;
                //    StdItem.DuraMax = ((short)Query.FieldByName("DuraMax").AsInteger);
                //    StdItem.AC = MakeLong(Math.Round(Convert.ToDouble(Query.FieldByName("Ac").AsInteger)), Math.Round(Convert.ToDouble(Query.FieldByName("Ac2").AsInteger)));
                //    StdItem.MAC = MakeLong(Math.Round(Convert.ToDouble(Query.FieldByName("Mac").AsInteger)), Math.Round(Convert.ToDouble(Query.FieldByName("MAc2").AsInteger)));
                //    StdItem.DC = MakeLong(Math.Round(Convert.ToDouble(Query.FieldByName("Dc").AsInteger)), Math.Round(Convert.ToDouble(Query.FieldByName("Dc2").AsInteger)));
                //    StdItem.MC = MakeLong(Math.Round(Convert.ToDouble(Query.FieldByName("Mc").AsInteger)), Math.Round(Convert.ToDouble(Query.FieldByName("Mc2").AsInteger)));
                //    StdItem.SC = MakeLong(Math.Round(Convert.ToDouble(Query.FieldByName("Sc").AsInteger)), Math.Round(Convert.ToDouble(Query.FieldByName("Sc2").AsInteger)));
                //    StdItem.Need = Query.FieldByName("Need").AsInteger;
                //    StdItem.NeedLevel = Query.FieldByName("NeedLevel").AsInteger;
                //    StdItem.Price = Query.FieldByName("Price").AsInteger;
                //    StdItem.NeedIdentify = 1;
                //    // GetGameLogItemNameList(StdItem.Name);
                //    if (StdItemList.Count == Idx)
                //    {
                //        StdItemList.Add(StdItem);
                //        result = 1;
                //    }
                //    else
                //    {
                //        DBShare.OutMainMessage(Format("加载物品(Idx:%d Name:%s)数据失败！！！", new string[] { Idx, StdItem.Name }));
                //        result = -100;
                //        return result;
                //    }
                //    Query.Next;
                //}
            }
            finally
            {
                //Query.Close;
            }
            return result;
        }

        public static int LoadMagicDB()
        {
            int result;
            int i;
            TMagic Magic;
            const string sSQLString = "select * from Magic";
            result = -1;
            //for (i = 0; i < m_MagicList.Count; i++)
            //{
            //                    Dispose(((TMagic)(m_MagicList[i])));
            //}
            //m_MagicList.Clear();
            //Query.SQL.Clear;
            //Query.SQL.Add(sSQLString);
            //try
            //{
            //    Query.Open;
            //}
            //finally
            //{
            //    result = -2;
            //}
            //for (i = 0; i < Query.RecordCount; i++)
            //{
            //    Magic = new TMagic();
            //    Magic.wMagicID = Query.FieldByName("MagId").AsInteger;
            //    Magic.sMagicName = Query.FieldByName("MagName").AsString;
            //    Magic.btEffectType = Query.FieldByName("EffectType").AsInteger;
            //    Magic.btEffect = Query.FieldByName("Effect").AsInteger;
            //    Magic.wSpell = Query.FieldByName("Spell").AsInteger;
            //    Magic.wPower = Query.FieldByName("Power").AsInteger;
            //    Magic.wMaxPower = Query.FieldByName("MaxPower").AsInteger;
            //    Magic.btJob = Query.FieldByName("Job").AsInteger;
            //    Magic.TrainLevel[0] = Query.FieldByName("NeedL1").AsInteger;
            //    Magic.TrainLevel[1] = Query.FieldByName("NeedL2").AsInteger;
            //    Magic.TrainLevel[2] = Query.FieldByName("NeedL3").AsInteger;
            //    Magic.TrainLevel[3] = Query.FieldByName("NeedL3").AsInteger;
            //    Magic.MaxTrain[0] = Query.FieldByName("L1Train").AsInteger;
            //    Magic.MaxTrain[1] = Query.FieldByName("L2Train").AsInteger;
            //    Magic.MaxTrain[2] = Query.FieldByName("L3Train").AsInteger;
            //    Magic.MaxTrain[3] = Magic.MaxTrain[2];
            //    Magic.btTrainLv = 3;
            //    Magic.dwDelayTime = Query.FieldByName("Delay").AsInteger;
            //    Magic.btDefSpell = Query.FieldByName("DefSpell").AsInteger;
            //    Magic.btDefPower = Query.FieldByName("DefPower").AsInteger;
            //    Magic.btDefMaxPower = Query.FieldByName("DefMaxPower").AsInteger;
            //    Magic.sDescr = Query.FieldByName("Descr").AsString;
            //    if (Magic.wMagicID > 0)
            //    {
            //        m_MagicList.Add(Magic);
            //    }
            //    else
            //    {
            //                            Dispose(Magic);
            //    }
            //    result = 1;
            //    Query.Next;
            //}
            //Query.Close;
            return result;
        }

        public static void freelist()
        {
            //int i;
            //for (i = 0; i < StdItemList.Count; i++)
            //{
            //    Dispose(((TStdItem)(StdItemList[i])));
            //}
            //StdItemList.Clear();
            //for (i = 0; i < m_MagicList.Count; i++)
            //{
            //    Dispose(((TMagic)(m_MagicList[i])));
            //}
            //m_MagicList.Clear();
        }

        public void initialization()
        {
            //CoInitialize(null);
            //Query = new object(null);
            //Query.DatabaseName = DBShare.sDBName;
            //StdItemList = new object();
            //m_MagicList = new object();
        }

        public void finalization()
        {
            //Query.Free;
            //CoUnInitialize;
            //freelist();
        }

    }
}