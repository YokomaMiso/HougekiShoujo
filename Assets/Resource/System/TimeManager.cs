using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool slow;
    public bool stop;
    const float slowRate = 0.1f;

    public void SetSlow(bool _slow) { slow = _slow; }
    public void SetStop(bool _stop) { stop = _stop; }

    public float GetDeltaTime()
    {
        if (stop) { return 0.0f; }
        if (slow) { return Time.deltaTime * slowRate; }

        return Time.deltaTime;
    }

    public float TimeRate() 
    {
        if (stop) { return 0.0f; }
        if (slow) { return slowRate; }

        return 1.0f;
    }
}
