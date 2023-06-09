using System;
using System.Runtime.CompilerServices;

namespace SystemModule.NativeList.Abstracts
{
    /// <summary>
    /// Basic class for shrinkable buffer based on weak references, 
    /// I made this is as a separate class to have an ability to use it as a basic class,
    /// because the basic functionality works better than composition.
    /// </summary>
    /// <typeparam name="TItem">Has to be a class instance.</typeparam>
    public abstract class WeakShrinkableCollectionBase<TItem> where TItem : class
    {
        private const byte DecreaseTryingMax = 8;

        private WeakReference<TItem>[] _buffer;
        private bool[] _isAlive;
        private int _count;
        private int _firstEmptyPosition;
        private byte _decreaseTrying;

        public WeakShrinkableCollectionBase()
        {
            InitializeDefaultMembers();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FindFirstEmptyPosition()
        {
            while (_firstEmptyPosition < _buffer.Length && _isAlive[_firstEmptyPosition])
            {
                _firstEmptyPosition++;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void IncreaseBuffer()
        {
            Array.Resize(ref _buffer, _buffer.Length * 2);
            Array.Resize(ref _isAlive, _buffer.Length);
        }

        /// <summary>
        /// Returns current items number, does not correspond the length of the buffer.
        /// </summary>
        protected int ElementsCount => _count;

        /// <summary>
        /// Actual buffer.
        /// </summary>
        protected WeakReference<TItem>[] ElementsBuffer => _buffer;

        /// <summary>
        /// Buffer that marks what item is available.
        /// </summary>
        protected bool[] IsAliveBuffer => _isAlive;

        /// <summary>
        /// Returns true if buffer can be half smaller.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool NeedToDecreaseBuffer()
        {
            return _buffer.Length > 1 && _count <= _buffer.Length / 2;
        }

        /// <summary>
        /// Tries to decrease buffer, will not decrease the buffer for the first several times, 
        /// just to avoid the situation when we have to increase the buffer right after we decreased it.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void TryDecreaseBuffer()
        {
            if (_decreaseTrying < DecreaseTryingMax)
            {
                _decreaseTrying++;
            }
            else
            {
                _decreaseTrying = 0;

                WeakReference<TItem>[] newBuffer = new WeakReference<TItem>[_buffer.Length / 2];
                bool[] newIsAlive = new bool[newBuffer.Length];

                for (int x = 0, y = 0; x < _buffer.Length; x++)
                {
                    if (_isAlive[x])
                    {
                        newBuffer[y] = _buffer[x];
                        newIsAlive[y] = true;

                        y++;
                    }
                }

                _firstEmptyPosition = _count;
                _buffer = newBuffer;
                _isAlive = newIsAlive;
            }
        }

        /// <summary>
        /// Adds a new item on the first empty position.
        /// </summary>
        /// <remarks>
        /// Will increase the buffer if it is full.
        /// </remarks>
        protected void AddNewItem(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (_firstEmptyPosition == _buffer.Length)
            {
                IncreaseBuffer();
            }

            if (_buffer[_firstEmptyPosition] == null)
            {
                _buffer[_firstEmptyPosition] = new WeakReference<TItem>(item);
            }
            else
            {
                _buffer[_firstEmptyPosition].SetTarget(item);
            }

            _isAlive[_firstEmptyPosition] = true;
            _count++;

            FindFirstEmptyPosition();
        }

        /// <summary>
        /// Return first input of the input element.
        /// </summary>
        protected bool CheckItemExisted(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            bool result = false;

            int newEmptyPosition = -1;
            TItem tempItem;

            for (int i = 0; i < _buffer.Length; i++)
            {
                if (!_isAlive[i])
                {
                    continue;
                }

                if (!_buffer[i].TryGetTarget(out tempItem))
                {
                    _isAlive[i] = false;

                    if (newEmptyPosition == -1)
                    {
                        newEmptyPosition = i;
                    }

                    _count--;
                }
                else if (tempItem == item)
                {
                    result = true;

                    break;
                }
            }

            if (newEmptyPosition != -1 && newEmptyPosition < _firstEmptyPosition)
            {
                _firstEmptyPosition = newEmptyPosition;
            }

            return result;
        }

        /// <summary>
        /// Removes first input. Can shrink the collection if finds out that the collection can be half smaller.
        /// </summary>
        protected bool RemoveFirstInput(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            bool result = false;

            int newEmptyPosition = -1;
            TItem tempItem;

            for (int i = 0; i < _buffer.Length; i++)
            {
                if (!_isAlive[i])
                {
                    continue;
                }

                if (!_buffer[i].TryGetTarget(out tempItem))
                {
                    _isAlive[i] = false;

                    if (newEmptyPosition == -1)
                    {
                        newEmptyPosition = i;
                    }

                    _count--;
                }
                else if (tempItem == item)
                {
                    _isAlive[i] = false;

                    if (newEmptyPosition == -1)
                    {
                        newEmptyPosition = i;
                    }

                    _count--;

                    result = true;

                    break;
                }
            }

            if (newEmptyPosition != -1 && newEmptyPosition < _firstEmptyPosition)
            {
                _firstEmptyPosition = newEmptyPosition;
            }

            if (result && NeedToDecreaseBuffer())
            {
                TryDecreaseBuffer();
            }

            return result;
        }

        /// <summary>
        /// Sets all members to default.
        /// </summary>
        protected void InitializeDefaultMembers()
        {
            _buffer = new WeakReference<TItem>[1];
            _isAlive = new bool[1];

            _count = 0;
            _firstEmptyPosition = 0;
            _decreaseTrying = 0;
        }
    }
}