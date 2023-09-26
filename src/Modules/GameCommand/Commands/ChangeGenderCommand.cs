using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 调整指定玩家性别
    /// </summary>
    [Command("ChangeGender", "调整指定玩家性别", "人物名称 性别(男、女)", 10)]
    public class ChangeGenderCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
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
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!Enum.TryParse(nSex.ToString(), out PlayerGender playSex))
            {
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor != null)
            {
                if (mIPlayerActor.Gender != playSex)
                {
                    mIPlayerActor.Gender = playSex;
                    mIPlayerActor.FeatureChanged();
                    PlayerActor.SysMsg(mIPlayerActor.ChrName + " 的性别已改变。", MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    PlayerActor.SysMsg(mIPlayerActor.ChrName + " 的性别未改变!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayerActor.SysMsg(sHumanName + "没有在线!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}