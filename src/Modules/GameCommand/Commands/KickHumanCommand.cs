using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 剔除指定玩家下线
    /// </summary>
    [Command("KickHuman", "剔除指定玩家下线", CommandHelp.GameCommandHumanLocalHelpMsg, 10)]
    public class KickHumanCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumName))
            {
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumName);
            if (mIPlayerActor != null)
            {
                mIPlayerActor.BoKickFlag = true;
                mIPlayerActor.BoEmergencyClose = true;
                //m_IPlayerActor.m_boPlayOffLine = false;
                //m_IPlayerActor.m_boNotOnlineAddExp = false;
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}