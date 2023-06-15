namespace SystemModule
{
    /// <summary>
    /// 机器人脚本系统(Robots)
    /// </summary>
    public interface IAutoBotSystem
    {
        void Initialize();

        void ReLoadRobot();

        void Run();
    }
}