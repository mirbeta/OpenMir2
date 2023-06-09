﻿using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    [Command("DisableSendMsg", "", "人物名称", 10)]
    public class DisableSendMsgCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName)) {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor != null) {
                mIPlayerActor.FilterSendMsg = true;
            }
            SystemShare.DisableSendMsgList.Add(sHumanName);
            SystemShare.SaveDisableSendMsgList();
            PlayerActor.SysMsg(sHumanName + " 已加入禁言列表。", MsgColor.Green, MsgType.Hint);
        }
    }
}