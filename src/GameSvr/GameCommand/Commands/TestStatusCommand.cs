using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("TestStatus", "", 10)]
    public class TestStatusCommand : Command
    {
        [ExecuteCommand]
        public void TestStatus(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var nType = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            var nTime = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (PlayObject.Permission < 6)
            {
                return;
            }

            //if ((!(nType >= Grobal2.ushort.GetLowerBound(0) && nType<= Grobal2.ushort..Length)) || (nTime < 0))
            //{
            //    this.SysMsg("命令格式: @" + sCmd + " 类型(0..11) 时长", TMsgColor.c_Red, TMsgType.t_Hint);
            //    return;
            //}
            PlayObject.StatusArr[nType] = (ushort)(nTime * 1000);
            PlayObject.StatusArrTick[nType] = HUtil32.GetTickCount();
            PlayObject.CharStatus = PlayObject.GetCharStatus();
            PlayObject.StatusChanged();
            PlayObject.SysMsg(string.Format("状态编号:{0} 时间长度: {1} 秒", nType, nTime), MsgColor.Green, MsgType.Hint);
        }
    }
}