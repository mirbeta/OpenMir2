using GameSvr.Npc;
using GameSvr.Player;
using GameSvr.Script;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("NpcScript", "重新读取面对面NPC脚本", "重新读取面对面NPC脚本", 10)]
    public class NpcScriptCommand : BaseCommond
    {
        [DefaultCommand]
        public void NpcScript(PlayObject PlayObject)
        {
            var sScriptFileName = string.Empty;
            var nNPCType = -1;
            var BaseObject = PlayObject.GetPoseCreate();
            if (BaseObject != null)
            {
                for (var i = 0; i < M2Share.WorldEngine.MerchantList.Count; i++)
                {
                    if (M2Share.WorldEngine.MerchantList[i] == BaseObject)
                    {
                        nNPCType = 0;
                        break;
                    }
                }
                for (var i = 0; i < M2Share.WorldEngine.QuestNpcList.Count; i++)
                {
                    if (M2Share.WorldEngine.QuestNpcList[i] == BaseObject)
                    {
                        nNPCType = 1;
                        break;
                    }
                }
            }
            if (nNPCType < 0)
            {
                PlayObject.SysMsg("命令使用方法不正确，必须与NPC面对面，才能使用此命令!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nNPCType == 0)
            {
                var Merchant = (Merchant)BaseObject;
                sScriptFileName = M2Share.BasePath + M2Share.Config.EnvirDir + ScriptConst.sMarket_Def + Merchant.m_sScript + "-" + Merchant.MapName + ".txt";
            }
            if (nNPCType == 1)
            {
                var NormNpc = (NormNpc)BaseObject;
                sScriptFileName = M2Share.BasePath + M2Share.Config.EnvirDir + ScriptConst.sNpc_def + NormNpc.CharName + "-" + NormNpc.MapName + ".txt";
            }
            if (File.Exists(sScriptFileName))
            {
                var LoadList = new StringList();
                try
                {
                    LoadList.LoadFromFile(sScriptFileName);
                }
                catch
                {
                    PlayObject.SysMsg("读取脚本文件错误: " + sScriptFileName, MsgColor.Red, MsgType.Hint);
                }
                for (var i = 0; i < LoadList.Count; i++)
                {
                    var sScriptLine = LoadList[i].Trim();
                    sScriptLine = HUtil32.ReplaceChar(sScriptLine, ' ', ',');
                    PlayObject.SysMsg(i + "," + sScriptLine, MsgColor.Blue, MsgType.Hint);
                }
            }
        }
    }
}