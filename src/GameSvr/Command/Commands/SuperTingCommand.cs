using GameSvr.Actor;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 随机传送一个指定玩家和他身边的人
    /// </summary>
    [GameCommand("SuperTing", "随机传送一个指定玩家和他身边的人", GameCommandConst.g_sGameCommandSuperTingHelpMsg, 10)]
    public class SuperTingCommand : BaseCommond
    {
        [DefaultCommand]
        public void SuperTing(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sRange = @Params.Length > 1 ? @Params[1] : "";
            PlayObject MoveHuman;
            IList<TBaseObject> HumanList;
            if (sRange == "" || string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var nRange = HUtil32._MAX(10, HUtil32.Str_ToInt(sRange, 2));
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                HumanList = new List<TBaseObject>();
                M2Share.UserEngine.GetMapRageHuman(m_PlayObject.Envir, m_PlayObject.CurrX, m_PlayObject.CurrY, nRange, HumanList);
                for (var i = 0; i < HumanList.Count; i++)
                {
                    MoveHuman = HumanList[i] as PlayObject;
                    if (MoveHuman != PlayObject)
                    {
                        MoveHuman.MapRandomMove(MoveHuman.HomeMap, 0);
                    }
                }
                HumanList = null;
            }
            else
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}