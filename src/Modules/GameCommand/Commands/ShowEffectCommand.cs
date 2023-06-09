using SystemModule;

namespace CommandModule.Commands
{
    /// <summary>
    /// 播放特效
    /// </summary>
    [Command("ShowEffect", "播放特效", 10)]
    public class ShowEffectCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sEffect = @params.Length > 0 ? @params[0] : "";
            var sTime = @params.Length > 1 ? @params[1] : "";
            //int nEffectType;
            //TFlowerEvent FlowerEvent = null;
            //if ((sEffect == "") || (HUtil32.StrToInt(sEffect, -1) <= 0))
            //{
            //    if (Settings.Config.boGMShowFailMsg)
            //    {
            //        PlayerActor.SysMsg("命令格式: @" + this.Attributes.Name + " 烟花类型(79-85)", MsgColor.c_Red, MsgType.t_Hint);
            //    }
            //    return;
            //}
            //nEffectType = HUtil32.StrToInt(sEffect, -1);
            //switch (nEffectType)
            //{
            //    case 79:
            //        FlowerEvent = new TFlowerEvent(PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, Grobal2.ET_FIREFLOWER_1, 4000);
            //        break;
            //    case 80:
            //        FlowerEvent = new TFlowerEvent(PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, Grobal2.ET_FIREFLOWER_2, 4000);
            //        break;
            //    case 81:
            //        FlowerEvent = new TFlowerEvent(PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, Grobal2.ET_FIREFLOWER_3, 4000);
            //        break;
            //    case 82:
            //        FlowerEvent = new TFlowerEvent(PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, Grobal2.ET_FIREFLOWER_4, 4000);
            //        break;
            //    case 83:
            //        FlowerEvent = new TFlowerEvent(PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, Grobal2.ET_FIREFLOWER_5, 4000);
            //        break;
            //    case 84:
            //        FlowerEvent = new TFlowerEvent(PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, Grobal2.ET_FIREFLOWER_6, 4000);
            //        break;
            //    case 85:// 如梦似雾烟花
            //        FlowerEvent = new TFlowerEvent(PlayerActor.SysMsgm_PEnvir, PlayerActor.CurrX, PlayerActor.CurrY, Grobal2.ET_FIREFLOWER_7, 4000);
            //        break;
            //    case 87:// 护体神盾  受攻击
            //        PlayerActor.SendRefMsg(Messages.RM_MYSHOW, Grobal2.ET_PROTECTION_STRUCK, 0, 0, 0, "");
            //        break;
            //    case 89:// 护体神盾  破盾
            //        PlayerActor.SendRefMsg(Messages.RM_MYSHOW, Grobal2.ET_PROTECTION_PIP, 0, 0, 0, "");
            //        break;
            //    case 90: // 人物升级动画
            //        PlayerActor.SendRefMsg(Messages.RM_MYSHOW, Grobal2.ET_OBJECTLEVELUP, 0, 0, 0, "");
            //        break;
            //}
            //switch (nEffectType)
            //{
            //    // Modify the A .. B: 79 .. 85
            //    case 79:
            //        Settings.g_EventManager.AddEvent(FlowerEvent);
            //        break;
            //}
        }
    }
}