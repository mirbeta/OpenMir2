using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 暂时不清楚干啥的
    /// </summary>
    [GameCommand("AdjustExp", "", 10)]
    public class AdjustExpCommand : BaseCommond
    {
        [DefaultCommand]
        public void AdjustExp(string[] @Params, PlayObject PlayObject)
        {
            if (PlayObject.Permission < 6)
            {
                return;
            }
        }
    }
}