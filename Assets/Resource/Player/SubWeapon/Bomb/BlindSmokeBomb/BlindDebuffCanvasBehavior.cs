using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindDebuffCanvasBehavior : MonoBehaviour
{
    float timer = 0;
    const float startTime = 0.5f;
    const float lifeTime = 15;


    void Update()
    {
        IngameData.GameData myIngameData = OSCManager.OSCinstance.myNetIngameData.mainPacketData.inGameData;
        if (!myIngameData.alive)
        {
            Destroy(gameObject);
            return;
        }

        timer += Managers.instance.timeManager.GetDeltaTime();

        if (timer < startTime)
        {
            float seedValue = Mathf.Clamp01(timer / startTime);
            float value = Mathf.Sqrt(seedValue);
            transform.GetChild(0).localScale = Vector3.Lerp(Vector3.one * 5, Vector3.one, value);
        }
        else if (timer < lifeTime)
        {
            float seedValue = Mathf.Clamp01((timer - startTime) / (lifeTime - startTime));
            float value = seedValue * seedValue;
            transform.GetChild(0).localScale = Vector3.Lerp(Vector3.one, Vector3.one * 5, value);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
