using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SystemModule.Core.Run.Action
{
    /// <summary>
    /// 易用组件
    /// </summary>
    public class EasyTask
    {
        static EasyTask()
        {
            InitCompletedTask();
        }

        private static readonly ConcurrentDictionary<object, Timer> timers = new ConcurrentDictionary<object, Timer>();

#if DEBUG

        /// <summary>
        /// Timers
        /// </summary>
        public static ConcurrentDictionary<object, Timer> Timers => timers;

#endif

        /// <summary>
        /// 延迟执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delayTimeSpan"></param>
        public static void DelayRun(TimeSpan delayTimeSpan, System.Action action)
        {
            DelayRun(delayTimeSpan.Milliseconds, action);
        }

        /// <summary>
        /// 延迟执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delayTimeSpan"></param>
        /// <param name="status"></param>
        public static void DelayRun<T>(TimeSpan delayTimeSpan, T status, Action<T> action)
        {
            DelayRun(delayTimeSpan.Milliseconds, status, action);
        }

        /// <summary>
        /// 延迟执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        public static void DelayRun(int delay, System.Action action)
        {
            object obj = new object();
            Timer timer = new Timer((o) =>
            {
                if (timers.TryRemove(o, out Timer timer1))
                {
                    timer1.Dispose();
                }
                action?.Invoke();
            }, obj, delay, Timeout.Infinite);
            timers.TryAdd(obj, timer);
        }

        /// <summary>
        /// 延迟执行
        /// </summary>
        /// <param name="action"></param>
        /// <param name="delay"></param>
        /// <param name="status"></param>
        public static void DelayRun<T>(int delay, T status, Action<T> action)
        {
            object obj = new object();
            Timer timer = new Timer((o) =>
            {
                if (timers.TryRemove(o, out Timer timer1))
                {
                    timer1.Dispose();
                }
                action?.Invoke(status);
            }, obj, delay, Timeout.Infinite);
            timers.TryAdd(obj, timer);
        }

        /// <summary>
        /// Task异步
        /// </summary>
        /// <param name="statu"></param>
        /// <param name="action"></param>
        public static Task Run<T>(T statu, Action<T> action)
        {
            return Task.Factory.StartNew(() =>
            {
                action.Invoke(statu);
            });
        }

        /// <summary>
        /// Task异步
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="function"></param>
        /// <returns></returns>
        public static Task<TResult> Run<TResult>(Func<TResult> function)
        {
            return Task.Factory.StartNew(function);
        }

        /// <summary>
        /// Task异步
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Task Run(System.Action action)
        {
            return Task.Factory.StartNew(action);
        }

        /// <summary>
        /// 已完成的Task
        /// </summary>
        public static Task CompletedTask { get; private set; }

        private static void InitCompletedTask()
        {
            CompletedTask = Task.CompletedTask;
        }
    }
}