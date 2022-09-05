namespace SystemModule.RandomSelector
{
    public class WeightedItem<T>
    {
        /// <summary>
        /// 权重
        /// </summary>
        public int Weight;

        /// <summary>
        /// 元素
        /// </summary>
        public readonly T Value;

        /// <summary>
        /// 累计权重
        /// </summary>
        protected int CumulativeWeight;

        public WeightedItem(T value, int weight)
        {
            Value = value;
            Weight = weight;
            CumulativeWeight = 0;
        }
    }
}