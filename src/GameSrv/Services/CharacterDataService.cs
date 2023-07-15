using M2Server;
using NLog;
using System.Collections.Concurrent;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packets.ServerPackets;

namespace GameSrv.Services
{
    public class QueryPlayData
    {
        public int QueryId;
        public int QuetyCount;
    }

    /// <summary>
    /// 玩家数据处理服务
    /// </summary>
    public static class CharacterDataService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly ConcurrentDictionary<int, ServerRequestData> ReceivedMap = new ConcurrentDictionary<int, ServerRequestData>();
        private static readonly ConcurrentQueue<QueryPlayData> QueryProcessList = new ConcurrentQueue<QueryPlayData>();
        private static readonly ConcurrentQueue<int> SaveProcessList = new ConcurrentQueue<int>();
        private static readonly ConcurrentDictionary<int, LoadPlayerDataPacket> LoadPlayDataMap = new ConcurrentDictionary<int, LoadPlayerDataPacket>();

        public static void Enqueue(int queryId, ServerRequestData data)
        {
            ReceivedMap.TryAdd(queryId, data);
            Logger.Debug($"执行任务Id:{queryId}成功");
        }

        private static bool GetDataSrvMessage(int queryId, ref int nIdent, ref int nRecog, ref byte[] data)
        {
            var result = false;
            HUtil32.EnterCriticalSection(M2Share.UserDBCriticalSection);
            try
            {
                if (ReceivedMap.TryGetValue(queryId, out var respPack))
                {
                    if (respPack == null)
                    {
                        return false;
                    }
                    var serverPacket = SerializerUtil.Deserialize<ServerRequestMessage>(EDCode.DecodeBuff(respPack.Message));
                    if (serverPacket == null)
                    {
                        return false;
                    }
                    nIdent = serverPacket.Ident;
                    nRecog = serverPacket.Recog;
                    data = respPack.Packet;
                    result = true;
                }
                ReceivedMap.TryRemove(queryId, out _);
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.UserDBCriticalSection);
            }
            return result;
        }

        public static bool GetPlayData(int queryId, ref CharacterDataInfo playerData)
        {
            if (!LoadPlayDataMap.TryGetValue(queryId, out var loadPlayDataPacket))
                return false;
            LoadPlayDataMap.TryRemove(queryId, out _);
            playerData = loadPlayDataPacket.HumDataInfo;
            return true;
        }

        /// <summary>
        /// 查询角色数据
        /// </summary>
        public static bool QueryCharacterData(string account, string chrName, string addr, ref int queryId, int certCode)
        {
            var result = false;
            var loadHum = new LoadCharacterData()
            {
                Account = account,
                ChrName = chrName,
                UserAddr = addr,
                SessionID = certCode
            };
            if (LoadRcd(loadHum, ref queryId))
            {
                result = true;
            }
            SystemShare.Config.nLoadDBCount++;
            return result;
        }

        /// <summary>
        /// 保存玩家数据到DB
        /// </summary>
        /// <returns></returns>
        public static bool SaveCharacterData(SavePlayerRcd saveRcd, ref int queryId)
        {
            SystemShare.Config.nSaveDBCount++;
            return SaveRcd(saveRcd, ref queryId);
        }

        private static bool SaveRcd(SavePlayerRcd saveRcd, ref int queryId)
        {
            queryId = GetQueryId();
            var packet = new ServerRequestMessage(Messages.DB_SAVEHUMANRCD, saveRcd.SessionID, 0, 0, 0);
            var saveHumData = new SaveCharacterData(saveRcd.Account, saveRcd.ChrName, saveRcd.CharacterData);
            if (GameShare.DataServer.SendRequest(queryId, packet, saveHumData))
            {
                SaveProcessList.Enqueue(queryId);
                return true;
            }
            Logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            return false;
        }

        public static void ProcessSaveQueue()
        {
            if (SaveProcessList.TryPeek(out var queryId))//todo 保存数据优化一下流程，M2Server.需等待DBSrv结果，异步通知即可
            {
                var nIdent = 0;
                var nRecog = 0;
                byte[] data = null;
                if (GetDataSrvMessage(queryId, ref nIdent, ref nRecog, ref data))
                {
                    if (nIdent == Messages.DBR_SAVEHUMANRCD && nRecog == 1)
                    {
                        // M2Share.FrontEngine.RemoveSaveList(queryId);
                    }
                    SaveProcessList.TryDequeue(out _);
                }
            }
        }

        public static void ProcessQueryQueue()
        {
            if (QueryProcessList.TryDequeue(out var queryData))
            {
                if (queryData.QuetyCount >= 10000)
                {
                    Logger.Warn("超过最大查询次数,放弃此次保存.");
                    return;
                }
                var nIdent = 0;
                var nRecog = 0;
                byte[] data = null;
                if (GetDataSrvMessage(queryData.QueryId, ref nIdent, ref nRecog, ref data))
                {
                    if (nIdent == Messages.DBR_LOADHUMANRCD && nRecog == 1 && data.Length > 0)
                    {
                        var responsePacket = SerializerUtil.Deserialize<LoadPlayerDataPacket>(EDCode.DecodeBuff(data));
                        responsePacket.ChrName = EDCode.DeCodeString(responsePacket.ChrName);
                        LoadPlayDataMap.TryAdd(queryData.QueryId, responsePacket);
                    }
                    QueryProcessList.TryDequeue(out _);
                }
                else
                {
                    queryData.QuetyCount++;
                    QueryProcessList.Enqueue(queryData);
                }
            }
        }

        private static bool LoadRcd(LoadCharacterData loadHuman, ref int queryId)
        {
            var nQueryId = GetQueryId();
            var packet = new ServerRequestMessage(Messages.DB_LOADHUMANRCD, 0, 0, 0, 0);
            if (GameShare.DataServer.SendRequest(nQueryId, packet, loadHuman))
            {
                QueryProcessList.Enqueue(new QueryPlayData()
                {
                    QueryId = nQueryId
                });
                queryId = nQueryId;
                Logger.Debug($"查询玩家数据任务ID:[{queryId}]");
                return true;
            }
            Logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            return false;
        }

        private static int GetQueryId()
        {
            SystemShare.Config.nDBQueryID++;
            if (SystemShare.Config.nDBQueryID > int.MaxValue - 1)
            {
                SystemShare.Config.nDBQueryID = 1;
            }
            return SystemShare.Config.nDBQueryID;
        }
    }
}