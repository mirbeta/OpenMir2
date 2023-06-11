using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 剔除制定玩家下线
    /// </summary>
    [Command("Kill", "剔除面对面玩家下线", "玩家名称", 10)]
    public class KillCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            IActor baseObject;
            if (!string.IsNullOrEmpty(sHumanName))
            {
                baseObject = SystemShare.WorldEngine.GetPlayObject(sHumanName);
                if (baseObject == null)
                {
                    PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                    return;
                }
            }
            else
            {
                baseObject = PlayerActor.GetPoseCreate();
                if (baseObject == null)
                {
                    PlayerActor.SysMsg("命令使用方法不正确，必须与角色面对面站好!!!", MsgColor.Red, MsgType.Hint);
                    return;
                }
            }
            baseObject.Die();
        }
    }
}