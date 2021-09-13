using SystemModule;
using System;
using System.Collections.Generic;
using GameSvr.CommandSystem;
using System.Collections;

namespace GameSvr
{
    /// <summary>
    /// 随机传送一个指定玩家和他身边的人
    /// </summary>
    [GameCommand("SuperTing", "随机传送一个指定玩家和他身边的人", 10)]
    public class SuperTingCommand : BaseCommond
    {
        [DefaultCommand]
        public void SuperTing(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sRange = @Params.Length > 1 ? @Params[1] : "";
            TPlayObject m_PlayObject;
            TPlayObject MoveHuman;
            IList<TBaseObject> HumanList;
            if (sRange == "" || sHumanName == "" || sHumanName != "" && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandSuperTingHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var nRange = HUtil32._MAX(10, HUtil32.Str_ToInt(sRange, 2));
            m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                HumanList = new List<TBaseObject>();
                M2Share.UserEngine.GetMapRageHuman(m_PlayObject.m_PEnvir, m_PlayObject.m_nCurrX, m_PlayObject.m_nCurrY, nRange, HumanList);
                for (var i = 0; i < HumanList.Count; i++)
                {
                    MoveHuman = HumanList[i] as TPlayObject;
                    if (MoveHuman != PlayObject)
                    {
                        MoveHuman.MapRandomMove(MoveHuman.m_sHomeMap, 0);
                    }
                }
                HumanList = null;
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}