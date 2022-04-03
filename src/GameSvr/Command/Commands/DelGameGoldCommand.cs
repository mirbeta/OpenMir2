using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [GameCommand("DelGameGold", "调整指定玩家游戏币", help: "人物名称 数量", 10)]
    public class DelGameGoldCommand : BaseCommond
    {
        [DefaultCommand]
        public void DelGameGold(string[] @params, TPlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            TPlayObject m_PlayObject;
            var sHumName = @params.Length > 0 ? @params[0] : ""; //玩家名称
            var nPoint = @params.Length > 1 ? Convert.ToInt32(@params[1]) : 0; //数量
            if (sHumName == "" || nPoint <= 0)
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.m_nGameGold > nPoint)
                {
                    m_PlayObject.m_nGameGold -= nPoint;
                }
                else
                {
                    nPoint = m_PlayObject.m_nGameGold;
                    m_PlayObject.m_nGameGold = 0;
                }
                m_PlayObject.GoldChanged();
                PlayObject.SysMsg(sHumName + "的游戏点已减少" + nPoint + '.', MsgColor.Green, MsgType.Hint);
                m_PlayObject.SysMsg("游戏点已减少" + nPoint + '.', MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}