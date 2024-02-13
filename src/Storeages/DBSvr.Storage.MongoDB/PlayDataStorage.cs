using DBSrv.Storage.Impl;
using DBSrv.Storage.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using OpenMir2;
using OpenMir2.Packets.ServerPackets;

namespace DBSrv.Storage.MongoDB
{
    public class PlayDataStorage : IPlayDataStorage
    {
        private readonly Dictionary<string, int> _mirQuickMap;
        private readonly Dictionary<int, int> _quickIndexIdMap;
        private readonly PlayQuickList _mirQuickIdList;
        private readonly StorageOption _storageOption;
        private IMongoCollection<CharacterDataInfo> humDataInfo;
        private int _recordCount;

        public PlayDataStorage(StorageOption storageOption)
        {
            _mirQuickMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _mirQuickIdList = new PlayQuickList();
            _recordCount = -1;
            _quickIndexIdMap = new Dictionary<int, int>();
            _storageOption = storageOption;
        }

        private void ConnectMongoDB()
        {
            if (humDataInfo != null)
            {
                return;
            }
            if (!_storageOption.ConnectionString.StartsWith("mongodb://"))
            {
                throw new Exception("错误的MongoDB链接字符串,请检查链接字符串.");
            }
            MongoClient client = new MongoClient(_storageOption.ConnectionString);
            IMongoDatabase db = client.GetDatabase("mir2");
            humDataInfo = db.GetCollection<CharacterDataInfo>("PlayObject");
        }

        public void LoadQuickList()
        {
            IList<PlayQuick> AccountList;
            IList<string> ChrNameList;
            _mirQuickMap.Clear();
            _mirQuickIdList.Clear();
            _recordCount = -1;
            AccountList = new List<PlayQuick>();
            ChrNameList = new List<string>();
            bool success = false;
            MongoClient dbConnection = Open(ref success);
            if (!success)
            {
                return;
            }
            try
            {
                humDataInfo.CountDocuments("{}");
                var users = humDataInfo.AsQueryable().ToList();
            }
            finally
            {
                //Close(dbConnection);
            }
            for (var nIndex = 0; nIndex < AccountList.Count; nIndex++)
            {
                _mirQuickIdList.AddRecord(AccountList[nIndex].Account, ChrNameList[nIndex], 0, AccountList[nIndex].SelectID);
            }
            AccountList = null;
            ChrNameList = null;
            //m_MirQuickList.SortString(0, m_MirQuickList.Count - 1);
        }

        private MongoClient Open(ref bool success)
        {
            var dbConnection = new MongoClient(_storageOption.ConnectionString);
            try
            {
                dbConnection.GetDatabase("mir2");
                success = true;
            }
            catch (Exception e)
            {
                LogService.Error("打开数据库[MySql]失败.");
                LogService.Error(e.StackTrace);
                success = false;
            }
            return dbConnection;
        }

        private void Close()
        {

        }

        public int Index(string sName)
        {
            if (_mirQuickMap.ContainsKey(sName))
            {
                return _mirQuickMap[sName];
            }
            return -1;
        }

        public int Get(int nIndex, ref CharacterDataInfo HumanRCD)
        {
            int result = -1;
            if (nIndex < 0)
            {
                return result;
            }
            if (_mirQuickMap.Count < nIndex)
            {
                return result;
            }
            if (GetRecord(nIndex, ref HumanRCD))
            {
                result = nIndex;
            }
            return result;
        }

        public bool Get(string sName, ref CharacterDataInfo HumanRCD)
        {
            throw new NotImplementedException();
        }

        public CharacterData Query(int playerId)
        {
            throw new NotImplementedException();
        }

        public bool Update(string nIndex, CharacterDataInfo HumanRCD)
        {
            bool result = false;
            //if ((nIndex >= 0) && (_mirQuickMap.Count >= nIndex))
            //{
            //    if (UpdateRecord(nIndex, ref HumanRCD))
            //    {
            //        result = true;
            //    }
            //}
            return result;
        }

        public bool UpdateQryChar(int nIndex, QueryChr QueryChrRcd)
        {
            bool result = false;
            if ((nIndex >= 0) && (_mirQuickMap.Count > nIndex))
            {
                if (UpdateChrRecord(nIndex, QueryChrRcd))
                {
                    result = true;
                }
            }
            return result;
        }

        private bool UpdateChrRecord(int playerId, QueryChr QueryChrRcd)
        {
            bool result = false;
            return result;
        }

