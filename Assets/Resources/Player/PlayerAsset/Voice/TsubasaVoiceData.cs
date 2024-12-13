using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TsubasaVoiceData", menuName = "Create/PlayerData/TsubasaVoiceData", order = 1)]
public class TsubasaVoiceData : PlayerVoiceData
{
    [SerializeField] AudioClip[] reload2;
    [SerializeField] AudioClip[] reload3;
    [SerializeField] AudioClip[] reload4;

    public override AudioClip GetReload(float _t)
    {
        if (_t <= 0) { return reload4[Random.Range(0, reload.Length)]; }
        else if (_t < 0.33f) { return reload3[Random.Range(0, reload.Length)]; }
        else if (_t < 0.66f) { return reload2[Random.Range(0, reload.Length)]; }
        else { return reload[Random.Range(0, reload.Length)]; }
    }
}
