using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MachingRoomData
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public unsafe struct RoomData
    {
        public fixed int bannerNum[8];
        public fixed int selectedCharacterID[6];
        public fixed bool readyPlayers[6];
        public int hostPlayer;
        public bool gameStart;
    }

    public static int bannerEmpty = -1;
    public static int bannerMaxCount = 8;
    public static int playerMaxCount = 6;
}
