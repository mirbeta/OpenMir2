using M2Server.CommandSystem;

namespace M2Server
{
    [GameCommand("RestHero", "", 10)]
    public class RestHeroCommand : BaseCommond
    {
        [DefaultCommand]
        public void RestHero(string[] @Params, TPlayObject PlayObject)
        {
            //if ((PlayObject.m_MyHero != null))
            //{
            //   // ((THeroObject)(PlayObject.m_MyHero)).RestHero();
            //}
        }
    }
}