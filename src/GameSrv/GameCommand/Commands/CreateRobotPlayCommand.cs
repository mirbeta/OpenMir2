using GameSrv.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands
{
    /// <summary>
    /// 增加AI玩家
    /// </summary>
    [Command("AddRobotPlay", "增加机器人玩家", "数量 地图 X Y", 0)]
    public class CreateRobotPlayCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            if (@params[0] == "?")
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var userCount = HUtil32.StrToInt(@params[0], 1);
            short nX = 0;
            short nY = 0;
            if (@params.Length >= 3)
            {
                string mapName = string.IsNullOrEmpty(@params[1]) ? "" : @params[1];
                nX = HUtil32.StrToInt16(@params[2], 0);
                nY = HUtil32.StrToInt16(@params[3], 0);
                if (string.IsNullOrEmpty(mapName) || (nX == 0 || nY == 0))
                {
                    PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                    return;
                }
                for (int i = 0; i < userCount; i++)
                {
                    M2Share.WorldEngine.AddAiLogon(new RoBotLogon()
                    {
                        sChrName = RandomNumber.GetInstance().GenerateRandomNumber(RandomNumber.GetInstance().Random(4, 8)),
                        sFilePath = M2Share.Config.EnvirDir,
                        sConfigListFileName = M2Share.Config.sAIConfigListFileName,
                        sHeroConfigListFileName = M2Share.Config.sHeroAIConfigListFileName,
                        sMapName = mapName,
                        nX = nX,
                        nY = nY
                    });
                }
            }
            else
            {
                for (int i = 0; i < userCount; i++)
                {
                    string sMapName = World.WorldServer.GetHomeInfo(ref nX, ref nY);
                    M2Share.WorldEngine.AddAiLogon(new RoBotLogon()
                    {
                        sChrName = RandomNumber.GetInstance().GenerateRandomNumber(RandomNumber.GetInstance().Random(4, 8)),
                        sFilePath = M2Share.Config.EnvirDir,
                        sConfigListFileName = M2Share.Config.sAIConfigListFileName,
                        sHeroConfigListFileName = M2Share.Config.sHeroAIConfigListFileName,
                        sMapName = sMapName,
                        nX = nX,
                        nY = nY
                    });
                }
            }
            PlayObject.SysMsg($"已添加[{userCount}]个假人玩家,共[{M2Share.WorldEngine.RobotPlayerCount + userCount}]个假人玩家", MsgColor.Green, MsgType.Hint);
        }
    }
}