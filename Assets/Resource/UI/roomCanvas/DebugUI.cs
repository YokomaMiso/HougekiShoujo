using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    void Update()
    {
        RoomManager rm = Managers.instance.roomManager;

        //èâä˙âª
        for (int i = 0; i < 2; i++)
        {
            Transform roomDataTransform = transform.GetChild(i);

            Transform banner = roomDataTransform.GetChild(0);
            for (int j = 0; j < MachingRoomData.bannerMaxCount; j++)
            {
                banner.GetChild(j).GetComponent<Text>().text = (-1).ToString();
            }

            Transform charaIDs = roomDataTransform.GetChild(1);
            Transform readys = roomDataTransform.GetChild(2);
            for (int j = 0; j < MachingRoomData.playerMaxCount; j++)
            {
                charaIDs.GetChild(j).GetComponent<Text>().text = 0.ToString();
                readys.GetChild(j).GetComponent<Text>().text = false.ToString();
            }
        }

        //ë„ì¸
        for (int i = 0; i < 6; i++)
        {
            MachingRoomData.RoomData roomData = OSCManager.OSCinstance.GetRoomData(i);

            bool isMine = i == Managers.instance.playerID;

            Transform roomDataTransform;
            if (isMine) { roomDataTransform = transform.GetChild(0); }
            else { roomDataTransform = transform.GetChild(1); }

            Transform banner = roomDataTransform.GetChild(0);
            Transform charaIDs = roomDataTransform.GetChild(1);
            Transform readys = roomDataTransform.GetChild(2);

            if (isMine && roomData.myBannerNum != MachingRoomData.bannerEmpty)
            {
                banner.GetChild(roomData.myBannerNum).GetComponent<Text>().text = Managers.instance.playerID.ToString();
            }
            else
            {
                if (roomData.myBannerNum != MachingRoomData.bannerEmpty)
                {
                    //banner.GetChild(roomData.myBannerNum).GetComponent<Text>().text = roomData.myBannerNum.ToString();
                    banner.GetChild(roomData.myBannerNum).GetComponent<Text>().text = i.ToString();
                }
            }
            charaIDs.GetChild(i).GetComponent<Text>().text = roomData.GetSelectedCharacterID(i).ToString();

            readys.GetChild(i).GetComponent<Text>().text = roomData.GetReadyPlayers(i).ToString();
        }
    }
}
