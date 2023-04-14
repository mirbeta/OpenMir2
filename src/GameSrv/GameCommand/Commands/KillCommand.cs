using GameSrv.Actor;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 剔除制定玩家下线
    /// </summary>
    [Command("Kill", "剔除面对面玩家下线", "玩家名称", 10)]
    public class KillCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            string sHumanName = @params.Length > 0 ? @params[0] : "";
            BaseObject baseObject;
            if (!string.IsNullOrEmpty(sHumanName)) {
                baseObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
                if (baseObject == null) {
                    playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                    return;
                }
            }
            else {
                baseObject = playObject.GetPoseCreate();
                if (baseObject == null) {
                    playObject.SysMsg("命令使用方法不正确，必须与角色面对面站好!!!", MsgColor.Red, MsgType.Hint);
                    return;
                }
            }
            baseObject.Die();
        }
    }
}