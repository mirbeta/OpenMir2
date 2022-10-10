using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("AddGameGold", "调整指定玩家游戏币", "人物名称  金币数量", 10)]
    public class AddGameGoldCommand : Command
    {
        [ExecuteCommand]
        public void AddGameGold(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";
            var nPoint = @params.Length > 1 ? Convert.ToInt32(@params[1]) : 0;
            if (PlayObject.Permission < 6)
            {
                return;
            }
            if (string.IsNullOrEmpty(sHumName) || nPoint <= 0)
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.m_nGameGold + nPoint < 2000000)
                {
                    m_PlayObject.m_nGameGold += nPoint;
                }
                else
                {
                    nPoint = 2000000 - m_PlayObject.m_nGameGold;
                    m_PlayObject.m_nGameGold = 2000000;
                }
                m_PlayObject.GoldChanged();
                PlayObject.SysMsg(sHumName + "的游戏点已增加" + nPoint + '.', MsgColor.Green, MsgType.Hint);
                m_PlayObject.SysMsg("游戏点已增加" + nPoint + '.', MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}