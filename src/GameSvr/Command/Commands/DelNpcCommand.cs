using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 删除对面面NPC
    /// </summary>
    [GameCommand("DelNpc", "删除对面面NPC", 10)]
    public class DelNpcCommand : BaseCommond
    {
        [DefaultCommand]
        public void DelNpc(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            const string sDelOK = "删除NPC成功...";
            var sParam1 = @Params.Length > 0 ? @Params[0] : "";
            if (sParam1 != "" && sParam1[0] == '?')
            {
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            TBaseObject BaseObject = PlayObject.GetPoseCreate();
            if (BaseObject != null)
            {
                for (var i = 0; i < M2Share.UserEngine.m_MerchantList.Count; i++)
                {
                    if (M2Share.UserEngine.m_MerchantList[i] == BaseObject)
                    {
                        BaseObject.m_boGhost = true;
                        BaseObject.m_dwGhostTick = HUtil32.GetTickCount();
                        BaseObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        PlayObject.SysMsg(sDelOK, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                }
                for (var i = 0; i < M2Share.UserEngine.QuestNPCList.Count; i++)
                {
                    if (M2Share.UserEngine.QuestNPCList[i] == BaseObject)
                    {
                        BaseObject.m_boGhost = true;
                        BaseObject.m_dwGhostTick = HUtil32.GetTickCount();
                        BaseObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        PlayObject.SysMsg(sDelOK, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                }
            }
            PlayObject.SysMsg(M2Share.g_sGameCommandDelNpcMsg, TMsgColor.c_Red, TMsgType.t_Hint);
        }
    }
}