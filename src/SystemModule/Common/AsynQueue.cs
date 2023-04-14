using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SystemModule.Common
{
    [Serializable]
    public class EventArgs<T> : EventArgs
    {
        public T Argument;

        public EventArgs() : this(default(T))
        {
        }

        public EventArgs(T argument)
        {
            Argument = argument;
        }
    }

    public class AsynQueue<T>
    {
        //队列是否正在处理数据
        private int isProcessing;
        //有线程正在处理数据
        private const int Processing = 1;
        //没有线程处理数据
        private const int UnProcessing = 0;
        //队列是否可用
        private volatile bool enabled = true;
        private Task currentTask;
        public event Action<T> ProcessItemFunction;
        public event EventHandler<EventArgs<Exception>> ProcessException;
        private readonly ConcurrentQueue<T> queue;

        public AsynQueue()
        {
            queue = new ConcurrentQueue<T>();
        }

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        public void Start()
        {
            Thread process_Thread = new Thread(PorcessItem)
            {
                IsBackground = true
            };
            process_Thread.Start();
        }

        public void Enqueue(T items)
        {
            if (items != null)
            {
                queue.Enqueue(items);
                DataAdded();
            }
            else
            {
                throw new ArgumentException("items");
            }
        }

        //数据添加完成后通知消费者线程处理
        private void DataAdded()
        {
            if (enabled)
            {
                if (!IsProcessingItem())
                {
                    currentTask = Task.Factory.StartNew(ProcessItemLoop);
                }
            }
        }

        //判断是否队列有线程正在处理 
        private bool IsProcessingItem()
        {
            return !(Interlocked.CompareExchange(ref isProcessing, Processing, UnProcessing) == 0);
        }

        private void ProcessItemLoop()
        {
            if (!enabled && queue.IsEmpty)
            {
                Interlocked.Exchange(ref isProcessing, 0);
                return;
            }
            //处理的线程数 是否小于当前最大任务数
            //if (Thread.VolatileRead(ref runingCore) <= this.MaxTaskCount)
            //{
            T publishFrame;
            if (queue.TryDequeue(out publishFrame))
            {
                try
                {
                    ProcessItemFunction(publishFrame);
                }
                catch (Exception ex)
                {
                    OnProcessException(ex);
                }
            }

            if (enabled && !queue.IsEmpty)
            {
                currentTask = Task.Factory.StartNew(ProcessItemLoop);
            }
            else
            {
                Interlocked.Exchange(ref isProcessing, UnProcessing);
            }
        }

        /// <summary>
        ///定时处理线程调用函数  
        ///主要是监视入队的时候线程 没有来的及处理的情况
        /// </summary>
        private void PorcessItem(object state)
        {
            int sleepCount = 0;
            int sleepTime = 1000;
            while (enabled)
            {
                //如果队列为空则根据循环的次数确定睡眠的时间
                if (queue.IsEmpty)
                {
                    if (sleepCount == 0)
                    {
                        sleepTime = 1000;
                    }
                    else if (sleepCount <= 3)
                    {
                        sleepTime = 1000 * 3;
                    }
                    else
                    {
                        sleepTime = 1000 * 50;
                    }
                    sleepCount++;
                    Thread.Sleep(sleepTime);
                }
                else
                {
                    //判断是否队列有线程正在处理 
                    if (enabled && Interlocked.CompareExchange(ref isProcessing, Processing, UnProcessing) == 0)
                    {
                        if (!queue.IsEmpty)
                        {
                            currentTask = Task.Factory.StartNew(ProcessItemLoop);
                        }
                        else
                        {
                            Interlocked.Exchange(ref isProcessing, 0);
                        }
                        sleepCount = 0;
                        sleepTime = 1000;
                    }
                }
            }
        }

        public void Flsuh()
        {
            Stop();

            if (currentTask != null)
            {
                currentTask.Wait();
            }

            while (!queue.IsEmpty)
            {
                try
                {
                    T publishFrame;
                    if (queue.TryDequeue(out publishFrame))
                    {
                        ProcessItemFunction(publishFrame);
                    }
                }
                catch (Exception ex)
                {
                    OnProcessException(ex);
                }
            }
            currentTask = null;
        }

        public void Stop()
        {
            this.enabled = false;
        }

        private void OnProcessException(Exception ex)
        {
            EventHandler<EventArgs<Exception>> tempException = ProcessException;
            Interlocked.CompareExchange(ref ProcessException, null, null);

            if (tempException != null)
            {
                ProcessException(ex, new EventArgs<Exception>(ex));
            }
        }
    }
}