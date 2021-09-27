using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 搜索指定玩家所在地图XY坐标
    /// </summary>
    [GameCommand("SearchHuman", "搜索指定玩家所在地图XY坐标", 10)]
    public class SearchHumanCommand : BaseCommond
    {
        [DefaultCommand]
        public void SearchHuman(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            TPlayObject m_PlayObject;
            if (PlayObject.m_boProbeNecklace || PlayObject.m_btPermission >= 6)
            {
                if (string.IsNullOrEmpty(sHumanName))
                {
                    PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称", TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                if (HUtil32.GetTickCount() - PlayObject.m_dwProbeTick > 10000 || PlayObject.m_btPermission >= 3)
                {
                    PlayObject.m_dwProbeTick = HUtil32.GetTickCount();
                    m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
                    if (m_PlayObject != null)
                    {
                        PlayObject.SysMsg(sHumanName + " 现在位于 " + m_PlayObject.m_PEnvir.sMapDesc + '(' + m_PlayObject.m_PEnvir.sMapName + ") " + m_PlayObject.m_nCurrX + ':'
                            + PlayObject.m_nCurrY, TMsgColor.c_Blue, TMsgType.t_Hint);
                    }
                    else
                    {
                        PlayObject.SysMsg(sHumanName + " 现在不在线，或位于其它服务器上!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg((HUtil32.GetTickCount() - PlayObject.m_dwProbeTick) / 1000 - 10 + " 秒之后才可以再使用此功能!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}