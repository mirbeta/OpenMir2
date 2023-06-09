using M2Server.Actor;
using SystemModule;
using SystemModule;

namespace CommandSystem
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
                PlayerActor.SysMsgCharPushed(BaseObject.GetBackDir(PlayerActor.SysMsgDir), nCount);
            }
            else
            {
                PlayerActor.SysMsgCharPushed(SystemShare.RandomNumber.RandomByte(nType), nCount);
            }
        }
    }
}