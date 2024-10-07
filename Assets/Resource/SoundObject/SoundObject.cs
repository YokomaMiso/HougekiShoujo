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

    GameObject loopBGMInstance;

    public void ReceiveSound(AudioClip _clip, SOUND_TYPE _type, bool _loop)
    {
        clip = _clip;

        OptionData oData = Managers.instance.optionData;

        source = gameObject.GetComponent<AudioSource>();
        source.loop = _loop;
        source.clip = clip;

        float volume = oData.masterVolume;
        float pitch = 1.0f;

        switch (_type)
        {
            case SOUND_TYPE.BGM:
                volume *= oData.bgmVolume;
                if (_loop) { lifeTime = Mathf.Infinity; }
                else { lifeTime = clip.length; }
                isBGM = true;
                break;

            case SOUND_TYPE.SFX:
            case SOUND_TYPE.VOICE:
                volume *= oData.sfxVolume;
                pitch *= Random.Range(0.8f, 1.2f);
                lifeTime = clip.length;
                break;
        }

        source.volume = volume;
        source.pitch = pitch;

        source.Play();
    }
    public void SpawnLoopBGM(AudioClip _clip)
    {
        loopBGMInstance = SoundManager.PlayBGM(_clip);
        loopBGMInstance.GetComponent<AudioSource>().Stop();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
            if (loopBGMInstance) { loopBGMInstance.GetComponent<AudioSource>().Play(); }
        }
    }
}
