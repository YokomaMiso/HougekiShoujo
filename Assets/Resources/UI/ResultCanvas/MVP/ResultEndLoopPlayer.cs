using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultEndLoopPlayer : MonoBehaviour
{
    float timer;
    const float endLoopStartTime = 3.75f;

    AudioClip clip;
    public void SetClip(AudioClip _clip) { clip = _clip; }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer> endLoopStartTime)
        {
            SoundManager.PlayBGM(clip);
            Destroy(this);
        }
    }
}
