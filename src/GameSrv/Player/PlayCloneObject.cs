using GameSrv.Maps;
using SystemModule.Data;

namespace GameSrv.Player {
    public class PlayCloneObject : PlayObject {

        public PlayCloneObject(PlayObject PlayObject) : base() {
            this.ChrName = "Clone";
            this.CurrX = PlayObject.CurrX;
            this.CurrY = PlayObject.CurrY;
            this.Direction = GetBackDir(PlayObject.Direction);
            this.Envir = PlayObject.Envir;
            this.Gender = PlayObject.Gender;
            this.Hair = PlayObject.Hair;
            this.Envir.AddToMap(this.CurrX, this.CurrY, CellType.Play, this.ActorId, this);
            this.SendRefMsg(Messages.RM_TURN, this.Direction, this.CurrX, this.CurrY, 0, this.ChrName);
        }

        protected override bool Operate(ProcessMessage ProcessMsg) {
            return base.Operate(ProcessMsg);
        }
    }
}

