using GameSrv.Actor;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 随机传送一个指定玩家和他身边的人
    /// </summary>
    [Command("SuperTing", "随机传送一个指定玩家和他身边的人", CommandHelp.GameCommandSuperTingHelpMsg, 10)]
    public class SuperTingCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sRange = @Params.Length > 1 ? @Params[1] : "";
            PlayObject MoveHuman;
            IList<BaseObject> HumanList;
            if (string.IsNullOrEmpty(sRange) || string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            int nRange = HUtil32._MAX(10, HUtil32.StrToInt(sRange, 2));
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null) {
                HumanList = new List<BaseObject>();
                M2Share.WorldEngine.GetMapRageHuman(m_PlayObject.Envir, m_PlayObject.CurrX, m_PlayObject.CurrY, nRange, ref HumanList);
                for (int i = 0; i < HumanList.Count; i++) {
                    MoveHuman = HumanList[i] as PlayObject;
                    if (MoveHuman != PlayObject) {
                        MoveHuman.MapRandomMove(MoveHuman.HomeMap, 0);
                    }
                }
            }
            else {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}