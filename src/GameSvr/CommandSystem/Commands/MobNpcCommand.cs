using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 在当前XY坐标创建NPC
    /// </summary>
    [GameCommand("MobNpc", "在当前XY坐标创建NPC", 10)]
    public class MobNpcCommand : BaseCommond
    {
        [DefaultCommand]
        public void MobNpc(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sParam1 = @Params.Length > 0 ? @Params[0] : "";
            var sParam2 = @Params.Length > 1 ? @Params[1] : "";
            var sParam3 = @Params.Length > 2 ? @Params[2] : "";
            var sParam4 = @Params.Length > 3 ? @Params[3] : "";
            if (sParam1 == "" || sParam2 == "" || sParam1 != "" && sParam1[0] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandMobNpcHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var nAppr = HUtil32.Str_ToInt(sParam3, 0);
            var boIsCastle = HUtil32.Str_ToInt(sParam4, 0) == 1;
            if (sParam1 == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " NPC名称 脚本文件名 外形(数字) 属沙城(0,1)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            short nX = 0;
            short nY = 0;
            TMerchant  Merchant = new TMerchant();
            Merchant.m_sCharName = sParam1;
            Merchant.m_sMapName = PlayObject.m_sMapName;
            Merchant.m_PEnvir = PlayObject.m_PEnvir;
            Merchant.m_wAppr = (ushort)nAppr;
            Merchant.m_nFlag = 0;
            Merchant.m_boCastle = boIsCastle;
            Merchant.m_sScript = sParam2;
            PlayObject.GetFrontPosition(ref nX, ref nY);
            Merchant.m_nCurrX = nX;
            Merchant.m_nCurrY = nY;
            Merchant.Initialize();
            M2Share.UserEngine.AddMerchant(Merchant);
        }
    }
}