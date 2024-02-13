using OpenMir2.NativeList.Enums;
using OpenMir2.NativeList.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenMir2.NativeList.Utils
{
    /// <summary>
    /// Structure that makes possible to generate a set of values automatically.
    /// </summary>
    public struct FlexibleSet<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// Just a structure for enumeration to make it as quick as possible. 
        /// </summary>
        public struct Enumerator : IEnumerator<TItem>
        {
            private readonly Func<TItem, TItem> _stepping;
            private FlexibleRange<TItem> _range;
            private readonly Equality _sidesEquality;

            private bool _started;

            public Enumerator(FlexibleSet<TItem> set)
            {
                _range = set.Range;
                _sidesEquality = _range.Comparer.Compare(_range.Left, _range.Right);

                if (_sidesEquality == Equality.Equal)
                {
                    throw new Exception("Cannot enumerate range when left and right sides are equal.");
                }

                _started = false;
                _stepping = set._stepping;

                Current = default(TItem);
            }

            public TItem Current { get; private set; }

            Object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (!_started)
                {
                    if (_range.IsLeftStrictly)
                    {
                        Current = _range.Left;
                    }
                    else
                    {
                        Current = _stepping.Invoke(_range.Left);
                    }

                    _started = true;

                    return true;
                }
                else
                {
                    Current = _stepping.Invoke(Current);
                }

                Equality compareResult = _range.Comparer.Compare(Current, _range.Right);

                if (_range.IsRightStrictly)
                {
                    return compareResult == Equality.Equal || compareResult == _sidesEquality;
                }
                else
                {
                    return compareResult == _sidesEquality;
                }
            }

            public void Reset()
            { /*DO NOTHING*/ }
            public void Dispose()
            { /*DO NOTHING*/ }
        }

        private readonly Func<TItem, TItem> _stepping;

        public FlexibleSet(FlexibleRange<TItem> range, Func<TItem, TItem> stepping)
        {
            ArgumentsGuard.ThrowIfNull(stepping);

            Range = range;

            _stepping = stepping;
        }

        /// <summary>
        /// Range for values generation.
        /// </summary>
        public FlexibleRange<TItem> Range { get; private set; }

        /// <summary>
        /// Returns structure for items enumeration.
        /// </summary>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Was made for LINQ compatibility.
        /// </remarks>
        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Was made for LINQ compatibility.
        /// </remarks>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}