using System;
using System.Collections.Concurrent;
using System.Threading;
using SystemModule;

namespace GameSvr
{
    public class HumDataService
    {
        private static readonly ConcurrentDictionary<int, RequestServerPacket> _receivedMap = new ConcurrentDictionary<int, RequestServerPacket>();

        public static bool DBSocketConnected()
        {
            return true;
        }

        public static void AddToProcess(int queryId, RequestServerPacket data)
        {
            _receivedMap.TryAdd(queryId, data);
        }

        private static bool GetDBSockMsg(int nQueryID, ref int nIdent, ref int nRecog, ref byte[] data, int dwTimeOut, bool boLoadRcd)
        {
            bool result = false;
            bool boLoadDBOK = false;
            RequestServerPacket respPack = null;
            const string sLoadDBTimeOut = "[RunDB] 读取人物数据超时...";
            const string sSaveDBTimeOut = "[RunDB] 保存人物数据超时...";
            var dwTimeOutTick = HUtil32.GetTickCount();
            while (true)
            {
                if ((HUtil32.GetTickCount() - dwTimeOutTick) > dwTimeOut)
                {
                    Console.WriteLine("获取DBServer消息超时...");
                    break;
                }
                HUtil32.EnterCriticalSection(M2Share.UserDBSection);
                try
                {
                    if (_receivedMap.ContainsKey(nQueryID))
                    {
                        respPack = _receivedMap[nQueryID];
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
                    var serverPacket = ProtoBufDecoder.DeSerialize<ServerMessagePacket>(EDcode.DecodeBuff(respPack.Message));
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
                M2Share.ErrorMessage(boLoadRcd ? sLoadDBTimeOut : sSaveDBTimeOut);
            }
            if ((HUtil32.GetTickCount() - dwTimeOutTick) > M2Share.dwRunDBTimeMax)
            {
                M2Share.dwRunDBTimeMax = HUtil32.GetTickCount() - dwTimeOutTick;
            }
            return result;
        }

        public static bool LoadHumRcdFromDB(string sAccount, string sCharName, string sStr, ref THumDataInfo HumanRcd, int nCertCode)
        {
            bool result = false;
            HumanRcd = new THumDataInfo();
            var loadHum = new LoadHumDataPacket()
            {
                sAccount = sAccount,
                sChrName = sCharName,
                sUserAddr = sStr,
                nSessionID = nCertCode
            };
            if (LoadRcd(loadHum, ref HumanRcd))
            {
                HumanRcd.Data.sCharName = sCharName;
                HumanRcd.Data.sAccount = sAccount;
                if (HumanRcd.Data.sCharName == sCharName && (HumanRcd.Data.sAccount == "" || HumanRcd.Data.sAccount == sAccount))
                {
                    result = true;
                }
            }
            M2Share.g_Config.nLoadDBCount++;
            return result;
        }

        /// <summary>
        /// 保存玩家数据到DB
        /// </summary>
        /// <returns></returns>
        public static bool SaveHumRcdToDB(string sAccount, string sCharName, int nSessionID, THumDataInfo HumanRcd)
        {
            M2Share.g_Config.nSaveDBCount++;
            return SaveRcd(sAccount, sCharName, nSessionID, HumanRcd);
        }

        private static bool SaveRcd(string sAccount, string sCharName, int nSessionID, THumDataInfo HumanRcd)
        {
            int nIdent = 0;
            int nRecog = 0;
            byte[] data = null;
            int nQueryID = GetQueryID(M2Share.g_Config);
            bool result = false;
            var packet = new ServerMessagePacket(Grobal2.DB_SAVEHUMANRCD, nSessionID, 0, 0, 0);
            var saveHumData = new SaveHumDataPacket();
            saveHumData.sAccount = sAccount;
            saveHumData.sCharName = sCharName;
            saveHumData.HumDataInfo = HumanRcd;
            if (M2Share.DataServer.SendRequest(nQueryID, packet, saveHumData))
            {
                if (GetDBSockMsg(nQueryID, ref nIdent, ref nRecog, ref data, 5000, false))
                {
                    if (nIdent == Grobal2.DBR_SAVEHUMANRCD && nRecog == 1)
                    {
                        result = true;
                    }
                    else
                    {
                        M2Share.ErrorMessage($"[RunDB] 保存人物({sCharName})数据失败");
                    }
                }
            }
            else
            {
                Console.WriteLine("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            }
            return result;
        }

        private static bool LoadRcd(LoadHumDataPacket loadHuman, ref THumDataInfo HumanRcd)
        {
            bool result = true;
            int nIdent = 0;
            int nRecog = 0;
            byte[] humRespData = null;
            int nQueryID = GetQueryID(M2Share.g_Config);
            var packet = new ServerMessagePacket(Grobal2.DB_LOADHUMANRCD, 0, 0, 0, 0);
            if (M2Share.DataServer.SendRequest(nQueryID, packet, loadHuman))
            {
                if (GetDBSockMsg(nQueryID, ref nIdent, ref nRecog, ref humRespData, 5000, true))
                {
                    if (nIdent == Grobal2.DBR_LOADHUMANRCD)
                    {
                        if (nRecog == 1)
                        {
                            humRespData = EDcode.DecodeBuff(humRespData);
                            var responsePacket = ProtoBufDecoder.DeSerialize<LoadHumanRcdResponsePacket>(humRespData);
                            var sDBCharName = EDcode.DeCodeString(responsePacket.sChrName);
                            if (sDBCharName == loadHuman.sChrName)
                            {
                                HumanRcd = responsePacket.HumDataInfo;
                                result = true;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("DBSvr链接丢失，请确认DBSvr服务状态是否正常。");
            }
            return result;
        }

        private static int GetQueryID(GameSvrConfig Config)
        {
            Config.nDBQueryID++;
            if (Config.nDBQueryID > int.MaxValue - 1)
            {
                Config.nDBQueryID = 1;
            }
            return Config.nDBQueryID;
        }
    }
}

