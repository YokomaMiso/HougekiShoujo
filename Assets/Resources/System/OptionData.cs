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
    public float sfxVolume = 1.0f;
    public float voiceVolume = 1.0f;
    public bool cameraShakeOn = true;
    public float mortarSensitive = 20.0f;
    public bool displayTutorial = true;
    public int languageNum = 0;

    public void Init(OptionData _od)
    {
        playerName = _od.playerName;
        masterVolume = _od.masterVolume;
        bgmVolume = _od.bgmVolume;
        sfxVolume = _od.sfxVolume;
        voiceVolume = _od.voiceVolume;
        cameraShakeOn = _od.cameraShakeOn;
        mortarSensitive = _od.mortarSensitive;
        displayTutorial = _od.displayTutorial;
        languageNum = _od.languageNum;
    }
}
