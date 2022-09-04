using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 查询师徒当前所在位置
    /// </summary>
    [GameCommand("SearchMaster", "查询师徒当前所在位置", 0)]
    public class SearchMasterCommand : BaseCommond
    {
        [DefaultCommand]
        public void SearchMaster(PlayObject PlayObject)
        {
            if (PlayObject.m_sMasterName == "")
            {
                PlayObject.SysMsg(M2Share.g_sYouAreNotMasterMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayObject.m_boMaster)
            {
                if (PlayObject.m_MasterList.Count <= 0)
                {
                    PlayObject.SysMsg(M2Share.g_sYourMasterListNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                PlayObject.SysMsg(M2Share.g_sYourMasterListNowLocateMsg, MsgColor.Green, MsgType.Hint);
                for (var i = 0; i < PlayObject.m_MasterList.Count; i++)
                {
                    var Human = PlayObject.m_MasterList[i];
                    PlayObject.SysMsg(Human.CharName + " " + Human.m_PEnvir.MapDesc + "(" + Human.CurrX + ":" + Human.CurrY + ")", MsgColor.Green, MsgType.Hint);
                    Human.SysMsg(M2Share.g_sYourMasterSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                    Human.SysMsg(PlayObject.CharName + " " + PlayObject.m_PEnvir.MapDesc + "(" + PlayObject.CurrX + ":" + PlayObject.CurrY + ")", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                if (PlayObject.m_MasterHuman == null)
                {
                    PlayObject.SysMsg(M2Share.g_sYourMasterNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                PlayObject.SysMsg(M2Share.g_sYourMasterNowLocateMsg, MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(PlayObject.m_MasterHuman.CharName + " " + PlayObject.m_MasterHuman.m_PEnvir.MapDesc + "(" + PlayObject.m_MasterHuman.CurrX + ":"
                    + PlayObject.m_MasterHuman.CurrY + ")", MsgColor.Green, MsgType.Hint);
                PlayObject.m_MasterHuman.SysMsg(M2Share.g_sYourMasterListSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                PlayObject.m_MasterHuman.SysMsg(PlayObject.CharName + " " + PlayObject.m_PEnvir.MapDesc + "(" + PlayObject.CurrX + ":" + PlayObject.CurrY + ")",
                    MsgColor.Green, MsgType.Hint);
            }
        }
    }
}