using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("MapInfo", "显示当前地图信息", 10)]
    public class MapInfoCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            string sMap = @params[2];
            short nX = HUtil32.StrToInt16(@params[0], 0);
            short nY = HUtil32.StrToInt16(@params[1], 0);
            if (!string.IsNullOrEmpty(sMap) && nX >= 0 && nY >= 0)
            {
                SystemModule.Maps.IEnvirnoment map = SystemShare.MapMgr.FindMap(sMap);
                if (map != null && map.IsValidCell(nX, nY))
                {
                    ref SystemModule.Data.MapCellInfo cellInfo = ref map.GetCellInfo(nX, nY, out bool cellSuccess);
                    if (cellSuccess)
                    {
                        PlayerActor.SysMsg("标志: " + cellInfo.Attribute, MsgColor.Green, MsgType.Hint);
                        if (cellInfo.IsAvailable)
                        {
                            PlayerActor.SysMsg("对象数: " + cellInfo.Count, MsgColor.Green, MsgType.Hint);
                        }
                    }
                    else
                    {
                        PlayerActor.SysMsg("取地图单元信息失败: " + sMap, MsgColor.Red, MsgType.Hint);
                    }
                }
            }
            else
            {
                PlayerActor.SysMsg("请按正确格式输入: " + this.Command.Name + " 地图号 X Y", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}