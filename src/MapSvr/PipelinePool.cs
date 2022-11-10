using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MapSvr
{
    public class PipelinePool
    {
        /// <summary>
        /// 用于存储和管理管道的进程池
        /// </summary>
        private static readonly ConcurrentDictionary<Guid, Pipeline> ServerPool = new ConcurrentDictionary<Guid, Pipeline>();

        /// <summary>
        /// 创建一个新的管道
        /// </summary>
        private static void CreatePipeLine()
        {
            lock (ServerPool)
            {
                if (ServerPool.Count < Pipeline.MaxNumberOfServerInstances)
                {
                    var pipe = new Pipeline();
                    pipe.Start();
                    ServerPool.TryAdd(pipe.ID, pipe);
                }
            }

            Console.WriteLine($"管道池添加新管道 当前管道总数{ServerPool.Count}");
        }

        /// <summary>
        /// 根据ID从管道池中释放一个管道
        /// </summary>
        /// <param name="Id"></param>
        private static void DisposablePipeLine(Guid Id)
        {
            lock (ServerPool)
            {
                Console.WriteLine($"开始尝试释放,管道{Id}");
                if (ServerPool.TryRemove(Id, out Pipeline pipe))
                    Console.WriteLine($"管道{Id},已经关闭,并完成资源释放");
                else
                    Console.WriteLine($"未找到ID为{Id}的管道");
                if (ServerPool.Count == 0)
                    CreatePipeLine();
            }
        }

        /// <summary>
        ///  (异步)创建一个新的管道进程
        /// </summary>
        public static async void CreatePipeLineAsync() => await Task.Run(CreatePipeLine);

        /// <summary>
        /// (异步)根据ID从管道池中释放一个管道
        /// </summary>
        /// <param name="id"></param>
        public static async void DisposablePipeLineAsync(Guid id) => await Task.Run(() => { DisposablePipeLine(id); });
    }
}