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
            List<WeightedItem<T>> items = new List<WeightedItem<T>>(WeightedSelector.Items);
            List<T> resultList = new List<T>();

            do
            {
                WeightedItem<T> item = WeightedSelector.Option.AllowDuplicate ? BinarySelect(items) : LinearSelect(items);
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

            List<WeightedItem<T>> items = WeightedSelector.Items;

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