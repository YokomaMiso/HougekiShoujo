using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    const float lowPassMax = 22000;
    const float lowPassMin = 1200;
    static float blindScale;
    static bool blind;

    SOUND_TYPE soundType;

    AudioLowPassFilter lowPassInstance = null;
    float lifeTime;
    AudioClip clip;
    AudioSource source;

    float timer;

    [SerializeField, Range(0.0f, 2.0f)] float bgmRate = 0.5f;
    [SerializeField, Range(0.0f, 2.0f)] float sfxRate = 1.0f;
    [SerializeField, Range(0.0f, 2.0f)] float uiSfxRate = 1.0f;
    [SerializeField, Range(0.0f, 2.0f)] float voiceRate = 1.0f;
    [SerializeField, Range(0.0f, 2.0f)] float uiVoiceRate = 1.0f;

    public static void SetBlind(float _t)
    {
        blind = true;
        blindScale = _t;
    }
    public static void ResetBlind()
    {
        blind = false;
        blindScale = 0;
    }

    public void ReceiveSound(AudioClip _clip, SOUND_TYPE _type, bool _loop)
    {
        clip = _clip;

        source = gameObject.GetComponent<AudioSource>();
        source.loop = _loop;
        source.clip = clip;

        float volume = SoundManager.masterVolume;
        float pitch = 1.0f;

        soundType = _type;
        switch (soundType)
        {
            case SOUND_TYPE.BGM:
                volume *= SoundManager.bgmVolume * bgmRate;
                if (_loop) { lifeTime = Mathf.Infinity; }
                else { lifeTime = clip.length; }
                source.spatialBlend = 0;
                break;

            case SOUND_TYPE.SFX:
                pitch *= Random.Range(0.95f, 1.05f);
                volume *= SoundManager.sfxVolume * sfxRate;
                lifeTime = clip.length;
                break;

            case SOUND_TYPE.SFX_FOR_UI:
                volume *= SoundManager.sfxVolume * uiSfxRate;
                if (_loop) { lifeTime = Mathf.Infinity; }
                else { lifeTime = clip.length; }
                source.spatialBlend = 0;
                break;

            case SOUND_TYPE.VOICE:
                volume *= SoundManager.voiceVolume * voiceRate;
                lifeTime = clip.length;
                break;
            case SOUND_TYPE.VOICE_FOR_UI:
                volume *= SoundManager.voiceVolume * uiVoiceRate;
                lifeTime = clip.length;
                source.spatialBlend = 0;
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

        if (blind)
        {
            if (lowPassInstance == null) { CreateLowPassFilter(); }
            else { LowPassFilterUpdate(); }
        }
        else
        {
            if (lowPassInstance != null) { Destroy(lowPassInstance); }
        }
    }

    void CreateLowPassFilter()
    {
        lowPassInstance = transform.AddComponent<AudioLowPassFilter>();
        lowPassInstance.cutoffFrequency = Mathf.Lerp(lowPassMin, lowPassMax, blindScale);
    }

    void LowPassFilterUpdate()
    {
        lowPassInstance.cutoffFrequency = Mathf.Lerp(lowPassMin, lowPassMax, blindScale);
    }
}
