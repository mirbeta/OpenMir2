using SystemModule;

namespace GameSvr
{
    public class TClientConf : Packets
    {
        public bool boClientCanSet;
        public bool boRunHuman;
        public bool boRunMon;
        public bool boRunNpc;
        public bool boWarRunAll;
        public byte btDieColor;
        public ushort wSpellTime;
        public ushort wHitIime;
        public ushort wItemFlashTime;
        public byte btItemSpeed;
        public bool boCanStartRun;
        public bool boParalyCanRun;
        public bool boParalyCanWalk;
        public bool boParalyCanHit;
        public bool boParalyCanSpell;
        public bool boShowRedHPLable;
        public bool boShowHPNumber;
        public bool boShowJobLevel;
        public bool boDuraAlert;
        public bool boMagicLock;
        public bool boAutoPuckUpItem;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(boClientCanSet);
            writer.Write(boRunHuman);
            writer.Write(boRunMon);
            writer.Write(boRunNpc);
            writer.Write(boWarRunAll);
            writer.Write(btDieColor);
            writer.Write(wSpellTime);
            writer.Write(wHitIime);
            writer.Write(wItemFlashTime);
            writer.Write(btItemSpeed);
            writer.Write(boCanStartRun);
            writer.Write(boParalyCanRun);
            writer.Write(boParalyCanWalk);
            writer.Write(boParalyCanHit);
            writer.Write(boParalyCanSpell);
            writer.Write(boShowRedHPLable);
            writer.Write(boShowHPNumber);
            writer.Write(boShowJobLevel);
            writer.Write(boDuraAlert);
            writer.Write(boMagicLock);
            writer.Write(boAutoPuckUpItem);
        }
    }
}