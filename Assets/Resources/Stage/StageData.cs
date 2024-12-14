using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Create/StageData/StageData", order = 1)]

public class StageData : ScriptableObject
{
    [SerializeField, Header("Stage Name"), TextArea(1, 3)] string[] stageName;
    [SerializeField, Header("Stage Prefab")] GameObject stagePrefab;
    [SerializeField, Header("Stage Size")] Vector3 stageSize;
    [SerializeField, Header("Stage Position")] Vector3[] defaultPosition;

    [SerializeField, Header("Stage BGM Data")] BGMData BGM;

    [SerializeField, Header("Directional Light")] GameObject directionalLight;

    [SerializeField, Header("Stage Screen Shot")] Sprite screenShot;
    [SerializeField, Header("Stage MiniMap")] Sprite minimap;

    [SerializeField, Header("MiniMap MinX")] float minX;
    [SerializeField, Header("MiniMap MaxX")] float maxX;
    [SerializeField, Header("MiniMap MinZ")] float minZ;
    [SerializeField, Header("MiniMap MaxZ")] float maxZ;

    public string GetStageName() { return  stageName[(int)Managers.instance.nowLanguage]; }
    public GameObject GetStagePrefab() { return stagePrefab; }
    public Vector3 GetStageSize() { return stageSize; }
    public float GetStageRadius() { return Mathf.Sqrt((stageSize.x * stageSize.x) + (stageSize.z * stageSize.z)) * 1.5f; }
    public Vector3 GetDefaultPosition(int _num) { return defaultPosition[_num]; }

    public BGMData GetBGMData() { return BGM; }

    public GameObject GetDirectionalLight() { return directionalLight; }

    public Sprite GetScreenShot() {  return screenShot; }
    public Sprite GetMinimap() { return minimap; }

    public float GetMinX() { return minX; }
    public float GetMinZ() { return minZ; }
    public float GetMaxX() { return maxX; }
    public float GetMaxZ() { return maxZ; }

}
