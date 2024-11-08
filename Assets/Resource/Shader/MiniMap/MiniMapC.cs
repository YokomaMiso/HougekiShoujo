using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapC : MonoBehaviour
{
    public Material miniMap;
    public void SetMiniMapMat(Material _mat) { miniMap = _mat; }
    public Material GetMiniMapMat() { return miniMap; }

    [SerializeField] public AllStageData allStageData;

    public float minX;
    public float minZ;
    public float maxX;
    public float maxZ;

    void Update()
    {
        BindPlayerPosInShader();
    }

    void Start()
    {
        MachingRoomData.RoomData roomData;
        if (Managers.instance.playerID == 0)
        {
            roomData = OSCManager.OSCinstance.roomData;
        }
        else
        {
            roomData = OSCManager.OSCinstance.GetRoomData(0);
        }

        StageData nowStageData = allStageData.GetStageData(roomData.stageNum);
        if (nowStageData != null)
        {
            SetMapBounds(
                nowStageData.GetMinX(),
                nowStageData.GetMaxX(),
                nowStageData.GetMinZ(),
                nowStageData.GetMaxZ(),
                nowStageData.GetMinimap()
            );
        }
    }

    public void SetMapBounds(float _minX,float _maxX,float _minZ,float _maxZ, Sprite minimapSprite)
    {
        minX = _minX;
        maxX = _maxX;
        minZ = _minZ;
        maxZ = _maxZ;

        Texture minimapTexture = minimapSprite.texture;

        miniMap.SetTexture("_MainTex", minimapTexture);
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
            if (!nowPlayer.GetAlive()) { continue; }

            MachingRoomData.RoomData oscRoomData = OSCManager.OSCinstance.GetRoomData(num);

            //Set color from number
            playerTeams[arrayIndex] = oscRoomData.myTeamNum;

            //Set position from Player instance
            Vector3 playerPos = nowPlayer.transform.position;
            float normalizedX = ((playerPos.x - minX)/(maxX-minX));
            float normalizedZ = ((playerPos.z - minZ) / (maxZ - minZ));

            //playerPos.x = (playerPos.x + 50) / 100;
            //playerPos.z = (playerPos.z + 52) / 100;

            playerPositions[arrayIndex] = new Vector4(normalizedX, 0, normalizedZ, 1);

            arrayIndex++;
        }

        if (miniMap != null)
        {
            miniMap.SetVectorArray("_PlayerPositions", playerPositions);
            miniMap.SetFloatArray("_PlayerTeam", playerTeams);
        }
    }
}
