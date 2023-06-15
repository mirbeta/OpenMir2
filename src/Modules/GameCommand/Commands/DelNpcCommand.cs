using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 删除对面面NPC
    /// </summary>
    [Command("DelNpc", "删除对面面NPC", 10)]
    public class DelNpcCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            var baseObject = PlayerActor.GetPoseCreate();
            if (baseObject != null)
            {
                //for (var i = 0; i < SystemShare.WorldEngine.MerchantList.Count; i++)
                //{
                //    if (SystemShare.WorldEngine.MerchantList[i] == baseObject)
                //    {
                //        baseObject.Ghost = true;
                //        baseObject.GhostTick = HUtil32.GetTickCount();
                //        baseObject.SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");
                //        PlayerActor.SysMsg(sDelOk, MsgColor.Red, MsgType.Hint);
                //        return;
                //    }
                //}
                //for (var i = 0; i < SystemShare.WorldEngine.QuestNpcList.Count; i++)
                //{
                //    if (SystemShare.WorldEngine.QuestNpcList[i] == baseObject)
                //    {
                //        baseObject.Ghost = true;
                //        baseObject.GhostTick = HUtil32.GetTickCount();
                //        baseObject.SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");
                //        PlayerActor.SysMsg(sDelOk, MsgColor.Red, MsgType.Hint);
                //        return;
                //    }
                //}
            }
            PlayerActor.SysMsg(CommandHelp.GameCommandDelNpcMsg, MsgColor.Red, MsgType.Hint);
        }
    }
}