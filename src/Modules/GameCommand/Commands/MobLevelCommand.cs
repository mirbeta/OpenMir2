using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    /// <summary>
    /// 显示你屏幕上你近处所有怪与人的详细情况
    /// </summary>
    [Command("MobLevel", "显示你屏幕上你近处所有怪与人的详细情况", 10)]
    public class MobLevelCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            IList<IActor> baseObjectList = new List<IActor>();
            PlayerActor.Envir.GetRangeBaseObject(PlayerActor.CurrX, PlayerActor.CurrY, 2, true, baseObjectList);
            for (var i = 0; i < baseObjectList.Count; i++)
            {
                PlayerActor.SysMsg(baseObjectList[i].GetBaseObjectInfo(), MsgColor.Green, MsgType.Hint);
            }

            baseObjectList.Clear();
        }
    }
}