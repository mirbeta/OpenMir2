using DBSvr.Storage.Model;
using System.Collections.Specialized;
using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;

namespace DBSvr.Storage
{
    /// <summary>
    /// 玩家数据服务接口
    /// </summary>
    public interface IPlayDataService
    {
        void LoadQuickList();

        int Index(string sName);

        int Get(int nIndex, ref THumDataInfo HumanRCD);

        int GetQryChar(int nIndex, ref QueryChr QueryChrRcd);

        bool Update(int nIndex, ref THumDataInfo HumanRCD);

        bool UpdateQryChar(int nIndex, QueryChr QueryChrRcd);

        bool Add(ref THumDataInfo HumanRCD);

        int Find(string sChrName, StringDictionary List);

        bool Delete(int nIndex);

        bool Delete(string sChrName);

        int Count();
    }
}