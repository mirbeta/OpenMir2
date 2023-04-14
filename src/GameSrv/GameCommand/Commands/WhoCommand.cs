using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 当前服务器在线人数
    /// </summary>
    [Command("Who", "查看在线人数", "统计服务器在线人数", 10)]
    public class WhoCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            var offlineCount = 0;
            playObject.HearMsg($"当前服务器在线人数: {M2Share.WorldEngine.PlayObjectCount}({offlineCount}/{(M2Share.WorldEngine.PlayObjectCount - offlineCount)})");
        }
    }
}