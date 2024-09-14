using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    void Update()
    {
        RoomManager rm = Managers.instance.roomManager;

        for (int i = 0; i < 2; i++)
        {
            Transform roomDataTransform = transform.GetChild(0);

            Transform banner = roomDataTransform.GetChild(0);
            for (int j = 0; j < MachingRoomData.bannerMaxCount; j++)
            {
                banner.GetChild(i).GetComponent<Text>().text = (-1).ToString();
            }

            Transform charaIDs = roomDataTransform.GetChild(1);
            Transform readys = roomDataTransform.GetChild(2);
            for (int j = 0; j < MachingRoomData.playerMaxCount; j++)
            {
                charaIDs.GetChild(i).GetComponent<Text>().text = 0.ToString();
                readys.GetChild(i).GetComponent<Text>().text = false.ToString();
            }
        }
        for (int i = 0; i < 6; i++)
        {
            MachingRoomData.RoomData roomData = OSCManager.OSCinstance.GetRoomData(i);

            Transform roomDataTransform;
            if (i == Managers.instance.playerID) { roomDataTransform = transform.GetChild(0); }
            else { roomDataTransform = transform.GetChild(1); }
            Transform banner = roomDataTransform.GetChild(0);
            Transform charaIDs = roomDataTransform.GetChild(1);
            Transform readys = roomDataTransform.GetChild(2);

            int[] allBannerNum = new int[8];
            for (int j = 0; j < 8; j++) { allBannerNum[j] = rm.GetBannerNumFromAllPlayer(j); }

            if (roomData.myBannerNum != MachingRoomData.bannerEmpty)
            {
                banner.GetChild(roomData.myBannerNum).GetComponent<Text>().text = roomData.GetBannerNum(roomData.myBannerNum).ToString();
            }

            charaIDs.GetChild(i).GetComponent<Text>().text = roomData.GetSelectedCharacterID(i).ToString();

            readys.GetChild(i).GetComponent<Text>().text = roomData.GetReadyPlayers(i).ToString();
        }
    }
}
