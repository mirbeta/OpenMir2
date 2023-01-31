using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 当前服务器在线人数
    /// </summary>
    [Command("Who", "查看在线人数", "统计服务器在线人数", 10)]
    public class WhoCommand : Command
    {
        [ExecuteCommand]
        public static void Who(PlayObject PlayObject)
        {
            var offlineCount = 0;
            PlayObject.HearMsg(string.Format("当前服务器在线人数: {0}({1}/{2})", M2Share.WorldEngine.PlayObjectCount, offlineCount, (M2Share.WorldEngine.PlayObjectCount - offlineCount)));
        }
    }
}