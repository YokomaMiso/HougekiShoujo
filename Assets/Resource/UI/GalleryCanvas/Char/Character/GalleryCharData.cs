using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GalleryCharacterData", menuName = "Gallery/GalleryCharacterData")]
public class CharacterData : ScriptableObject
{
    public CharacterPair charData;
    public List<WeaponPair> weapon;
    public Sprite sketh;
    public Material iconMaterial;
    public List<RuntimeAnimatorController> animatorControllers;
    //public List<AudioClip> CharVoice;
    public List<VoiceTextPair> CharVoice;
}


[System.Serializable]
public class VoiceTextPair
{
    public AudioClip voiceClip;
    public string voiceText;
}

[System.Serializable]
public class WeaponPair
{
    public Sprite weaponIcon;
    public string weaponName;
    [TextArea]public string weaponText;
}

[System.Serializable]
public class CharacterPair
{
    public string characterName;
    public string schoolText;
    public string schoolYear;
    [TextArea]public string charDesign;
    public Sprite characterIcon;
    public Sprite characterIllustration;
    public Sprite schoolIcon;
    public Sprite emoteIllustration;
}