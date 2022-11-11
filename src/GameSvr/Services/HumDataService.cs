using NLog;
using System.Collections.Concurrent;
using SystemModule;
using SystemModule.Packets.ServerPackets;

namespace GameSvr.Services
{
    public static class PlayerDataService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly ConcurrentDictionary<int, ServerRequestData> ReceivedMap = new ConcurrentDictionary<int, ServerRequestData>();
        private static readonly Queue<int> queryProcessList = new Queue<int>();
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
            var boLoadDBOK = false;
            ServerRequestData respPack = null;
            const string sLoadDBTimeOut = "[RunDB] 读取人物数据超时...";
            const string sSaveDBTimeOut = "[RunDB] 保存人物数据超时...";
            var timeOutTick = HUtil32.GetTickCount();
            while (true)
            {
                if ((HUtil32.GetTickCount() - timeOutTick) > dwTimeOut)
                {
                    Logger.Debug("获取DBServer消息超时...");
                    break;
                }
                HUtil32.EnterCriticalSection(M2Share.UserDBSection);
                try
                {
                    if (ReceivedMap.ContainsKey(queryId))
                    {
                        if (ReceivedMap.TryGetValue(queryId, out respPack))
                        {
                            if (respPack == null)
                            {
                                Thread.Sleep(1);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(1);
                        continue;
                    }
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(M2Share.UserDBSection);
                }
                if (respPack != null && respPack.PacketLen > 0)
                {
                    var serverPacket = ProtoBufDecoder.DeSerialize<ServerRequestMessage>(EDCode.DecodeBuff(respPack.Message));
                    if (serverPacket == null)
                    {
                        return false;
                    }
                    nIdent = serverPacket.Ident;
                    nRecog = serverPacket.Recog;
                    data = respPack.Packet;
                    boLoadDBOK = true;
                    result = true;
                    break;
                }
                Thread.Sleep(1);
            }
            if (!boLoadDBOK)
            {
                M2Share.Log.LogError(boLoadRcd ? sLoadDBTimeOut : sSaveDBTimeOut);
            }
            if ((HUtil32.GetTickCount() - timeOutTick) > M2Share.dwRunDBTimeMax)
            {
                M2Share.dwRunDBTimeMax = HUtil32.GetTickCount() - timeOutTick;
            }
            return result;
        }

        public static bool GetPlayData(int queryId,ref PlayerDataInfo playerData)
        {
            if (loadPlayDataMap.ContainsKey(queryId))
            {
                playerData = loadPlayDataMap[queryId].HumDataInfo;
                return true;
            }
            return false;
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
        public static bool SaveHumRcdToDB(string account, string chrName, int sessionId, PlayerDataInfo HumanRcd)
        {
            M2Share.Config.nSaveDBCount++;
            return SaveRcd(account, chrName, sessionId, HumanRcd);
        }

        private static bool SaveRcd(string account, string chrName, int sessionId, PlayerDataInfo HumanRcd)
        {
            var nIdent = 0;
            var nRecog = 0;
            byte[] data = null;
            var nQueryId = GetQueryId();
            var result = false;
            var packet = new ServerRequestMessage(Grobal2.DB_SAVEHUMANRCD, sessionId, 0, 0, 0);
            var saveHumData = new SavePlayerDataMessage();
            saveHumData.Account = account;
            saveHumData.ChrName = chrName;
            saveHumData.HumDataInfo = HumanRcd;
            if (M2Share.DataServer.SendRequest(nQueryId, packet, saveHumData))
            {
                saveProcessList.Enqueue(nQueryId);
            }
            else
            {
                Logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            }
            return result;
        }

        public static void ProcessSaveList()
        {
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
                        // result = true;
                    }
                    else
                    {
                        // Logger.Error($"[RunDB] 保存人物({chrName})数据失败");
                    }
                }
            }
        }

        public static void ProcessQueryList()
        {
            while (queryProcessList.Count > 0)
            {
                var queryId = queryProcessList.Dequeue();
                var nIdent = 0;
                var nRecog = 0;
                byte[] data = null;
                if (GetDBSockMsg(queryId, ref nIdent, ref nRecog, ref data, 5000, true))
                {
                    if (nIdent == Grobal2.DBR_LOADHUMANRCD)
                    {
                        if (nRecog == 1)
                        {
                            var humRespData = EDCode.DecodeBuff(data);
                            var responsePacket = ProtoBufDecoder.DeSerialize<LoadPlayerDataPacket>(humRespData);
                            responsePacket.ChrName = EDCode.DeCodeString(responsePacket.ChrName);
                            loadPlayDataMap.TryAdd(queryId, responsePacket);
                            /*var chrName = EDCode.DeCodeString(responsePacket.ChrName);
                            if (chrName == loadHuman.ChrName)
                            {
                                HumanRcd = new PlayerDataInfo();
                                HumanRcd = responsePacket.HumDataInfo;
                                result = true;
                            }*/
                        }
                    }
                }
            }
        }

        private static bool LoadRcd(LoadPlayerDataMessage loadHuman, ref int queryId)
        {
            var nQueryId = GetQueryId();
            var packet = new ServerRequestMessage(Grobal2.DB_LOADHUMANRCD, 0, 0, 0, 0);
            if (M2Share.DataServer.SendRequest(nQueryId, packet, loadHuman))
            {
                queryProcessList.Enqueue(nQueryId);
                queryId = nQueryId;
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

