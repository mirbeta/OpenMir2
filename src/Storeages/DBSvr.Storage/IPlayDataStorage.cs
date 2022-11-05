using DBSvr.Storage.Model;
using System.Collections.Specialized;
using SystemModule.Packet.ServerPackets;

namespace DBSvr.Storage
{
    /// <summary>
    /// 角色数据
    /// </summary>
    public interface IPlayDataStorage
    {
        void LoadQuickList();

        int Index(string sName);

        int Get(int nIndex, ref HumDataInfo HumanRCD);
        
        bool Get(string chrName, ref HumDataInfo HumanRCD);

        HumInfoData Query(int playerId);
        
        bool GetQryChar(int nIndex, ref QueryChr QueryChrRcd);

        bool Update(string chrName, HumDataInfo HumanRCD);

        bool UpdateQryChar(int nIndex, QueryChr QueryChrRcd);

        bool Add(HumDataInfo HumanRCD);

        int Find(string sChrName, StringDictionary List);

        bool Delete(int nIndex);

        bool Delete(string sChrName);

        int Count();
    }
}