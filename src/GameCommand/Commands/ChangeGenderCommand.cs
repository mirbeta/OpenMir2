using M2Server.Player;
using M2Server;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands
{
    /// <summary>
    /// 调整指定玩家性别
    /// </summary>
    [Command("ChangeGender", "调整指定玩家性别", "人物名称 性别(男、女)", 10)]
    public class ChangeGenderCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sSex = @params.Length > 1 ? @params[1] : "";
            var nSex = -1;
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
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!Enum.TryParse(nSex.ToString(), out PlayGender playSex))
            {
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null)
            {
                if (mPlayObject.Gender != playSex)
                {
                    mPlayObject.Gender = playSex;
                    mPlayObject.FeatureChanged();
                    playObject.SysMsg(mPlayObject.ChrName + " 的性别已改变。", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(mPlayObject.ChrName + " 的性别未改变!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                playObject.SysMsg(sHumanName + "没有在线!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}