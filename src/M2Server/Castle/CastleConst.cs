namespace M2Server.Castle
{
    /// <summary>
    /// 守卫
    /// </summary>
    public struct ArcherUnit
    {
        public short nX;
        public short nY;
        public string sName;
        public bool nStatus;
        public ushort nHP;
        public int BaseObject;
    }

    public struct AttackerInfo
    {
        public DateTime AttackDate;
        public string GuildName;
        public GuildInfo Guild;
    }
}