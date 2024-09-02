using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static bool slow;
    public static bool stop;
    const float slowRate = 0.1f;

    public static float deltaTime;

    void Update()
    {
        if (stop)
        {
            deltaTime = 0;
            return;
        }

        deltaTime = Time.deltaTime;
        if (slow) { deltaTime *= slowRate; }
    }

    static public float GetTime()
    {
        if (stop) { return 0; }
        if (slow) { return Time.deltaTime * 0.1f; }

        return Time.deltaTime;
    }

    public static float TimeRate() { return deltaTime / Time.deltaTime; }
}
