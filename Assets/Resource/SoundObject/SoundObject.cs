using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    int soundID = -1;
    float lifeTime;
    AudioClip clip;
    AudioSource source;

    bool isBGM = false;
    float timer;

    public void ReceiveSound(AudioClip _clip, SOUND_TYPE _type, bool _loop)
    {
        clip = _clip;

        source = gameObject.GetComponent<AudioSource>();
        source.loop = _loop;
        source.clip = clip;

        float volume = SoundManager.masterVolume;
        float pitch = 1.0f;

        switch (_type)
        {
            case SOUND_TYPE.BGM:
                volume *= SoundManager.bgmVolume/2;
                if (_loop) { lifeTime = Mathf.Infinity; }
                else { lifeTime = clip.length; }
                isBGM = true;
                source.spatialBlend = 0;
                break;

            case SOUND_TYPE.SFX:
                pitch *= Random.Range(0.9f, 1.1f);
                volume *= SoundManager.sfxVolume;
                lifeTime = clip.length;
                break;

            case SOUND_TYPE.SFX_FOR_UI:
                volume *= SoundManager.sfxVolume;
                lifeTime = clip.length;
                source.spatialBlend = 0;
                break;

            case SOUND_TYPE.VOICE:
                volume *= SoundManager.voiceVolume;
                lifeTime = clip.length;
                break;
        }

        source.volume = volume;
        source.pitch = pitch;

        source.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime) { Destroy(gameObject); }
    }
}
