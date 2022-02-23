namespace SelGate
{
    public struct TMessageData
    {
        public byte[] Body;
        public int MsgLen => Body.Length;
        public int UserCientId;
    }
}