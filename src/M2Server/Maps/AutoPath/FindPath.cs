namespace M2Server.Maps.AutoPath
{
    public class FindPath : PathMap {
        public PointInfo[] Path {
            get {
                return _path;
            }
            set {
                _path = value;
            }
        }

        private PointInfo[] _path;
        private Envirnoment _pathEnvir;
        public short BeginX;
        public short BeginY;
        public short EndX;
        public short EndY;

        public FindPath() : base() {
            this.StartFind = false;
        }

        public void Stop() {
            this.StartFind = false;
            BeginX = -1;
            BeginY = -1;
            EndX = -1;
            EndY = -1;
            this.PathMapArray = new PathcellSuccess[0, 0];
            this.PathMapArray = null;
        }

        public PointInfo[] Find(short stopX, short stopY, bool run) {
            EndX = stopX;
            EndY = stopY;
            return this.FindPathOnMap(stopX, stopY, run);
        }

        public PointInfo[] Find(Envirnoment envir, short startX, short startY, short stopX, short stopY, bool run) {
            this.Width = envir.Width;
            this.Height = envir.Height;
            BeginX = startX;
            BeginY = startY;
            EndX = stopX;
            EndY = stopY;
            _path = null;
            _pathEnvir = envir;
            this.StartFind = true;
            this.PathMapArray = this.FillPathMap(startX, startY, stopX, stopY);
            return this.FindPathOnMap(stopX, stopY, run);
        }

        public void SetStartPos(short startX, short startY) {
            BeginX = startX;
            BeginY = startY;
            this.PathMapArray = this.FillPathMap(startX, startY, -1, -1);
        }

        public int GetCost(short x, short y, int direction) {
            int result;
            int nX;
            int nY;
            if (_pathEnvir != null) {
                direction = direction & 7;
                if ((x < 0) || (x >= this.ClientRect.Right - this.ClientRect.Left) || (y < 0) || (y >= this.ClientRect.Bottom - this.ClientRect.Top)) {
                    result = -1;
                }
                else {
                    nX = this.MapX(x);
                    nY = this.MapY(y);
                    if (_pathEnvir.CanWalkEx(nX, nY, false)) {
                        result = 4;
                    }
                    else {
                        result = -1;
                    }
                    // 如果是斜方向,则COST增加
                    if (((direction & 1) == 1) && (result > 0)) {
                        result = result + (result >> 1); // 应为Result*sqt(2),此处近似为1.5
                    }
                }
            }
            else {
                result = -1;
            }
            return result;
        }
    }
}