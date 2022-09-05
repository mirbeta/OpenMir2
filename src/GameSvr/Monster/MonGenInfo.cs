using GameSvr.Actor;

namespace GameSvr.Monster
{
    public class MonGenInfo
    {
        public string sMapName;
        public int nX;
        public int nY;
        public string sMonName;
        public int nRange;
        public int nCount;
        public int nActiveCount;
        public int dwZenTime;
        public int nMissionGenRate;
        public IList<BaseObject> CertList;
        public int CertCount;
        public object Envir;
        public int nRace;
        /// <summary>
        /// ��ʼˢ��ʱ��
        /// </summary>
        public int dwStartTick;
    }
}