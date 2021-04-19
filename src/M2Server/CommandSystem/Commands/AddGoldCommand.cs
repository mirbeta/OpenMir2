using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 调整指定玩家金币
    /// </summary>
    [GameCommand("AddGold", "调整指定玩家金币", 10)]
    public class AddGoldCommand : BaseCommond
    {
        [DefaultCommand]
        public void AddGold(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            TPlayObject m_PlayObject;
            var sHumName = @Params.Length > 0 ? @Params[0] : "";//玩家名称
            var nCount = @Params.Length > 1 ? Convert.ToInt32(@Params[1]) : 0;//金币数量
            var nServerIndex = 0;
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
            if (sHumName == "" || nCount <= 0)
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称  金币数量", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.m_nGold + nCount < m_PlayObject.m_nGoldMax)
                {
                    m_PlayObject.m_nGold += nCount;
                }
                else
                {
                    nCount = m_PlayObject.m_nGoldMax - m_PlayObject.m_nGold;
                    m_PlayObject.m_nGold = m_PlayObject.m_nGoldMax;
                }
                m_PlayObject.GoldChanged();
                PlayObject.SysMsg(sHumName + "的金币已增加" + nCount + ".", TMsgColor.c_Green, TMsgType.t_Hint);
                if (M2Share.g_boGameLogGold)
                {
                    //M2Share.AddGameDataLog("14" + "\09" + PlayObject.m_sMapName + "\09" + (PlayObject.m_nCurrX).ToString() + "\09" + (PlayObject.m_nCurrY).ToString() 
                    //    + "\09" + PlayObject.m_sCharName + "\09" + M2Share.sSTRING_GOLDNAME + "\09" + (nCount).ToString() + "\09" + "1" + "\09" + sHumName);
                }
            }
            else
            {
                if (M2Share.UserEngine.FindOtherServerUser(sHumName, ref nServerIndex))
                {
                    PlayObject.SysMsg(sHumName + " 现在" + nServerIndex + "号服务器上", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    M2Share.FrontEngine.AddChangeGoldList(PlayObject.m_sCharName, sHumName, nCount);
                    PlayObject.SysMsg(sHumName + " 现在不在线，等其上线时金币将自动增加", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
        }
    }
}