using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SystemModule.NativeList.Helpers;

namespace SystemModule.NativeList.Utils
{
    /// <summary>
    /// Special buffer that was created for multi-thread writing and single reading.
    /// Based on idea about two buffer: front buffer for writing, back buffer for operation.
    /// So when all threads uses front buffer we can manipulate with back buffer 
    /// and then when we need or front buffer is filled completely we can switch them and continue to work.
    /// </summary>
    public class SwapBuffer<TITem>
    {
        private int _concurrentNumber;
        private int _currentIndex;
        private int _lastIndex;
        private int _length;

        private TITem[] _frontBuffer;
        private TITem[] _backBuffer;

        private SemaphoreSlim _semaphore;
        private Object _lock;
        private SpinLock _spinlock;
        private Action<TITem[], int> _bufferExchangeCallback;
        private bool _invokeCallbackAsTask;

        public SwapBuffer(int concurrentNumber, int bufferLength, Action<TITem[], int> callback, bool invokeCallbackAsTask = true)
        {
            ArgumentsGuard.ThrowIfLessOrEqualZero(concurrentNumber);
            ArgumentsGuard.ThrowIfLessOrEqualZero(bufferLength);
            ArgumentsGuard.ThrowIfNull(callback);

            _concurrentNumber = concurrentNumber;
            _currentIndex = 0;
            _length = bufferLength;

            _bufferExchangeCallback = callback;
            _invokeCallbackAsTask = invokeCallbackAsTask;

            _frontBuffer = new TITem[_length];
            _backBuffer = new TITem[_length];

            _semaphore = new SemaphoreSlim(_concurrentNumber);
            _lock = new Object();
            _spinlock = new SpinLock();
        }
        
        /// <summary>
        /// Returns number of filled cells from the front buffer.
        /// </summary>
        public int GetCount()
        {
            //Takes our ticket, that guarantees that
            //other thread will not switch the buffer.
            _semaphore.Wait();

            bool lockTaken = false;

            _spinlock.Enter(ref lockTaken);

            var result = _currentIndex;

            if (lockTaken)
                _spinlock.Exit(false);

            _semaphore.Release();

            return result;
        }

        /// <summary>
        /// Write an entry to the front buffer.
        /// </summary>
        /// <remarks>
        /// If front buffer has no free space will try to swap buffers.
        /// </remarks>
        public void Write(TITem item)
        {
            while (!TryWriteValue(item))
                TryExchangeBuffer();
        }

        /// <summary>
        /// Swaps buffers by user need.
        /// </summary>
        public void ExchangeBuffer()
        {
            //I have to guarantee that no other
            //thread will try to swap buffer
            Monitor.Enter(_lock);

            InternalExchangeBuffer();
        }

        /// <summary>
        /// Tries to write an entry into the front buffer.
        /// If operation is success returns true.
        /// If front buffer does not have any free space returns false.
        /// </summary>
        private bool TryWriteValue(TITem item)
        {
            //Takes our ticket, that guarantees that
            //other thread will not switch the buffer.
            _semaphore.Wait();

            bool lockTaken = false;
            int index = 0;

            //Spin lock much faster that locking
            //And this is the only operation that
            //has to be performed in a complete single access.
            _spinlock.Enter(ref lockTaken);

            if (_currentIndex >= _length)
            {
                //If we have no free index
                //we have to release all our lock 
                //and try to swap buffers.

                if (lockTaken)
                    _spinlock.Exit(false);

                _semaphore.Release();

                return false;
            }
            else
            {
                index = _currentIndex;

                _currentIndex++;
            }

            if (lockTaken)
                _spinlock.Exit(false);

            //We do not have to lock indexing operation
            //because it works with only one cell of the array
            _frontBuffer[index] = item;

            _semaphore.Release();

            return true;
        }

        /// <summary>
        /// Internal function. This is "try" function 
        /// because I suspect that this method can be invoked from several 
        /// threads at the same time, but I do not want to all of them to swap buffers,
        /// so only one thread will actually swap a buffer, and all others has to do nothing after.
        /// </summary>
        private void TryExchangeBuffer()
        {
            //Only one thread allowed
            //I wanted to use spin lock,
            //but I don't know how long it would be.
            //because for the swap object has to block
            //the whole semaphore
            Monitor.Enter(_lock);

            bool lockTaken = false;

            _spinlock.Enter(ref lockTaken);

            if (_currentIndex < _length)
            {
                //In the case when one of threads
                //have swapped the buffer already
                if (lockTaken)
                    _spinlock.Exit(false);

                Monitor.Exit(_lock);

                return;
            }

            if (lockTaken)
                _spinlock.Exit(false);

            InternalExchangeBuffer();
        }

        /// <summary>
        /// Actually swaps the buffers.
        /// </summary>
        private void InternalExchangeBuffer()
        {
            //We have to consume all permissions from the semaphore
            //to prevent using buffers from other threads
            for (int i = 0; i < _concurrentNumber; i++)
                _semaphore.Wait();

            var currentBuffer = _frontBuffer;

            _frontBuffer = _backBuffer;
            _backBuffer = currentBuffer;

            _lastIndex = _currentIndex;

            _currentIndex = 0;

            //When we swapped the buffers we releases
            //almost all thread-sync objects
            Monitor.Exit(_lock);

            //live just one in the locked state
            _semaphore.Release(_concurrentNumber - 1);

            //to invoke the callback and after
            //it work release the last one semaphore
            if(_invokeCallbackAsTask)
                Task.Factory.StartNew(InvokeCallback);
            else
                InvokeCallback();
        }

        /// <summary>
        /// Invokes callback in the safe environment
        /// </summary>
        private void InvokeCallback()
        {
            try
            {
                _bufferExchangeCallback.Invoke(_backBuffer, _lastIndex);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                Trace.WriteLine(exception.StackTrace);
            }
            finally
            {
                //This is to prevent buffer swapping when
                //callback works with back buffer
                _semaphore.Release();
            }
        }
    }
}
