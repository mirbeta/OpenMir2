using GameSvr.Actor;
using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 剔除制定玩家下线
    /// </summary>
    [GameCommand("Kill", "剔除面对面玩家下线", "玩家名称", 10)]
    public class KillCommand : BaseCommond
    {
        [DefaultCommand]
        public void Kill(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            BaseObject BaseObject;
            if (!string.IsNullOrEmpty(sHumanName))
            {
                BaseObject = M2Share.UserEngine.GetPlayObject(sHumanName);
                if (BaseObject == null)
                {
                    PlayObject.SysMsg(string.Format(GameCommandConst.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                    return;
                }
            }
            else
            {
                BaseObject = PlayObject.GetPoseCreate();
                if (BaseObject == null)
                {
                    PlayObject.SysMsg("命令使用方法不正确，必须与角色面对面站好!!!", MsgColor.Red, MsgType.Hint);
                    return;
                }
            }
            BaseObject.Die();
        }
    }
}