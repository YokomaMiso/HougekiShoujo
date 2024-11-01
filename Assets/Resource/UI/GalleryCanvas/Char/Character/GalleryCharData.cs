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
    //public List<AudioClip> CharVoice;
    public List<VoiceTextPair> CharVoice;
}


[System.Serializable]
public class VoiceTextPair
{
    public AudioClip voiceClip;
    public string voiceText;
}