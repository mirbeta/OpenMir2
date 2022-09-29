using DBSvr.Storage.Model;
using System.Collections.Specialized;
using SystemModule.Packet.ServerPackets;

namespace DBSvr.Storage
{
    /// <summary>
    /// 玩家数据服务接口
    /// </summary>
    public interface IPlayDataStorage
    {
        void LoadQuickList();

        int Index(string sName);

        int Get(int nIndex, ref HumDataInfo HumanRCD);
        
        bool Get(string chrName, ref HumDataInfo HumanRCD);
        
        int GetQryChar(int nIndex, ref QueryChr QueryChrRcd);

        bool Update(string chrName, ref HumDataInfo HumanRCD);

        bool UpdateQryChar(int nIndex, QueryChr QueryChrRcd);

        bool Add(ref HumDataInfo HumanRCD);

        int Find(string sChrName, StringDictionary List);

        bool Delete(int nIndex);

        bool Delete(string sChrName);

        int Count();
    }
}