using GameSvr.Actor;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 删除对面面NPC
    /// </summary>
    [GameCommand("DelNpc", "删除对面面NPC", 10)]
    public class DelNpcCommand : BaseCommond
    {
        [DefaultCommand]
        public void DelNpc(TPlayObject PlayObject)
        {
            const string sDelOK = "删除NPC成功...";
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
                        PlayObject.SysMsg(sDelOK, MsgColor.Red, MsgType.Hint);
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
                        PlayObject.SysMsg(sDelOK, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                }
            }
            PlayObject.SysMsg(M2Share.g_sGameCommandDelNpcMsg, MsgColor.Red, MsgType.Hint);
        }
    }
}