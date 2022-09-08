namespace GameSvr.Maps
{
    public class TMapPoint
    {
        public int X
        {
            get
            {
                return FX;
            }
            set
            {
                FX = value;
            }
        }

        public int Y
        {
            get
            {
                return FY;
            }
            set
            {
                FY = value;
            }
        }

        public bool Through
        {
            get
            {
                return FThrough;
            }
            set
            {
                FThrough = value;
            }
        }

        private int FX;
        private int FY;
        private bool FThrough;

        public TMapPoint(int nX, int nY)
        {
            FX = nX;
            FY = nY;
            FThrough = false;
        }
    }
}