        public bool Add(CharacterDataInfo HumanRCD)
        {
            bool result = false;
            int nIndex;
            string sChrName = HumanRCD.Header.Name;
            if (_mirQuickMap.TryGetValue(sChrName, out nIndex))
            {
                if (nIndex >= 0)
                {
                    return false;
                }
            }
            else
            {
                nIndex = _recordCount;
                _recordCount++;
                if (AddRecord(ref nIndex, ref HumanRCD))
                {
                    _mirQuickMap.Add(sChrName, nIndex);
                    _quickIndexIdMap.Add(nIndex, nIndex);
                    result = true;
                }
            }
            return result;
        }

        private bool GetRecord(int nIndex, ref CharacterDataInfo HumanRCD)
        {
            int playerId = 0;
            if (HumanRCD == null)
            {
                playerId = _quickIndexIdMap[nIndex];
            }
            if (playerId == 0)
            {
                return false;
            }
            GetChrRecord(playerId, ref HumanRCD);
            GetAbilGetRecord(playerId, ref HumanRCD);
            GetBonusAbilRecord(playerId, ref HumanRCD);
            GetMagicRecord(playerId, ref HumanRCD);
            GetItemRecord(playerId, ref HumanRCD);
            GetStorageRecord(playerId, ref HumanRCD);
            GetPlayerStatus(playerId, ref HumanRCD);
            return true;
        }

        private void GetChrRecord(int playerId, ref CharacterDataInfo HumanRCD)
        {
        }

        private void GetAbilGetRecord(int playerId, ref CharacterDataInfo HumanRCD)
        {
        }

        private void GetBonusAbilRecord(int playerId, ref CharacterDataInfo HumanRCD)
        {
        }

        private void GetMagicRecord(int playerId, ref CharacterDataInfo HumanRCD)
        {
        }

        private void GetItemRecord(int playerId, ref CharacterDataInfo HumanRCD)
        {
        }

        private void GetStorageRecord(int playerId, ref CharacterDataInfo HumanRCD)
        {
        }

        private void GetPlayerStatus(int playerId, ref CharacterDataInfo HumanRCD)
        {
        }

        private bool AddRecord(ref int nIndex, ref CharacterDataInfo HumanRCD)
        {
            return InsertRecord(HumanRCD.Data, ref nIndex);
        }

        private bool InsertRecord(CharacterData hd, ref int nIndex)
        {
            return true;
        }

        private bool UpdateRecord(int nIndex, ref CharacterDataInfo HumanRCD)
        {
            bool result = true;
            return result;
        }

        private void UpdateRecord(int Id, CharacterDataInfo HumanRCD)
        {

        }

        private void UpdateAblity(int playerId, CharacterDataInfo HumanRCD)
        {
        }

        private void UpdateItem(int playerId, CharacterDataInfo HumanRCD)
        {
        }

        private void SaveItemStorge(int playerId, CharacterDataInfo HumanRCD)
        {
        }

        private void SavePlayerMagic(int playerId, CharacterDataInfo HumanRCD)
        {
        }

        private void UpdateBonusability(int playerId, CharacterDataInfo HumanRCD)
        {
        }

        private void UpdateQuest(int Id, CharacterDataInfo HumanRCD)
        {
        }

        private void UpdateStatus(int playerId, CharacterDataInfo HumanRCD)
        {
        }

        public int Find(string sChrName, StringDictionary List)
        {
            int result;
            for (var i = 0; i < _mirQuickMap.Count; i++)
            {
                //if (HUtil32.CompareLStr(m_MirQuickList[i], sChrName, sChrName.Length))
                //{
                //    List.Add(m_MirQuickList[i], m_MirQuickList.Values[i]);
                //}
            }
            result = List.Count;
            return result;
        }

        public bool Delete(int nIndex)
        {
            for (var i = 0; i < _mirQuickMap.Count; i++)
            {
                //if (((int)m_MirQuickList.Values[i]) == nIndex)
                //{
                //    s14 = m_MirQuickList[i];
                //    if (DeleteRecord(nIndex))
                //    {
                //        m_MirQuickList.Remove(i);
                //        result = true;
                //        break;
                //    }
                //}
            }
            return false;
        }

        private bool DeleteRecord(int nIndex)
        {
            bool result = true;
            int PlayerId = _quickIndexIdMap[nIndex];
            return result;
        }

        public int Count()
        {
            return _mirQuickMap.Count;
        }

        public bool Delete(string sChrName)
        {
            //int nIndex = m_MirQuickList.GetIndex(sChrName);
            //if (nIndex < 0)
            //{
            //    return result;
            //}
            //if (DeleteRecord(nIndex))
            //{
            //    m_MirQuickList.Remove(nIndex);
            //    result = true;
            //}
            return false;
        }

        public bool GetQryChar(int nIndex, ref QueryChr QueryChrRcd)
        {
            return false;
        }
    }
}