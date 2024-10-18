using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GalleryCharacterData", menuName = "Gallery/GalleryCharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite characterIcon;
    public Sprite characterIllustration;
    public Material iconMaterial;
    public List<RuntimeAnimatorController> animatorControllers;
}