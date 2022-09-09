using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整指定玩家师傅名称
    /// </summary>
    [GameCommand("ChangeMasterName", "调整指定玩家师傅名称", "人物名称 师徒名称(如果为 无 则清除)", 10)]
    public class ChangeMasterNameCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeMasterName(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sMasterName = @Params.Length > 1 ? @Params[1] : "";
            var sIsMaster = @Params.Length > 2 ? @Params[2] : "";
            if (string.IsNullOrEmpty(sHumanName) || sMasterName == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                if (string.Compare(sMasterName, "无", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    m_PlayObject.m_sMasterName = "";
                    m_PlayObject.RefShowName();
                    m_PlayObject.m_boMaster = false;
                    PlayObject.SysMsg(sHumanName + " 的师徒名清除成功。", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    m_PlayObject.m_sMasterName = sMasterName;
                    if (sIsMaster != "" && sIsMaster[0] == '1')
                    {
                        m_PlayObject.m_boMaster = true;
                    }
                    else
                    {
                        m_PlayObject.m_boMaster = false;
                    }
                    m_PlayObject.RefShowName();
                    PlayObject.SysMsg(sHumanName + " 的师徒名更改成功。", MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}