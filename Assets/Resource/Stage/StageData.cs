using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Create/StageData", order = 1)]

public class StageData : ScriptableObject
{
    [SerializeField] GameObject[] stageObject;

    public int GetStageLength() { return stageObject.Length; }
    public GameObject GetStageObject(int _num) 
    {
        if (_num > stageObject.Length) { _num = 0; }
        return stageObject[_num]; 
    }
}
