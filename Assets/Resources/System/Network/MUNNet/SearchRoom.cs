using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class SearchRoom : MonobitEngine.MonoBehaviour
{
    public void searchRoom()
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
            foreach(RoomData player in MonobitNetwork.GetRoomData())
            {
                
            }
        }

        return ;
    }
}
