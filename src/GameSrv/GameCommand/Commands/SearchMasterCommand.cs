using GameSrv.Player;
using M2Server;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 查询师徒当前所在位置
    /// </summary>
    [Command("SearchMaster", "查询师徒当前所在位置")]
    public class SearchMasterCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (string.IsNullOrEmpty(playObject.MasterName)) {
                playObject.SysMsg(Settings.YouAreNotMasterMsg, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.IsMaster) {
                if (playObject.MasterList.Count <= 0) {
                    playObject.SysMsg(Settings.YourMasterListNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                playObject.SysMsg(Settings.YourMasterListNowLocateMsg, MsgColor.Green, MsgType.Hint);
                for (var i = 0; i < playObject.MasterList.Count; i++) {
                    var human = playObject.MasterList[i];
                    playObject.SysMsg(human.ChrName + " " + human.Envir.MapDesc + "(" + human.CurrX + ":" + human.CurrY + ")", MsgColor.Green, MsgType.Hint);
                    human.SysMsg(Settings.YourMasterSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                    human.SysMsg(playObject.ChrName + " " + playObject.Envir.MapDesc + "(" + playObject.CurrX + ":" + playObject.CurrY + ")", MsgColor.Green, MsgType.Hint);
                }
            }
            else {
                if (playObject.MasterHuman == null) {
                    playObject.SysMsg(Settings.YourMasterNotOnlineMsg, MsgColor.Red, MsgType.Hint);
                    return;
                }
                playObject.SysMsg(Settings.YourMasterNowLocateMsg, MsgColor.Red, MsgType.Hint);
                playObject.SysMsg(playObject.MasterHuman.ChrName + " " + playObject.MasterHuman.Envir.MapDesc + "(" + playObject.MasterHuman.CurrX + ":"
                    + playObject.MasterHuman.CurrY + ")", MsgColor.Green, MsgType.Hint);
                playObject.MasterHuman.SysMsg(Settings.YourMasterListSearchLocateMsg, MsgColor.Green, MsgType.Hint);
                playObject.MasterHuman.SysMsg(playObject.ChrName + " " + playObject.Envir.MapDesc + "(" + playObject.CurrX + ":" + playObject.CurrY + ")",
                    MsgColor.Green, MsgType.Hint);
            }
        }
    }
}