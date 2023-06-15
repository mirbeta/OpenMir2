namespace SystemModule
{
    public interface IMonsterActor : IActor
    {
        bool IsSlave { get; set; }

        byte SlaveExpLevel { get; set; }

        int ProcessRunCount { get; set; }

        int FightExp { get; set; }

        int WalkStep { get; set; }

        int WalkWait { get; set; }

        void Initialize();

        void SearchViewRange();

        void Run();

        bool ReAliveEx(MonGenInfo monGenInfo);
    }
}