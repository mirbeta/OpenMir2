using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemModule.RandomSelector
{
    public abstract class SelectorBase<T>
    {
        protected readonly WeightedSelector<T> WeightedSelector;

        protected SelectorBase(WeightedSelector<T> weightedSelector)
        {
            WeightedSelector = weightedSelector;
        }

        /// <summary>
        /// 执行二进制筛选
        /// </summary>
        protected WeightedItem<T> BinarySelect(List<WeightedItem<T>> items)
        {
            if (items.Count == 0)
            {
                throw new InvalidOperationException("没有元素可以筛选");
            }

            int index = Array.BinarySearch(WeightedSelector.CumulativeWeights, new Random().Next(1, items.Sum(i => i.Weight) + 1));
            //如果存在接近的匹配项，二进制搜索返回的负数会比搜索的第1个索引少1。
            if (index < 0)
            {
                index = -index - 1;
            }

            return items[index];
        }

        /// <summary>
        /// 线性筛选
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected WeightedItem<T> LinearSelect(List<WeightedItem<T>> items)
        {
            // 只对具有允许重复项的多选功能有用，它会随着时间从列表中删除项目。 在这些条件下没有消耗更多性能让二进制搜索起作用。
            if (!items.Any())
            {
                throw new InvalidOperationException("没有元素可以筛选");
            }

            int count = 0;
            int seed = new Random().Next(1, items.Sum(i => i.Weight) + 1);
            foreach (WeightedItem<T> item in items)
            {
                count += item.Weight;
                if (seed <= count)
                {
                    return item;
                }
            }

            return items.FirstOrDefault();
        }
    }
}