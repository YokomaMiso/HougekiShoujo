using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapC : MonoBehaviour
{

    MachingRoomData.RoomData roomData;

    //ミニマップ
    public Material miniMap;
    public void SetMiniMapMat(Material _mat) { miniMap = _mat; }
    public Material GetMiniMapMat() { return miniMap; }

    int MaxPlayer = 6;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 6; i++)
        {
            if (OSCManager.OSCinstance.GetRoomData(i).myBannerNum == -1) { MaxPlayer--; }
            Debug.Log("MaxPlayer: " + MaxPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        BindPlayerPosInShader();
    }


    void BindPlayerPosInShader()
    {
        //マップスケールの計算式将来で修正します
        int PlayerCount = 0;
        Vector4[] playerPositions = new Vector4[MaxPlayer];
        float[] playerTeams = new float[MaxPlayer];
        for (int num = 0; num < MaxPlayer; num++)
        {

            if (num == Managers.instance.playerID) { roomData = OSCManager.OSCinstance.roomData; }
            else { roomData = OSCManager.OSCinstance.GetRoomData(num); }
            if (roomData.myBannerNum == -1) { continue; }

            if (num == Managers.instance.playerID)
            {
                if (OSCManager.OSCinstance.GetRoomData(num).myBannerNum % 2 == 0)
                {
                    playerTeams[PlayerCount] = 1;
                }
                else
                {
                    playerTeams[PlayerCount] = 0;
                }
                Vector3 playerPos = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData.playerPos;
                playerPos.x = (playerPos.x + 50) / 100;
                playerPos.z = (playerPos.z + 52) / 100;

                playerPositions[PlayerCount] = new Vector4(playerPos.x, 0, playerPos.z, 1);
                PlayerCount++;
            }
            else
            {
                //Vector3 playerPos = OSCManager.OSCinstance.GetIngameData(GetPlayerID()).mainPacketData.inGameData.playerPos;
            }
        }



        if (miniMap != null)
        {
            miniMap.SetVectorArray("_PlayerPositions", playerPositions);
            miniMap.SetFloatArray("_PlayerTeam", playerTeams);
        }
    }
}
