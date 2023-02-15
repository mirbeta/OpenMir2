using System;
using System.Runtime.CompilerServices;

namespace SystemModule.NativeList.Abstracts
{
    /// <summary>
    /// Basic class for shrinkable buffer, 
    /// I made this is as a separate class to have an ability to use it as a basic class,
    /// because the basic functionality works better than composition.
    /// </summary>
    public abstract class ShrinkableCollectionBase<TItem>
    {
        private const byte DecreaseTryingMax = 8;
        
        private TItem[] _buffer;
        private bool[] _isAlive;
        private int _count;
        private int _firstEmptyPosition;
        private byte _decreaseTrying;

        public ShrinkableCollectionBase()
        {
            InitializeDefaultMembers();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FindFirstEmptyPosition()
        {
            while (_firstEmptyPosition < _buffer.Length && _isAlive[_firstEmptyPosition])
                _firstEmptyPosition++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void IncreaseBuffer()
        {
            Array.Resize(ref _buffer, _buffer.Length * 2);
            Array.Resize(ref _isAlive, _buffer.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool NeedToDecreaseBuffer()
        {
            return _buffer.Length > 1 && _count <= _buffer.Length / 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void TryDecreaseBuffer()
        {
            if (_decreaseTrying < DecreaseTryingMax)
            {
                _decreaseTrying++;
            }
            else
            {
                _decreaseTrying = 0;

                var newBuffer = new TItem[_buffer.Length / 2];
                var newIsAlive = new bool[newBuffer.Length];

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
        /// Returns current items number, does not correspond the length of the buffer.
        /// </summary>
        protected int ElementsCount => _count;

        /// <summary>
        /// Actual buffer.
        /// </summary>
        protected TItem[] ElementsBuffer => _buffer;

        /// <summary>
        /// Buffer that marks what item is available.
        /// </summary>
        protected bool[] IsAliveBuffer => _isAlive;

        /// <summary>
        /// Adds a new item on the first empty position.
        /// </summary>
        /// <remarks>
        /// Will increase the buffer if it is full.
        /// </remarks>
        protected void AddNewItem(TItem item)
        {
            if (_firstEmptyPosition == _buffer.Length)
                IncreaseBuffer();

            _buffer[_firstEmptyPosition] = item;
            _isAlive[_firstEmptyPosition] = true;
            _count++;

            FindFirstEmptyPosition();
        }

        /// <summary>
        /// Return first input of the input element.
        /// </summary>
        protected bool CheckItemExisted(TItem item)
        {
            bool result = false;

            for (int i = 0; i < _buffer.Length; i++)
            {
                if (!_isAlive[i])
                    continue;

                if (Object.Equals(_buffer[i], item))
                {
                    result = true;

                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Removes first input. Can shrink the collection if finds out that the collection can be half smaller.
        /// </summary>
        protected bool RemoveFirstInput(TItem item)
        {
            bool result = false;

            int newEmptyPosition = -1;

            for (int i = 0; i < _buffer.Length; i++)
            {
                if (!_isAlive[i])
                    continue;

                if (Object.Equals(_buffer[i], item))
                {
                    _buffer[i] = default;

                    _isAlive[i] = false;

                    if (newEmptyPosition == -1)
                        newEmptyPosition = i;

                    _count--;

                    result = true;

                    break;
                }
            }

            if (newEmptyPosition != -1 && newEmptyPosition < _firstEmptyPosition)
                _firstEmptyPosition = newEmptyPosition;

            if (result && NeedToDecreaseBuffer())
                TryDecreaseBuffer();

            return result;
        }

        /// <summary>
        /// Sets all members to default.
        /// </summary>
        protected void InitializeDefaultMembers()
        {
            _buffer = new TItem[1];
            _isAlive = new bool[1];

            _count = 0;
            _firstEmptyPosition = 0;
            _decreaseTrying = 0;
        }
    }
}
