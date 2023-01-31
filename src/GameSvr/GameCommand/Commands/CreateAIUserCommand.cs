using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 增加AI玩家
    /// </summary>
    [Command("AddRebotPlay", "增加机器人玩家", "数量", 0)]
    public class CreateAIUserCommand : Command
    {
        [ExecuteCommand]
        public void AddRebotPlay(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            var userCount = @params.Length > 0 ? int.Parse(@params[0]) : 1;
            //todo 可以指定是随机刷还是指定地图和坐标
            if (userCount == 0)
            {
                userCount = 1;
            }
            for (var i = 0; i < userCount; i++)
            {
                short nX = 0;
                short nY = 0;
                var sMapName = M2Share.WorldEngine.GetHomeInfo(ref nX, ref nY);
                M2Share.WorldEngine.AddAiLogon(new RoBotLogon()
                {
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
            if (userCount > 0)
            {
                M2Share.WorldEngine.StartAi();
            }
        }
    }
}