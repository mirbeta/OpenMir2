using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [Command("DelGold", "调整指定玩家游戏币", help: "人物名称 数量", 10)]
    public class DelGoldCommand : Command
    {
        [ExecuteCommand]
        public void DelGold(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumName = @Params.Length > 0 ? @Params[0] : "";
            int nCount = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (sHumName == "" || nCount <= 0)
            {
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (sHumName == "" || nCount <= 0)
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.Gold > nCount)
                {
                    m_PlayObject.Gold -= nCount;
                }
                else
                {
                    nCount = m_PlayObject.Gold;
                    m_PlayObject.Gold = 0;
                }
                m_PlayObject.GoldChanged();
                PlayObject.SysMsg(sHumName + "的金币已减少" + nCount + ".", MsgColor.Green, MsgType.Hint);
                if (M2Share.GameLogGold)
                {
                    M2Share.EventSource.AddEventLog(13, PlayObject.MapName + "\09" + PlayObject.CurrX + "\09" + PlayObject.CurrY + "\09"
                                                        + PlayObject.ChrName + "\09" + Grobal2.sSTRING_GOLDNAME + "\09" + nCount + "\09" + "1" + "\09" + sHumName);
                }
            }
            else
            {
                int nServerIndex = 0;
                if (M2Share.WorldEngine.FindOtherServerUser(sHumName, ref nServerIndex))
                {
                    PlayObject.SysMsg(sHumName + "现在" + nServerIndex + "号服务器上", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    M2Share.FrontEngine.AddChangeGoldList(PlayObject.ChrName, sHumName, -nCount);
                    PlayObject.SysMsg(sHumName + "现在不在线，等其上线时金币将自动减少", MsgColor.Green, MsgType.Hint);
                }
            }
        }
    }
}