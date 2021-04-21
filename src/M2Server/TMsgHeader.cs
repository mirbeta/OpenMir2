namespace M2Server
{
    public struct TMsgHeader
    {
        public uint dwCode;
        public int nSocket;
        public short wGSocketIdx;
        public short wIdent;
        public short wUserListIndex;
        public int nLength;
    }
}