using System.Buffers;

namespace ConsoleApp1
{
    public class HighPerfArray<T> : IDisposable
    {
        /// <summary>
        /// 数组池
        /// </summary>
        protected ArrayPool<T> ArrayPool { get; private set; }

        /// <summary>
        /// 源数组
        /// </summary>
        protected T[] SourceArray { get; private set; }

        /// <summary>
        /// 可操作数组
        /// </summary>
        public Memory<T> Memory { get; protected set; }

        /// <summary>
        /// 真实长度
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="len">数组长度</param>
        public HighPerfArray(int len)
        {
            ArrayPool = ArrayPool<T>.Shared;
            Init(len);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pool">数组池</param>
        /// <param name="len">长度</param>
        public HighPerfArray(ArrayPool<T> pool, int len)
        {
            ArrayPool = pool;
            Init(len);
        }

        protected virtual void Init(int len)
        {
            SourceArray = ArrayPool.Rent(len);
            Memory = SourceArray.AsMemory(0, len);
            Memory.Span.Clear(); // 保证这是一个清空了的数组
            InitLength();
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void SetValue(int index, T data)
        {
            Memory.Span[index] = data;
        }

        /// <summary>
        /// 切割
        /// </summary>
        /// <param name="start"></param>
        /// <param name="len"></param>
        public void Slice(int start, int len)
        {
            Memory = Memory.Slice(start, len);
            InitLength();
        }

        /// <summary>
        /// 克隆本数组
        /// </summary>
        /// <returns></returns>
        public HighPerfArray<T> Clone()
        {
            var cloneData = new HighPerfArray<T>(ArrayPool, Length);
            for (int i = 0; i < Length; i++)
            {
                cloneData.SetValue(i, Memory.Span[i]);
            }

            return cloneData;
        }

        /// <summary>
        /// 是否已经销毁
        /// </summary>
        public bool IsDispose { get; private set; }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!IsDispose)
            {
                // 回收数组
                ArrayPool.Return(SourceArray, false);
                SourceArray = null;

                // 内存置空
                Memory = null;
                IsDispose = true;
            }
        }

        ~HighPerfArray()
        {
            Dispose(); // GC回收的时候手动释放资源
        }

        private void InitLength()
        {
            Length = Memory.Length;
        }

    }
}