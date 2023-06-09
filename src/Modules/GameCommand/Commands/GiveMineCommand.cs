using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace CommandSystem
{
    /// <summary>
    /// 给指定纯度的矿石
    /// </summary>
    [Command("GiveMine", "给指定纯度的矿石", "矿石名称 数量 持久", 10)]
    public class GiveMineCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sMineName = @params.Length > 0 ? @params[0] : "";
            var nMineCount = @params.Length > 0 ? HUtil32.StrToInt(@params[1], 0) : 0;
            var nDura = @params.Length > 0 ? HUtil32.StrToInt(@params[2], 0) : 0;
            if (PlayerActor.Permission < this.Command.PermissionMin)
            {
                PlayerActor.SysMsg(CommandHelp.GameCommandPermissionTooLow, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sMineName) || !string.IsNullOrEmpty(sMineName) && sMineName[0] == '?' ||
                nMineCount <= 0)
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nDura <= 0)
            {
                nDura = SystemShare.RandomNumber.Random(18) + 3;
            }
            // 如纯度不填,则随机给纯度
            for (var i = 0; i < nMineCount; i++)
            {
                var userItem = new UserItem();
                if (ItemSystem.CopyToUserItemFromName(sMineName, ref userItem))
                {
                    var stdItem = ItemSystem.GetStdItem(userItem.Index);
                    if (stdItem != null && stdItem.StdMode == 43)
                    {
                        if (PlayerActor.IsAddWeightAvailable(stdItem.Weight * nMineCount))
                        {
                            userItem.Dura = Convert.ToUInt16(nDura * 1000);
                            if (userItem.Dura > userItem.DuraMax)
                            {
                                userItem.Dura = userItem.DuraMax;
                            }

                            PlayerActor.ItemList.Add(userItem);
                            PlayerActor.SendAddItem(userItem);
                            if (stdItem.NeedIdentify == 1)
                            {
                                //    M2Share.EventSource.AddEventLog(5, PlayerActor.MapName + "\09" + PlayerActor.CurrX + "\09" + PlayerActor.CurrY + "\09" +
                                //                                       PlayerActor.ChrName + "\09" + stdItem.Name + "\09" + userItem.MakeIndex + "\09" + userItem.Dura + "/"
                                //                                       + userItem.DuraMax + "\09" + PlayerActor.ChrName);
                            }
                        }
                    }
                }
                else
                {
                    userItem = null;
                    break;
                }
            }
        }
    }
}