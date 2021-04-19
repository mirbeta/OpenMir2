using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server.Command
{
    /// <summary>
    /// 调整指定玩家游戏币
    /// </summary>
    [GameCommand("AddGameGold", "调整指定玩家游戏币", 10)]
    public class AddGameGoldCommand : BaseCommond
    {
        [DefaultCommand]
        public void AddGameGold(string[] @params, TPlayObject PlayObject)
        {
            TPlayObject m_PlayObject;
            var sHumName = @params.Length > 0 ? @params[0] : "";
            var nPoint = @params.Length > 1 ? Convert.ToInt32(@params[1]) : 0;
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
            if ((sHumName == "") || (nPoint <= 0))
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称  金币数量", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                if ((m_PlayObject.m_nGameGold + nPoint) < 2000000)
                {
                    m_PlayObject.m_nGameGold += nPoint;
                }
                else
                {
                    nPoint = 2000000 - m_PlayObject.m_nGameGold;
                    m_PlayObject.m_nGameGold = 2000000;
                }
                m_PlayObject.GoldChanged();
                PlayObject.SysMsg(sHumName + "的游戏点已增加" + nPoint + '.', TMsgColor.c_Green, TMsgType.t_Hint);
                m_PlayObject.SysMsg("游戏点已增加" + nPoint + '.', TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}