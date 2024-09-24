using System.Runtime.InteropServices;

public class MachingRoomData
{
    public static readonly int playerMaxCount = 6;
    public static readonly int bannerEmpty = -1;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RoomData
    {
        //For Each
        public int myID;
        public int myTeamNum;
        public int selectedCharacterID;
        public bool ready;

        //For Server
        public int playerCount;
        public bool gameStart;
        
        //For Network
        public bool isInData;
        public bool isHandshaking;
    }

}
