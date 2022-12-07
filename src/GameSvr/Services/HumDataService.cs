using NLog;
using System.Collections.Concurrent;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packets.ServerPackets;

namespace GameSvr.Services
{
    public class QueryPlayData
    {
        public int QueryId;
        public int QuetyCount;
    }

    public static class PlayerDataService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly ConcurrentDictionary<int, ServerRequestData> ReceivedMap = new ConcurrentDictionary<int, ServerRequestData>();
        private static readonly Queue<QueryPlayData> queryProcessList = new Queue<QueryPlayData>();
        private static readonly Queue<int> saveProcessList = new Queue<int>();
        private static readonly ConcurrentDictionary<int, LoadPlayerDataPacket> loadPlayDataMap = new ConcurrentDictionary<int, LoadPlayerDataPacket>();

        public static bool SocketConnected()
        {
            return true;
        }

        public static void Enqueue(int queryId, ServerRequestData data)
        {
            ReceivedMap.TryAdd(queryId, data);
        }

        private static bool GetDBSockMsg(int queryId, ref int nIdent, ref int nRecog, ref byte[] data, int dwTimeOut, bool boLoadRcd)
        {
            var result = false;
            HUtil32.EnterCriticalSection(M2Share.UserDBSection);
            try
            {
                if (ReceivedMap.ContainsKey(queryId))
                {
                    ServerRequestData respPack;
                    if (ReceivedMap.TryGetValue(queryId, out respPack))
                    {
                        if (respPack == null)
                        {
                            return false;
                        }
                        var serverPacket = ServerPackSerializer.Deserialize<ServerRequestMessage>(EDCode.DecodeBuff(respPack.Message));
                        if (serverPacket == null)
                        {
                            return false;
                        }
                        nIdent = serverPacket.Ident;
                        nRecog = serverPacket.Recog;
                        data = respPack.Packet;
                        result = true;
                    }
                    ReceivedMap.TryRemove(queryId, out var delData);
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.UserDBSection);
            }
            return result;
        }

        public static bool GetPlayData(int queryId, ref PlayerDataInfo playerData)
        {
            if (!loadPlayDataMap.ContainsKey(queryId))
                return false;
            playerData = loadPlayDataMap[queryId].HumDataInfo;
            return true;
        }

        public static bool LoadHumRcdFromDB(string sAccount, string sChrName, string sStr, ref int queryId, int nCertCode)
        {
            var result = false;
            var loadHum = new LoadPlayerDataMessage()
            {
                Account = sAccount,
                ChrName = sChrName,
                UserAddr = sStr,
                SessionID = nCertCode
            };
            if (LoadRcd(loadHum, ref queryId))
            {
                result = true;
                /*HumanRcd.Data.ChrName = sChrName;
                HumanRcd.Data.Account = sAccount;
                if (HumanRcd.Data.ChrName == sChrName && (string.IsNullOrEmpty(HumanRcd.Data.Account) || HumanRcd.Data.Account == sAccount))
                {
                    result = true;
                }*/
            }
            M2Share.Config.nLoadDBCount++;
            return result;
        }

        /// <summary>
        /// 保存玩家数据到DB
        /// </summary>
        /// <returns></returns>
        public static bool SaveHumRcdToDB(SavePlayerRcd saveRcd, ref int queryId)
        {
            M2Share.Config.nSaveDBCount++;
            return SaveRcd(saveRcd, ref queryId);
        }

        private static bool SaveRcd(SavePlayerRcd saveRcd, ref int queryId)
        {
            queryId = GetQueryId();
            var packet = new ServerRequestMessage(Grobal2.DB_SAVEHUMANRCD, saveRcd.SessionID, 0, 0, 0);
            var saveHumData = new SavePlayerDataMessage(saveRcd.Account, saveRcd.ChrName, saveRcd.HumanRcd);
            if (M2Share.DataServer.SendRequest(queryId, packet, saveHumData))
            {
                saveProcessList.Enqueue(queryId);
                return true;
            }
            Logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            return false;
        }

        public static void ProcessSaveList()
        {
            var tempList = new List<int>();
            while (saveProcessList.Count > 0)
            {
                var queryId = saveProcessList.Dequeue();
                var nIdent = 0;
                var nRecog = 0;
                byte[] data = null;
                if (GetDBSockMsg(queryId, ref nIdent, ref nRecog, ref data, 5000, false))
                {
                    if (nIdent == Grobal2.DBR_SAVEHUMANRCD && nRecog == 1)
                    {
                        M2Share.FrontEngine.RemoveSaveList(queryId);
                    }
                }
                else
                {
                    tempList.Add(queryId);
                }
            }
            if (tempList.Count > 0)
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    saveProcessList.Enqueue(tempList[i]);
                }
            }
        }

        public static void ProcessQueryList()
        {
            var tempList = new List<QueryPlayData>();
            while (queryProcessList.Count > 0)
            {
                var queryData = queryProcessList.Dequeue();
                if (queryData.QuetyCount >= 50)
                {
                    continue;
                }
                var nIdent = 0;
                var nRecog = 0;
                byte[] data = null;
                if (GetDBSockMsg(queryData.QueryId, ref nIdent, ref nRecog, ref data, 5000, true))
                {
                    if (nIdent == Grobal2.DBR_LOADHUMANRCD && nRecog == 1)
                    {
                        var humRespData = EDCode.DecodeBuff(data);
                        var responsePacket = ServerPackSerializer.Deserialize<LoadPlayerDataPacket>(humRespData);
                        responsePacket.ChrName = EDCode.DeCodeString(responsePacket.ChrName);
                        loadPlayDataMap.TryAdd(queryData.QueryId, responsePacket);
                    }
                }
                else
                {
                    queryData.QuetyCount++;
                    tempList.Add(queryData);
                }
            }
            if (tempList.Count > 0)
            {
                for (int i = 0; i < tempList.Count; i++)
                {
                    queryProcessList.Enqueue(tempList[i]);
                }
            }
        }

        private static bool LoadRcd(LoadPlayerDataMessage loadHuman, ref int queryId)
        {
            var nQueryId = GetQueryId();
            var packet = new ServerRequestMessage(Grobal2.DB_LOADHUMANRCD, 0, 0, 0, 0);
            if (M2Share.DataServer.SendRequest(nQueryId, packet, loadHuman))
            {
                queryProcessList.Enqueue(new QueryPlayData()
                {
                    QueryId = nQueryId
                });
                queryId = nQueryId;
                Logger.Info($"查询[{queryId}]");
                return true;
            }
            Logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            return false;
        }

        private static int GetQueryId()
        {
            M2Share.Config.nDBQueryID++;
            if (M2Share.Config.nDBQueryID > int.MaxValue - 1)
            {
                M2Share.Config.nDBQueryID = 1;
            }
            return M2Share.Config.nDBQueryID;
        }
    }
}