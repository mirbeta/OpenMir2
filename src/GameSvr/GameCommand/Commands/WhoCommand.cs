using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 当前服务器在线人数
    /// </summary>
    [Command("Who", "查看在线人数", "统计服务器在线人数", 10)]
    public class WhoCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject)
        {
            var offlineCount = 0;
            PlayObject.HearMsg($"当前服务器在线人数: {M2Share.WorldEngine.PlayObjectCount}({offlineCount}/{(M2Share.WorldEngine.PlayObjectCount - offlineCount)})");
        }
    }
}