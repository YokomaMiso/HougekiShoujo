using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MachingRoomData
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RoomData
    {
        public int[] bannerNum;
        public int[] selectedCharacterID;
        public bool[] readyPlayers;
        public int hostPlayer;
        public bool gameStart;
        const int bannerEmpty = -1;
    }
}
