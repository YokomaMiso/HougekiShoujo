using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SOUND_TYPE { BGM = 0, SFX, VOICE };

public class SoundManager : MonoBehaviour
{
    [SerializeField] GameObject soundObjectPrefab;
    static GameObject soundObject;
    static Transform thisTransform;

    void Start()
    {
        soundObject = soundObjectPrefab;
        thisTransform = transform;
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

        return obj;
    }
    public static GameObject PlayBGMIntro(AudioClip _startClip, AudioClip _loopClip, Transform _transform = null)
    {
        if (_transform == null) { _transform = Camera.main.transform.GetChild(0); }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_startClip, SOUND_TYPE.BGM, false);
        obj.AddComponent<LoopBehavior>().SetLoopClip(_loopClip);

        return obj;
    }
}
