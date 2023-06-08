using M2Server.Actor;
using M2Server.Player;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands
{
    /// <summary>
    /// 删除对面面NPC
    /// </summary>
    [Command("DelNpc", "删除对面面NPC", 10)]
    public class DelNpcCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(PlayObject playObject)
        {
            const string sDelOk = "删除NPC成功...";
            var baseObject = playObject.GetPoseCreate();
            if (baseObject != null)
            {
                //for (var i = 0; i < M2Share.WorldEngine.MerchantList.Count; i++)
                //{
                //    if (M2Share.WorldEngine.MerchantList[i] == baseObject)
                //    {
                //        baseObject.Ghost = true;
                //        baseObject.GhostTick = HUtil32.GetTickCount();
                //        baseObject.SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");
                //        playObject.SysMsg(sDelOk, MsgColor.Red, MsgType.Hint);
                //        return;
                //    }
                //}
                //for (var i = 0; i < M2Share.WorldEngine.QuestNpcList.Count; i++)
                //{
                //    if (M2Share.WorldEngine.QuestNpcList[i] == baseObject)
                //    {
                //        baseObject.Ghost = true;
                //        baseObject.GhostTick = HUtil32.GetTickCount();
                //        baseObject.SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");
                //        playObject.SysMsg(sDelOk, MsgColor.Red, MsgType.Hint);
                //        return;
                //    }
                //}
            }
            playObject.SysMsg(CommandHelp.GameCommandDelNpcMsg, MsgColor.Red, MsgType.Hint);
        }
    }
}