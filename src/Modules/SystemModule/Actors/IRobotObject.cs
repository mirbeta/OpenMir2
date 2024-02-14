namespace SystemModule.Actors
{
    public interface IRobotObject
    {
        void Initialize();
        void LoadScript();
        void ReloadScript();
        void Run();
    }
}