using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("DelGameGold", "调整指定玩家游戏币", help: "人物名称 数量", 10)]
    public class DelGameGoldCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            string sHumName = @params.Length > 0 ? @params[0] : ""; //玩家名称
            int nPoint = @params.Length > 1 ? Convert.ToInt32(@params[1]) : 0; //数量
            if (sHumName == "" || nPoint <= 0)
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.GameGold > nPoint)
                {
                    m_PlayObject.GameGold -= nPoint;
                }
                else
                {
                    nPoint = m_PlayObject.GameGold;
                    m_PlayObject.GameGold = 0;
                }
                m_PlayObject.GoldChanged();
                PlayObject.SysMsg(sHumName + "的游戏点已减少" + nPoint + '.', MsgColor.Green, MsgType.Hint);
                m_PlayObject.SysMsg("游戏点已减少" + nPoint + '.', MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}