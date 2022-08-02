namespace LoginGate
{
    public struct TMessageData
    {
        public int MessageId;

        public byte[] Body;

        public int MsgLen => Body.Length;
    }
}