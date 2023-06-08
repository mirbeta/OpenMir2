using M2Server.Maps;
using SystemModule;
using SystemModule.Data;

namespace M2Server.Player {
    public class PlayCloneObject : PlayObject {

        public PlayCloneObject(PlayObject PlayObject) : base() {
            this.ChrName = "Clone";
            this.CurrX = PlayObject.CurrX;
            this.CurrY = PlayObject.CurrY;
            this.Dir = GetBackDir(PlayObject.Dir);
            this.Envir = PlayObject.Envir;
            this.Gender = PlayObject.Gender;
            this.Hair = PlayObject.Hair;
            this.Envir.AddMapObject(this.CurrX, this.CurrY, CellType.Play, this.ActorId, this);
            this.SendRefMsg(Messages.RM_TURN, this.Dir, this.CurrX, this.CurrY, 0, this.ChrName);
        }

        protected override bool Operate(ProcessMessage ProcessMsg) {
            return base.Operate(ProcessMsg);
        }
    }
}

