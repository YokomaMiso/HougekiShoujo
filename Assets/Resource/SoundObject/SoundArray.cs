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

    public static void PlaySound(AudioClip _clip)
    {
        GameObject obj = Instantiate(soundObject, thisTransform);
        obj.GetComponent<SoundObject>().ReceiveSound(_clip);
        soundArray.Add(obj);
    }

    public static void DestroyReport(GameObject _obj)
    {
        soundArray.Remove(_obj);
    }
}
