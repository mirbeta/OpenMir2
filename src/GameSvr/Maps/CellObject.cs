namespace GameSvr.Maps
{
    /// <summary>
    /// 地图上的对象
    /// </summary>
    public class CellObject
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int CellObjId;
        /// <summary>
        /// Cell对象类型
        /// </summary>
        public CellType CellType;
        /// <summary>
        /// 添加时间
        /// </summary>
        public int AddTime;
        /// <summary>
        /// 对象释放已释放
        /// </summary>
        public bool ObjectDispose;

        /// <summary>
        /// 所属线程
        /// </summary>
        private int SpawnThread;
        
        public LinkedListNode<CellObject> Node;
        public LinkedListNode<CellObject> NodeThreaded;

        public void Add()
        {
            /*Node = Envir.Objects.AddLast(this);
            if (CellType == CellType.MovingObject)
            {
                SpawnThread = CurrentMap.Thread;
                NodeThreaded = Envir.MobThreads[SpawnThread].ObjectsList.AddLast(this);
            }*/
        }

        public void Remove()
        {
            /*Envir.Objects.Remove(Node);
            if (CellType == CellType.MovingObject)
            {
                Envir.MobThreads[SpawnThread].ObjectsList.Remove(NodeThreaded);
            }*/
        }

    }
}