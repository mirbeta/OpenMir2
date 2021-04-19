using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 剔除面对面玩家下线
    /// </summary>
    [GameCommand("Kill", "剔除面对面玩家下线", 10)]
    public class KillCommand : BaseCommond
    {
        [DefaultCommand]
        public void Kill(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            TBaseObject BaseObject;
            if (sHumanName != "")
            {
                BaseObject = M2Share.UserEngine.GetPlayObject(sHumanName);
                if (BaseObject == null)
                {
                    PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
            }
            else
            {
                BaseObject = PlayObject.GetPoseCreate();
                if (BaseObject == null)
                {
                    PlayObject.SysMsg("命令使用方法不正确，必须与角色面对面站好！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
            }
            BaseObject.Die();
        }
    }
}