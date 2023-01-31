using GameSvr.Maps;
using NLog;
using System.Text;

namespace GameSvr
{
    public class MapPipeClient
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly NamePipeClient[] pipeClientMaps = new NamePipeClient[100];

        private void PipeStream()
        {
            try
            {
                _logger.Info("Connecting to MapSvr...");
                for (int i = 0; i < pipeClientMaps.Length; i++)
                {
                    pipeClientMaps[i] = new NamePipeClient();
                    pipeClientMaps[i].Connect();
                }
            }
            catch (Exception)
            {
                throw new Exception("链接地图服务器初始化失败,请确认MapSvr服务端程序是否运行.");
            }
        }

        public static MapCellInfo GetCellInfo(int nX, int nY, ref bool success)
        {
            int ram = RandomNumber.GetInstance().Random(pipeClientMaps.Length);
            pipeClientMaps[ram].SendPipeMessage(Encoding.UTF8.GetBytes("123"));
            pipeClientMaps[ram].Close();
            return null;
        }
    }
}