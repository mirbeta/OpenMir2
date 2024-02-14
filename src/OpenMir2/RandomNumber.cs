using System;
using System.Collections.Generic;
using System.Text;

namespace OpenMir2
{
    /// <summary>
    /// 生成随机数类
    /// </summary>
    public class RandomNumber
    {
        private static Random random = null;

        //定义一个私有的静态全局变量来保存该类的唯一实例
        private static RandomNumber singleton;
        //定义一个只读静态对象 
        //且这个对象是在程序运行时创建的
        private static readonly object syncObject = new();
        private static readonly char[] Constant =
        {
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
    };
        private RandomNumber() { }

        public static RandomNumber GetInstance()
        {
            if (singleton == null)
            {
                lock (syncObject)
                {
                    if (singleton == null)
                    {
                        random = new Random();
                        singleton = new RandomNumber();
                    }
                }
            }
            return singleton;
        }

        /// <summary>
        /// 从指定列表中随机取出指定个数整数以新列表返回
        /// </summary>
        /// <param name="sourceList">原列表</param>
        /// <param name="selectCount">要选取个数</param>
        /// <returns>新列表</returns>
        public IList<int> RandomSelect(IList<int> sourceList, int selectCount)
        {
            if (selectCount > sourceList.Count)
            {
                throw new ArgumentOutOfRangeException("selectCount必需大于sourceList.Count");
            }

            IList<int> resultList = new List<int>();
            for (int i = 0; i < selectCount; i++)
            {
                int nextIndex = GetRandomNumber(1, sourceList.Count);
                int nextNumber = sourceList[nextIndex - 1];
                sourceList.RemoveAt(nextIndex - 1);
                resultList.Add(nextNumber);
            }
            return resultList;
        }

        /// <summary>
        /// 生成一个整数大于等于最小值，小于等于最大值
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>整数，大于等于最小值，小于等于最大值</returns>
        public int GetRandomNumber(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue + 1);
        }

        /// <summary>
        /// 返回非负随机数。
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public int Random()
        {
            return random.Next();
        }

        /// <summary>
        /// 返回一个小于所指定最大值的非负随机数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Random(int value)
        {
            if (value < int.MaxValue)
            {
                return random.Next(value);
            }
            throw new Exception("错误的数值");
        }

        public string GenerateRandomNumber(int Length)
        {
            StringBuilder newRandom = new(62);
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(Constant[random.Next(62)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 返回一个小于所指定最大值的非负随机数。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte RandomByte(byte value)
        {
            if (value < byte.MaxValue)
            {
                return (byte)random.Next(value);
            }
            throw new Exception("错误的数值");
        }


        /// <summary>
        /// 返回一个小于所指定最大值的非负随机数。
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public int Random(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }
    }
}