using DBSvr.Storage.Model;
using System.Collections;
using System.Collections.Generic;

namespace DBSvr.Storage
{
    /// <summary>
    /// 角色记录
    /// </summary>
    public interface IPlayRecordStorage
    {
        void LoadQuickList();

        int Index(string sName);

        HumRecordData Get(int nIndex, ref bool success);

        HumRecordData GetBy(int nIndex, ref bool success);

        int FindByAccount(string sAccount, ref IList<QuickId> ChrList);

        int ChrCountOfAccount(string sAccount);

        bool Add(HumRecordData HumRecord);

        bool Delete(string sName);

        bool Update(int nIndex, ref HumRecordData HumDBRecord);

        void UpdateBy(int nIndex, ref HumRecordData HumDBRecord);

        int FindByName(string sChrName, ArrayList ChrList);
    }
}