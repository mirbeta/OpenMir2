using System.Collections.Specialized;
using SystemModule;

namespace DBSvr
{
    public interface IPlayDataService
    {
        void LoadQuickList();

        int Index(string sName);

        int Get(int nIndex, ref THumDataInfo HumanRCD);

        int GetQryChar(int nIndex, ref TQueryChr QueryChrRcd);

        bool Update(int nIndex, ref THumDataInfo HumanRCD);

        bool UpdateQryChar(int nIndex, TQueryChr QueryChrRcd);

        bool Add(ref THumDataInfo HumanRCD);

        int Find(string sChrName, StringDictionary List);

        bool Delete(int nIndex);

        bool Delete(string sChrName);

        int Count();
    }
}