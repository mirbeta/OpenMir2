namespace LoginGate.Packet
{
    public struct MessageData
    {
        public int ConnectionId;
        public string ClientIP;
        public byte[] Body;
        public int MsgLen => Body.Length;
    }
}