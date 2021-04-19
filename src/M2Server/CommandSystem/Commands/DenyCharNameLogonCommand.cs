using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 将指定玩家添加到禁止人物列表
    /// </summary>
    [GameCommand("DenyCharNameLogon", "将指定玩家添加到禁止人物列表", 10)]
    public class DenyCharNameLogonCommand : BaseCommond
    {
        [DefaultCommand]
        public void DenyCharNameLogon(string[] @Params, TPlayObject PlayObject)
        {
            var sCharName = @Params.Length > 0 ? @Params[0] : "";
            var sFixDeny = @Params.Length > 1 ? @Params[1] : "";

            if (sCharName == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 人物名称 是否永久封(0,1)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            try
            {
                if (sFixDeny != "" && sFixDeny[0] == '1')
                {
                    //M2Share.g_DenyChrNameList.Add(sCharName, ((1) as Object));
                    M2Share.SaveDenyChrNameList();
                    PlayObject.SysMsg(sCharName + "已加入禁止人物列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    //M2Share.g_DenyChrNameList.Add(sCharName, ((0) as Object));
                    PlayObject.SysMsg(sCharName + "已加入临时禁止人物列表", TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            finally
            {
            }
        }
    }
}