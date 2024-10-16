using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerifListBehavior : MonoBehaviour
{
    [SerializeField] GameObject serifWindow;

    public void AddSerif(MachingRoomData.RoomData _roomData, RADIO_CHAT_ID _chatID)
    {
        if (OSCManager.OSCinstance.roomData.myTeamNum != _roomData.myTeamNum) { return; }

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<CharaSerifBehavior>().ChatNumAdd();
        }

        GameObject serifInstance = Instantiate(serifWindow, transform);
        serifInstance.GetComponent<CharaSerifBehavior>().SetSerif(_roomData, _chatID);
    }
}
