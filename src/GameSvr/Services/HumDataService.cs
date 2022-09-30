using NLog;
using System.Collections.Concurrent;
using SystemModule;
using SystemModule.Packet.ServerPackets;

namespace GameSvr.Services
{
    public class HumDataService
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly ConcurrentDictionary<int, RequestServerPacket> ReceivedMap = new ConcurrentDictionary<int, RequestServerPacket>();

        public static bool DBSocketConnected()
        {
            return true;
        }

        public static void AddToProcess(int queryId, RequestServerPacket data)
        {
            ReceivedMap.TryAdd(queryId, data);
        }

        private static bool GetDBSockMsg(int queryId, ref int nIdent, ref int nRecog, ref byte[] data, int dwTimeOut, bool boLoadRcd)
        {
            var result = false;
            var boLoadDBOK = false;
            RequestServerPacket respPack = null;
            const string sLoadDBTimeOut = "[RunDB] 读取人物数据超时...";
            const string sSaveDBTimeOut = "[RunDB] 保存人物数据超时...";
            var timeOutTick = HUtil32.GetTickCount();
            while (true)
            {
                if ((HUtil32.GetTickCount() - timeOutTick) > dwTimeOut)
                {
                    _logger.Debug("获取DBServer消息超时...");
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
                    var serverPacket = ProtoBufDecoder.DeSerialize<ServerMessagePacket>(EDCode.DecodeBuff(respPack.Message));
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
                M2Share.Log.Error(boLoadRcd ? sLoadDBTimeOut : sSaveDBTimeOut);
            }
            if ((HUtil32.GetTickCount() - timeOutTick) > M2Share.dwRunDBTimeMax)
            {
                M2Share.dwRunDBTimeMax = HUtil32.GetTickCount() - timeOutTick;
            }
            return result;
        }

        public static bool LoadHumRcdFromDB(string sAccount, string sChrName, string sStr, ref HumDataInfo HumanRcd, int nCertCode)
        {
            var result = false;
            var loadHum = new LoadHumDataPacket()
            {
                Account = sAccount,
                ChrName = sChrName,
                UserAddr = sStr,
                SessionID = nCertCode
            };
            if (LoadRcd(loadHum, ref HumanRcd))
            {
                HumanRcd.Data.sChrName = sChrName;
                HumanRcd.Data.Account = sAccount;
                if (HumanRcd.Data.sChrName == sChrName && (string.IsNullOrEmpty(HumanRcd.Data.Account) || HumanRcd.Data.Account == sAccount))
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
        public static bool SaveHumRcdToDB(string sAccount, string sChrName, int nSessionID, HumDataInfo HumanRcd)
        {
            M2Share.Config.nSaveDBCount++;
            return SaveRcd(sAccount, sChrName, nSessionID, HumanRcd);
        }

        private static bool SaveRcd(string sAccount, string sChrName, int nSessionID, HumDataInfo HumanRcd)
        {
            var nIdent = 0;
            var nRecog = 0;
            byte[] data = null;
            var nQueryId = GetQueryId();
            var result = false;
            var packet = new ServerMessagePacket(Grobal2.DB_SAVEHUMANRCD, nSessionID, 0, 0, 0);
            var saveHumData = new SaveHumDataPacket();
            saveHumData.Account = sAccount;
            saveHumData.ChrName = sChrName;
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
                        _logger.Error($"[RunDB] 保存人物({sChrName})数据失败");
                    }
                }
            }
            else
            {
                _logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            }
            return result;
        }

        private static bool LoadRcd(LoadHumDataPacket loadHuman, ref HumDataInfo HumanRcd)
        {
            var result = false;
            var nIdent = 0;
            var nRecog = 0;
            byte[] humRespData = null;
            var nQueryID = GetQueryId();
            var packet = new ServerMessagePacket(Grobal2.DB_LOADHUMANRCD, 0, 0, 0, 0);
            if (M2Share.DataServer.SendRequest(nQueryID, packet, loadHuman))
            {
                if (GetDBSockMsg(nQueryID, ref nIdent, ref nRecog, ref humRespData, 5000, true))
                {
                    if (nIdent == Grobal2.DBR_LOADHUMANRCD)
                    {
                        if (nRecog == 1)
                        {
                            humRespData = EDCode.DecodeBuff(humRespData);
                            var responsePacket = ProtoBufDecoder.DeSerialize<LoadHumanRcdResponsePacket>(humRespData);
                            var sDBChrName = EDCode.DeCodeString(responsePacket.sChrName);
                            if (sDBChrName == loadHuman.ChrName)
                            {
                                HumanRcd = new HumDataInfo();
                                HumanRcd = responsePacket.HumDataInfo;
                                result = true;
                            }
                        }
                    }
                }
            }
            else
            {
                _logger.Warn("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
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

