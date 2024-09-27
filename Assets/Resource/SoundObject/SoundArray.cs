using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundArray : MonoBehaviour
{
    public static List<GameObject> soundArray = new List<GameObject>();
    static GameObject soundObject;
    static Transform thisTransform;

    [SerializeField] GameObject soundObjectPrefab;

    void Start()
    {
        soundObject = soundObjectPrefab;
        thisTransform = transform;
    }

    public static GameObject PlaySFX(AudioClip _clip, Vector3 _pos = default(Vector3))
    {
        GameObject obj = Instantiate(soundObject, thisTransform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, false);
        obj.transform.position = _pos;
        soundArray.Add(obj);

        return obj;
    }
    public static GameObject PlayBGM(AudioClip _clip)
    {
        GameObject obj = Instantiate(soundObject, thisTransform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip, true);
        soundArray.Add(obj);

        return obj;
    }
    public static void DestroyReport(GameObject _obj) { soundArray.Remove(_obj); }
}
