using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamChatSpriteChanger : MonoBehaviour
{
    [SerializeField] Texture[] tex;

    void Start()
    {
        MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.roomData;
        transform.GetComponent<RawImage>().texture = tex[oscRoomData.myTeamNum];   
    }
}
