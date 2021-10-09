using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 增加AI玩家
    /// </summary>
    [GameCommand("AddAIUser", "增加机器人玩家", "数量", 0)]
    public class CreateAIUserCommand : BaseCommond
    {
        [DefaultCommand]
        public void AddAIUser(string[] @params, TPlayObject PlayObject)
        {
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
                var sMapName = M2Share.UserEngine.GetHomeInfo(ref nX, ref nY);
                M2Share.UserEngine.AddAILogon(new TAILogon()
                {
                    sCharName = "玩家" + RandomNumber.GetInstance().Random() + "号",
                    sConfigFileName = "",
                    sHeroConfigFileName = "",
                    sFilePath = M2Share.g_Config.sEnvirDir,
                    sConfigListFileName = M2Share.g_Config.sAIConfigListFileName,
                    sHeroConfigListFileName = M2Share.g_Config.sHeroAIConfigListFileName,
                    sMapName = sMapName,
                    nX = nX,
                    nY = nY
                });
            }
        }
    }
}