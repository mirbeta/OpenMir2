using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    [Command("TestStatus", "", 10)]
    public class TestStatusCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var nType = @params.Length > 0 ? HUtil32.StrToInt(@params[0], 0) : 0;
            var nTime = @params.Length > 1 ? HUtil32.StrToInt(@params[1], 0) : 0;
            if (PlayerActor.Permission < 6)
            {
                return;
            }

            //if ((!(nType >= Grobal2.ushort.GetLowerBound(0) && nType<= Grobal2.ushort..Length)) || (nTime < 0))
            //{
            //    this.SysMsg("命令格式: @" + sCmd + " 类型(0..11) 时长", MsgColor.c_Red, MsgType.t_Hint);
            //    return;
            //}
            PlayerActor.StatusTimeArr[nType] = (ushort)(nTime * 1000);
            PlayerActor.StatusArrTick[nType] = HUtil32.GetTickCount();
            PlayerActor.CharStatus = PlayerActor.GetCharStatus();
            PlayerActor.StatusChanged();
            PlayerActor.SysMsg(string.Format("状态编号:{0} 时间长度: {1} 秒", nType, nTime), MsgColor.Green, MsgType.Hint);
        }
    }
}