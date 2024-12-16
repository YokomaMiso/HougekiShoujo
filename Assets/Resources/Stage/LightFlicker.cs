using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    Light lightCmp;
    [SerializeField] float minIntensity;
    [SerializeField] float maxIntensity;

    void Start()
    {
        lightCmp = GetComponent<Light>();
        lightCmp.intensity = RandomIntensity();
    }

    void Update()
    {
        lightCmp.intensity = RandomIntensity();
    }

    float RandomIntensity()
    {
        return Mathf.Lerp(minIntensity, maxIntensity, Mathf.Abs(Mathf.Sin(Time.time)));
    }
}
