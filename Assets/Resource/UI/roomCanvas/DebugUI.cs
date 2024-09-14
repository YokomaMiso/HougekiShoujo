using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    void Update()
    {
        for (int i = 0; i < 6; i++)
        {
            MachingRoomData.RoomData roomData = OSCManager.OSCinstance.GetRoomData(i);

            Transform roomDataTransform;
            if (i == Managers.instance.playerID) { roomDataTransform = transform.GetChild(0); }
            else { roomDataTransform = transform.GetChild(1); }

            int[] allBannerNum = Managers.instance.roomManager.GetAllBannerNum();

            Transform banner = roomDataTransform.GetChild(0);
            if (roomData.myBannerNum != MachingRoomData.bannerEmpty)
            {
                banner.GetChild(roomData.myBannerNum).GetComponent<Text>().text = roomData.GetBannerNum(roomData.myBannerNum).ToString();
            }

            Transform charaIDs = roomDataTransform.GetChild(1);
            charaIDs.GetChild(i).GetComponent<Text>().text = roomData.GetSelectedCharacterID(i).ToString();

            Transform readys = roomDataTransform.GetChild(2);
            readys.GetChild(i).GetComponent<Text>().text = roomData.GetReadyPlayers(i).ToString();
    }
}
}
