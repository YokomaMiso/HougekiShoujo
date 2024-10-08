using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionData", menuName = "Create/OptionData", order = 1)]
public class OptionData : ScriptableObject
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string playerName = "Player";
    public float masterVolume = 1.0f;
    public float bgmVolume = 0.5f;
    public float sfxVolume = 0.8f;
    public float voiceVolume = 0.7f;
    public bool cameraShakeOn = true;
    public float mortarSensitive = 35.0f;
}
