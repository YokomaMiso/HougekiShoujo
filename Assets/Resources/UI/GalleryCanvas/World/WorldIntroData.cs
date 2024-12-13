using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WorldIntroText", menuName = "Gallery/WorldIntroData")]
public class WorldIntroData : ScriptableObject
{
    public List<WorldIntroTextPair> worldIntro;
}

[System.Serializable]
public class WorldIntroTextPair
{
    public string Name;
    [TextArea] public string WorldIntroText;
}