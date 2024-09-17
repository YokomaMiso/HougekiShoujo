using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MachingRoomData
{
    public static readonly int bannerMaxCount = 8;
    public static readonly int playerMaxCount = 6;
    public static readonly int bannerEmpty = -1;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct RoomData
    {
        public int myID;
        public int myBannerNum;
        /*
        public int bannerNum0;
        public int bannerNum1;
        public int bannerNum2;
        public int bannerNum3;
        public int bannerNum4;
        public int bannerNum5;
        public int bannerNum6;
        public int bannerNum7;
        */
        public int selectedCharacterID1;
        public int selectedCharacterID2;
        public int selectedCharacterID3;
        public int selectedCharacterID4;
        public int selectedCharacterID5;
        public int selectedCharacterID6;
        public bool readyPlayers1;
        public bool readyPlayers2;
        public bool readyPlayers3;
        public bool readyPlayers4;
        public bool readyPlayers5;
        public bool readyPlayers6;
        public int hostPlayer;
        public bool gameStart;

        public bool isInData;

        /*
        public void SetBannerNum(int _num, int _value)
        {
            switch (_num)
            {
                case 0: bannerNum0 = _value; break;
                case 1: bannerNum1 = _value; break;
                case 2: bannerNum2 = _value; break;
                case 3: bannerNum3 = _value; break;
                case 4: bannerNum4 = _value; break;
                case 5: bannerNum5 = _value; break;
                case 6: bannerNum6 = _value; break;
                case 7: bannerNum7 = _value; break;
            }
        }
        public int GetBannerNum(int _num)
        {
            switch (_num)
            {
                default: return bannerNum0;
                case 1: return bannerNum1;
                case 2: return bannerNum2;
                case 3: return bannerNum3;
                case 4: return bannerNum4;
                case 5: return bannerNum5;
                case 6: return bannerNum6;
                case 7: return bannerNum7;
            }
        }
        */

        public void SetSelectedCharacterID(int _num, int _value)
        {
            switch (_num)
            {
                case 0: selectedCharacterID1 = _value; break;
                case 1: selectedCharacterID2 = _value; break;
                case 2: selectedCharacterID3 = _value; break;
                case 3: selectedCharacterID4 = _value; break;
                case 4: selectedCharacterID5 = _value; break;
                case 5: selectedCharacterID6 = _value; break;
            }
        }
        public int GetSelectedCharacterID(int _num)
        {
            switch (_num)
            {
                default: return selectedCharacterID1;
                case 1: return selectedCharacterID2;
                case 2: return selectedCharacterID3;
                case 3: return selectedCharacterID4;
                case 4: return selectedCharacterID5;
                case 5: return selectedCharacterID6;
            }
        }

        public void SetReadyPlayers(int _num, bool _value)
        {
            switch (_num)
            {
                case 0: readyPlayers1 = _value; break;
                case 1: readyPlayers2 = _value; break;
                case 2: readyPlayers3 = _value; break;
                case 3: readyPlayers4 = _value; break;
                case 4: readyPlayers5 = _value; break;
                case 5: readyPlayers6 = _value; break;
            }
        }
        public bool GetReadyPlayers(int _num)
        {
            switch (_num)
            {
                default: return readyPlayers1;
                case 1: return readyPlayers2;
                case 2: return readyPlayers3;
                case 3: return readyPlayers4;
                case 4: return readyPlayers5;
                case 5: return readyPlayers6;
            }
        }
    }

}
