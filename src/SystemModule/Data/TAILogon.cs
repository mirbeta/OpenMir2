namespace SystemModule.Data;

/// <summary>
/// 假人登陆结构
/// </summary>
public class TAILogon
{
    public string sCharName;//名字
    public string sMapName;//地图
    public string sConfigFileName;//人物配置路径
    public string sHeroConfigFileName;//英雄配置路径
    public string sFilePath;
    public string sConfigListFileName;//人物配置列表目录
    public string sHeroConfigListFileName;//英雄配置列表目录
    public short nX;//X坐标
    public short nY;//Y坐标
}