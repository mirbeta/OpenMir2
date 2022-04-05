namespace RobotSvr
{

    public class Actor
    {
        public const int MAXACTORSOUND = 3;
        public const int CMMX = 150;
        public const int CMMY = 200;
        public const int HUMANFRAME = 600;
        public const int MONFRAME = 280;
        public const int EXPMONFRAME = 360;
        public const int SCULMONFRAME = 440;
        public const int ZOMBIFRAME = 430;
        public const int MERCHANTFRAME = 60;
        public const int MAXSAY = 5;
        public const int RUN_MINHEALTH = 10;
        public const int DEFSPELLFRAME = 10;
        public const int FIREHIT_READYFRAME = 6;
        public const int MAGBUBBLEBASE = 3890;
        // Ä§·¨¶ÜÐ§¹ûÍ¼Î»ÖÃ
        public const int MAGBUBBLESTRUCKBASE = 3900;
        // ±»¹¥»÷Ê±Ä§·¨¶ÜÐ§¹ûÍ¼Î»ÖÃ
        public const int MAXWPEFFECTFRAME = 5;
        public const int WPEFFECTBASE = 3750;
        public const int EffectBase = 0;

        public static TMonsterAction MA9 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 1, 7, 200, 0),
            ActWalk = new TActionInfo(64, 6, 2, 120, 3),
            ActAttack = new TActionInfo(64, 6, 2, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(64, 6, 2, 100, 0),
            ActDie = new TActionInfo(0, 1, 7, 140, 0),
            ActDeath = new TActionInfo(0, 1, 7, 0, 0)
        };

        // (8Frame) ´øµ¶ÎÀÊ¿
        public static TMonsterAction MA10 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 4, 200, 0),
            ActWalk = new TActionInfo(64, 6, 2, 120, 3),
            ActAttack = new TActionInfo(128, 4, 4, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(192, 2, 0, 100, 0),
            ActDie = new TActionInfo(208, 4, 4, 140, 0),
            ActDeath = new TActionInfo(272, 1, 0, 0, 0)
        };

        public static TMonsterAction MA11 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 120, 3),
            ActAttack = new TActionInfo(160, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 140, 0),
            ActDeath = new TActionInfo(340, 1, 0, 0, 0)
        };

        public static TMonsterAction MA12 = new TMonsterAction()
        {

            ActStand = new TActionInfo(0, 4, 4, 200, 0),
            ActWalk = new TActionInfo(64, 6, 2, 120, 3),
            ActAttack = new TActionInfo(128, 6, 2, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(192, 2, 0, 150, 0),
            ActDie = new TActionInfo(208, 4, 4, 160, 0),
            ActDeath = new TActionInfo(272, 1, 0, 0, 0)
        };

        public static TMonsterAction MA13 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(10, 8, 2, 160, 0),
            ActAttack = new TActionInfo(30, 6, 4, 120, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(110, 2, 0, 100, 0),
            ActDie = new TActionInfo(130, 10, 0, 120, 0),
            ActDeath = new TActionInfo(20, 9, 0, 150, 0)
        };
        // ÇØ°ñ ¿À¸¶
        // ¹é°ñÀÎ°æ¿ì(¼ÒÈ¯)
        public static TMonsterAction MA14 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 120, 0),
            ActDeath = new TActionInfo(340, 10, 0, 100, 0)
        };
        // µµ³¢´øÁö´Â ¿À¸¶
        public static TMonsterAction MA15 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 120, 0),
            ActDeath = new TActionInfo(1, 1, 0, 100, 0)
        };
        // °¡½º½î´Â ±¸µ¥±â
        public static TMonsterAction MA16 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 160, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 4, 6, 160, 0),
            ActDeath = new TActionInfo(0, 1, 0, 160, 0)
        };
        // ¹Ùµü²¨¸®´Â ¸÷
        public static TMonsterAction MA17 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 60, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 100, 0),
            ActDeath = new TActionInfo(340, 1, 0, 140, 0)
        };
        // ¿ì¸é±Í (Á×´Â°Å »¡¸®Á×À½)
        public static TMonsterAction MA19 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 140, 0),
            ActDeath = new TActionInfo(340, 1, 0, 140, 0)
        };
        // Á×¾ú´Ù »ì¾Æ³ª´Â Á»ºñ)
        // ´Ù½Ã »ì¾Æ³ª±â
        public static TMonsterAction MA20 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 120, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 100, 0),
            ActDeath = new TActionInfo(340, 10, 0, 170, 0)
        };

        public static TMonsterAction MA21 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(10, 6, 4, 120, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(20, 2, 0, 100, 0),
            ActDie = new TActionInfo(30, 10, 0, 160, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };

        public static TMonsterAction MA22 = new TMonsterAction()
        {
            ActStand = new TActionInfo(80, 4, 6, 200, 0),
            ActWalk = new TActionInfo(160, 6, 4, 160, 3),
            ActAttack = new TActionInfo(240, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(320, 2, 0, 100, 0),
            ActDie = new TActionInfo(340, 10, 0, 160, 0),
            ActDeath = new TActionInfo(0, 6, 4, 170, 0)
        };
        // ÁÖ¸¶¿Õ
        // ¼®»ó³ìÀ½
        public static TMonsterAction MA23 = new TMonsterAction()
        {
            ActStand = new TActionInfo(20, 4, 6, 200, 0),
            ActWalk = new TActionInfo(100, 6, 4, 160, 3),
            ActAttack = new TActionInfo(180, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(260, 2, 0, 100, 0),
            ActDie = new TActionInfo(280, 10, 0, 160, 0),
            ActDeath = new TActionInfo(0, 20, 0, 100, 0)
        };
        // Àü°¥, °ø°Ý 2°¡Áö
        public static TMonsterAction MA24 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 100, 0),
            ActCritical = new TActionInfo(240, 6, 4, 100, 0),
            ActStruck = new TActionInfo(320, 2, 0, 100, 0),
            ActDie = new TActionInfo(340, 10, 0, 140, 0),
            ActDeath = new TActionInfo(420, 1, 0, 140, 0)
        };
        // 4C080C
        public static TMonsterAction MA25 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(70, 10, 0, 200, 3),
            ActAttack = new TActionInfo(20, 6, 4, 120, 0),
            ActCritical = new TActionInfo(10, 6, 4, 120, 0),
            ActStruck = new TActionInfo(50, 2, 0, 100, 0),
            ActDie = new TActionInfo(60, 10, 0, 200, 0),
            ActDeath = new TActionInfo(80, 10, 0, 200, 3)
        };

        public static TMonsterAction MA26 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 1, 7, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 160, 0),
            ActAttack = new TActionInfo(56, 6, 2, 350, 0),
            ActCritical = new TActionInfo(64, 6, 2, 350, 0),
            ActStruck = new TActionInfo(0, 4, 4, 100, 0),
            ActDie = new TActionInfo(24, 10, 0, 120, 0),
            ActDeath = new TActionInfo(0, 0, 0, 150, 0)
        };

        public static TMonsterAction MA27 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 1, 7, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 160, 0),
            ActAttack = new TActionInfo(0, 0, 0, 250, 0),
            ActCritical = new TActionInfo(0, 0, 0, 250, 0),
            ActStruck = new TActionInfo(0, 0, 0, 100, 0),
            ActDie = new TActionInfo(0, 10, 0, 120, 0),
            ActDeath = new TActionInfo(0, 0, 0, 150, 0)

        };
        // ½Å¼ö (º¯½Å Àü)
        // µîÀå..
        public static TMonsterAction MA28 = new TMonsterAction()
        {
            ActStand = new TActionInfo(80, 4, 6, 200, 0),
            ActWalk = new TActionInfo(160, 6, 4, 160, 3),
            ActAttack = new TActionInfo(0, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 120, 0),
            ActDeath = new TActionInfo(0, 10, 0, 100, 0)
        };
        // ½Å¼ö (º¯½Å ÈÄ)
        // µîÀå..
        public static TMonsterAction MA29 = new TMonsterAction()
        {
            ActStand = new TActionInfo(80, 4, 6, 200, 0),
            ActWalk = new TActionInfo(160, 6, 4, 160, 3),
            ActAttack = new TActionInfo(240, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 10, 0, 100, 0),
            ActStruck = new TActionInfo(320, 2, 0, 100, 0),
            ActDie = new TActionInfo(340, 10, 0, 120, 0),
            ActDeath = new TActionInfo(0, 10, 0, 100, 0)
        };

        public static TMonsterAction MA30 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(0, 10, 0, 160, 3),
            ActAttack = new TActionInfo(10, 6, 4, 120, 0),
            ActCritical = new TActionInfo(10, 6, 4, 100, 0),
            ActStruck = new TActionInfo(20, 2, 0, 100, 0),
            ActDie = new TActionInfo(30, 20, 0, 120, 0),
            ActDeath = new TActionInfo(0, 10, 0, 140, 3)
        };
        // 4C09BC
        public static TMonsterAction MA31 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(0, 10, 0, 200, 3),
            ActAttack = new TActionInfo(10, 6, 4, 120, 0),
            ActCritical = new TActionInfo(0, 6, 4, 120, 0),
            ActStruck = new TActionInfo(0, 2, 8, 100, 0),
            ActDie = new TActionInfo(20, 10, 0, 200, 0),
            ActDeath = new TActionInfo(0, 10, 0, 200, 3)
        };
        public static TMonsterAction MA32 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 1, 9, 200, 0),
            ActWalk = new TActionInfo(0, 6, 4, 200, 3),
            ActAttack = new TActionInfo(0, 6, 4, 120, 0),
            ActCritical = new TActionInfo(0, 6, 4, 120, 0),
            ActStruck = new TActionInfo(0, 2, 8, 100, 0),
            ActDie = new TActionInfo(80, 10, 0, 80, 0),
            ActDeath = new TActionInfo(80, 10, 0, 200, 3)
        };
        public static TMonsterAction MA33 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 200, 3),
            ActAttack = new TActionInfo(160, 6, 4, 120, 0),
            ActCritical = new TActionInfo(340, 6, 4, 120, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 200, 0),
            ActDeath = new TActionInfo(260, 10, 0, 200, 0)
        };
        public static TMonsterAction MA34 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 200, 3),
            ActAttack = new TActionInfo(160, 6, 4, 120, 0),
            ActCritical = new TActionInfo(320, 6, 4, 120, 0),
            ActStruck = new TActionInfo(400, 2, 0, 100, 0),
            ActDie = new TActionInfo(420, 20, 0, 200, 0),
            ActDeath = new TActionInfo(420, 20, 0, 200, 0)
        };
        public static TMonsterAction MA35 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(30, 10, 0, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(0, 1, 9, 0, 0),
            ActDie = new TActionInfo(0, 0, 0, 0, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA111 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(30, 23, 0, 180, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(0, 1, 9, 0, 0),
            ActDie = new TActionInfo(0, 0, 0, 0, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA36 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(30, 20, 0, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(0, 1, 9, 0, 0),
            ActDie = new TActionInfo(0, 0, 0, 0, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA37 = new TMonsterAction()
        {
            ActStand = new TActionInfo(30, 4, 6, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(30, 4, 6, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(0, 1, 9, 0, 0),
            ActDie = new TActionInfo(0, 0, 0, 0, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA38 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(80, 6, 4, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(0, 0, 0, 0, 0),
            ActDie = new TActionInfo(0, 0, 0, 0, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA39 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 300, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(10, 6, 4, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(20, 2, 0, 150, 0),
            ActDie = new TActionInfo(30, 10, 0, 80, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA40 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 250, 0),
            ActWalk = new TActionInfo(80, 6, 4, 210, 3),
            ActAttack = new TActionInfo(160, 6, 4, 110, 0),
            ActCritical = new TActionInfo(580, 20, 0, 135, 0),
            ActStruck = new TActionInfo(240, 2, 0, 120, 0),
            ActDie = new TActionInfo(260, 20, 0, 130, 0),
            ActDeath = new TActionInfo(260, 20, 0, 130, 0)
        };
        public static TMonsterAction MA41 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 2, 8, 200, 0),
            ActWalk = new TActionInfo(0, 2, 8, 200, 0),
            ActAttack = new TActionInfo(0, 2, 8, 200, 0),
            ActCritical = new TActionInfo(0, 2, 8, 200, 0),
            ActStruck = new TActionInfo(0, 2, 8, 200, 0),
            ActDie = new TActionInfo(0, 2, 8, 200, 0),
            ActDeath = new TActionInfo(0, 2, 8, 200, 0)
        };
        public static TMonsterAction MA42 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(10, 8, 2, 160, 0),
            ActAttack = new TActionInfo(0, 0, 0, 0, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(0, 0, 0, 0, 0),
            ActDie = new TActionInfo(30, 10, 0, 120, 0),
            ActDeath = new TActionInfo(30, 10, 0, 150, 0)
        };
        public static TMonsterAction MA43 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 0),
            ActAttack = new TActionInfo(160, 6, 4, 160, 0),
            ActCritical = new TActionInfo(160, 6, 4, 160, 0),
            ActStruck = new TActionInfo(240, 2, 0, 150, 0),
            ActDie = new TActionInfo(260, 10, 0, 120, 0),
            ActDeath = new TActionInfo(340, 10, 0, 100, 0)
        };
        public static TMonsterAction MA44 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 10, 0, 300, 0),
            ActWalk = new TActionInfo(10, 6, 4, 150, 0),
            ActAttack = new TActionInfo(20, 6, 4, 150, 0),
            ActCritical = new TActionInfo(40, 10, 0, 150, 0),
            ActStruck = new TActionInfo(40, 2, 8, 150, 0),
            ActDie = new TActionInfo(30, 6, 4, 150, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA45 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 10, 0, 300, 0),
            ActWalk = new TActionInfo(0, 10, 0, 300, 0),
            ActAttack = new TActionInfo(10, 10, 0, 300, 0),
            ActCritical = new TActionInfo(10, 10, 0, 100, 0),
            ActStruck = new TActionInfo(0, 1, 9, 300, 0),
            ActDie = new TActionInfo(0, 1, 9, 300, 0),
            ActDeath = new TActionInfo(0, 1, 9, 300, 0)
        };
        public static TMonsterAction MA46 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 20, 0, 100, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(0, 0, 0, 0, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(0, 0, 0, 0, 0),
            ActDie = new TActionInfo(0, 0, 0, 0, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA47 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 0, 0, 200, 0),
            ActWalk = new TActionInfo(50, 10, 0, 200, 3),
            ActAttack = new TActionInfo(10, 6, 4, 120, 0),
            ActCritical = new TActionInfo(10, 6, 4, 120, 0),
            ActStruck = new TActionInfo(40, 10, 0, 100, 0),
            ActDie = new TActionInfo(0, 1, 0, 200, 0),
            ActDeath = new TActionInfo(0, 1, 0, 200, 0)
        };
        public static TMonsterAction MA48 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 160, 0),
            ActCritical = new TActionInfo(340, 6, 4, 160, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 160, 0),
            ActDeath = new TActionInfo(0, 1, 0, 160, 0)
        };
        public static TMonsterAction MA49 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 160, 0),
            ActCritical = new TActionInfo(340, 6, 4, 160, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 160, 0),
            ActDeath = new TActionInfo(420, 4, 6, 200, 0)
        };
        public static TMonsterAction MA50 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 0),
            ActAttack = new TActionInfo(160, 6, 4, 160, 0),
            ActCritical = new TActionInfo(340, 6, 4, 160, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 160, 0),
            ActDeath = new TActionInfo(420, 4, 6, 200, 0)
        };
        public static TMonsterAction MA51 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 20, 0, 150, 0),
            ActWalk = new TActionInfo(0, 20, 0, 150, 3),
            ActAttack = new TActionInfo(20, 10, 0, 150, 0),
            ActCritical = new TActionInfo(20, 10, 0, 150, 0),
            ActStruck = new TActionInfo(20, 2, 8, 100, 0),
            ActDie = new TActionInfo(400, 18, 0, 150, 0),
            ActDeath = new TActionInfo(400, 18, 0, 150, 0)
        };
        public static TMonsterAction MA131 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 20, 0, 150, 0),
            ActWalk = new TActionInfo(0, 20, 0, 150, 3),
            ActAttack = new TActionInfo(20, 20, 0, 150, 0),
            ActCritical = new TActionInfo(20, 10, 0, 150, 0),
            ActStruck = new TActionInfo(20, 2, 8, 100, 0),
            ActDie = new TActionInfo(400, 18, 0, 150, 0),
            ActDeath = new TActionInfo(400, 18, 0, 150, 0)
        };
        public static TMonsterAction MA52 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 150, 0),
            ActWalk = new TActionInfo(0, 4, 6, 150, 3),
            ActAttack = new TActionInfo(10, 4, 6, 300, 0),
            ActCritical = new TActionInfo(10, 4, 6, 300, 0),
            ActStruck = new TActionInfo(0, 4, 6, 150, 0),
            ActDie = new TActionInfo(0, 4, 6, 300, 0),
            ActDeath = new TActionInfo(0, 4, 6, 300, 0)
        };
        public static TMonsterAction MA53 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 150, 0),
            ActWalk = new TActionInfo(0, 4, 6, 150, 3),
            ActAttack = new TActionInfo(0, 4, 6, 150, 0),
            ActCritical = new TActionInfo(0, 4, 6, 150, 0),
            ActStruck = new TActionInfo(0, 4, 6, 150, 0),
            ActDie = new TActionInfo(0, 4, 6, 150, 0),
            ActDeath = new TActionInfo(0, 4, 6, 150, 0)
        };
        public static TMonsterAction MA54 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 0),
            ActAttack = new TActionInfo(160, 6, 4, 160, 0),
            ActCritical = new TActionInfo(340, 10, 0, 160, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 160, 0),
            ActDeath = new TActionInfo(420, 4, 6, 200, 0)
        };
        public static TMonsterAction MA55 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(0, 0, 0, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(0, 1, 9, 0, 0),
            ActDie = new TActionInfo(0, 0, 0, 0, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA56 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(10, 10, 0, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(0, 1, 9, 0, 0),
            ActDie = new TActionInfo(0, 0, 0, 0, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA57 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 3, 0, 160, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(3, 8, 0, 160, 0),
            ActCritical = new TActionInfo(22, 8, 0, 160, 0),
            ActStruck = new TActionInfo(0, 1, 9, 0, 0),
            ActDie = new TActionInfo(0, 0, 0, 0, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA58 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 1, 0, 160, 0),
            ActWalk = new TActionInfo(0, 0, 0, 0, 0),
            ActAttack = new TActionInfo(1, 34, 0, 160, 0),
            ActCritical = new TActionInfo(47, 33, 0, 160, 0),
            ActStruck = new TActionInfo(0, 1, 9, 0, 0),
            ActDie = new TActionInfo(0, 0, 0, 0, 0),
            ActDeath = new TActionInfo(0, 0, 0, 0, 0)
        };
        public static TMonsterAction MA59 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 10, 0, 300, 0),
            ActWalk = new TActionInfo(0, 0, 0, 150, 0),
            ActAttack = new TActionInfo(0, 10, 0, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 150, 0),
            ActStruck = new TActionInfo(0, 0, 0, 150, 0),
            ActDie = new TActionInfo(0, 0, 0, 150, 0),
            ActDeath = new TActionInfo(0, 0, 0, 150, 0)
        };
        public static TMonsterAction MA60 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 1, 0, 300, 0),
            ActWalk = new TActionInfo(0, 0, 0, 150, 0),
            ActAttack = new TActionInfo(0, 1, 0, 150, 0),
            ActCritical = new TActionInfo(0, 0, 0, 150, 0),
            ActStruck = new TActionInfo(0, 0, 0, 150, 0),
            ActDie = new TActionInfo(0, 0, 0, 150, 0),
            ActDeath = new TActionInfo(0, 0, 0, 150, 0)
        };
        // È£±â¿¬
        public static TMonsterAction MA65 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 10, 0, 150, 0),
            ActWalk = new TActionInfo(10, 10, 0, 150, 0),
            ActAttack = new TActionInfo(20, 10, 0, 150, 0),
            ActCritical = new TActionInfo(20, 10, 0, 150, 0),
            ActStruck = new TActionInfo(30, 4, 6, 100, 0),
            ActDie = new TActionInfo(40, 10, 0, 150, 0),
            ActDeath = new TActionInfo(40, 10, 0, 150, 0)
        };
        // ºñ¿ùÃµÁÖ
        public static TMonsterAction MA66 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 20, 0, 150, 0),
            ActWalk = new TActionInfo(0, 20, 0, 150, 3),
            ActAttack = new TActionInfo(20, 10, 0, 150, 0),
            ActCritical = new TActionInfo(20, 10, 0, 150, 0),
            ActStruck = new TActionInfo(30, 2, 8, 100, 0),
            ActDie = new TActionInfo(400, 18, 0, 150, 0),
            ActDeath = new TActionInfo(400, 18, 0, 150, 0)
        };
        // È£È¥±â¼®
        public static TMonsterAction MA67 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 150, 0),
            ActWalk = new TActionInfo(0, 4, 6, 150, 3),
            ActAttack = new TActionInfo(10, 4, 6, 300, 0),
            ActCritical = new TActionInfo(10, 4, 6, 300, 0),
            ActStruck = new TActionInfo(0, 4, 6, 150, 0),
            ActDie = new TActionInfo(0, 4, 6, 300, 0),
            ActDeath = new TActionInfo(0, 4, 6, 300, 0)
        };

        public static TMonsterAction MA91 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 120, 0),
            ActDeath = new TActionInfo(1040, 15, 0, 100, 0)
        };
        public static TMonsterAction MA92 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 120, 0),
            ActDeath = new TActionInfo(1060, 15, 0, 100, 0)
        };
        public static TMonsterAction MA93 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 3),
            ActAttack = new TActionInfo(160, 6, 4, 100, 0),
            ActCritical = new TActionInfo(0, 0, 0, 0, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 120, 0),
            ActDeath = new TActionInfo(1080, 15, 0, 100, 0)
        };
        public static TMonsterAction MAG25 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 0),
            ActAttack = new TActionInfo(160, 6, 4, 160, 0),
            ActCritical = new TActionInfo(340, 10, 0, 160, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 160, 0),
            ActDeath = new TActionInfo(426, 4, 6, 120, 0)
        };
        public static TMonsterAction MAG26 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 0),
            ActAttack = new TActionInfo(160, 6, 4, 120, 0),
            ActCritical = new TActionInfo(340, 7, 3, 120, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 160, 0),
            ActDeath = new TActionInfo(422, 4, 6, 120, 0)
        };
        public static TMonsterAction MAG27 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 0),
            ActAttack = new TActionInfo(160, 6, 4, 120, 0),
            ActCritical = new TActionInfo(340, 10, 0, 120, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 160, 0),
            ActDeath = new TActionInfo(420, 10, 0, 120, 0)
        };
        public static TMonsterAction MAG28 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 160, 0),
            ActAttack = new TActionInfo(160, 10, 0, 120, 0),
            ActCritical = new TActionInfo(340, 6, 4, 120, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 9, 1, 160, 0),
            ActDeath = new TActionInfo(420, 4, 6, 120, 0)
        };
        public static TMonsterAction MAG29 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 120, 0),
            ActAttack = new TActionInfo(160, 6, 4, 110, 0),
            ActCritical = new TActionInfo(340, 8, 2, 110, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 8, 2, 120, 0),
            ActDeath = new TActionInfo(420, 7, 3, 120, 0)
        };
        public static TMonsterAction MAG30 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 120, 0),
            ActAttack = new TActionInfo(160, 6, 4, 110, 0),
            ActCritical = new TActionInfo(340, 8, 2, 110, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 120, 0),
            ActDeath = new TActionInfo(420, 9, 1, 120, 0)
        };
        public static TMonsterAction MAG31 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 140, 0),
            ActAttack = new TActionInfo(160, 6, 4, 110, 0),
            ActCritical = new TActionInfo(340, 8, 2, 110, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 7, 3, 120, 0),
            ActDeath = new TActionInfo(420, 7, 3, 120, 0)
        };
        public static TMonsterAction MA120 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 1, 9, 200, 0),
            ActWalk = new TActionInfo(0, 0, 0, 200, 3),
            ActAttack = new TActionInfo(80, 10, 0, 120, 0),
            ActCritical = new TActionInfo(0, 0, 0, 120, 0),
            ActStruck = new TActionInfo(160, 2, 8, 100, 0),
            ActDie = new TActionInfo(240, 10, 0, 200, 0),
            ActDeath = new TActionInfo(240, 10, 0, 200, 0)
        };
        public static TMonsterAction MA121 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 200, 3),
            ActAttack = new TActionInfo(160, 6, 4, 120, 0),
            ActCritical = new TActionInfo(340, 6, 4, 120, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 8, 2, 200, 0),
            ActDeath = new TActionInfo(260, 8, 2, 200, 0)
        };
        public static TMonsterAction MA122 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 10, 0, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 200, 3),
            ActAttack = new TActionInfo(160, 6, 4, 120, 0),
            ActCritical = new TActionInfo(340, 6, 4, 120, 0),
            ActStruck = new TActionInfo(240, 2, 0, 100, 0),
            ActDie = new TActionInfo(260, 10, 0, 200, 0),
            ActDeath = new TActionInfo(260, 10, 0, 200, 0)
        };
        public static TMonsterAction MA123 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 200, 3),
            ActAttack = new TActionInfo(160, 6, 4, 120, 0),
            ActCritical = new TActionInfo(400, 6, 4, 120, 0),
            ActStruck = new TActionInfo(240, 2, 8, 100, 0),
            ActDie = new TActionInfo(320, 10, 0, 200, 0),
            ActDeath = new TActionInfo(400, 10, 0, 200, 0)
        };
        public static TMonsterAction MA123_815 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 200, 3),
            ActAttack = new TActionInfo(160, 6, 4, 120, 0),
            ActCritical = new TActionInfo(400, 10, 0, 120, 0),
            ActStruck = new TActionInfo(240, 2, 8, 100, 0),
            ActDie = new TActionInfo(320, 10, 0, 200, 0),
            ActDeath = new TActionInfo(400, 10, 0, 200, 0)
        };
        public static TMonsterAction MA123_825 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 200, 3),
            ActAttack = new TActionInfo(160, 9, 1, 120, 0),
            ActCritical = new TActionInfo(400, 9, 1, 120, 0),
            ActStruck = new TActionInfo(240, 2, 8, 100, 0),
            ActDie = new TActionInfo(320, 10, 0, 200, 0),
            ActDeath = new TActionInfo(400, 10, 0, 200, 0)
        };
        public static TMonsterAction MA123_827 = new TMonsterAction()
        {
            ActStand = new TActionInfo(0, 4, 6, 200, 0),
            ActWalk = new TActionInfo(80, 6, 4, 200, 3),
            ActAttack = new TActionInfo(160, 6, 4, 120, 0),
            ActCritical = new TActionInfo(400, 6, 4, 120, 0),
            ActStruck = new TActionInfo(240, 2, 8, 100, 0),
            ActDie = new TActionInfo(320, 10, 0, 200, 0),
            ActDeath = new TActionInfo(480, 10, 0, 200, 0)
        };

        public static byte[,] WORDER = { { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1 }, { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 0, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1, 1, 1 } };
        public static byte[] EffDir = { 0, 0, 1, 1, 1, 1, 1, 0 };
        public static TMonsterAction GetRaceByPM(int race, short Appr)
        {
            TMonsterAction result;
            switch (race)
            {
                case 9:
                    // 01
                    result = MA9;
                    break;
                case 10:
                    // 475D70
                    // 02
                    result = MA10;
                    break;
                case 11:
                    // 475D7C
                    // 03
                    result = MA11;
                    break;
                case 12:
                    // 475D88
                    // 04
                    result = MA12;
                    break;
                case 13:
                    // 475D94
                    // 05
                    result = MA13;
                    break;
                case 14:
                    // 475DA0
                    // 06
                    result = MA14;
                    break;
                case 15:
                    // 475DAC
                    // 07
                    result = MA15;
                    break;
                case 16:
                    // 475DB8
                    // 08
                    result = MA16;
                    break;
                case 17:
                    // 475DC4
                    // 06
                    result = MA14;
                    break;
                case 18:
                    // 475DAC
                    // 06
                    result = MA14;
                    break;
                case 19:
                    // 475DAC
                    // 0A
                    result = MA19;
                    break;
                case 20:
                    // 475DDC
                    // 0A
                    result = MA19;
                    break;
                case 21:
                    // 475DDC
                    // 0A
                    result = MA19;
                    break;
                case 22:
                    // 475DDC
                    // 07
                    result = MA15;
                    break;
                case 23:
                    // 475DB8
                    // 06
                    result = MA14;
                    break;
                case 24:
                    // 475DAC
                    // 04
                    result = MA12;
                    break;
                case 25:
                    // 475D94
                    // 04
                    result = MAG25;
                    break;
                case 26:
                    // 04
                    result = MAG26;
                    break;
                case 27:
                    // 04
                    result = MAG27;
                    break;
                case 28:
                    // 04
                    result = MAG28;
                    break;
                case 29:
                    // 04
                    result = MAG29;
                    break;
                case 30:
                    // 09
                    result = MA17;
                    break;
                case 31:
                    // 475DD0
                    // 09
                    result = MA17;
                    break;
                case 32:
                    // 475DD0
                    // 0F
                    result = MA24;
                    break;
                case 33:
                    // 475E18
                    // 10
                    result = MA25;
                    break;
                case 34:
                    // 475E24
                    // 11
                    result = MA30;
                    break;
                case 35:
                    // 475E30  ³àÔÂ¶ñÄ§
                    // 12
                    result = MA31;
                    break;
                case 36:
                    // 475E3C
                    // 13
                    result = MA32;
                    break;
                case 37:
                    // 475E48
                    // 0A
                    result = MA19;
                    break;
                case 38:
                    // 475DDC
                    // 04
                    result = MAG29;
                    break;
                case 39:
                    // 04
                    result = MAG30;
                    break;
                case 40:
                    // 0A
                    result = MA19;
                    break;
                case 41:
                    // 475DDC
                    // 0B
                    result = MA20;
                    break;
                case 42:
                    // 475DE8
                    // 0B
                    result = MA20;
                    break;
                case 43:
                    // 475DE8
                    // 0C
                    result = MA21;
                    break;
                case 44:
                    // 475DF4
                    // 0C
                    result = MAG31;
                    break;
                case 45:
                    // 0A
                    result = MA19;
                    break;
                case 46:
                    // 475DDC
                    // 0A
                    result = MA50;
                    break;
                case 47:
                    // 0D
                    result = MA22;
                    break;
                case 48:
                    // 475E00
                    // 0E
                    result = MA23;
                    break;
                case 49:
                    // 475E0C
                    // 0E
                    result = MA23;
                    break;
                case 50:
                    // 475E0C
                    // 27
                    switch (Appr)
                    {
                        case 23:
                            // 475F32
                            // 01
                            result = MA36;
                            break;
                        case 24:
                            // 475F77
                            // 02
                            result = MA37;
                            break;
                        case 25:
                            // 475F80 no Act
                            // 02
                            result = MA37;
                            break;
                        case 27:
                            // 475F80
                            // 02
                            result = MA37;
                            break;
                        case 32:
                            // 475F80
                            // 02
                            result = MA37;
                            break;
                        case 33:
                            // 475F80
                            // 02
                            result = MA35;
                            break;
                        case 35:
                            // 475F80
                            // 03
                            result = MA41;
                            break;
                        case 36:
                            // 475F89
                            // 03
                            result = MA41;
                            break;
                        case 37:
                            // 475F89
                            // 03
                            result = MA41;
                            break;
                        case 38:
                            // 475F89
                            // 03
                            result = MA41;
                            break;
                        case 39:
                            // 475F89
                            // 03
                            result = MA41;
                            break;
                        case 40:
                            // 475F89
                            // 03
                            result = MA41;
                            break;
                        case 41:
                            // 475F89
                            // 03
                            result = MA41;
                            break;
                        case 42:
                            // 475F89
                            // 04
                            result = MA46;
                            break;
                        case 43:
                            // 475F92
                            // 04
                            result = MA46;
                            break;
                        case 44:
                            // 475F92
                            // 04
                            result = MA46;
                            break;
                        case 45:
                            // 475F92
                            // 04
                            result = MA46;
                            break;
                        case 46:
                            // 475F92
                            // 04
                            result = MA46;
                            break;
                        case 47:
                            // 475F92
                            // 04
                            result = MA46;
                            break;
                        case 48:
                            // 475F92
                            // 03
                            result = MA41;
                            break;
                        case 49:
                            // 4777B3
                            // 03
                            result = MA41;
                            break;
                        case 50:
                            // 4777B3
                            // 03
                            result = MA41;
                            break;
                        case 52:
                            // 4777B3
                            // 03
                            result = MA41;
                            break;
                        case 53:
                            // 4777B3
                            // 03
                            result = MA41;
                            break;
                        // Modify the A .. B: 54 .. 058
                        case 54:
                            result = MA59;
                            break;
                        // Modify the A .. B: 94 .. 098
                        case 94:
                            result = MA59;
                            break;
                        case 59:
                            result = MA60;
                            break;
                        // Modify the A .. B: 60 .. 068
                        case 60:
                            result = MA55;
                            break;
                        // Modify the A .. B: 70 .. 075
                        case 70:
                            result = MA55;
                            break;
                        // Modify the A .. B: 76 .. 080
                        case 76:
                            result = MA35;
                            break;
                        // norm
                        // Modify the A .. B: 81 .. 083
                        case 81:
                            result = MA56;
                            break;
                        case 84:
                            // hero
                            result = MA57;
                            break;
                        case 85:
                            result = MA58;
                            break;
                        // Modify the A .. B: 90 .. 092
                        case 90:
                            result = MA55;
                            break;
                        case 111:
                            result = MA111;
                            break;
                        case 132:
                            result = MA131;
                            break;
                        default:
                            result = MA35;
                            break;
                    }
                    break;
                case 51:
                    // 0A
                    result = MA50;
                    break;
                case 52:
                    // 0A
                    result = MA19;
                    break;
                case 53:
                    // 475DDC
                    // 0A
                    result = MA19;
                    break;
                case 54:
                    // 475DDC
                    // 14
                    result = MA28;
                    break;
                case 55:
                    // 475E54
                    // 15
                    result = MA29;
                    break;
                case 56:
                    // 475E60
                    // 22
                    result = MA43;
                    break;
                case 57:
                    // 475EFC
                    // 22
                    result = MA15;
                    break;
                case 58:
                    // 22
                    result = MA15;
                    break;
                case 60:
                    // 16
                    result = MA33;
                    break;
                case 61:
                    // 475E6C
                    // 16
                    result = MA33;
                    break;
                case 62:
                    // 475E6C
                    // 16
                    result = MA33;
                    break;
                case 63:
                    // 475E6C
                    // 17
                    result = MA34;
                    break;
                case 64:
                    // 475E78
                    // 18
                    result = MA19;
                    break;
                case 65:
                    // 475E84
                    // 18
                    result = MA19;
                    break;
                case 66:
                    // 475E84
                    // 18
                    result = MA19;
                    break;
                case 67:
                    // 475E84
                    // 18
                    result = MA19;
                    break;
                case 68:
                    // 475E84
                    // 18
                    result = MA19;
                    break;
                case 69:
                    // 475E84
                    // 18
                    result = MA19;
                    break;
                case 70:
                    // 475E84
                    // 19
                    result = MA33;
                    break;
                case 71:
                    // 475E90
                    // 19
                    result = MA33;
                    break;
                case 72:
                    // 475E90
                    // 19
                    result = MA33;
                    break;
                case 73:
                    // 475E90
                    // 1A
                    result = MA19;
                    break;
                case 74:
                    // 475E9C
                    // 1B
                    result = MA19;
                    break;
                case 75:
                    // 475EA8
                    // 1C
                    result = MA39;
                    break;
                case 76:
                    // 475EB4
                    // 1D
                    result = MA38;
                    break;
                case 77:
                    // 475EC0
                    // 1E
                    result = MA39;
                    break;
                case 78:
                    // 475ECC
                    // 1F
                    result = MA40;
                    break;
                case 79:
                    // 475ED8
                    // 20
                    result = MA19;
                    break;
                case 80:
                    // 475EE4
                    // 21
                    result = MA42;
                    break;
                case 81:
                    // 475EF0
                    // 22
                    result = MA43;
                    break;
                case 83:
                    // 475EFC
                    // 23
                    result = MA44;
                    break;
                case 84:
                    // 475F08
                    // 24
                    result = MA47;
                    break;
                case 85:
                    // 475F14
                    // 24
                    result = MA47;
                    break;
                case 86:
                    // 475F14
                    // 24
                    result = MA47;
                    break;
                case 87:
                    // 475F14
                    // 24
                    result = MA47;
                    break;
                case 88:
                    // 475F14
                    // 24
                    result = MA47;
                    break;
                case 89:
                    // 475F14
                    // 24
                    result = MA47;
                    break;
                case 90:
                    // 475F14
                    // 11
                    result = MA47;
                    break;
                case 91:
                    // 475E30
                    // 06
                    result = MA91;
                    break;
                case 92:
                    // 06
                    result = MA92;
                    break;
                case 93:
                    // 06
                    result = MA93;
                    break;
                case 94:
                    // 14
                    result = MA28;
                    break;
                case 95:
                    // 475E54
                    // 15
                    result = MA29;
                    break;
                case 98:
                    // 475E60
                    // 25
                    result = MA27;
                    break;
                case 99:
                    // 475F20
                    // 26
                    result = MA26;
                    break;
                case 101:
                    // 475F29
                    // 19
                    result = MA33;
                    break;
                case 102:
                    // 475E90
                    result = MA48;
                    break;
                case 103:
                    result = MA49;
                    break;
                case 104:
                    result = MA49;
                    break;
                case 105:
                    result = MA49;
                    break;
                case 106:
                    result = MA50;
                    break;
                case 109:
                    result = MA51;
                    break;
                case 110:
                    result = MA50;
                    break;
                case 111:
                    result = MA54;
                    break;
                // Modify the A .. B: 113 .. 115
                case 113:
                    result = MA33;
                    break;
                case 117:
                    result = MA67;
                    break;
                case 118:
                case 119:
                    result = MA65;
                    break;
                case 120:
                    result = MA66;
                    break;
                case 121:
                    result = MA121;
                    break;
                case 122:
                    result = MA122;
                    break;
                case 123:
                    switch (Appr)
                    {
                        case 812:
                            result = MA120;
                            break;
                        case 815:
                            result = MA123_815;
                            break;
                        case 825:
                            result = MA123_825;
                            break;
                        case 827:
                            result = MA123_827;
                            break;
                        default:
                            result = MA123;
                            break;
                    }
                    break;
                default:
                    result = MA19;
                    break;
            }
            return result;
        }

        public static int GetOffset(int Appr)
        {
            int result;
            int nRace;
            int nPos;
            result = 0;
            if ((Appr >= 1000))
            {
                return result;
            }
            nRace = Appr / 10;
            nPos = Appr % 10;
            switch (nRace)
            {
                case 0:
                    result = nPos * 280;
                    break;
                case 1:
                    result = nPos * 230;
                    break;
                // Modify the A .. B: 2, 3, 7 .. 12
                case 2:
                case 3:
                case 7:
                    result = nPos * 360;
                    break;
                case 4:
                    result = nPos * 360;
                    if (nPos == 1)
                    {
                        result = 600;
                    }
                    break;
                case 5:
                    result = nPos * 430;
                    break;
                case 6:
                    result = nPos * 440;
                    break;
                case 13:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 360;
                            break;
                        case 2:
                            result = 440;
                            break;
                        case 3:
                            result = 550;
                            break;
                        default:
                            result = nPos * 360;
                            break;
                    }
                    break;
                case 14:
                    result = nPos * 360;
                    break;
                case 15:
                    result = nPos * 360;
                    break;
                case 16:
                    result = nPos * 360;
                    break;
                case 17:
                    switch (nPos)
                    {
                        case 2:
                            result = 920;
                            break;
                        default:
                            result = nPos * 350;
                            break;
                    }
                    break;
                case 18:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 520;
                            break;
                        case 2:
                            result = 950;
                            break;
                        default:
                            result = 494 + nPos * 360;
                            break;
                    }
                    break;
                case 19:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 370;
                            break;
                        case 2:
                            result = 810;
                            break;
                        case 3:
                            result = 1250;
                            break;
                        case 4:
                            result = 1630;
                            break;
                        case 5:
                            result = 2010;
                            break;
                        case 6:
                            result = 2390;
                            break;
                    }
                    break;
                case 20:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 360;
                            break;
                        case 2:
                            result = 720;
                            break;
                        case 3:
                            result = 1080;
                            break;
                        case 4:
                            result = 1440;
                            break;
                        case 5:
                            result = 1800;
                            break;
                        case 6:
                            result = 2350;
                            break;
                        case 7:
                            result = 3060;
                            break;
                    }
                    break;
                case 21:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 460;
                            break;
                        case 2:
                            result = 820;
                            break;
                        case 3:
                            result = 1180;
                            break;
                        case 4:
                            result = 1540;
                            break;
                        case 5:
                            result = 1900;
                            break;
                        case 6:
                            result = 2440;
                            break;
                        case 7:
                            result = 2570;
                            break;
                        default:
                            result = 2700;
                            break;
                    }
                    break;
                case 22:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 430;
                            break;
                        case 2:
                            result = 1290;
                            break;
                        case 3:
                            result = 1810;
                            break;
                        case 4:
                            result = 2320;
                            break;
                        case 5:
                            result = 2920;
                            break;
                        case 6:
                            result = 3270;
                            break;
                        case 7:
                            result = 3620;
                            break;
                    }
                    break;
                case 23:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 340;
                            break;
                        case 2:
                            result = 680;
                            break;
                        case 3:
                            result = 1180;
                            break;
                        case 4:
                            // Fox mob 3
                            result = 1770;
                            break;
                        case 5:
                            // Fox mob 4
                            result = 2610;
                            break;
                        case 6:
                            // Fox mob 5
                            result = 2950;
                            break;
                        case 7:
                            // Fox mob 6
                            result = 3290;
                            break;
                        case 8:
                            // Fox mob 7
                            result = 3750;
                            break;
                        case 9:
                            result = 4460;
                            break;
                    }
                    break;
                case 24:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 510;
                            break;
                        case 2:
                            result = 1090;
                            break;
                        default:
                            result = 510 * (nPos + 1);
                            break;
                    }
                    break;
                case 25:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 510;
                            break;
                        case 2:
                            result = 1020;
                            break;
                        case 3:
                            result = 1370;
                            break;
                        case 4:
                            result = 1720;
                            break;
                        case 5:
                            result = 2070;
                            break;
                        case 6:
                            result = 2740;
                            break;
                        case 7:
                            result = 3780;
                            break;
                        case 8:
                            result = 3820;
                            break;
                        case 9:
                            result = 4170;
                            break;
                    }
                    break;
                case 26:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 340;
                            break;
                        case 2:
                            result = 680;
                            break;
                        case 3:
                            result = 1190;
                            break;
                        case 4:
                            result = 1930;
                            break;
                        case 5:
                            result = 2100;
                            break;
                        case 6:
                            result = 2440;
                            break;
                        case 7:
                            result = 2540;
                            break;
                        case 8:
                            result = 3570;
                            break;
                    }
                    break;
                case 28:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                    }
                    break;
                case 29:
                    result = 360 * nPos;
                    break;
                case 32:
                    switch (nPos)
                    {
                        case 0:
                            // mon33
                            result = 0;
                            break;
                        case 1:
                            result = 440;
                            break;
                        case 2:
                            result = 820;
                            break;
                        case 3:
                            result = 1360;
                            break;
                        case 4:
                            result = 2650;
                            break;
                        case 5:
                            result = 2680;
                            break;
                        case 6:
                            result = 2790;
                            break;
                        case 7:
                            result = 2900;
                            break;
                        case 8:
                            result = 3500;
                            break;
                        case 9:
                            result = 3930;
                            break;
                    }
                    break;
                case 33:
                    switch (nPos)
                    {
                        case 0:
                            // mon34
                            result = 20;
                            break;
                        case 1:
                            result = 720;
                            break;
                        case 2:
                            result = 1160;
                            break;
                        case 3:
                            result = 1770;
                            break;
                        case 4:
                            result = 1780;
                            break;
                        case 5:
                            result = 1790;
                            break;
                        case 6:
                            result = 1840;
                            break;
                        case 7:
                            result = 2540;
                            break;
                        case 8:
                            result = 2900;
                            break;
                        default:
                            result = (nPos - 7) * 360 + 2900;
                            break;
                    }
                    break;
                case 34:
                    switch (nPos)
                    {
                        case 0:
                            // mon35
                            result = 0;
                            break;
                        case 1:
                            result = 680;
                            break;
                        case 2:
                            result = 1030;
                            break;
                        default:
                            result = (nPos - 6) * 360 + 1800;
                            break;
                    }
                    break;
                case 35:
                    switch (nPos)
                    {
                        case 0:
                            // mon35
                            result = 0;
                            break;
                        case 1:
                            result = 810;
                            break;
                        case 2:
                            result = 1800;
                            break;
                        case 3:
                            result = 2610;
                            break;
                        case 4:
                            result = 3420;
                            break;
                        case 5:
                            result = 4390;
                            break;
                        case 6:
                            result = 5200;
                            break;
                        case 7:
                            result = 6170;
                            break;
                        case 8:
                            result = 6980;
                            break;
                        case 9:
                            result = 7790;
                            break;
                    }
                    break;
                case 81:
                    switch (nPos)
                    {
                        case 0:
                            result = 8760;
                            break;
                        case 1:
                            result = 9570;
                            break;
                        case 2:
                            result = 10380;
                            break;
                        case 3:
                            // ...
                            result = 11030;
                            break;
                        case 4:
                            result = 12000;
                            break;
                        case 5:
                            result = 13800;
                            break;
                        case 6:
                            result = 14770;
                            break;
                        case 7:
                            result = 15580;
                            break;
                        case 8:
                            result = 16390;
                            break;
                        case 9:
                            result = 17360;
                            break;
                    }
                    break;
                case 82:
                    switch (nPos)
                    {
                        case 0:
                            result = 18330;
                            break;
                        case 1:
                            result = 19300;
                            break;
                        case 2:
                            result = 20270;
                            break;
                        case 3:
                            result = 21240;
                            break;
                        case 4:
                            result = 22050;
                            break;
                        case 5:
                            result = 22860;
                            break;
                        case 6:
                            result = 23990;
                            break;
                        case 7:
                            result = 24800;
                            break;
                        case 8:
                            result = 25930;
                            break;
                    }
                    break;
                case 70:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 360;
                            break;
                        case 2:
                            result = 720;
                            break;
                        case 3:
                            result = 0;
                            break;
                        case 4:
                            result = 350;
                            break;
                        case 5:
                            result = 780;
                            break;
                        case 6:
                            result = 1130;
                            break;
                        case 7:
                            result = 1560;
                            break;
                        case 8:
                            result = 1910;
                            break;
                    }
                    break;
                case 80:
                    switch (nPos)
                    {
                        case 0:
                            result = 0;
                            break;
                        case 1:
                            result = 80;
                            break;
                        case 2:
                            result = 300;
                            break;
                        case 3:
                            result = 301;
                            break;
                        case 4:
                            result = 302;
                            break;
                        case 5:
                            result = 320;
                            break;
                        case 6:
                            result = 321;
                            break;
                        case 7:
                            result = 322;
                            break;
                        case 8:
                            result = 321;
                            break;
                    }
                    break;
                case 90:
                    switch (nPos)
                    {
                        case 0:
                            result = 80;
                            break;
                        case 1:
                            result = 168;
                            break;
                        case 2:
                            result = 184;
                            break;
                        case 3:
                            result = 200;
                            break;
                        case 4:
                            result = 1770;
                            break;
                        case 5:
                            result = 1780;
                            break;
                        case 6:
                            result = 1790;
                            break;
                    }
                    break;
                default:
                    result = nPos * 360;
                    break;
            }
            return result;
        }

        public static int GetNpcOffset(int nAppr)
        {
            int result;
            result = 0;
            switch (nAppr)
            {
                // Modify the A .. B: 0 .. 22
                case 0:
                    result = nAppr * 60;
                    break;
                case 23:
                    result = 1380;
                    break;
                case 24:
                case 25:
                    result = (nAppr - 24) * 60 + 1470;
                    break;
                case 27:
                case 32:
                    result = (nAppr - 26) * 60 + 1620 - 30;
                    break;
                // Modify the A .. B: 26, 28, 29, 30, 31, 33 .. 41
                case 26:
                case 28:
                case 29:
                case 30:
                case 31:
                case 33:
                    result = (nAppr - 26) * 60 + 1620;
                    break;
                case 42:
                case 43:
                    result = 2580;
                    break;
                // Modify the A .. B: 44 .. 47
                case 44:
                    result = 2640;
                    break;
                // Modify the A .. B: 48 .. 50
                case 48:
                    result = (nAppr - 48) * 60 + 2700;
                    break;
                case 51:
                    result = 2880;
                    break;
                case 52:
                    result = 2960;
                    break;
                // Modify the A .. B: 54 .. 058
                case 54:
                    result = 4490 + 10 * (nAppr - 54);
                    break;
                // Modify the A .. B: 94 .. 098
                case 94:
                    result = 4490 + 10 * (nAppr - 94);
                    break;
                case 59:
                    result = 4540;
                    break;
                // Modify the A .. B: 60 .. 67
                case 60:
                    result = 3060 + 60 * (nAppr - 60);
                    break;
                case 68:
                    result = 3600;
                    break;
                // Modify the A .. B: 70 .. 75
                case 70:
                    result = 3780 + 10 * (nAppr - 70);
                    break;
                // Modify the A .. B: 76 .. 77
                case 76:
                    result = 3840 + 60 * (nAppr - 76);
                    break;
                // Modify the A .. B: 81 .. 83
                case 81:
                    result = 3960 + 20 * (nAppr - 81);
                    break;
                // Modify the A .. B: 78 .. 80
                case 78:
                    result = 4060 + 60 * (nAppr - 78);
                    break;
                case 84:
                    result = 4030;
                    break;
                case 85:
                    result = 4250;
                    break;
                // Modify the A .. B: 90 .. 92
                case 90:
                    result = 3750 + 10 * (nAppr - 90);
                    break;
                // Modify the A .. B: 100 .. 123
                case 100:
                    switch (nAppr)
                    {
                        case 111:
                            result = 740;
                            break;
                        case 112:
                            result = 810;
                            break;
                        case 113:
                            result = 820;
                            break;
                        case 114:
                            result = 830;
                            break;
                        case 115:
                            result = 840;
                            break;
                        case 116:
                            result = 850;
                            break;
                        case 117:
                            result = 860;
                            break;
                        case 118:
                            result = 870;
                            break;
                        case 119:
                            result = 900;
                            break;
                        case 120:
                            result = 930;
                            break;
                        case 121:
                            result = 970;
                            break;
                        case 122:
                            result = 980;
                            break;
                        case 123:
                            result = 990;
                            break;
                        default:
                            result = (nAppr - 100) * 70;
                            break;
                    }
                    break;
                case 130:
                    result = 4240;
                    break;
                case 131:
                    result = 4560;
                    break;
                case 132:
                    result = 4770;
                    break;
                case 133:
                    result = 4810;
                    break;
            }
            return result;
        }
    }

    public struct TActionInfo
    {
        public short start;
        public short frame;
        public short skip;
        public short ftime;
        public short usetick;

        public TActionInfo(short start, short frame, short skip, short ftime, short usetick)
        {
            this.start = start;
            this.frame = frame;
            this.skip = skip;
            this.ftime = ftime;
            this.usetick = usetick;
        }
    }

    public class TMonsterAction
    {
        public TActionInfo ActStand;
        public TActionInfo ActWalk;
        public TActionInfo ActAttack;
        public TActionInfo ActCritical;
        public TActionInfo ActStruck;
        public TActionInfo ActDie;
        public TActionInfo ActDeath;
    }
}

