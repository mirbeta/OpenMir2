using System;
using System.Collections.Generic;

namespace SystemModule.RandomSelector
{
    /// <summary>
    /// 多选器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultipleSelector<T> : SelectorBase<T>
    {
        public MultipleSelector(WeightedSelector<T> weightedSelector) : base(weightedSelector)
        {
            
        }

        public List<T> Select(int count)
        {
            Validate(ref count);
            var items = new List<WeightedItem<T>>(WeightedSelector.Items);
            var resultList = new List<T>();

            do
            {
                var item = WeightedSelector.Option.AllowDuplicate ? BinarySelect(items) : LinearSelect(items);
                resultList.Add(item.Value);
                if (!WeightedSelector.Option.AllowDuplicate)
                {
                    items.Remove(item);
                }
            } while (resultList.Count < count);
            return resultList;
        }

        private void Validate(ref int count)
        {
            if (count <= 0)
            {
                throw new InvalidOperationException("筛选个数必须大于0");
            }

            var items = WeightedSelector.Items;

            if (items.Count == 0)
            {
                throw new InvalidOperationException("没有元素可以被筛选");
            }

            if (!WeightedSelector.Option.AllowDuplicate && items.Count < count)
            {
                count = items.Count;
            }
        }
    }
}