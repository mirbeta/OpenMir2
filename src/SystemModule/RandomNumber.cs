using System;
using System.Collections.Generic;

namespace SystemModule
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
        private static readonly object syncObject = new object();

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
                throw new ArgumentOutOfRangeException("selectCount必需大于sourceList.Count");
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