using GameSrv.Actor;
using GameSrv.Npc;
using GameSrv.Player;
using GameSrv.Script;
using SystemModule.Common;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("NpcScript", "重新读取面对面NPC脚本", "重新读取面对面NPC脚本", 10)]
    public class NpcScriptCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            string sScriptFileName = string.Empty;
            int nNpcType = -1;
            BaseObject baseObject = playObject.GetPoseCreate();
            if (baseObject != null) {
                for (int i = 0; i < M2Share.WorldEngine.MerchantList.Count; i++) {
                    if (M2Share.WorldEngine.MerchantList[i] == baseObject) {
                        nNpcType = 0;
                        break;
                    }
                }
                for (int i = 0; i < M2Share.WorldEngine.QuestNpcList.Count; i++) {
                    if (M2Share.WorldEngine.QuestNpcList[i] == baseObject) {
                        nNpcType = 1;
                        break;
                    }
                }
            }
            if (nNpcType < 0) {
                playObject.SysMsg("命令使用方法不正确，必须与NPC面对面，才能使用此命令!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nNpcType == 0) {
                Merchant merchant = (Merchant)baseObject;
                sScriptFileName = M2Share.GetEnvirFilePath(ScriptConst.sMarket_Def + merchant.ScriptName + "-" + merchant.MapName + ".txt");
            }
            if (nNpcType == 1) {
                NormNpc normNpc = (NormNpc)baseObject;
                sScriptFileName = M2Share.GetEnvirFilePath(ScriptConst.sNpc_def + normNpc.ChrName + "-" + normNpc.MapName + ".txt");
            }
            if (File.Exists(sScriptFileName)) {
                using StringList loadList = new StringList();
                try {
                    loadList.LoadFromFile(sScriptFileName);
                }
                catch {
                    playObject.SysMsg("读取脚本文件错误: " + sScriptFileName, MsgColor.Red, MsgType.Hint);
                }
                for (int i = 0; i < loadList.Count; i++) {
                    string sScriptLine = loadList[i].Trim();
                    sScriptLine = HUtil32.ReplaceChar(sScriptLine, ' ', ',');
                    playObject.SysMsg(i + "," + sScriptLine, MsgColor.Blue, MsgType.Hint);
                }
            }
        }
    }
}