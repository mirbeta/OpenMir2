namespace GameSvr.Monster.Monsters
{
    public class PetSuperGuard : SuperGuard
    {
        public PetSuperGuard() : base()
        {
            ViewRange = 7;
            m_nLight = 4;
            m_boAttackPet = false;
        }
    }
}

