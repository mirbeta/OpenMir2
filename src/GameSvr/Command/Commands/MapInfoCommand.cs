using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [Command("MapInfo", "显示当前地图信息", 10)]
    public class MapInfoCommand : Commond
    {
        [ExecuteCommand]
        public void ShowMapInfo(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            var sMap = @params[2];
            var nX = (short)HUtil32.StrToInt(@params[0], 0);
            var nY = (short)HUtil32.StrToInt(@params[1], 0);
            if (!string.IsNullOrEmpty(sMap) && nX >= 0 && nY >= 0)
            {
                var Map = M2Share.MapMgr.FindMap(sMap);
                if (Map != null)
                {
                    var cellSuccess = false;
                    var cellInfo = Map.GetCellInfo(nX, nY, ref cellSuccess);
                    if (cellSuccess)
                    {
                        playObject.SysMsg("标志: " + cellInfo.Attribute, MsgColor.Green, MsgType.Hint);
                        if (cellInfo.IsAvailable)
                        {
                            playObject.SysMsg("对象数: " + cellInfo.Count, MsgColor.Green, MsgType.Hint);
                        }
                    }
                    else
                    {
                        playObject.SysMsg("取地图单元信息失败: " + sMap, MsgColor.Red, MsgType.Hint);
                    }
                }
            }
            else
            {
                playObject.SysMsg("请按正确格式输入: " + M2Share.GameCommand.MapInfo.CommandName + " 地图号 X Y", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}