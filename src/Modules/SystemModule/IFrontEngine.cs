using SystemModule.Data;

namespace SystemModule
{
    public interface IFrontEngine
    {
        void AddChangeGoldList(string sGameMasterName, string sGetGoldUserName, int nGold);
        void AddToLoadRcdList(LoadDBInfo loadRcdInfo);
        void AddToSaveRcdList(SavePlayerRcd SaveRcd);
        void DeleteHuman(int nGateIndex, int nSocket);
        /// <summary>
        /// 是否在保存队列
        /// </summary>
        /// <param name="sChrName"></param>
        /// <returns></returns>
        bool InSaveRcdList(string sChrName);
        bool IsFull();
        bool IsIdle();
        void ProcessGameDate();
        void RemoveSaveList(int queryId);
        int SaveListCount();
    }
}
