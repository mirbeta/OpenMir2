namespace SystemModule
{
    public interface IRobotObject
    {
        void Initialize();
        void LoadScript();
        void ReloadScript();
        void Run();
    }
}