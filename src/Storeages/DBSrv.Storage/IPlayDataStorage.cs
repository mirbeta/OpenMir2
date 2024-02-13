using DBSrv.Storage.Model;
using OpenMir2.Packets.ServerPackets;
using System.Collections.Specialized;

namespace DBSrv.Storage
{
    /// <summary>
    /// 角色数据存储接口
    /// </summary>
    public interface IPlayDataStorage
    {
        void LoadQuickList();

        int Index(string sName);

        int Get(int nIndex, ref CharacterDataInfo HumanRCD);

        bool Get(string chrName, ref CharacterDataInfo HumanRCD);

        CharacterData Query(int playerId);

        bool GetQryChar(int nIndex, ref QueryChr QueryChrRcd);

        bool Update(string chrName, CharacterDataInfo HumanRCD);

        bool UpdateQryChar(int nIndex, QueryChr QueryChrRcd);

        bool Add(CharacterDataInfo HumanRCD);

        int Find(string sChrName, StringDictionary List);

        bool Delete(int nIndex);

        bool Delete(string sChrName);

        int Count();
    }
}