using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 暂时不清楚干啥的
    /// </summary>
    [Command("AdjustExp", "", 10)]
    public class AdjustExpCommand : Commond
    {
        [ExecuteCommand]
        public void AdjustExp(string[] @Params, PlayObject PlayObject)
        {
            if (PlayObject.Permission < 6)
            {
                return;
            }
        }
    }
}