namespace LoginGate.Packet
{
    public struct MessageData
    {
        public string ConnectionId;
        public byte[] Body;
        public int MsgLen => Body.Length;
    }
}