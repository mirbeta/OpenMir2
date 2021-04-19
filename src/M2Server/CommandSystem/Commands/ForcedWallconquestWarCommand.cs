using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 开始工程战役
    /// </summary>
    [GameCommand("ForcedWallconquestWar", "开始工程战役", 10)]
    public class ForcedWallconquestWarCommand : BaseCommond
    {
        [DefaultCommand]
        public void ForcedWallconquestWar(string[] @Params, TPlayObject PlayObject)
        {
            var sCASTLENAME = @Params.Length > 0 ? @Params[0] : "";
            if (sCASTLENAME == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 城堡名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Castle = M2Share.CastleManager.Find(sCASTLENAME);
            if (Castle != null)
            {
                Castle.m_boUnderWar = !Castle.m_boUnderWar;
                if (Castle.m_boUnderWar)
                {
                    Castle.m_boShowOverMsg = false;
                    Castle.m_WarDate = DateTime.Now;
                    Castle.m_dwStartCastleWarTick = HUtil32.GetTickCount();
                    Castle.StartWallconquestWar();
                    M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_212, M2Share.nServerIndex, "");
                    var s20 = "[" + Castle.m_sName + " 攻城战已经开始]";
                    M2Share.UserEngine.SendBroadCastMsg(s20, TMsgType.t_System);
                    M2Share.UserEngine.SendServerGroupMsg(grobal2.SS_204, M2Share.nServerIndex, s20);
                    Castle.MainDoorControl(true);
                }
                else
                {
                    Castle.StopWallconquestWar();
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandSbkGoldCastleNotFoundMsg, sCASTLENAME), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}