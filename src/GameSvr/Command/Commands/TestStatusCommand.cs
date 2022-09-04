using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("TestStatus", "", 10)]
    public class TestStatusCommand : BaseCommond
    {
        [DefaultCommand]
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
            PlayObject.m_wStatusTimeArr[nType] = Convert.ToByte(nTime * 1000);
            PlayObject.m_dwStatusArrTick[nType] = HUtil32.GetTickCount();
            PlayObject.m_nCharStatus = PlayObject.GetCharStatus();
            PlayObject.StatusChanged();
            PlayObject.SysMsg(string.Format("状态编号:{0} 时间长度: {1} 秒", nType, nTime), MsgColor.Green, MsgType.Hint);
        }
    }
}