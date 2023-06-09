namespace SystemModule.Data
{
    /// <summary>
    /// 怪物掉落物品
    /// </summary>
    public record struct MonsterDropItem
    {
        public int MaxPoint;
        public int SelPoint;
        public string ItemName;
        public int Count;
    }
}