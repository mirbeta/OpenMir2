using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 播放特效
    /// </summary>
    [GameCommand("ShowEffect", "播放特效", 10)]
    public class ShowEffectCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowEffect(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sEffect = @Params.Length > 0 ? @Params[0] : "";
            var sTime = @Params.Length > 1 ? @Params[1] : "";
            //int nEffectType;
            //TFlowerEvent FlowerEvent = null;
            //if ((sEffect == "") || (HUtil32.Str_ToInt(sEffect, -1) <= 0))
            //{
            //    if (M2Share.g_Config.boGMShowFailMsg)
            //    {
            //        PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 烟花类型(79-85)", TMsgColor.c_Red, TMsgType.t_Hint);
            //    }
            //    return;
            //}
            //nEffectType = HUtil32.Str_ToInt(sEffect, -1);
            //switch (nEffectType)
            //{
            //    case 79:
            //        FlowerEvent = new TFlowerEvent(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, Grobal2.ET_FIREFLOWER_1, 4000);
            //        break;
            //    case 80:
            //        FlowerEvent = new TFlowerEvent(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, Grobal2.ET_FIREFLOWER_2, 4000);
            //        break;
            //    case 81:
            //        FlowerEvent = new TFlowerEvent(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, Grobal2.ET_FIREFLOWER_3, 4000);
            //        break;
            //    case 82:
            //        FlowerEvent = new TFlowerEvent(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, Grobal2.ET_FIREFLOWER_4, 4000);
            //        break;
            //    case 83:
            //        FlowerEvent = new TFlowerEvent(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, Grobal2.ET_FIREFLOWER_5, 4000);
            //        break;
            //    case 84:
            //        FlowerEvent = new TFlowerEvent(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, Grobal2.ET_FIREFLOWER_6, 4000);
            //        break;
            //    case 85:// 如梦似雾烟花
            //        FlowerEvent = new TFlowerEvent(PlayObject.m_PEnvir, PlayObject.m_nCurrX, PlayObject.m_nCurrY, Grobal2.ET_FIREFLOWER_7, 4000);
            //        break;
            //    case 87:// 护体神盾  受攻击
            //        PlayObject.SendRefMsg(Grobal2.RM_MYSHOW, Grobal2.ET_PROTECTION_STRUCK, 0, 0, 0, "");
            //        break;
            //    case 89:// 护体神盾  破盾
            //        PlayObject.SendRefMsg(Grobal2.RM_MYSHOW, Grobal2.ET_PROTECTION_PIP, 0, 0, 0, "");
            //        break;
            //    case 90: // 人物升级动画
            //        PlayObject.SendRefMsg(Grobal2.RM_MYSHOW, Grobal2.ET_OBJECTLEVELUP, 0, 0, 0, "");
            //        break;
            //}
            //switch (nEffectType)
            //{
            //    // Modify the A .. B: 79 .. 85
            //    case 79:
            //        M2Share.g_EventManager.AddEvent(FlowerEvent);
            //        break;
            //}
        }
    }
}