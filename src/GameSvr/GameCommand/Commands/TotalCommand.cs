using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 总在线数
    /// </summary>
    [Command("Total", "查看在线人数", "统计服务器在线人数", 10)]
    public class TotalCommand : Command
    {
        [ExecuteCommand]
        public static void Total(PlayObject PlayObject)
        {
            PlayObject.HearMsg(string.Format("总在线数{0}", M2Share.g_nTotalHumCount));
        }
    }
}