namespace SystemModule.Data
{
    public record struct SendMessage
    {
        public int wIdent;
        public int wParam;
        public int nParam1;
        public int nParam2;
        public int nParam3;
        /// <summary>
        /// 延时时间
        /// </summary>
        public int DeliveryTime;
        /// <summary>
        /// 对象唯一ID
        /// </summary>
        public int ActorId;
        public bool LateDelivery;
        public string Buff;
    }
}
