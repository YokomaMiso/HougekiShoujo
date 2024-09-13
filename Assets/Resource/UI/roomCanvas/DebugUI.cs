using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    void Update()
    {
        for (int i = 0; i < 2; i++)
        {
            Transform roomDataTransform=transform.GetChild(i);
            MachingRoomData.RoomData roomData = Managers.instance.roomManager.ReadRoomData(i == 0);

            Transform banner = roomDataTransform.GetChild(0);
            for(int j = 0; j < MachingRoomData.bannerMaxCount;j++)
            {
                banner.GetChild(j).GetComponent<Text>().text=roomData.GetBannerNum(j).ToString();
            }
            Transform charaIDs = roomDataTransform.GetChild(1);
            for (int j = 0; j < MachingRoomData.playerMaxCount; j++)
            {
                charaIDs.GetChild(j).GetComponent<Text>().text = roomData.GetSelectedCharacterID(j).ToString();
            }
            Transform readys = roomDataTransform.GetChild(2);
            for (int j = 0; j < MachingRoomData.playerMaxCount; j++)
            {
                readys.GetChild(j).GetComponent<Text>().text = roomData.GetReadyPlayers(j).ToString();
            }
        }
    }
}
