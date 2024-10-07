using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBehavior : MonoBehaviour
{
    AudioClip loopClip;

    public void SetLoopClip(AudioClip _clip) 
    {
        loopClip = _clip;
        GetComponent<SoundObject>().SpawnLoopBGM(loopClip);
    }
}
