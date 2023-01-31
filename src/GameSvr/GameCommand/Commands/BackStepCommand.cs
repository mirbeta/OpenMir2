using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 推开范围内对象
    /// </summary>
    [Command("BackStep", "推开范围内对象", 10)]
    public class BackStepCommand : Command
    {
        [ExecuteCommand]
        public void BackStep(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var nType = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            var nCount = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (PlayObject.Permission < 6)
            {
                return;
            }
            nType = HUtil32._MIN(nType, 8);
            if (nType == 0)
            {
                PlayObject.CharPushed(PlayObject.GetBackDir(PlayObject.Direction), nCount);
            }
            else
            {
                PlayObject.CharPushed(M2Share.RandomNumber.RandomByte((byte)nType), nCount);
            }
        }
    }
}