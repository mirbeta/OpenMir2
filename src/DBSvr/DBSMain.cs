using System;
using System.IO;
using System.Net.Sockets;

namespace DBSvr
{
    public partial class TFrmDBSrv
    {
        private int n334 = 0;
        private int n344 = 0;
        private int n348 = 0;
        private string s34C = String.Empty;
        private bool m_boRemoteClose = false;

        public TFrmDBSrv()
        {

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
