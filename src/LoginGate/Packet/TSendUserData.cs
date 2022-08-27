namespace LoginGate.Packet
{
    public struct TMessageData
    {
        public string ConnectionId;
        public byte[] Body;
        public int MsgLen => Body.Length;
    }
}