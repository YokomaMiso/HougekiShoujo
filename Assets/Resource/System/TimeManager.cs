using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static bool slow;
    public static bool stop;
    const float slowRate = 0.1f;

    static public float GetDeltaTime()
    {
        if (stop) { return 0; }
        if (slow) { return Time.deltaTime * slowRate; }

        return Time.deltaTime;
    }

    public static float TimeRate() 
    {
        if (stop) { return 0.0f; }
        if (slow) { return slowRate; }

        return 1.0f;
    }
}
