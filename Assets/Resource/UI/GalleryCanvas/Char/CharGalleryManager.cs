using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharGalleryManager", menuName = "Gallery/CharGalleryManager")]
public class CharGalleryManager : ScriptableObject
{
    public List<CharacterData> characterList;
}
