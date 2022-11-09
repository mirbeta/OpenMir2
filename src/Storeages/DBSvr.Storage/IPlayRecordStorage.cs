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

        PlayerRecordData Get(int nIndex, ref bool success);

        PlayerRecordData GetBy(int nIndex, ref bool success);

        int FindByAccount(string sAccount, ref IList<PlayQuick> ChrList);

        /// <summary>
        /// 获取账号下角色数量
        /// </summary>
        /// <returns></returns>
        int ChrCountOfAccount(string sAccount);

        bool Add(PlayerRecordData HumRecord);

        /// <summary>
        /// 删除记录索引值
        /// </summary>
        /// <returns></returns>
        bool Delete(string sName);

        bool Update(int nIndex, ref PlayerRecordData HumDBRecord);

        void UpdateBy(int nIndex, ref PlayerRecordData HumDBRecord);

        int FindByName(string sChrName, ArrayList ChrList);
    }
}