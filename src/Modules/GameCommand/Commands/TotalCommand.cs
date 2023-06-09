using SystemModule;

namespace CommandSystem
{
    /// <summary>
    /// 总在线数
    /// </summary>
    [Command("Total", "查看在线人数", "统计服务器在线人数", 10)]
    public class TotalCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.HearMsg($"总在线数{SystemShare.TotalHumCount}");
        }
    }
}