namespace SystemModule.Data
{
    public record struct StartPoint
    {
        public string MapName;
        public short CurrX;
        public short CurrY;
        public bool NotAllowSay;
        public int Range;
        public int Type;
        public int PkZone;
        public int PkFire;
        public byte Shape;
    }
}