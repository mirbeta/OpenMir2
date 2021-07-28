using SystemModule;
using M2Server;
using M2Server.CommandSystem;

namespace M2Servers
{
    /// <summary>
    /// 调整英雄等级
    /// </summary>
    [GameCommand("HeroLevel", "", 10)]
    public class HeroLevelCommand : BaseCommond
    {
        [DefaultCommand]
        public void HeroLevel(string[] @Params, TPlayObject PlayObject)
        {
            //string sHeroName = @Params.Length > 0 ? @Params[0] : "";
            //int nLevel = @Params.Length > 1 ? System.Convert.ToInt32(@Params[1]) : 0;
            //int nOLevel;
            //if ((PlayObject.m_btPermission < this.Attributes.nPermissionMin))
            //{
            //    PlayObject.   SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);// 权限不够
            //    return;
            //}
            //if (sHeroName == "")
            //{
            //    if (M2Share.g_Config.boGMShowFailMsg)
            //    {
            //        PlayObject.  SysMsg("命令格式: @" + this.Attributes.Name + " 英雄名称 等级", TMsgColor.c_Red, TMsgType.t_Hint);
            //    }
            //    return;
            //}
            //var TargetObject = M2Share.UserEngine.GetHeroObject(sHeroName);
            //if (TargetObject != null)
            //{
            //    nOLevel = TargetObject.m_Abil.Level;
            //    //PlayObject.m_Abil.Level = HUtil32._MAX(1, HUtil32._MIN(M2Share.MAXUPLEVEL, nLevel));
            //    TargetObject.HasLevelUp(1);
            //    // 等级调整记录日志
            //    M2Share.AddGameDataLog("17" + "\09" + TargetObject.m_sMapName + "\09" + TargetObject.m_nCurrX + "\09" + TargetObject.m_nCurrY + "\09"
            //                           + TargetObject.m_sCharName + "\09" + TargetObject.m_Abil.Level + "\09" + PlayObject.m_sCharName + "\09" + "+(" + nLevel + ")" + "\09" + "(英雄)");
            //    PlayObject.SysMsg("英雄 " + sHeroName + " 等级调整完成。", TMsgColor.BB_Fuchsia, TMsgType.t_Hint);
            //    if (M2Share.g_Config.boShowMakeItemMsg)
            //    {
            //        M2Share.MainOutMessage("[英雄等级调整] " + sHeroName + "(" + (nOLevel).ToString() + " -> " + (TargetObject.m_Abil.Level).ToString() + ")");
            //    }
            //}
            //else
            //{
            //    PlayObject.  SysMsg("英雄" + sHeroName + "现在不在线。", TMsgColor.c_Red, TMsgType.t_Hint);
            //}
        }
    }
}