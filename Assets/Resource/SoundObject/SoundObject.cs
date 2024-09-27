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
        float volume = oData.masterVolume;

        if (isBGM) { volume *= oData.bgmVolume; }
        else { volume *= oData.sfxVolume; }

        source = gameObject.AddComponent<AudioSource>();
        source.loop = isBGM;
        source.clip = clip;
        source.volume = volume;
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
