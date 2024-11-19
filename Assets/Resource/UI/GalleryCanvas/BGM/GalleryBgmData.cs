using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SoundCollection", menuName = "Gallery/SoundData")]
public class GallerySoundData : ScriptableObject
{
    public List<Sounds> sounds;
}

[System.Serializable]
public class Sounds
{
    public string Name;
    public string Author;
    public string Time;
    public AudioClip BgmClip;
}