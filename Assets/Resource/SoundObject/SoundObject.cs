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

    public void ReceiveSound(AudioClip _clip, bool _isBGM)
    {
        clip = _clip;
        lifeTime = clip.length;

        isBGM = _isBGM;

        OptionData oData = Managers.instance.optionData;

        source = gameObject.GetComponent<AudioSource>();
        source.loop = isBGM;
        source.clip = clip;

        float volume = oData.masterVolume;
        float pitch = 1.0f;
        if (isBGM) { volume *= oData.bgmVolume; }
        else { volume *= oData.sfxVolume; pitch *= Random.Range(0.8f, 1.2f); }
        source.volume = volume;
        source.pitch = pitch;

        source.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime) { Destroy(gameObject); }
    }
}
