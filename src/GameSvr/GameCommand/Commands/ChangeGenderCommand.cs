using GameSvr.Actor;
using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家性别
    /// </summary>
    [Command("ChangeGender", "调整指定玩家性别", "人物名称 性别(男、女)", 10)]
    public class ChangeGenderCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sSex = @Params.Length > 1 ? @Params[1] : "";
            int nSex = -1;
            if (sSex == "Man" || sSex == "男" || sSex == "0")
            {
                nSex = 0;
            }
            if (sSex == "WoMan" || sSex == "女" || sSex == "1")
            {
                nSex = 1;
            }
            if (string.IsNullOrEmpty(sHumanName) || nSex == -1)
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayGender playSex;
            if (!Enum.TryParse(nSex.ToString(), out playSex))
            {
                return;
            }
            PlayObject m_PlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.Gender != playSex)
                {
                    m_PlayObject.Gender = playSex;
                    m_PlayObject.FeatureChanged();
                    PlayObject.SysMsg(m_PlayObject.ChrName + " 的性别已改变。", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    PlayObject.SysMsg(m_PlayObject.ChrName + " 的性别未改变!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg(sHumanName + "没有在线!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}