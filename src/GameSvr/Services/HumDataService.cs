using System;
using System.Threading;
using SystemModule;

namespace GameSvr
{
    public class HumDataService
    {
        public static bool DBSocketConnected()
        {
            return true;
        }

        private static bool GetDBSockMsg(int nQueryID, ref int nIdent, ref int nRecog, ref string sStr, int dwTimeOut, bool boLoadRcd)
        {
            bool result = false;
            bool boLoadDBOK = false;
            byte[] sData;
            string s28 = string.Empty;
            string s2C = string.Empty;
            string sCheckFlag = string.Empty;
            string sDefMsg = string.Empty;
            string s38 = string.Empty;
            int nLen;
            int nCheckCode;
            ClientPacket DefMsg;
            const string sLoadDBTimeOut = "[RunDB] 读取人物数据超时...";
            const string sSaveDBTimeOut = "[RunDB] 保存人物数据超时...";
            int dwTimeOutTick = HUtil32.GetTickCount();
            while (true)
            {
                if ((HUtil32.GetTickCount() - dwTimeOutTick) > dwTimeOut)
                {
                    break;
                }
                sData = null;
                HUtil32.EnterCriticalSection(M2Share.UserDBSection);
                try
                {
                    sData = M2Share.g_Config.sDBSocketRecvBuff;
                    M2Share.g_Config.sDBSocketRecvBuff = null;
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(M2Share.UserDBSection);
                }
                if (sData!=null && sData.Length>0)
                {
                    s28 = "";
                    sData = HUtil32.ArrestStringEx(sData, '#', '!', ref s28);
                    if (s28 != "")
                    {
                        s28 = HUtil32.GetValidStr3(s28, ref s2C, new[] { "/" });
                        nLen = s28.Length;
                        unsafe
                        {
                            if (nLen >= 12 && HUtil32.Str_ToInt(s2C, 0) == nQueryID) //sizeof(ClientMessage)
                            {
                                nCheckCode = HUtil32.MakeLong(HUtil32.Str_ToInt(s2C, 0) ^ 170, nLen);
                                byte[] data = new byte[sizeof(int)];
                                fixed (byte* by = data)
                                {
                                    *(int*)by = nCheckCode;
                                }
                                sCheckFlag = EDcode.EncodeBuffer(data, data.Length);
                                if (HUtil32.CompareBackLStr(s28, sCheckFlag, sCheckFlag.Length))
                                {
                                    if (nLen == Grobal2.DEFBLOCKSIZE)
                                    {
                                        sDefMsg = s28;
                                        s38 = "";
                                    }
                                    else
                                    {
                                        sDefMsg = s28.Substring(0, Grobal2.DEFBLOCKSIZE);
                                        s38 = s28.Substring(Grobal2.DEFBLOCKSIZE, s28.Length - Grobal2.DEFBLOCKSIZE - 6);
                                    }
                                    DefMsg = EDcode.DecodePacket(sDefMsg);
                                    nIdent = DefMsg.Ident;
                                    nRecog = DefMsg.Recog;
                                    sStr = s38;
                                    boLoadDBOK = true;
                                    result = true;
                                    break;
                                }
                                else
                                {
                                    M2Share.g_Config.nLoadDBErrorCount++;
                                    break;
                                }
                            }
                            else
                            {
                                M2Share.g_Config.nLoadDBErrorCount++;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
            if (!boLoadDBOK)
            {
                if (boLoadRcd)
                {
                    M2Share.ErrorMessage(sLoadDBTimeOut);
                }
                else
                {
                    M2Share.ErrorMessage(sSaveDBTimeOut);
                }
            }
            if ((HUtil32.GetTickCount() - dwTimeOutTick) > M2Share.dwRunDBTimeMax)
            {
                M2Share.dwRunDBTimeMax = HUtil32.GetTickCount() - dwTimeOutTick;
            }
            M2Share.g_Config.boDBSocketWorking = false;
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
            string sStr = string.Empty;
            int nQueryID = GetQueryID(M2Share.g_Config);
            bool result = false;
            var packet = new ServerMessagePacket(Grobal2.DB_SAVEHUMANRCD, nSessionID, 0, 0, 0);
            var saveHumData = new SaveHumDataPacket();
            saveHumData.sAccount = sAccount;
            saveHumData.sCharName = sCharName;
            saveHumData.HumDataInfo = HumanRcd;
            M2Share.DataServer.SendRequest(nQueryID, packet, saveHumData);
            if (GetDBSockMsg(nQueryID, ref nIdent, ref nRecog, ref sStr, 5000, false))
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
            return result;
        }

        private static bool LoadRcd(LoadHumDataPacket loadHuman, ref THumDataInfo HumanRcd)
        {
            bool result = true;
            int nIdent = 0;
            int nRecog = 0;
            string sHumanRcdStr = string.Empty;
            int nQueryID = GetQueryID(M2Share.g_Config);
            var packet = new ServerMessagePacket(Grobal2.DB_LOADHUMANRCD, 0, 0, 0, 0);
            M2Share.DataServer.SendRequest(nQueryID, packet, loadHuman);
            if (GetDBSockMsg(nQueryID, ref nIdent, ref nRecog, ref sHumanRcdStr, 5000, true))
            {
                if (nIdent == Grobal2.DBR_LOADHUMANRCD)
                {
                    if (nRecog == 1)
                    {
                        /*sHumanRcdStr = HUtil32.GetValidStr3(sHumanRcdStr, ref sDBMsg, '/');
                        var sDBCharName = EDcode.DeCodeString(sDBMsg);
                        if (sDBCharName == loadHuman.sChrName)
                        {
                            var dataBuff = EDcode.DecodeBuffer(sHumanRcdStr);
                            HumanRcd = new THumDataInfo(dataBuff);
                            result = true;
                        }*/
                    }
                    else
                    {
                        result = false;
                    }
                }
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

