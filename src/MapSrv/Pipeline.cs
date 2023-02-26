using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace MapSrv
{
    public class Pipeline : IDisposable
    {
        public Guid ID { get; }
        private readonly NamedPipeServerStream Server;
        private Task Task;
        private readonly AutoResetEvent Get, Got;
        private string inputContext;
        private StreamWriter Writer;
        private StreamReader Reader;
        public const int MaxNumberOfServerInstances = 100;
        private const string PipeName = "mirmap.pipe";
        private const int ServerWaitReadMillisecs = 10000; //10s
        private const int MaxTimeout = 3;

        public Pipeline()
        {
            ID = Guid.NewGuid();
            Get = new AutoResetEvent(false);
            Got = new AutoResetEvent(false);
            Server = new NamedPipeServerStream(PipeName, PipeDirection.InOut, MaxNumberOfServerInstances, PipeTransmissionMode.Byte, PipeOptions.None, 1024, 1024);
        }

        public void Start()
        {
            Task = Task.Factory.StartNew(TaskRun);
        }

        private void TaskRun()
        {
            Server.WaitForConnection();
            PipelinePool.CreatePipeLineAsync();
            try
            {
                Writer = new StreamWriter(Server);
                Reader = new StreamReader(Server);
                while (true)
                {
                    var input = TryReadLine();
                    if (string.IsNullOrEmpty(input)) break;
                    //Do Somethin....
                    Console.WriteLine($"Server {ID} Get Message:{input}");
                    Writer.WriteLine($"Server Get Message:{input}");
                    Writer.Flush();
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine($"管道{ID}超时次数过多，视为丢失链接");
            }
            Console.WriteLine($"管道{ID}即将关闭");
            Dispose();
        }

        private void readerThread()
        {
            Get.WaitOne();
            inputContext = Reader.ReadLine();
            Got.Set();
        }

        private string TryReadLine()
        {
            int TimeOutCount = 0;
            var thread = new Thread(readerThread);
            thread.Start();
            Get.Set();
            while (!Got.WaitOne(ServerWaitReadMillisecs))
            {
                if (TimeOutCount > MaxTimeout)
                {
                    thread.Abort();
                    throw new TimeoutException();
                }
                Console.WriteLine($"管道{ID}第{TimeOutCount}次超时");
            }
            return inputContext;
        }

        public void Dispose()
        {
            Server.Close();
            Server.Dispose();
            Get.Dispose();
            Got.Dispose();
            PipelinePool.DisposablePipeLineAsync(ID);
        }
    }
}