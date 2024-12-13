using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class SearchRoom : MonobitEngine.MonoBehaviour
{
    public void GetRoomData()
    {
        if(!MonobitNetwork.isConnect)
        {
            MonobitNetwork.playerName = OSCManager.OSCinstance.roomData.playerName;
            MonobitNetwork.autoJoinLobby = true;

            MonobitNetwork.ConnectServer("v1.0");

            return;
        }

        if(!MonobitNetwork.inRoom)
        {
            foreach (RoomData room in MonobitNetwork.GetRoomData())
            {
                //room.playerCount

                return;
            }
        }


        return;
    }
}
