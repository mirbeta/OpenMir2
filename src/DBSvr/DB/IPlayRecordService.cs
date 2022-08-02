using System.Collections;
using System.Collections.Generic;
using SystemModule;

namespace DBSvr
{
    public interface IPlayRecordService
    {
        void LoadQuickList();

        int Index(string sName);

        HumRecordData Get(int nIndex, ref bool success);

        HumRecordData GetBy(int nIndex, ref bool success);

        int FindByAccount(string sAccount, ref IList<TQuickID> ChrList);

        int ChrCountOfAccount(string sAccount);

        bool Add(HumRecordData HumRecord);

        bool Delete(string sName);

        bool Update(int nIndex, ref HumRecordData HumDBRecord);

        void UpdateBy(int nIndex, ref HumRecordData HumDBRecord);

        int FindByName(string sChrName, ArrayList ChrList);
    }
}