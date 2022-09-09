using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 查看时间日期
    /// </summary>
    [GameCommand("Date", "查看时间日期", "", 0)]
    public class DataCommand : BaseCommond
    {
        [DefaultCommand]
        public void Date(PlayObject PlayObject)
        {
            PlayObject.SysMsg(M2Share.g_sNowCurrDateTime + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), MsgColor.Blue, MsgType.Hint);
        }
    }
}