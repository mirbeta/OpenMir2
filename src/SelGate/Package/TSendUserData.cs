namespace SelGate.Package
{
    public struct TMessageData
    {
        public string SessionId;

        public byte[] Body;
        public int MsgLen => Body.Length;
    }
}