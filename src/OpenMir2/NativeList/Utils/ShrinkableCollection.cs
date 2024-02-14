using OpenMir2.NativeList.Abstracts;
using OpenMir2.NativeList.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenMir2.NativeList.Utils
{
    /// <summary>
    /// Non-abstract realization of the <see cref="ShrinkableCollectionBase{TItem}"/> to use it as is and as composition.
    /// </summary>
    public class ShrinkableCollection<TItem> : ShrinkableCollectionBase<TItem>, ICollection<TItem>
    {
        private class ShrinkableCollectionEnumenator : IEnumerator<TItem>
        {
            private ShrinkableCollection<TItem> _collection;
            private int _index;

            public ShrinkableCollectionEnumenator(ShrinkableCollection<TItem> collection)
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
                        Current = _collection.ElementsBuffer[_index];

                        return true;
                    }
                }

                return false;
            }

            /// <inheritdoc/>
            public void Reset()
            {
                Current = default;
                _index = -1;
            }

            public void Dispose()
            {
                Reset();

                _collection = null;
            }
        }

        /// <inheritdoc/>
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

            if (ElementsCount > array.Length - arrayIndex)
            {
                throw new ArgumentException("Not enough space to copy the collection.", nameof(array));
            }

            for (int i = 0; i < ElementsBuffer.Length && arrayIndex < array.Length; i++)
            {
                if (ElementsBuffer[i] != null)
                {
                    array[arrayIndex++] = ElementsBuffer[i];
                }
            }
        }

        /// <inheritdoc/>
        public IEnumerator<TItem> GetEnumerator()
        {
            return new ShrinkableCollectionEnumenator(this);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}