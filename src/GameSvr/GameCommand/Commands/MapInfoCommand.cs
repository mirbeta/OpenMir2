using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("MapInfo", "显示当前地图信息", 10)]
    public class MapInfoCommand : Command
    {
        [ExecuteCommand]
        public void ShowMapInfo(string[] @params, PlayObject playObject)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            string sMap = @params[2];
            short nX = (short)HUtil32.StrToInt(@params[0], 0);
            short nY = (short)HUtil32.StrToInt(@params[1], 0);
            if (!string.IsNullOrEmpty(sMap) && nX >= 0 && nY >= 0)
            {
                Maps.Envirnoment Map = M2Share.MapMgr.FindMap(sMap);
                if (Map != null)
                {
                    bool cellSuccess = false;
                    Maps.MapCellInfo cellInfo = Map.GetCellInfo(nX, nY, ref cellSuccess);
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
                playObject.SysMsg("请按正确格式输入: " + this.GameCommand.Name + " 地图号 X Y", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}