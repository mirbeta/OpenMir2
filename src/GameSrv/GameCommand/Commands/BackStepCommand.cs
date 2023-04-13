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
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            byte nType = (byte)(@params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0);
            byte nCount = (byte)(@params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0);
            if (playObject.Permission < 6)
            {
                return;
            }
            nType = (byte)HUtil32._MIN(nType, 8);
            if (nType == 0)
            {
                playObject.CharPushed(BaseObject.GetBackDir(playObject.Direction), nCount);
            }
            else
            {
                playObject.CharPushed(M2Share.RandomNumber.RandomByte((byte)nType), nCount);
            }
        }
    }
}