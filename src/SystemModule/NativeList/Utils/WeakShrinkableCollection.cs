using System;
using System.Collections;
using System.Collections.Generic;
using SystemModule.NativeList.Abstracts;
using SystemModule.NativeList.Helpers;

namespace SystemModule.NativeList.Utils
{
    /// <summary>
    /// Non-abstract realization of the <see cref="WeakShrinkableCollectionBase{TItem}"/> to use it as is and as composition.
    /// </summary>
    public class WeakShrinkableCollection<TItem> : WeakShrinkableCollectionBase<TItem>, ICollection<TItem> where TItem : class
    {
        private class WeakShrinkableCollectionEnumenator : IEnumerator<TItem>
        {
            private WeakShrinkableCollection<TItem> _collection;
            private int _index;

            public WeakShrinkableCollectionEnumenator(WeakShrinkableCollection<TItem> collection)
            {
                _collection = collection;

                Reset();
            }

            /// <inheritdoc/>
            public TItem Current { get; private set; }

            /// <inheritdoc/>
            object IEnumerator.Current => Current;

            /// <inheritdoc/>
            public bool MoveNext()
            {
                _index++;

                for (; _index < _collection.ElementsBuffer.Length; _index++)
                {
                    if (_collection.IsAliveBuffer[_index])
                    {
                        if (_collection.ElementsBuffer[_index].TryGetTarget(out TItem temp))
                        {
                            Current = temp;

                            return true;
                        }
                        else
                        {
                            _collection.IsAliveBuffer[_index] = false;
                        }
                    }
                }

                return false;
            }

            /// <inheritdoc/>
            public void Reset()
            {
                _collection.TryDecreaseBuffer();

                Current = null;
                _index = -1;
            }

            public void Dispose()
            {
                Reset();
                _collection = null;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// May not correspond to the number of lived elements.
        /// </summary>
        public int Count => ElementsCount;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public void Add(TItem item)
        {
            AddNewItem(item);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            InitializeDefaultMembers();
        }

        /// <inheritdoc/>
        public bool Contains(TItem item)
        {
            return CheckItemExisted(item);
        }

        /// <inheritdoc/>
        public bool Remove(TItem item)
        {
            return RemoveFirstInput(item);
        }

        /// <inheritdoc/>
        public void CopyTo(TItem[] array, int arrayIndex)
        {
            ArgumentsGuard.ThrowIfNull(array);
            ArgumentsGuard.ThrowIfNotInRange(arrayIndex, FlexibleRanges.Create(array));

            var tempCollection = new List<TItem>();

            for (int i = 0; i < ElementsBuffer.Length; i++)
            {
                if (IsAliveBuffer[i])
                {
                    if (ElementsBuffer[i].TryGetTarget(out TItem tempItem))
                        tempCollection.Add(tempItem);
                    else
                        IsAliveBuffer[i] = false;
                }
            }

            TryDecreaseBuffer();

            if (tempCollection.Count > array.Length - arrayIndex)
                throw new ArgumentException("Not enough space to copy the collection.", nameof(array));

            for (int i = 0; i < tempCollection.Count; i++)
                array[arrayIndex++] = tempCollection[i];
        }

        /// <inheritdoc/>
        public IEnumerator<TItem> GetEnumerator()
        {
            return new WeakShrinkableCollectionEnumenator(this);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
