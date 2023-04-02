using System;
using System.Collections.Generic;

namespace SystemModule;

public class IdWorker
{
    //机器ID
    private static int workerId;
    private static readonly int twepoch = 40415203; //唯一时间，这是一个避免重复的随机量，自行设定不要大于当前时间戳
    private static int sequence = 0;
    private static readonly int workerIdBits = 1; //机器码字节数。4个字节用来保存机器码(定义为Long类型会出现，最大偏移64位，所以左移64位没有意义)
    public static long maxWorkerId = -1L ^ -1L << workerIdBits; //最大机器ID
    private static readonly int sequenceBits = 8; //计数器字节数，10个字节用来保存计数码
    private static readonly int workerIdShift = sequenceBits; //机器码数据左移位数，就是后面计数器占用的位数
    private static readonly int timestampLeftShift = sequenceBits + workerIdBits; //时间戳左移动位数就是机器码和计数器总字节数
    public static int sequenceMask = -1 ^ -1 << sequenceBits; //一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成
    private int lastTimestamp = -1;

    /// <summary> 
    /// 机器码
    /// </summary>
    /// <param name="workerId"></param>
    public IdWorker(long workerId)
    {
        if (workerId > maxWorkerId || workerId < 0)
            throw new Exception($"worker Id can't be greater than {workerId} or less than 0 ");
        IdWorker.workerId = 2;
    }

    public int NextId()
    {
        lock (this)
        {
            int timestamp = TimeGen();
            if (lastTimestamp == timestamp)
            {
                //同一微妙中生成ID
                sequence = (sequence + 1) & sequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                if (sequence == 0)
                {
                    //一微妙内产生的ID计数已达上限，等待下一微妙
                    timestamp = TillNextMillis(lastTimestamp);
                }
            }
            else
            {
                //不同微秒生成ID
                sequence = 0; //计数清0
            }

            if (timestamp < lastTimestamp)
            {
                //如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
                throw new Exception($"Clock moved backwards.  Refusing to generate id for {lastTimestamp - timestamp} milliseconds");
            }

            lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
            return (timestamp - twepoch << timestampLeftShift) | workerId << workerIdShift | sequence;
        }
    }

    public IList<int> Take(int count)
    {
        List<int> list = new List<int>();

        for (int j = 0; j < count; j++)
        {
            int timestamp = TimeGen();

            if (lastTimestamp == timestamp)
            {
                //同一微妙中生成ID
                sequence = (sequence + 1) & sequenceMask; //用&运算计算该微秒内产生的计数是否已经到达上限
                if (sequence == 0)
                {
                    //一微妙内产生的ID计数已达上限，等待下一微妙
                    timestamp = TillNextMillis(lastTimestamp);
                }
            }
            else
            {
                //不同微秒生成ID
                sequence = 0; //计数清0
            }

            if (timestamp < lastTimestamp)
            {
                //如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
                throw new Exception($"Clock moved backwards.  Refusing to generate id for {lastTimestamp - timestamp} milliseconds");
            }

            lastTimestamp = timestamp; //把当前时间戳保存为最后生成ID的时间戳
            list.Add((timestamp - twepoch << timestampLeftShift) | (workerId << workerIdShift) | sequence);
        }

        return list;
    }

    /// <summary>
    /// 获取下一微秒时间戳
    /// </summary>
    /// <param name="lastTimestamp"></param>
    /// <returns></returns>
    private int TillNextMillis(int lastTimestamp)
    {
        int timestamp = TimeGen();
        while (timestamp <= lastTimestamp)
        {
            timestamp = TimeGen();
        }
        return timestamp;
    }

    /// <summary>
    /// 生成当前时间戳
    /// </summary>
    /// <returns></returns>
    private int TimeGen()
    {
        return Environment.TickCount;
    }
}