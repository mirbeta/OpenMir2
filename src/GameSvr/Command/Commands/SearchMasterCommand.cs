using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 查询师徒当前所在位置
    /// </summary>
    [GameCommand("SearchMaster", "查询师徒当前所在位置", 0)]
    public class SearchMasterCommand : BaseCommond
    {
        [DefaultCommand]
        public void SearchMaster(TPlayObject PlayObject)
        {
            TPlayObject Human;
            if (PlayObject.m_sMasterName == "")
            {
                PlayObject.SysMsg(M2Share.g_sYouAreNotMasterMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (PlayObject.m_boMaster)
            {
                if (PlayObject.m_MasterList.Count <= 0)
                {
                    PlayObject.SysMsg(M2Share.g_sYourMasterListNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                PlayObject.SysMsg(M2Share.g_sYourMasterListNowLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                for (var i = 0; i < PlayObject.m_MasterList.Count; i++)
                {
                    Human = PlayObject.m_MasterList[i];
                    PlayObject.SysMsg(Human.m_sCharName + " " + Human.m_PEnvir.sMapDesc + "(" + Human.m_nCurrX + ":" + Human.m_nCurrY + ")", TMsgColor.c_Green, TMsgType.t_Hint);
                    Human.SysMsg(M2Share.g_sYourMasterSearchLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                    Human.SysMsg(PlayObject.m_sCharName + " " + PlayObject.m_PEnvir.sMapDesc + "(" + PlayObject.m_nCurrX + ":" + PlayObject.m_nCurrY + ")", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                if (PlayObject.m_MasterHuman == null)
                {
                    PlayObject.SysMsg(M2Share.g_sYourMasterNotOnlineMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                PlayObject.SysMsg(M2Share.g_sYourMasterNowLocateMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                PlayObject.SysMsg(PlayObject.m_MasterHuman.m_sCharName + " " + PlayObject.m_MasterHuman.m_PEnvir.sMapDesc + "(" + PlayObject.m_MasterHuman.m_nCurrX + ":"
                    + PlayObject.m_MasterHuman.m_nCurrY + ")", TMsgColor.c_Green, TMsgType.t_Hint);
                PlayObject.m_MasterHuman.SysMsg(M2Share.g_sYourMasterListSearchLocateMsg, TMsgColor.c_Green, TMsgType.t_Hint);
                PlayObject.m_MasterHuman.SysMsg(PlayObject.m_sCharName + " " + PlayObject.m_PEnvir.sMapDesc + "(" + PlayObject.m_nCurrX + ":" + PlayObject.m_nCurrY + ")", 
                    TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}