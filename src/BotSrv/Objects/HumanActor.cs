namespace BotSrv.Objects
{
    public struct THumanAction
    {
        public TActionInfo ActStand;
        public TActionInfo ActWalk;
        public TActionInfo ActRun;
        public TActionInfo ActRushLeft;
        public TActionInfo ActRushRight;
        public TActionInfo ActWarMode;
        public TActionInfo ActHit;
        public TActionInfo ActUnitHit;
        public TActionInfo ActHeavyHit;
        public TActionInfo ActBigHit;
        public TActionInfo ActFireHitReady;
        public TActionInfo ActSpell;
        public TActionInfo ActSitdown;
        public TActionInfo ActStruck;
        public TActionInfo ActDie;
        public TActionInfo ActRush2;
        public TActionInfo ActSmiteHit;
        public TActionInfo ActSmiteLongHit;
        public TActionInfo ActSmiteLongHit2;
        public TActionInfo ActSmiteLongHit3;
        public TActionInfo ActSmiteWideHit;
        public TActionInfo ActMagic_104;
        public TActionInfo ActMagic_105;
        public TActionInfo ActMagic_106;
        public TActionInfo ActMagic_107;
        public TActionInfo ActMagic_108;
        public TActionInfo ActMagic_109;
        public TActionInfo ActMagic_110;
        public TActionInfo ActMagic_111;
        public TActionInfo ActMagic_112;
        public TActionInfo ActMagic_113;
        public TActionInfo ActMagic_114;
    }

    public class THumAction
    {
        public static THumanAction HA = new()
        {
            ActStand = new TActionInfo { start = 0, frame = 4, skip = 4, ftime = 200, usetick = 0 },
            ActWalk = new TActionInfo { start = 64, frame = 6, skip = 2, ftime = 90, usetick = 2 },
            ActRun = new TActionInfo { start = 128, frame = 6, skip = 2, ftime = 115, usetick = 3 },
            ActRushLeft = new TActionInfo { start = 128, frame = 3, skip = 5, ftime = 120, usetick = 3 },
            ActRushRight = new TActionInfo { start = 131, frame = 3, skip = 5, ftime = 120, usetick = 3 },
            ActWarMode = new TActionInfo { start = 192, frame = 1, skip = 0, ftime = 200, usetick = 0 },
            ActHit = new TActionInfo { start = 200, frame = 6, skip = 2, ftime = 85, usetick = 0 },
            ActUnitHit = new TActionInfo { start = 200, frame = 17, skip = 2, ftime = 85, usetick = 0 },
            ActHeavyHit = new TActionInfo { start = 264, frame = 6, skip = 2, ftime = 90, usetick = 0 },
            ActBigHit = new TActionInfo { start = 328, frame = 8, skip = 0, ftime = 70, usetick = 0 },
            ActFireHitReady = new TActionInfo { start = 192, frame = 1, skip = 0, ftime = 100, usetick = 0 },
            ActSpell = new TActionInfo { start = 392, frame = 6, skip = 2, ftime = 60, usetick = 0 },
            ActSitdown = new TActionInfo { start = 456, frame = 2, skip = 0, ftime = 300, usetick = 0 },
            ActStruck = new TActionInfo { start = 472, frame = 3, skip = 5, ftime = 70, usetick = 0 },
            ActDie = new TActionInfo { start = 536, frame = 4, skip = 4, ftime = 120, usetick = 0 },
            ActRush2 = new TActionInfo { start = 080, frame = 8, skip = 2, ftime = 77, usetick = 3 },
            ActSmiteHit = new TActionInfo { start = 160, skip = 15, frame = 5, ftime = 56, usetick = 0 },
            ActSmiteLongHit = new TActionInfo { start = 1920, frame = 5, skip = 5, ftime = 45, usetick = 0 },
            ActSmiteLongHit2 = new TActionInfo { start = 320, frame = 6, skip = 4, ftime = 80, usetick = 0 },
            ActSmiteLongHit3 = new TActionInfo { start = 320, frame = 6, skip = 4, ftime = 100, usetick = 0 },
            ActSmiteWideHit = new TActionInfo { start = 560, frame = 10, skip = 0, ftime = 78, usetick = 0 },
            ActMagic_104 = new TActionInfo { start = 640, frame = 6, skip = 4, ftime = 92, usetick = 0 },
            ActMagic_105 = new TActionInfo { start = 880, frame = 10, skip = 0, ftime = 88, usetick = 0 },
            ActMagic_106 = new TActionInfo { start = 800, frame = 8, skip = 2, ftime = 88, usetick = 0 },
            ActMagic_107 = new TActionInfo { start = 1040, frame = 13, skip = 7, ftime = 72, usetick = 0 },
            ActMagic_108 = new TActionInfo { start = 1200, frame = 6, skip = 4, ftime = 95, usetick = 0 },
            ActMagic_109 = new TActionInfo { start = 1440, frame = 12, skip = 8, ftime = 78, usetick = 0 },
            ActMagic_110 = new TActionInfo { start = 1600, frame = 12, skip = 8, ftime = 78, usetick = 0 },
            ActMagic_111 = new TActionInfo { start = 1760, frame = 14, skip = 6, ftime = 65, usetick = 0 },
            ActMagic_112 = new TActionInfo { start = 720, frame = 6, skip = 4, ftime = 95, usetick = 0 },
            ActMagic_113 = new TActionInfo { start = 400, frame = 12, skip = 8, ftime = 70, usetick = 0 },
            ActMagic_114 = new TActionInfo { start = 400, frame = 12, skip = 8, ftime = 85, usetick = 0 }
        };
    }
}

