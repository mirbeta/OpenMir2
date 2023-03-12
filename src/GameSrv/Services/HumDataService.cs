using System.Collections.Concurrent;
using NLog;
using SystemModule.Data;
using SystemModule.Packets.ServerPackets;

namespace GameSrv.Services {
    public class QueryPlayData {
        public int QueryId;
        public int QuetyCount;
    }

    public static class PlayerDataService {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly ConcurrentDictionary<int, ServerRequestData> ReceivedMap = new ConcurrentDictionary<int, ServerRequestData>();
        private static readonly Queue<QueryPlayData> QueryProcessList = new Queue<QueryPlayData>();
        private static readonly Queue<int> SaveProcessList = new Queue<int>();
        private static readonly ConcurrentDictionary<int, LoadPlayerDataPacket> LoadPlayDataMap = new ConcurrentDictionary<int, LoadPlayerDataPacket>();

        public static bool SocketConnected() {
            return true;
        }

        public static void Enqueue(int queryId, ServerRequestData data) {
            ReceivedMap.TryAdd(queryId, data);
        }

        private static bool GetDbSrvMessage(int queryId, ref int nIdent, ref int nRecog, ref byte[] data) {
            bool result = false;
            HUtil32.EnterCriticalSection(M2Share.UserDBCriticalSection);
            try {
                if (ReceivedMap.TryGetValue(queryId, out var respPack)) {
                    if (respPack == null) {
                        return false;
                    }
                    var serverPacket = SerializerUtil.Deserialize<ServerRequestMessage>(EDCode.DecodeBuff(respPack.Message));
                    if (serverPacket == null) {
                        return false;
                    }
                    nIdent = serverPacket.Ident;
                    nRecog = serverPacket.Recog;
                    data = respPack.Packet;
                    result = true;
                }
                ReceivedMap.TryRemove(queryId, out _);
            }
            finally {
                HUtil32.LeaveCriticalSection(M2Share.UserDBCriticalSection);
            }
            return result;
        }

        public static bool GetPlayData(int queryId, ref PlayerDataInfo playerData)
        {
            if (!LoadPlayDataMap.TryGetValue(queryId, out var loadPlayDataPacket))
                return false;
            playerData = loadPlayDataPacket.HumDataInfo;
            return true;
        }

        public static bool LoadHumRcdFromDB(string sAccount, string sChrName, string sStr, ref int queryId, int nCertCode) {
            bool result = false;
            var loadHum = new LoadPlayerDataMessage() {
                Account = sAccount,
                ChrName = sChrName,
                UserAddr = sStr,
                SessionID = nCertCode
            };
            if (LoadRcd(loadHum, ref queryId)) {
                result = true;
            }
            M2Share.Config.nLoadDBCount++;
            return result;
        }

        /// <summary>
        /// 保存玩家数据到DB
        /// </summary>
        /// <returns></returns>
        public static bool SaveHumRcdToDB(SavePlayerRcd saveRcd, ref int queryId) {
            M2Share.Config.nSaveDBCount++;
            return SaveRcd(saveRcd, ref queryId);
        }

        private static bool SaveRcd(SavePlayerRcd saveRcd, ref int queryId) {
            queryId = GetQueryId();
            ServerRequestMessage packet = new ServerRequestMessage(Messages.DB_SAVEHUMANRCD, saveRcd.SessionID, 0, 0, 0);
            SavePlayerDataMessage saveHumData = new SavePlayerDataMessage(saveRcd.Account, saveRcd.ChrName, saveRcd.HumanRcd);
            if (M2Share.DataServer.SendRequest(queryId, packet, saveHumData)) {
                SaveProcessList.Enqueue(queryId);
                return true;
            }
            Logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            return false;
        }

        public static void ProcessSaveList() {
            //todo 保存数据优化一下流程，GameSrv无需等待DBSrv结果，异步通知即可
            if (SaveProcessList.Count > 0) {
                IList<int> tempList = new List<int>();
                while (SaveProcessList.Count > 0) {
                    int queryId = SaveProcessList.Dequeue();
                    int nIdent = 0;
                    int nRecog = 0;
                    byte[] data = null;
                    if (GetDbSrvMessage(queryId, ref nIdent, ref nRecog, ref data)) {
                        if (nIdent == Messages.DBR_SAVEHUMANRCD && nRecog == 1) {
                            M2Share.FrontEngine.RemoveSaveList(queryId);
                        }
                    }
                    else {
                        tempList.Add(queryId);
                    }
                }
                if (tempList.Count > 0) {
                    for (int i = 0; i < tempList.Count; i++) {
                        SaveProcessList.Enqueue(tempList[i]);
                    }
                }
            }
        }

        public static void ProcessQueryList() {
            if (QueryProcessList.Count > 0) {
                IList<QueryPlayData> tempList = new List<QueryPlayData>();
                while (QueryProcessList.Count > 0) {
                    QueryPlayData queryData = QueryProcessList.Dequeue();
                    if (queryData.QuetyCount >= 50) {
                        continue;
                    }
                    int nIdent = 0;
                    int nRecog = 0;
                    byte[] data = null;
                    if (GetDbSrvMessage(queryData.QueryId, ref nIdent, ref nRecog, ref data)) {
                        if (nIdent == Messages.DBR_LOADHUMANRCD && nRecog == 1 && data.Length > 0)
                        {
                            var responsePacket = SerializerUtil.Deserialize<LoadPlayerDataPacket>(EDCode.DecodeBuff(data));
                            responsePacket.ChrName = EDCode.DeCodeString(responsePacket.ChrName);
                            LoadPlayDataMap.TryAdd(queryData.QueryId, responsePacket);
                        }
                    }
                    else {
                        queryData.QuetyCount++;
                        tempList.Add(queryData);
                    }
                }
                if (tempList.Count > 0) {
                    for (int i = 0; i < tempList.Count; i++) {
                        QueryProcessList.Enqueue(tempList[i]);
                    }
                }
            }
        }

        private static bool LoadRcd(LoadPlayerDataMessage loadHuman, ref int queryId) {
            int nQueryId = GetQueryId();
            ServerRequestMessage packet = new ServerRequestMessage(Messages.DB_LOADHUMANRCD, 0, 0, 0, 0);
            if (M2Share.DataServer.SendRequest(nQueryId, packet, loadHuman)) {
                QueryProcessList.Enqueue(new QueryPlayData() {
                    QueryId = nQueryId
                });
                queryId = nQueryId;
                Logger.Info($"查询[{queryId}]");
                return true;
            }
            Logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            return false;
        }

        private static int GetQueryId() {
            M2Share.Config.nDBQueryID++;
            if (M2Share.Config.nDBQueryID > int.MaxValue - 1) {
                M2Share.Config.nDBQueryID = 1;
            }
            return M2Share.Config.nDBQueryID;
        }
    }
}