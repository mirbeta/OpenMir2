using System.Threading;

namespace DBSvr
{
    public class ServerApp
    {
        private string s34C = string.Empty;
        private bool m_boRemoteClose = false;
        private Timer logTimer;

        public ServerApp(TFrmUserSoc userSoc)
        {

        }

        public void Start()
        {
            m_boRemoteClose = false;
            DBShare.boOpenDBBusy = true;
            DBShare.Initialization();
            DBShare.LoadConfig();
            DBShare.nHackerNewChrCount = 0;
            DBShare.nHackerDelChrCount = 0;
            DBShare.nHackerSelChrCount = 0;
            DBShare.n4ADC1C = 0;
            DBShare.n4ADC20 = 0;
            DBShare.n4ADC24 = 0;
            DBShare.n4ADC28 = 0;
        }

        private void StateTimer(object obj)
        {
            // LbTransCount.Text = (n348).ToString();
            // n348 = 0;
            // if (ServerList.Count > 0)
            // {
            //     Label1.Text = "已连接...";
            // }
            // else
            // {
            //     Label1.Text = "未连接!!";
            // }
            // Label2.Text = "连接数: " + (ServerList.Count).ToString();
            // LbUserCount.Text = (UsrSoc.FrmUserSoc.GetUserCount()).ToString();
            // if (DBShare.boOpenDBBusy)
            // {
            //     if (DBShare.n4ADB18 > 0)
            //     {
            //         if (!DBShare.bo4ADB1C)
            //         {
            //             Label4.Text = "[1/4] " + (Math.Round((DBShare.n4ADB10 / DBShare.n4ADB18) * 1.0e2)).ToString() + "% " + (DBShare.n4ADB14).ToString() + "/" + (DBShare.n4ADB18).ToString();
            //         }
            //     }
            //     if (DBShare.n4ADB04 > 0)
            //     {
            //         if (!DBShare.boHumDBReady)
            //         {
            //             Label4.Text = "[3/4] " + (Math.Round((DBShare.n4ADAFC / DBShare.n4ADB04) * 1.0e2)).ToString() + "% " + (DBShare.n4ADB00).ToString() + "/" + (DBShare.n4ADB04).ToString();
            //         }
            //     }
            //     if (DBShare.n4ADAF0 > 0)
            //     {
            //         if (!DBShare.boDataDBReady)
            //         {
            //             Label4.Text = "[4/4] " + (Math.Round((DBShare.n4ADAE4 / DBShare.n4ADAF0) * 1.0e2)).ToString() + "% " + (DBShare.n4ADAE8).ToString() + "/" + (DBShare.n4ADAEC).ToString() + "/" + (DBShare.n4ADAF0).ToString();
            //         }
            //     }
            // }
            // LbAutoClean.Text = (DBShare.g_nClearIndex).ToString() + "/(" + (DBShare.g_nClearCount).ToString() + "/" + (DBShare.g_nClearItemIndexCount).ToString() + ")/" + (DBShare.g_nClearRecordCount).ToString();
            // Label8.Text = "H-QyChr=" + (DBShare.g_nQueryChrCount).ToString();
            // Label9.Text = "H-NwChr=" + (DBShare.nHackerNewChrCount).ToString();
            // Label10.Text = "H-DlChr=" + (DBShare.nHackerDelChrCount).ToString();
            // Label11.Text = "Dubb-Sl=" + (DBShare.nHackerSelChrCount).ToString();
        }

        public void StartService()
        {
            //StartTimer.Enabled = false;
            //DBShare.boOpenDBBusy = true;
            //HumDB.HumChrDB = new TFileHumDB(DBShare.sHumDBFilePath + "Hum.DB");
            //HumDB = new TFileDB(DBShare.sDataDBFilePath + "Mir.DB");
            //DBShare.boOpenDBBusy = false;
            //DBShare.boAutoClearDB = true;
            //IDSocCli.FrmIDSoc.OpenConnect();
            logTimer = new Timer(StateTimer, null, 1000, 10000);
            DBShare.OutMainMessage("服务器已启动...");
        }

        public void StopService()
        {
            
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
}