using MemoryPack;

namespace SystemModule.Data
{
    [MemoryPackable]
    public partial class RecordHeader
    {
        public string sAccount { get; set; }
        public string Name { get; set; }
        public int SelectID { get; set; }
        public double dCreateDate { get; set; }
        public bool Deleted { get; set; }
        public double UpdateDate { get; set; }
        public double CreateDate { get; set; }
    }
}