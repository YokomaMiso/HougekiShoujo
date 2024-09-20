using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionData", menuName = "Create/OptionData", order = 1)]
public class OptionData : ScriptableObject
{
    public float masterVolume = 1.0f;
    public float bgmVolume = 0.5f;
    public float sfxVolume = 0.5f;
    public bool cameraShakeOn = true;
    public float mortarSensitive = 25.0f;
}
