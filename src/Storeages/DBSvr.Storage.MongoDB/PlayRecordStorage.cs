using DBSrv.Storage.Model;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DBSrv.Storage.MongoDB
{
    public class PlayRecordStorage : IPlayRecordStorage
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private int _recordCount;
        private readonly Dictionary<string, int> _quickList;
        private readonly Dictionary<int, string> _indexQuickList;
        private readonly PlayQuickList _quickIdList;
        /// <summary>
        /// 已被删除的记录号
        /// </summary>
        private readonly IList<int> _deletedList;

        private readonly StorageOption _storageOption;

        public PlayRecordStorage(StorageOption option)
        {
            _quickList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            _indexQuickList = new Dictionary<int, string>();
            _quickIdList = new PlayQuickList();
            _deletedList = new List<int>();
            _recordCount = 0;
            _storageOption = option;
        }

        public void LoadQuickList()
        {
            _quickList.Clear();
            _quickIdList.Clear();
            _deletedList.Clear();
        }

        private void Close()
        {

        }

        public int Index(string sName)
        {
            if (_quickList.TryGetValue(sName, out int nIndex))
            {
                return nIndex;
            }
            return -1;
        }

        public PlayerRecordData Get(int nIndex, ref bool success)
        {
            return GetRecord(nIndex, ref success);
        }

        private PlayerRecordData GetRecord(int nIndex, ref bool success)
        {
            var humRecord = new PlayerRecordData();
            return humRecord;
        }

        public int FindByName(string sChrName, ArrayList ChrList)
        {
            for (var i = 0; i < _quickList.Count; i++)
            {
                //if (HUtil32.CompareLStr(m_QuickList[i], sChrName, sChrName.Length))
                //{
                //    ChrList.Add(m_QuickList[i], m_QuickList.Values[i]);
                //}
            }
            return ChrList.Count;
        }

        public PlayerRecordData GetBy(int nIndex, ref bool success)
        {
            if (nIndex > 0) return GetRecord(nIndex, ref success);
            success = false;
            return default;
        }

        public int FindByAccount(string sAccount, ref IList<PlayQuick> ChrList)
        {
            IList<PlayQuick> ChrNameList = null;
            _quickIdList.GetChrList(sAccount, ref ChrNameList);
            if (ChrNameList != null)
            {
                for (var i = 0; i < ChrNameList.Count; i++)
                {
                    ChrList.Add(ChrNameList[i]);
                }
            }
            return ChrList.Count;
        }

        public int ChrCountOfAccount(string sAccount)
        {
            var result = 0;
            IList<PlayQuick> ChrList = null;
            _quickIdList.GetChrList(sAccount, ref ChrList);
            var success = false;
            if (ChrList != null)
            {
                for (var i = 0; i < ChrList.Count; i++)
                {
                    PlayerRecordData HumDBRecord = GetBy(ChrList[i].Index, ref success);
                    if (success && !HumDBRecord.Deleted)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        public bool Add(PlayerRecordData HumRecord)
        {
            bool result = false;
            if (_quickList.ContainsKey(HumRecord.Header.Name))
            {
                if (_quickList[HumRecord.Header.Name] > 0)
                {
                    return false;
                }
            }
            else
            {
                int nIndex = 0;
                if (_deletedList.Count > 0)
                {
                    nIndex = _deletedList[0];
                    _deletedList.RemoveAt(0);
                }
                else
                {
                    nIndex = _recordCount;
                    _recordCount++;
                }
                if (UpdateRecord(HumRecord, true, ref nIndex))
                {
                    _quickList.Add(HumRecord.Header.Name, nIndex);
                    _quickIdList.AddRecord(HumRecord.sAccount, HumRecord.sChrName, nIndex, HumRecord.Header.SelectID);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        private bool UpdateRecord(PlayerRecordData HumRecord, bool boNew, ref int nIndex)
        {
            bool result = false;
            return result;
        }

        public bool Delete(string sName)
        {
            IList<PlayQuick> ChrNameList = null;
            var result = false;
            int n10 = _quickList[sName];
            if (n10 < 0)
            {
                return result;
            }
            PlayerRecordData HumRecord = Get(n10, ref result);
            //if (DeleteRecord(m_IndexQuickList[n10]))
            //{
            //    m_QuickList.Remove(n10);
            //    result = true;
            //}
            if (result)
            {
                var n14 = _quickIdList.GetChrList(HumRecord.sAccount, ref ChrNameList);
                if (n14 >= 0)
                {
                    _quickIdList.DelRecord(n14, HumRecord.sChrName);
                }
            }
            return result;
        }

        private bool DeleteRecord(int nIndex)
        {
            throw new NotImplementedException();
        }

        public bool Update(int nIndex, ref PlayerRecordData HumDBRecord)
        {
            var result = false;
            if (nIndex < 0)
            {
                return false;
            }
            if (_quickList.Count < nIndex)
            {
                return false;
            }
            if (UpdateRecord(HumDBRecord, false, ref nIndex))
            {
                result = true;
            }
            return result;
        }

        public void UpdateBy(int nIndex, ref PlayerRecordData HumDBRecord)
        {
            UpdateRecord(HumDBRecord, false, ref nIndex);
        }
    }
}