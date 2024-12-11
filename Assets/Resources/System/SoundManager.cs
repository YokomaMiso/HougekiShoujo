using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public enum SOUND_TYPE { BGM = 0, SFX, SFX_FOR_UI, VOICE, VOICE_FOR_UI };

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject soundObjectPrefab;
    static GameObject soundObject;
    static Transform thisTransform;

    public static AudioSource nowBGMAudioSource = null;

    public static float masterVolume = 1.0f;
    public static float bgmVolume = 0.5f;
    public static float sfxVolume = 0.5f;
    public static float voiceVolume = 0.5f;

    /*
    [SerializeField] GameObject interactPrefab;
    static GameObject interactBGMObject;
    static GameObject interactInstance;
    */

    void Start()
    {
        OptionData oData = Managers.instance.optionData;

        masterVolume = oData.masterVolume;
        bgmVolume = oData.bgmVolume;
        sfxVolume = oData.sfxVolume;
        voiceVolume = oData.voiceVolume;

        soundObject = soundObjectPrefab;
        //interactBGMObject = interactPrefab;
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
    public static GameObject PlayVoiceForUI(AudioClip _clip, Transform _transform = null)
    {
        if (_transform == null) { _transform = Camera.main.transform.GetChild(0); }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, SOUND_TYPE.VOICE_FOR_UI, false);

        return obj;
    }
    public static GameObject PlaySFX(AudioClip _clip, Transform _transform = null)
    {
        if (_transform == null) { _transform = Camera.main.transform.GetChild(0); }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, SOUND_TYPE.SFX, false);

        return obj;
    }
    public static GameObject PlaySFXForUI(AudioClip _clip, Transform _transform = null, bool _loop = false)
    {
        if (_transform == null) { _transform = thisTransform; }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, SOUND_TYPE.SFX_FOR_UI, _loop);

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
    /*
    public static GameObject PlayInteractBGM(int _num)
    {
        InteractBGMObject ibo;

        if (interactInstance == null) 
        {
            interactInstance = Instantiate(interactBGMObject, Managers.instance.transform); 
        }
        ibo = interactInstance.GetComponent<InteractBGMObject>();
        ibo.ChangeBGM(_num);

        GameObject obj = ibo.GetObject(_num);
        SetNowBGM(obj.GetComponent<AudioSource>());

        return obj;
    }
    */
    public static GameObject PlayJingleForResult(AudioClip _clip, Transform _transform = null)
    {
        if (_transform == null) { _transform = Camera.main.transform.GetChild(0); }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, SOUND_TYPE.BGM, false);

        SetNowBGM(obj.GetComponent<AudioSource>());

        return obj;
    }
}
