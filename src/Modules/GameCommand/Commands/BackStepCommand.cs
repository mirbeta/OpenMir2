using SystemModule;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 推开范围内对象
    /// </summary>
    [Command("BackStep", "推开范围内对象", 10)]
    public class BackStepCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var nType = (byte)(@params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0);
            var nCount = (byte)(@params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0);
            if (PlayerActor.Permission < 6)
            {
                return;
            }
            nType = (byte)HUtil32._MIN(nType, 8);
            if (nType == 0)
            {
                PlayerActor.CharPushed(PlayerActor.GetBackDir(PlayerActor.Dir), nCount);
            }
            else
            {
                PlayerActor.CharPushed(SystemShare.RandomNumber.RandomByte(nType), nCount);
            }
        }
    }
}