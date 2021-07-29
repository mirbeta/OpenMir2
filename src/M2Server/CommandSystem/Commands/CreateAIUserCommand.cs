using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server.Command
{
    /// <summary>
    /// 增加AI玩家
    /// </summary>
    [GameCommand("AddAIUser", "增加AI玩家", 0)]
    public class CreateAIUserCommand : BaseCommond
    {
        [DefaultCommand]
        public void AddAIUser(string[] @params, TPlayObject PlayObject)
        {
            //var sHumName = @params.Length > 0 ? @params[0] : "";
            //var nPoint = @params.Length > 1 ? Convert.ToInt32(@params[1]) : 0;
            // if (PlayObject.m_btPermission < 6)
            // {
            //     return;
            // }
            // if (sHumName == "" || nPoint <= 0)
            // {
            //     PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称  金币数量", TMsgColor.c_Red, TMsgType.t_Hint);
            //     return;
            // }
            var userCount = @params.Length > 0 ? int.Parse(@params[0]) : 1;
            if (userCount == 0)
            {
                userCount = 1;
            }
            for (var i = 0; i < userCount; i++)
            {
                M2Share.UserEngine.AddAILogon(new TAILogon()
                {
                    sCharName = "玩家" + SystemModule.RandomNumber.GetInstance().Random() + "号",
                    sMapName = "0",
                    sConfigFileName = "",
                    sHeroConfigFileName = "",
                    sFilePath = M2Share.g_Config.sEnvirDir,
                    sConfigListFileName = M2Share.g_Config.sAIConfigListFileName,
                    sHeroConfigListFileName = M2Share.g_Config.sHeroAIConfigListFileName,
                    nX = 285,
                    nY = 608
                });
            }
        }
    }
}