using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 推开范围内对象
    /// </summary>
    [GameCommand("BackStep", "推开范围内对象", 10)]
    public class BackStepCommand : BaseCommond
    {
        [DefaultCommand]
        public void BackStep(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var nType = @Params.Length > 0 ? int.Parse(@Params[0]) : 0;
            var nCount = @Params.Length > 1 ? int.Parse(@Params[1]) : 0;
            if (PlayObject.m_btPermission < 6)
            {
                return;
            }
            nType = HUtil32._MIN(nType, 8);
            if (nType == 0)
            {
                PlayObject.CharPushed(PlayObject.GetBackDir(PlayObject.m_btDirection), nCount);
            }
            else
            {
                PlayObject.CharPushed((byte)M2Share.RandomNumber.Random(nType), nCount);
            }
        }
    }
}