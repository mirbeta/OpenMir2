﻿using SystemModule.Data;

namespace SystemModule
{
    public record struct MapDoor
    {
        public int DoorId;
        public short nX;
        public short nY;
        public DoorStatus Status;
        public int n08;

        public MapDoor()
        {
            // DoorId = M2Share.ActorMgr.GetNextIdentity();
        }
    }
}