using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Video;

public class TitleVideoBehavior : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    void Start()
    {
        GameObject obj = SoundManager.PlaySFXForUI(audioClip, null, true);
        //obj.AddComponent<AudioLowPassFilter>().cutoffFrequency = 2000;
    }
}
