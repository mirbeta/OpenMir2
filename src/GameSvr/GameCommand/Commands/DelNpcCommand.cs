using GameSvr.Actor;
using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 删除对面面NPC
    /// </summary>
    [Command("DelNpc", "删除对面面NPC", 10)]
    public class DelNpcCommand : Command
    {
        [ExecuteCommand]
        public static void DelNpc(PlayObject PlayObject)
        {
            const string sDelOK = "删除NPC成功...";
            BaseObject BaseObject = PlayObject.GetPoseCreate();
            if (BaseObject != null)
            {
                for (var i = 0; i < M2Share.WorldEngine.MerchantList.Count; i++)
                {
                    if (M2Share.WorldEngine.MerchantList[i] == BaseObject)
                    {
                        BaseObject.Ghost = true;
                        BaseObject.GhostTick = HUtil32.GetTickCount();
                        BaseObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        PlayObject.SysMsg(sDelOK, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                }
                for (var i = 0; i < M2Share.WorldEngine.QuestNpcList.Count; i++)
                {
                    if (M2Share.WorldEngine.QuestNpcList[i] == BaseObject)
                    {
                        BaseObject.Ghost = true;
                        BaseObject.GhostTick = HUtil32.GetTickCount();
                        BaseObject.SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        PlayObject.SysMsg(sDelOK, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                }
            }
            PlayObject.SysMsg(CommandHelp.GameCommandDelNpcMsg, MsgColor.Red, MsgType.Hint);
        }
    }
}