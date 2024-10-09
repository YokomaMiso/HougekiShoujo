using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public enum SOUND_TYPE { BGM = 0, SFX, VOICE };

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject soundObjectPrefab;
    static GameObject soundObject;
    static Transform thisTransform;

    static AudioSource nowBGMAudioSource = null;

    public static float masterVolume = 1.0f;
    public static float bgmVolume = 0.5f;
    public static float sfxVolume = 0.5f;
    public static float voiceVolume = 0.5f;

    void Start()
    {
        OptionData oData = Managers.instance.optionData;

        masterVolume = oData.masterVolume;
        bgmVolume = oData.bgmVolume;
        sfxVolume = oData.sfxVolume;
        voiceVolume = oData.voiceVolume;

        soundObject = soundObjectPrefab;
        thisTransform = transform;
    }

    public static void SetNowBGM(AudioSource _source) { nowBGMAudioSource = _source; }
    public static void BGMVolumeChange(float _volume)
    {
        if (nowBGMAudioSource == null) { return; }
        nowBGMAudioSource.volume = _volume;
    }
    public static GameObject PlayVoice(AudioClip _clip, Transform _transform = null)
    {
        if (_transform == null) { _transform = Camera.main.transform.GetChild(0); }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, SOUND_TYPE.VOICE, false);

        return obj;
    }
    public static GameObject PlaySFX(AudioClip _clip, Transform _transform = null)
    {
        if (_transform == null) { _transform = Camera.main.transform.GetChild(0); }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, SOUND_TYPE.SFX, false);

        return obj;
    }
    public static GameObject PlaySFXForUI(AudioClip _clip, Transform _transform = null)
    {
        if (_transform == null) { _transform = thisTransform; }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, SOUND_TYPE.SFX, false);

        return obj;
    }
    public static GameObject PlayBGM(AudioClip _clip, Transform _transform = null)
    {
        if (_transform == null) { _transform = Camera.main.transform.GetChild(0); }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, SOUND_TYPE.BGM, true);

        SetNowBGM(obj.GetComponent<AudioSource>());

        return obj;
    }
    public static GameObject PlayBGMIntro(AudioClip _startClip, AudioClip _loopClip, Transform _transform = null)
    {
        if (_transform == null) { _transform = Camera.main.transform.GetChild(0); }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_startClip, SOUND_TYPE.BGM, false);
        obj.AddComponent<LoopBehavior>().SetLoopClip(_loopClip);

        SetNowBGM(obj.GetComponent<AudioSource>());

        return obj;
    }
}
