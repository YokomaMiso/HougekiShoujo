using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    int soundID = -1;
    float lifeTime;
    AudioClip clip;
    AudioSource source;

    float timer;

    public void ReceiveSound(AudioClip _clip)
    {
        clip = _clip;
        lifeTime = clip.length;

        source = gameObject.AddComponent<AudioSource>();
        source.loop = false;
        source.clip = clip;
        source.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }
    }

    void OnDestroy()
    {
        SoundArray.DestroyReport(this.gameObject);    
    }
}
