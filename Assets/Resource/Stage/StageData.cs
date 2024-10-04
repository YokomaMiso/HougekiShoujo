using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Create/StageData/StageData", order = 1)]

public class StageData : ScriptableObject
{
    [SerializeField, Header("StagePrefab")] GameObject stagePrefab;
    [SerializeField, Header("Stage Size")] Vector3 stageSize;
    [SerializeField, Header("Stage Position")] Vector3[] defaultPosition;

    public GameObject GetStagePrefab() { return  stagePrefab; }
    public Vector3 GetStageSize() {  return stageSize; }
    public float GetStageRadius() { return Mathf.Sqrt((stageSize.x* stageSize.x)+(stageSize.z* stageSize.z)); }
    public Vector3 GetDefaultPosition(int _num) { return defaultPosition[_num]; }
}
