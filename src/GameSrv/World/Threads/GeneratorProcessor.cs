using NLog;
using System.Diagnostics;
using SystemModule.Generation.Entities;

namespace GameSrv.World.Threads
{
    public class GeneratorProcessor : TimerScheduledService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly StandardRandomizer _standardRandomizer = new StandardRandomizer();
        private readonly Stopwatch sw = new Stopwatch();

        public GeneratorProcessor() : base(TimeSpan.FromSeconds(10), "GeneratorProcessor")
        {

        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            GenerateIdThread();
            return Task.CompletedTask;
        }

        private void GenerateIdThread()
        {
            if (M2Share.ActorMgr.GenerateQueue.Count < 20000)
            {
                sw.Start();
                for (int i = 0; i < 100000; i++)
                {
                    int sequence = _standardRandomizer.NextInteger();
                    if (M2Share.ActorMgr.ActorsMap.ContainsKey(sequence))
                    {
                        while (true)
                        {
                            sequence = _standardRandomizer.NextInteger();
                            if (!M2Share.ActorMgr.ActorsMap.ContainsKey(sequence))
                            {
                                break;
                            }
                        }
                    }
                    while (sequence < 0)
                    {
                        sequence = Environment.TickCount + HUtil32.Sequence();
                        if (sequence > 0) break;
                    }
                    M2Share.ActorMgr.GenerateQueue.Enqueue(sequence);
                }
                sw.Stop();
                _logger.Debug($"Id生成完毕 耗时:{sw.Elapsed} 可用数:[{M2Share.ActorMgr.GenerateQueue.Count}]");
            }
        }
    }
}