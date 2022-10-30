namespace LoginGate.Packet
{
    public struct MessageData
    {
        public int ConnectionId;
        public byte[] Body;
        public int MsgLen => Body.Length;
    }
}