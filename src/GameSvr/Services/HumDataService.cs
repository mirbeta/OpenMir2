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
                    if (ReceivedMap.TryGetValue(queryId, out respPack))
                    {
                        if (respPack == null)
                        {
                            Thread.Sleep(1);
                            continue;
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
                if (respPack.PacketLen > 0)
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

        public static bool LoadHumRcdFromDB(string sAccount, string sChrName, string sStr, ref PlayerDataInfo HumanRcd, int nCertCode)
        {
            var result = false;
            var loadHum = new LoadPlayerDataMessage()
            {
                Account = sAccount,
                ChrName = sChrName,
                UserAddr = sStr,
                SessionID = nCertCode
            };
            if (LoadRcd(loadHum, ref HumanRcd))
            {
                HumanRcd.Data.ChrName = sChrName;
                HumanRcd.Data.Account = sAccount;
                if (HumanRcd.Data.ChrName == sChrName && (string.IsNullOrEmpty(HumanRcd.Data.Account) || HumanRcd.Data.Account == sAccount))
                {
                    result = true;
                }
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
                if (GetDBSockMsg(nQueryId, ref nIdent, ref nRecog, ref data, 5000, false))
                {
                    if (nIdent == Grobal2.DBR_SAVEHUMANRCD && nRecog == 1)
                    {
                        result = true;
                    }
                    else
                    {
                        Logger.Error($"[RunDB] 保存人物({chrName})数据失败");
                    }
                }
            }
            else
            {
                Logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            }
            return result;
        }

        private static bool LoadRcd(LoadPlayerDataMessage loadHuman, ref PlayerDataInfo HumanRcd)
        {
            var result = false;
            var nIdent = 0;
            var nRecog = 0;
            byte[] humRespData = null;
            var nQueryID = GetQueryId();
            var packet = new ServerRequestMessage(Grobal2.DB_LOADHUMANRCD, 0, 0, 0, 0);
            if (M2Share.DataServer.SendRequest(nQueryID, packet, loadHuman))
            {
                if (GetDBSockMsg(nQueryID, ref nIdent, ref nRecog, ref humRespData, 5000, true))
                {
                    if (nIdent == Grobal2.DBR_LOADHUMANRCD)
                    {
                        if (nRecog == 1)
                        {
                            humRespData = EDCode.DecodeBuff(humRespData);
                            var responsePacket = ProtoBufDecoder.DeSerialize<LoadPlayerDataPacket>(humRespData);
                            var chrName = EDCode.DeCodeString(responsePacket.ChrName);
                            if (chrName == loadHuman.ChrName)
                            {
                                HumanRcd = new PlayerDataInfo();
                                HumanRcd = responsePacket.HumDataInfo;
                                result = true;
                            }
                        }
                    }
                }
            }
            else
            {
                Logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            }
            return result;
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

