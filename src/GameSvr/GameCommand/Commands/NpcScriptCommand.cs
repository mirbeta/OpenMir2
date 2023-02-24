using GameSvr.Npc;
using GameSvr.Player;
using GameSvr.Script;
using SystemModule.Common;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    [Command("NpcScript", "重新读取面对面NPC脚本", "重新读取面对面NPC脚本", 10)]
    public class NpcScriptCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            string sScriptFileName = string.Empty;
            int nNPCType = -1;
            Actor.BaseObject BaseObject = PlayObject.GetPoseCreate();
            if (BaseObject != null) {
                for (int i = 0; i < M2Share.WorldEngine.MerchantList.Count; i++) {
                    if (M2Share.WorldEngine.MerchantList[i] == BaseObject) {
                        nNPCType = 0;
                        break;
                    }
                }
                for (int i = 0; i < M2Share.WorldEngine.QuestNpcList.Count; i++) {
                    if (M2Share.WorldEngine.QuestNpcList[i] == BaseObject) {
                        nNPCType = 1;
                        break;
                    }
                }
            }
            if (nNPCType < 0) {
                PlayObject.SysMsg("命令使用方法不正确，必须与NPC面对面，才能使用此命令!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nNPCType == 0) {
                Merchant Merchant = (Merchant)BaseObject;
                sScriptFileName = M2Share.GetEnvirFilePath(ScriptConst.sMarket_Def + Merchant.ScriptName + "-" + Merchant.MapName + ".txt");
            }
            if (nNPCType == 1) {
                NormNpc NormNpc = (NormNpc)BaseObject;
                sScriptFileName = M2Share.GetEnvirFilePath(ScriptConst.sNpc_def + NormNpc.ChrName + "-" + NormNpc.MapName + ".txt");
            }
            if (File.Exists(sScriptFileName)) {
                using StringList LoadList = new StringList();
                try {
                    LoadList.LoadFromFile(sScriptFileName);
                }
                catch {
                    PlayObject.SysMsg("读取脚本文件错误: " + sScriptFileName, MsgColor.Red, MsgType.Hint);
                }
                for (int i = 0; i < LoadList.Count; i++) {
                    string sScriptLine = LoadList[i].Trim();
                    sScriptLine = HUtil32.ReplaceChar(sScriptLine, ' ', ',');
                    PlayObject.SysMsg(i + "," + sScriptLine, MsgColor.Blue, MsgType.Hint);
                }
            }
        }
    }
}