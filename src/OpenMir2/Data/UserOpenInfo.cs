using OpenMir2.Packets.ServerPackets;

namespace OpenMir2.Data
{
    public class UserOpenInfo
    {
        /// <summary>
        /// 查询Id
        /// </summary>
        public int QueryId;
        /// <summary>
        /// 失败次数
        /// </summary>
        public int FailCount;
        public string ChrName;
        public LoadDBInfo LoadUser;
        public CharacterDataInfo HumanRcd;
    }
}