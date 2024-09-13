using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AllGameData
{
    [StructLayout(LayoutKind.Sequential)]
    public struct AllData
    {

        public IngameData.PlayerNetData pData;
        public MachingRoomData.RoomData rData;
    }
}
