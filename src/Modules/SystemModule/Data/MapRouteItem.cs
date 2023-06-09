namespace SystemModule
{
    /// <summary>
    /// 地图连接对象
    /// </summary>
    public record struct MapRouteItem
    {
        public int RouteId;
        public IEnvirnoment Envir;
        public short X;
        public short Y;
        public bool Flag;
    }
}