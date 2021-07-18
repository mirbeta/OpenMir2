using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
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
            var nPermission = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            var sParam1 = @Params.Length > 1 ? @Params[1] : "";
            TBaseObject BaseObject;
            const string sDelOK = "删除NPC成功...";
            if (sParam1 != "" && sParam1[0] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, new string[] { this.Attributes.Name, "" }), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            BaseObject = PlayObject.GetPoseCreate();
            if (BaseObject != null)
            {
                for (var i = 0; i < M2Share.UserEngine.m_MerchantList.Count; i++)
                {
                    if (M2Share.UserEngine.m_MerchantList[i] == BaseObject)
                    {
                        BaseObject.m_boGhost = true;

                        BaseObject.m_dwGhostTick = HUtil32.GetTickCount();
                        BaseObject.SendRefMsg(grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
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
                        BaseObject.SendRefMsg(grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        PlayObject.SysMsg(sDelOK, TMsgColor.c_Red, TMsgType.t_Hint);
                        return;
                    }
                }
            }
            PlayObject.SysMsg(M2Share.g_sGameCommandDelNpcMsg, TMsgColor.c_Red, TMsgType.t_Hint);
        }
    }
}