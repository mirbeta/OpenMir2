using SystemModule;
using SystemModule.Common;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("NpcScript", "重新读取面对面NPC脚本", "重新读取面对面NPC脚本", 10)]
    public class NpcScriptCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            var sScriptFileName = string.Empty;
            var baseObject = PlayerActor.GetPoseCreate();
            //if (baseObject != null)
            //{
            //    for (var i = 0; i < M2Share.WorldEngine.MerchantList.Count; i++)
            //    {
            //        if (M2Share.WorldEngine.MerchantList[i] == baseObject)
            //        {
            //            nNpcType = 0;
            //            break;
            //        }
            //    }
            //    for (var i = 0; i < M2Share.WorldEngine.QuestNpcList.Count; i++)
            //    {
            //        if (M2Share.WorldEngine.QuestNpcList[i] == baseObject)
            //        {
            //            nNpcType = 1;
            //            break;
            //        }
            //    }
            //}
            //if (nNpcType < 0)
            //{
            //    PlayerActor.SysMsg("命令使用方法不正确，必须与NPC面对面，才能使用此命令!!!", MsgColor.Red, MsgType.Hint);
            //    return;
            //}
            //if (nNpcType == 0)
            //{
            //    var merchant = (Merchant)baseObject;
            //    sScriptFileName = M2Share.GetEnvirFilePath(ScriptFlagConst.sMarket_Def + merchant.ScriptName + "-" + merchant.MapName + ".txt");
            //}
            //if (nNpcType == 1)
            //{
            //    var normNpc = (NormNpc)baseObject;
            //    sScriptFileName = M2Share.GetEnvirFilePath(ScriptFlagConst.sNpc_def + normNpc.ChrName + "-" + normNpc.MapName + ".txt");
            //}
            if (File.Exists(sScriptFileName))
            {
                using var loadList = new StringList();
                try
                {
                    loadList.LoadFromFile(sScriptFileName);
                }
                catch
                {
                    PlayerActor.SysMsg("读取脚本文件错误: " + sScriptFileName, MsgColor.Red, MsgType.Hint);
                }
                for (var i = 0; i < loadList.Count; i++)
                {
                    var sScriptLine = loadList[i].Trim();
                    sScriptLine = HUtil32.ReplaceChar(sScriptLine, ' ', ',');
                    PlayerActor.SysMsg(i + "," + sScriptLine, MsgColor.Blue, MsgType.Hint);
                }
            }
        }
    }
}