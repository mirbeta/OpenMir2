using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    public class ClientConf : Packets
    {
        public bool boGameAssist;
        public bool boWhisperRecord;
        public bool boMaketSystem;
        public bool boNoFog;
        public bool boStallSystem;
        public bool boShowHpBar;
        public bool boShowHpNumber;
        public bool boNoStruck;
        public bool boFastMove;
        public bool boNoWeight;
        public bool boShowFriend;
        public bool boShowRelationship;
        public bool boShowMail;
        public bool boShowRecharging;
        public bool boShowHelp;
        public bool boShowGameShop;
        public bool boGamepath;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(boGameAssist);
            writer.Write(boWhisperRecord);
            writer.Write(boMaketSystem);
            writer.Write(boNoFog);
            writer.Write(boStallSystem);
            writer.Write(boShowHpBar);
            writer.Write(boShowHpNumber);
            writer.Write(boNoStruck);
            writer.Write(boFastMove);
            writer.Write(boNoWeight);
            writer.Write(boShowFriend);
            writer.Write(boShowRelationship);
            writer.Write(boShowMail);
            writer.Write(boShowRecharging);
            writer.Write(boShowHelp);
            writer.Write(boShowGameShop);
            writer.Write(boGamepath);
        }
    }
}