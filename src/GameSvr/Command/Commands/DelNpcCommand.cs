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
        public void DelNpc(PlayObject PlayObject)
        {
            const string sDelOK = "删除NPC成功...";
            BaseObject BaseObject = PlayObject.GetPoseCreate();
            if (BaseObject != null)
            {
                for (var i = 0; i < M2Share.UserEngine.MerchantList.Count; i++)
                {
                    if (M2Share.UserEngine.MerchantList[i] == BaseObject)
                    {
                        BaseObject.Ghost = true;
                        BaseObject.GhostTick = HUtil32.GetTickCount();
                        BaseObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        PlayObject.SysMsg(sDelOK, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                }
                for (var i = 0; i < M2Share.UserEngine.QuestNpcList.Count; i++)
                {
                    if (M2Share.UserEngine.QuestNpcList[i] == BaseObject)
                    {
                        BaseObject.Ghost = true;
                        BaseObject.GhostTick = HUtil32.GetTickCount();
                        BaseObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        PlayObject.SysMsg(sDelOK, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                }
            }
            PlayObject.SysMsg(GameCommandConst.g_sGameCommandDelNpcMsg, MsgColor.Red, MsgType.Hint);
        }
    }
}