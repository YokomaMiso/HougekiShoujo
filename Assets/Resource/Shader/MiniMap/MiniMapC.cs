using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapC : MonoBehaviour
{
    public Material miniMap;
    public void SetMiniMapMat(Material _mat) { miniMap = _mat; }
    public Material GetMiniMapMat() { return miniMap; }

    void Update()
    {
        BindPlayerPosInShader();
    }

    void BindPlayerPosInShader()
    {
        int playerCount = OSCManager.OSCinstance.GetRoomData(0).playerCount;
        Vector4[] playerPositions = new Vector4[playerCount];
        float[] playerTeams = new float[playerCount];

        int arrayIndex = 0;

        for (int num = 0; num < playerCount; num++)
        {
            //Is there Player in now number?
            Player nowPlayer = Managers.instance.gameManager.GetPlayer(num);
            if (nowPlayer == null) { continue; }

            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(num);

            //Set color from number
            playerTeams[arrayIndex] = oscRoomData.myTeamNum;

            //Set position from Player instance
            Vector3 playerPos = nowPlayer.transform.position;
            playerPos.x = (playerPos.x + 50) / 100;
            playerPos.z = (playerPos.z + 52) / 100;

            playerPositions[arrayIndex] = new Vector4(playerPos.x, 0, playerPos.z, 1);

            arrayIndex++;
        }

        if (miniMap != null)
        {
            miniMap.SetVectorArray("_PlayerPositions", playerPositions);
            miniMap.SetFloatArray("_PlayerTeam", playerTeams);
        }
    }
}
