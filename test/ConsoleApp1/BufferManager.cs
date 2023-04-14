using System.Buffers;
using System.Diagnostics;

namespace ConsoleApp1
{

    public class BufferManager
    {
        private static readonly ArrayPool<byte> _pool = ArrayPool<byte>.Shared;

        public IArrayOwner<byte> GetBuffer(int size)
        {
            return _pool.RentArrayOwner(size);
        }

        public void ReturnBuffer(IArrayOwner<byte> buffer)
        {
            _pool.Return(buffer.Array);
        }
    }

    /// <summary>
    /// 定义数组持有者的接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IArrayOwner<T> : IDisposable
    {
        /// <summary>
        /// 获取数据有效数据长度
        /// </summary>
        int Length { get; }

        /// <summary>
        /// 获取持有的数组
        /// </summary>
        T[] Array { get; } 
    }
    
    /// <summary>
    /// 表示共享的数组池
    /// </summary>
    public static class ArrayPool
    {
        /// <summary>
        /// 租赁数组
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="length">有效数据长度</param>
        /// <returns></returns>
        public static IArrayOwner<T> Rent<T>(int length)
        {
            return ArrayPool<T>.Shared.RentArrayOwner(length);
        }
    }


    /// <summary>
    /// 提供ArrayPool的扩展
    /// </summary>
    public static class ArrayPoolExtensions
    {
        /// <summary>
        /// 申请可回收的IArrayOwner
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayPool"></param>
        /// <param name="length">有效数据长度</param>
        /// <returns></returns>
        public static IArrayOwner<T> RentArrayOwner<T>(this ArrayPool<T> arrayPool, int length)
        {
            return new ArrayOwner<T>(arrayPool, length);
        }

        /// <summary>
        /// 表示数组持有者
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [DebuggerDisplay("Length = {Length}")]
        private sealed class ArrayOwner<T> : Recyclable, IArrayOwner<T>
        {
            private readonly ArrayPool<T> arrayPool;

            /// <summary>
            /// 获取数据有效数据长度
            /// </summary>
            public int Length { get; }

            /// <summary>
            /// 获取持有的数组
            /// </summary>
            public T[] Array { get; }

            /// <summary>
            /// 数组持有者
            /// </summary>
            /// <param name="arrayPool"></param>
            /// <param name="minLength"></param> 
            public ArrayOwner(ArrayPool<T> arrayPool, int minLength)
            {
                this.arrayPool = arrayPool;
                this.Length = minLength;
                this.Array = arrayPool.Rent(minLength);
            }

            /// <summary>
            /// 归还数组
            /// </summary>
            /// <param name="disposing"></param>
            protected override void Dispose(bool disposing)
            {
                this.arrayPool.Return(this.Array);
            }
        }
    }
    
    /// <summary>
    /// 表示可回收对象的抽象基础类
    /// </summary>
    public abstract class Recyclable : IDisposable
    {
        /// <summary>
        /// 获取对象是否已回收
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// 将对象进行回收
        /// </summary>
        public void Dispose()
        {
            if (this.IsDisposed == false)
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }
            this.IsDisposed = true;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~Recyclable()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// 将对象进行回收
        /// </summary>
        /// <param name="disposing">是否也释放托管资源</param>
        protected abstract void Dispose(bool disposing);
    }
}