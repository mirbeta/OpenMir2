namespace ConsoleApp1
{
    public class ByteWave : HighPerfArray<byte>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="len"></param>
        public ByteWave(int len) : base(len)
        {

        }

        /// <summary>
        /// 克隆本数组
        /// </summary>
        /// <returns></returns>
        public new ByteWave Clone()
        {
            var cloneData = new ByteWave(Length);
            for (int i = 0; i < Length; i++)
            {
                cloneData.SetValue(i, Memory.Span[i]);
            }
            return cloneData;
        }
    }
}