using SystemModule;

namespace GameSvr.Maps
{
    /// <summary>
    /// 地图连接
    /// </summary>
    public class GateObject : EntityId
    {
        public Envirnoment DEnvir;
        public short nDMapX;
        public short nDMapY;
        public bool boFlag;
    }
}