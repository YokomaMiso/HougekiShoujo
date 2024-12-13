using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName = "BGMData", menuName = "Create/StageData/BGMData", order = 1)]

public class BGMData : ScriptableObject
{
    [SerializeField, Header("BGM Title")] string bgmTitle;
    [SerializeField, Header("BGM Artist")] string artistName;

    [SerializeField, Header("Stage BGM")] AudioClip BGM;

    public string GetBGMTitle() { return bgmTitle; }
    public string GetBGMArtistName() { return artistName; }
    public AudioClip GetBGM() { return BGM; }
}
