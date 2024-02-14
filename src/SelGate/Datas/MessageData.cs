namespace SelGate.Datas
{
    public struct MessageData
    {
        public string SessionId;

        public byte[] Body;
        public int MsgLen => Body.Length;
    }
}