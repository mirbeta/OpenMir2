namespace M2Server.Maps
{
    public class MapPoint
    {
        public int X
        {
            get
            {
                return nX;
            }
            set
            {
                nX = value;
            }
        }

        public int Y
        {
            get
            {
                return nY;
            }
            set
            {
                nY = value;
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

        private int nX;
        private int nY;
        private bool FThrough;

        public MapPoint(int nX, int nY)
        {
            this.nX = nX;
            this.nY = nY;
            FThrough = false;
        }
    }
}