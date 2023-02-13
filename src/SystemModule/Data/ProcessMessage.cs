namespace SystemModule.Data
{
    public record class ProcessMessage
    {
        public int wIdent;
        public int wParam;
        public int nParam1;
        public int nParam2;
        public int nParam3;
        public int DeliveryTime;
        public int BaseObject;
        public bool LateDelivery;
        public string Msg;
    }
}