namespace GameSvr.Monster.Monsters
{
    public class PetSuperGuard : SuperGuard
    {
        public PetSuperGuard() : base()
        {
            ViewRange = 7;
            Light = 4;
            m_boAttackPet = false;
        }
    }
}

