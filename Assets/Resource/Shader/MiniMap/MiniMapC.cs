using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapC : MonoBehaviour
{


    //¥ß¥Ë¥Þ¥Ã¥×
    public Material miniMap;
    public void SetMiniMapMat(Material _mat) { miniMap = _mat; }
    public Material GetMiniMapMat() { return miniMap; }

    //int MaxPlayer = 6;

    // Start is called before the first frame update
    void Start()
    {
        /*
        for (int i = 0; i < 6; i++)
        {
            //if (OSCManager.OSCinstance.GetRoomData(i).myBannerNum == -1) { MaxPlayer--; }

        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        BindPlayerPosInShader();
    }


    void BindPlayerPosInShader()
    {
        //¥Þ¥Ã¥×¥¹¥±©`¥EÎÓ‹ËãÊ½½«À´¤ÇÐÞÕý¤·¤Þ¤¹
        int playerCount = Managers.instance.gameManager.GetPlayerCount();
        Vector4[] playerPositions = new Vector4[playerCount];
        float[] playerTeams = new float[playerCount];

        int arrayIndex = 0;

        for (int num = 0; num < playerCount; num++)
        {
            //MachingRoomData.RoomData roomData;
            //if (num == Managers.instance.playerID) { roomData = OSCManager.OSCinstance.roomData; }
            //else { roomData = OSCManager.OSCinstance.GetRoomData(num); }
            //if (roomData.myBannerNum == -1) { continue; }

            //Is there Player in now number?
            GameObject nowPlayer = Managers.instance.gameManager.GetPlayer(num);
            if (nowPlayer == null) { continue; }

            //Set color from number
            playerTeams[arrayIndex] = num % 2;

            //Set position from Player instance
            Vector3 playerPos = nowPlayer.transform.position;
            playerPos.x = (playerPos.x + 50) / 100;
            playerPos.z = (playerPos.z + 52) / 100;

            playerPositions[arrayIndex] = new Vector4(playerPos.x, 0, playerPos.z, 1);

            arrayIndex++;
            /*
            if (num == Managers.instance.playerID)
            {
                Vector3 playerPos = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerPos;
                playerPos.x = (playerPos.x + 50) / 100;
                playerPos.z = (playerPos.z + 52) / 100;

                playerPositions[num] = new Vector4(playerPos.x, 0, playerPos.z, 1);
            }
            else
            {
                Vector3 playerPos = OSCManager.OSCinstance.GetIngameData(num).mainPacketData.inGameData.playerPos;
                playerPos.x = (playerPos.x + 50) / 100;
                playerPos.z = (playerPos.z + 52) / 100;

                playerPositions[num] = new Vector4(playerPos.x, 0, playerPos.z, 1);
            }
            */
        }

        //Debug.Log("MaxPlayer: " + playerPositions[1]);

        if (miniMap != null)
        {
            miniMap.SetVectorArray("_PlayerPositions", playerPositions);
            miniMap.SetFloatArray("_PlayerTeam", playerTeams);
        }
    }
}
