using GameSrv.Player;
using SystemModule.Data;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 增加AI玩家
    /// </summary>
    [Command("AddRobotPlay", "增加机器人玩家", "数量", 0)]
    public class CreateRobotPlayCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject PlayObject) {
            if (@params == null) {
                return;
            }
            int userCount = @params.Length > 0 ? int.Parse(@params[0]) : 1;
            //todo 可以指定是随机刷还是指定地图和坐标
            if (userCount == 0) {
                userCount = 1;
            }
            for (int i = 0; i < userCount; i++) {
                short nX = 0;
                short nY = 0;
                string sMapName = World.WorldServer.GetHomeInfo(ref nX, ref nY);
                M2Share.WorldEngine.AddAiLogon(new RoBotLogon() {
                    sChrName = "玩家" + RandomNumber.GetInstance().Random() + "号",
                    sConfigFileName = "",
                    sHeroConfigFileName = "",
                    sFilePath = M2Share.Config.EnvirDir,
                    sConfigListFileName = M2Share.Config.sAIConfigListFileName,
                    sHeroConfigListFileName = M2Share.Config.sHeroAIConfigListFileName,
                    sMapName = sMapName,
                    nX = nX,
                    nY = nY
                });
            }
            if (userCount > 0) {
                World.WorldServer.StartAi();
            }
        }
    }
}