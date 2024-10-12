using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "BGMData", menuName = "Create/StageData/BGMData", order = 1)]

public class BGMData : ScriptableObject
{
    [SerializeField, Header("BGM Title")] string bgmTitle;
    [SerializeField, Header("BGM Artist")] string artistName;

    [SerializeField, Header("Stage BGM Intro")] AudioClip BGMIntro;
    [SerializeField, Header("Stage BGM Loop")] AudioClip BGMLoop;

    public string GetBGMTitle() { return bgmTitle; }
    public string GetBGMArtistName() { return artistName; }
    public AudioClip GetBGMIntro() { return BGMIntro; }
    public AudioClip GetBGMLoop() { return BGMLoop; }
}
