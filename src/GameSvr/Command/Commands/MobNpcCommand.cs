using GameSvr.Npc;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 在当前XY坐标创建NPC
    /// </summary>
    [GameCommand("MobNpc", "在当前XY坐标创建NPC", M2Share.g_sGameCommandMobNpcHelpMsg, 10)]
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
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var nAppr = HUtil32.Str_ToInt(sParam3, 0);
            var boIsCastle = HUtil32.Str_ToInt(sParam4, 0) == 1;
            if (sParam1 == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            short nX = 0;
            short nY = 0;
            Merchant Merchant = new Merchant();
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
            Merchant.OnEnvirnomentChanged();
            M2Share.UserEngine.AddMerchant(Merchant);
        }
    }
}