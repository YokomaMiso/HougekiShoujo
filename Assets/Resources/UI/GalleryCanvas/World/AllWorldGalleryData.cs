using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WorldData", menuName = "Gallery/AllWorldData")]
public class AllWorldGalleryData : ScriptableObject
{
    public List<WorldGalleryData> worldGalleryDatas;
    public Material buildMat;
}
