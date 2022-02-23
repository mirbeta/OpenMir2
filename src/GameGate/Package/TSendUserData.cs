namespace GameGate
{
    public class TSendUserData
    {
        public byte[] Buffer;
        public int BufferLen => Buffer.Length;
        public int UserCientId;
    }
}