using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllStageData", menuName = "Create/StageData/AllStageData", order = 1)]

public class AllStageData : ScriptableObject
{
    [SerializeField] StageData[] stageObject;

    public int GetStageLength() { return stageObject.Length; }
    public StageData GetStageData(int _num)
    {
        if (_num > stageObject.Length) { _num = 0; }
        return stageObject[_num];
    }
}
