using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Packages;

namespace DBSvr
{
    public partial class TFrmDBSrv
    {
        private int n334 = 0;
        private TDefaultMessage m_DefMsg;
        private int n344 = 0;
        private int n348 = 0;
        private string s34C = String.Empty;
        private IList<TServerInfo> ServerList = null;
        private IList<THumSession> HumSessionList = null;
        private bool m_boRemoteClose = false;
        private readonly THumDB HumDB;

        public TFrmDBSrv()
        {

        }

        public void ServerSocketClientConnect(Object Sender, Socket Socket)
        {
            TServerInfo ServerInfo;
            //string sIPaddr = Socket.RemoteAddress;
            //if (!DBShare.CheckServerIP(sIPaddr))
            //{
            //    DBShare.OutMainMessage("非法服务器连接: " + sIPaddr);
            //    //Socket.Close;
            //    return;
            //}
            if (!DBShare.boOpenDBBusy)
            {
                ServerInfo = new TServerInfo();
                ServerInfo.bo08 = true;
                //ServerInfo.nSckHandle = Socket.SocketHandle;
                ServerInfo.sStr = "";
                ServerInfo.Socket = Socket;
                ServerList.Add(ServerInfo);
            }
            else
            {
                //Socket.Close;
            }
        }

        public void ServerSocketClientDisconnect(Object Sender, Socket Socket)
        {
            TServerInfo ServerInfo;
            for (var i = 0; i < ServerList.Count; i++)
            {
                ServerInfo = ServerList[i];
                //if (ServerInfo.nSckHandle == Socket.SocketHandle)
                //{
                //    ServerInfo = null;
                //    ServerList.RemoveAt(i);
                //    ClearSocket(Socket);
                //    break;
                //}
            }
        }

        public void ServerSocketClientError(Object Sender, Socket Socket)
        {

        }

        public void ServerSocketClientRead(Object Sender, Socket Socket)
        {
            TServerInfo ServerInfo;
            string s10;
            for (var i = 0; i < ServerList.Count; i++)
            {
                ServerInfo = ServerList[i];
                //if (ServerInfo.nSckHandle == Socket.SocketHandle)
                //{
                //    s10 = Socket.ReceiveText;
                //    DBShare.n4ADBF4++;
                //    if (s10 != "")
                //    {
                //        ServerInfo.sStr = ServerInfo.sStr + s10;
                //        if (s10.IndexOf("!") > 0)
                //        {
                //            ProcessServerPacket(ServerInfo);
                //            DBShare.n4ADBF8++;
                //            n348++;
                //            break;
                //        }
                //        else
                //        {
                //            if (ServerInfo.sStr.Length > 81920)
                //            {
                //                ServerInfo.sStr = "";
                //                DBShare.n4ADC2C++;
                //            }
                //        }
                //    }
                //    break;
                //}
            }
        }

        private void ProcessServerPacket(TServerInfo ServerInfo)
        {
            bool bo25;
            string sC = string.Empty;
            string s1C = string.Empty;
            string s20 = string.Empty;
            string s24 = string.Empty;
            int n14;
            int n18;
            short wE;
            short w10;
            TDefaultMessage DefMsg;
            if (DBShare.boOpenDBBusy)
            {
                return;
            }
            try
            {
                bo25 = false;
                s1C = ServerInfo.sStr;
                ServerInfo.sStr = "";
                s20 = "";
                s1C = HUtil32.ArrestStringEx(s1C, "#", "!", ref s20);
                if (s20 != "")
                {
                    s20 = HUtil32.GetValidStr3(s20, ref s24, new string[] { "/" });
                    n14 = s20.Length;
                    if ((n14 >= Grobal2.DEFBLOCKSIZE) && (s24 != ""))
                    {
                        //wE = HUtil32.Str_ToInt(s24, 0) ^ 170;
                        //w10 = n14;
                        //n18 = HUtil32.MakeLong(wE, w10);
                        //sC = EDcode.EncodeBuffer(n18, sizeof(int));
                        //s34C = s24;
                        if (HUtil32.CompareBackLStr(s20, sC, sC.Length))
                        {
                            ProcessServerMsg(s20, n14, ServerInfo.Socket);
                            bo25 = true;
                        }
                    }
                }
                if (s1C != "")
                {
                    DBShare.n4ADC00++;
                    //Label4.Text = "Error " + (DBShare.n4ADC00).ToString();
                }
                if (!bo25)
                {
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_FAIL, 0, 0, 0, 0);
                    SendSocket(ServerInfo.Socket, EDcode.EncodeMessage(m_DefMsg));
                    DBShare.n4ADC00++;
                    //Label4.Text = "Error " + (DBShare.n4ADC00).ToString();
                }
            }
            finally
            {
            }
        }

