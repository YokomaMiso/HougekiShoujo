using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (_transform == null) { _transform = thisTransform; }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, false);

        return obj;
    }
    public static GameObject PlayBGM(AudioClip _clip, Transform _transform = null)
    {
        if (_transform == null) { _transform = thisTransform; }

        GameObject obj = Instantiate(soundObject, _transform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, true);

        return obj;
    }
}
