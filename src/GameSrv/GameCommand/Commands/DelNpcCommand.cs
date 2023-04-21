using GameSrv.Actor;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 删除对面面NPC
    /// </summary>
    [Command("DelNpc", "删除对面面NPC", 10)]
    public class DelNpcCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            const string sDelOk = "删除NPC成功...";
            var baseObject = playObject.GetPoseCreate();
            if (baseObject != null) {
                for (var i = 0; i < GameShare.WorldEngine.MerchantList.Count; i++) {
                    if (GameShare.WorldEngine.MerchantList[i] == baseObject) {
                        baseObject.Ghost = true;
                        baseObject.GhostTick = HUtil32.GetTickCount();
                        baseObject.SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        playObject.SysMsg(sDelOk, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                }
                for (var i = 0; i < GameShare.WorldEngine.QuestNpcList.Count; i++) {
                    if (GameShare.WorldEngine.QuestNpcList[i] == baseObject) {
                        baseObject.Ghost = true;
                        baseObject.GhostTick = HUtil32.GetTickCount();
                        baseObject.SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        playObject.SysMsg(sDelOk, MsgColor.Red, MsgType.Hint);
                        return;
                    }
                }
            }
            playObject.SysMsg(CommandHelp.GameCommandDelNpcMsg, MsgColor.Red, MsgType.Hint);
        }
    }
}