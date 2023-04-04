using GameSrv.Actor;
using GameSrv.Player;

namespace GameSrv.GameCommand.Commands
{
    /// <summary>
    /// 推开范围内对象
    /// </summary>
    [Command("BackStep", "推开范围内对象", 10)]
    public class BackStepCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            byte nType = (byte)(@Params.Length > 0 ? HUtil32.StrToInt(@Params[0], 0) : 0);
            byte nCount = (byte)(@Params.Length > 1 ? HUtil32.StrToInt(@Params[1], 0) : 0);
            if (PlayObject.Permission < 6)
            {
                return;
            }
            nType = (byte)HUtil32._MIN(nType, 8);
            if (nType == 0)
            {
                PlayObject.CharPushed(BaseObject.GetBackDir(PlayObject.Direction), nCount);
            }
            else
            {
                PlayObject.CharPushed(M2Share.RandomNumber.RandomByte((byte)nType), nCount);
            }
        }
    }
}