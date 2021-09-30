using System.Threading;

namespace DBSvr
{
    public class ServerApp
    {

        public ServerApp()
        {

        }

        public void Start()
        {

        }

        public void StartService()
        {
            DBShare.Initialization();
            DBShare.LoadConfig();
            DBShare.nHackerNewChrCount = 0;
            DBShare.nHackerDelChrCount = 0;
            DBShare.nHackerSelChrCount = 0;
            DBShare.n4ADC1C = 0;
            DBShare.n4ADC20 = 0;
            DBShare.n4ADC24 = 0;
            DBShare.n4ADC28 = 0;
            DBShare.MainOutMessage("服务器已启动...");
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