        private void SendSocket(Socket Socket, string sMsg)
        {
            int n10;
            string s18;
            DBShare.n4ADBFC++;
            //n10 = HUtil32.MakeLong(HUtil32.Str_ToInt(s34C, 0) ^ 170, sMsg.Length + 6);
            //s18 = EDcode.EncodeBuffer(n10, sizeof(int));
            //Socket.SendText("#" + s34C + "/" + sMsg + s18 + "!");
        }

        private void ProcessServerMsg(string sMsg, int nLen, Socket Socket)
        {
            string sDefMsg;
            string sData;
            TDefaultMessage DefMsg;
            if (nLen == Grobal2.DEFBLOCKSIZE)
            {
                sDefMsg = sMsg;
                sData = "";
            }
            else
            {
                sDefMsg = sMsg.Substring(1 - 1, Grobal2.DEFBLOCKSIZE);
                sData = sMsg.Substring(Grobal2.DEFBLOCKSIZE + 1 - 1, sMsg.Length - Grobal2.DEFBLOCKSIZE - 6);
            }
            DefMsg = EDcode.DecodeMessage(sDefMsg);
            switch (DefMsg.Ident)
            {
                case Grobal2.DB_LOADHUMANRCD:
                    LoadHumanRcd(sData, Socket);
                    break;
                case Grobal2.DB_SAVEHUMANRCD:
                    SaveHumanRcd(DefMsg.Recog, sData, Socket);
                    break;
                case Grobal2.DB_SAVEHUMANRCDEX:
                    SaveHumanRcdEx(sData, DefMsg.Recog, Socket);
                    break;
                default:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_FAIL, 0, 0, 0, 0);
                    SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
                    DBShare.n4ADC04++;
                    break;
            }
        }

        private void LoadHumanRcd(string sMsg, Socket Socket)
        {
            string sHumName;
            string sAccount;
            string sIPaddr;
            int nIndex;
            int nSessionID;
            int nCheckCode;
            TDefaultMessage DefMsg;
            THumDataInfo HumanRCD = null;
            THumDataInfo HumanRCD2;
            bool boFoundSession = false;
            string str;
            TLoadHuman LoadHuman = new TLoadHuman(EDcode.DecodeBuffer(sMsg));
            sAccount = LoadHuman.sAccount;
            sHumName = LoadHuman.sChrName;
            sIPaddr = LoadHuman.sUserAddr;
            nSessionID = LoadHuman.nSessionID;
            nCheckCode = -1;
            if ((sAccount != "") && (sHumName != ""))
            {
                nCheckCode = IDSocCli.FrmIDSoc.CheckSessionLoadRcd(sAccount, sIPaddr, nSessionID, ref boFoundSession);
                if ((nCheckCode < 0) || !boFoundSession)
                {
                    DBShare.OutMainMessage("[非法请求] " + "帐号: " + sAccount + " IP: " + sIPaddr + " 标识: " + (nSessionID).ToString());
                }
            }
            if ((nCheckCode == 1) || boFoundSession)
            {
                try
                {
                    if (HumDB.OpenEx())
                    {
                        nIndex = HumDB.Index(sHumName);
                        if (nIndex >= 0)
                        {
                            if (HumDB.Get(nIndex, ref HumanRCD) < 0)
                            {
                                nCheckCode = -2;
                            }
                        }
                        else
                        {
                            nCheckCode = -3;
                        }
                    }
                    else
                    {
                        nCheckCode = -4;
                    }
                }
                finally
                {
                    HumDB.Close();
                }
            }
            if ((nCheckCode == 1) || boFoundSession)
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_LOADHUMANRCD, 1, 0, 0, 1);
                str = EDcode.EncodeBuffer(HumanRCD);
                SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg) + EDcode.EncodeString(sHumName) + "/" + EDcode.EncodeBuffer(HumanRCD));
            }
            else
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_LOADHUMANRCD, nCheckCode, 0, 0, 0);
                SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
            }
        }

        private void SaveHumanRcd(int nRecog, string sMsg, Socket Socket)
        {
            string sChrName = string.Empty;
            string sUserID = string.Empty;
            string sHumanRCD = string.Empty;
            int I;
            int nIndex;
            bool bo21;
            TDefaultMessage DefMsg;
            THumDataInfo HumanRCD = null;
            THumSession HumSession;
            sHumanRCD = HUtil32.GetValidStr3(sMsg, ref sUserID, new string[] { "/" });
            sHumanRCD = HUtil32.GetValidStr3(sHumanRCD, ref sChrName, new string[] { "/" });
            sUserID = EDcode.DeCodeString(sUserID);
            sChrName = EDcode.DeCodeString(sChrName);
            bo21 = false;
            if (sHumanRCD.Length == 10 * 4 / 3)
            {
                HumanRCD = new THumDataInfo(EDcode.DecodeBuffer(sHumanRCD));
            }
            else
            {
                bo21 = true;
            }
            if (!bo21)
            {
                bo21 = true;
                try
                {
                    if (HumDB.Open())
                    {
                        nIndex = HumDB.Index(sChrName);
                        if (nIndex < 0)
                        {
                            HumanRCD.Header.sName = sChrName;
                            HumDB.Add(ref HumanRCD);
                            nIndex = HumDB.Index(sChrName);
                        }
                        if (nIndex >= 0)
                        {
                            HumanRCD.Header.sName = sChrName;
                            HumDB.Update(nIndex, ref HumanRCD);
                            bo21 = false;
                        }
                    }
                }
                finally
                {
                    HumDB.Close();
                }
                IDSocCli.FrmIDSoc.SetSessionSaveRcd(sUserID);
            }
            if (!bo21)
            {
                for (I = 0; I < HumSessionList.Count; I++)
                {
                    HumSession = HumSessionList[I];
                    if ((HumSession.sChrName == sChrName) && (HumSession.nIndex == nRecog))
                    {
                        HumSession.dwTick30 = HUtil32.GetTickCount();
                        break;
                    }
                }
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_SAVEHUMANRCD, 1, 0, 0, 0);
                SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
            }
            else
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_LOADHUMANRCD, 0, 0, 0, 0);
                SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
            }
        }

        private void SaveHumanRcdEx(string sMsg, int nRecog, Socket Socket)
        {
            string sChrName = string.Empty;
            string sUserID = string.Empty;
            string sHumanRCD = string.Empty;
            bool bo21;
            TDefaultMessage DefMsg;
            THumDataInfo HumanRCD;
            THumSession HumSession;
            sHumanRCD = HUtil32.GetValidStr3(sMsg, ref sUserID, new string[] { "/" });
            sHumanRCD = HUtil32.GetValidStr3(sHumanRCD, ref sChrName, new string[] { "/" });
            sUserID = EDcode.DeCodeString(sUserID);
            sChrName = EDcode.DeCodeString(sChrName);
            for (var i = 0; i < HumSessionList.Count; i++)
            {
                HumSession = HumSessionList[i];
                if ((HumSession.sChrName == sChrName) && (HumSession.nIndex == nRecog))
                {
                    HumSession.bo24 = false;
                    HumSession.Socket = Socket;
                    HumSession.bo2C = true;
                    HumSession.dwTick30 = HUtil32.GetTickCount();
                    break;
                }
            }
            SaveHumanRcd(nRecog, sMsg, Socket);
        }

        public void Timer1Timer(System.Object Sender, System.EventArgs _e1)
        {
            //LbTransCount.Text = (n348).ToString();
            //n348 = 0;
            //if (ServerList.Count > 0)
            //{
            //    Label1.Text = "已连接...";
            //}
            //else
            //{
            //    Label1.Text = "未连接 !!";
            //}
            //Label2.Text = "连接数: " + (ServerList.Count).ToString();
            //LbUserCount.Text = (UsrSoc.FrmUserSoc.GetUserCount()).ToString();
            //if (DBShare.boOpenDBBusy)
            //{
            //    if (DBShare.n4ADB18 > 0)
            //    {
            //        if (!DBShare.bo4ADB1C)
            //        {
            //            Label4.Text = "[1/4] " + (Math.Round((DBShare.n4ADB10 / DBShare.n4ADB18) * 1.0e2)).ToString() + "% " + (DBShare.n4ADB14).ToString() + "/" + (DBShare.n4ADB18).ToString();
            //        }
            //    }
            //    if (DBShare.n4ADB04 > 0)
            //    {
            //        if (!DBShare.boHumDBReady)
            //        {
            //            Label4.Text = "[3/4] " + (Math.Round((DBShare.n4ADAFC / DBShare.n4ADB04) * 1.0e2)).ToString() + "% " + (DBShare.n4ADB00).ToString() + "/" + (DBShare.n4ADB04).ToString();
            //        }
            //    }
            //    if (DBShare.n4ADAF0 > 0)
            //    {
            //        if (!DBShare.boDataDBReady)
            //        {
            //            Label4.Text = "[4/4] " + (Math.Round((DBShare.n4ADAE4 / DBShare.n4ADAF0) * 1.0e2)).ToString() + "% " + (DBShare.n4ADAE8).ToString() + "/" + (DBShare.n4ADAEC).ToString() + "/" + (DBShare.n4ADAF0).ToString();
            //        }
            //    }
            //}
            //LbAutoClean.Text = (DBShare.g_nClearIndex).ToString() + "/(" + (DBShare.g_nClearCount).ToString() + "/" + (DBShare.g_nClearItemIndexCount).ToString() + ")/" + (DBShare.g_nClearRecordCount).ToString();
            //Label8.Text = "H-QyChr=" + (DBShare.g_nQueryChrCount).ToString();
            //Label9.Text = "H-NwChr=" + (DBShare.nHackerNewChrCount).ToString();
            //Label10.Text = "H-DlChr=" + (DBShare.nHackerDelChrCount).ToString();
            //Label11.Text = "Dubb-Sl=" + (DBShare.nHackerSelChrCount).ToString();
        }

        public void FormCreate(System.Object Sender, System.EventArgs _e1)
        {
            FileStream Conf;
            int nX;
            int nY;
            int nCode;
            m_boRemoteClose = false;
            DBShare.boOpenDBBusy = true;
            //Label4.Text = "";
            //LbAutoClean.Text = "-/-";
            //HumDB.HumChrDB = null;
            //HumDB = null;
            DBShare.LoadConfig();
            ServerList = new List<TServerInfo>();
            HumSessionList = new List<THumSession>();
            //Label5.Text = "FDB: " + DBShare.sDataDBFilePath + "Mir.DB  " + "Backup: " + DBShare.sBackupPath;
            n334 = 0;
            //ServerSocket.Address = DBShare.sServerAddr;
            //ServerSocket.Port = DBShare.nServerPort;
            //ServerSocket.Active = true;
            DBShare.n4ADBF4 = 0;
            DBShare.n4ADBF8 = 0;
            DBShare.n4ADBFC = 0;
            DBShare.n4ADC00 = 0;
            DBShare.n4ADC04 = 0;
            n344 = 2;
            n348 = 0;
            DBShare.nHackerNewChrCount = 0;
            DBShare.nHackerDelChrCount = 0;
            DBShare.nHackerSelChrCount = 0;
            DBShare.n4ADC1C = 0;
            DBShare.n4ADC20 = 0;
            DBShare.n4ADC24 = 0;
            DBShare.n4ADC28 = 0;
        }

        public void AniTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            //if (n334 > 7)
            //{
            //    n334 = 0;
            //}
            //else
            //{
            //    n334++;
            //}
            //switch (n334)
            //{
            //    case 0:
            //        Label3.Text = "|";
            //        break;
            //    case 1:
            //        Label3.Text = "/";
            //        break;
            //    case 2:
            //        Label3.Text = "--";
            //        break;
            //    case 3:
            //        Label3.Text = "\\";
            //        break;
            //    case 4:
            //        Label3.Text = "|";
            //        break;
            //    case 5:
            //        Label3.Text = "/";
            //        break;
            //    case 6:
            //        Label3.Text = "--";
            //        break;
            //    case 7:
            //        Label3.Text = "\\";
            //        break;
            //}
        }

        public void StartTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            //StartTimer.Enabled = false;
            //DBShare.boOpenDBBusy = true;
            //HumDB.HumChrDB = new TFileHumDB(DBShare.sHumDBFilePath + "Hum.DB");
            //HumDB = new TFileDB(DBShare.sDataDBFilePath + "Mir.DB");
            //DBShare.boOpenDBBusy = false;
            //DBShare.boAutoClearDB = true;
            //Label4.Text = "";
            //IDSocCli.FrmIDSoc.OpenConnect();
            //DBShare.OutMainMessage("服务器已启动...");
        }

        public void Timer2Timer(System.Object Sender, System.EventArgs _e1)
        {
            int i;
            THumSession HumSession;
            i = 0;
            while (true)
            {
                if (HumSessionList.Count <= i)
                {
                    break;
                }
                HumSession = HumSessionList[i];
                if (!HumSession.bo24)
                {
                    if (HumSession.bo2C)
                    {
                        if ((HUtil32.GetTickCount() - HumSession.dwTick30) > 20 * 1000)
                        {
                            HumSession = null;
                            HumSessionList.RemoveAt(i);
                            continue;
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - HumSession.dwTick30) > 2 * 60 * 1000)
                        {
                            HumSession = null;
                            HumSessionList.RemoveAt(i);
                            continue;
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - HumSession.dwTick30) > 40 * 60 * 1000)
                {
                    HumSession = null;
                    HumSessionList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }

        private void ClearSocket(Socket Socket)
        {
            int nIndex;
            THumSession HumSession;
            nIndex = 0;
            while (true)
            {
                if (HumSessionList.Count <= nIndex)
                {
                    break;
                }
                HumSession = HumSessionList[nIndex];
                if (HumSession.Socket == Socket)
                {
                    HumSession = null;
                    HumSessionList.RemoveAt(nIndex);
                    continue;
                }
                nIndex++;
            }
        }

        public bool CopyHumData(string sSrcChrName, string sDestChrName, string sUserID)
        {
            bool result;
            int n14;
            bool bo15;
            THumDataInfo HumanRCD = null;
            result = false;
            bo15 = false;
            try
            {
                if (HumDB.Open())
                {
                    n14 = HumDB.Index(sSrcChrName);
                    if ((n14 >= 0) && (HumDB.Get(n14, ref HumanRCD) >= 0))
                    {
                        bo15 = true;
                    }
                    if (bo15)
                    {
                        n14 = HumDB.Index(sDestChrName);
                        if ((n14 >= 0))
                        {
                            HumanRCD.Header.sName = sDestChrName;
                            HumanRCD.Data.sChrName = sDestChrName;
                            HumanRCD.Data.sAccount = sUserID;
                            HumDB.Update(n14, ref HumanRCD);
                            result = true;
                        }
                    }
                }
            }
            finally
            {
                HumDB.Close();
            }
            return result;
        }

        public void DelHum(string sChrName)
        {
            //try
            //{
            //    if (HumDB.HumChrDB.Open())
            //    {
            //        HumDB.HumChrDB.Delete(sChrName);
            //    }
            //}
            //finally
            //{
            //    HumDB.HumChrDB.Close();
            //}
        }
    }

    public class TServerInfo
    {
        public int nSckHandle;
        public string sStr;
        public bool bo08;
        public Socket Socket;
    }

    public class THumSession
    {
        public string sChrName;
        public int nIndex;
        public Socket Socket;
        public bool bo24;
        public bool bo2C;
        public long dwTick30;
    }
}

namespace DBSvr
{
    public class DBSMain
    {
        public static TFrmDBSrv FrmDBSrv = null;
    }
}

