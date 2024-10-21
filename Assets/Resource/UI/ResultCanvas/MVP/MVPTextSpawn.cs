using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MVPTextSpawn : MonoBehaviour
{
    float timer;
    const float startTime = 14.0f;
    const float childSub = 0.5f;

    const float limit = startTime + childSub * 4;

    readonly Vector3 startScale = Vector3.one * 6;
    readonly Vector3 endScale = Vector3.one * 0.8f;

    Image[] texts = new Image[3];

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            texts[i] = transform.GetChild(i).GetComponent<Image>();
            texts[i].transform.localScale = startScale;
            texts[i].color = Color.clear;
        }
    }

    void Update()
    {
        if (timer >= limit) { return; }

        timer += Time.deltaTime;
        if (timer >= limit) { timer = limit; }

        for (int i = 0; i < (int)AWARD_ID.MAX_NUM; i++)
        {
            float scaleValue;
            float subValue = startTime + childSub * (i - 1);
            if (timer < subValue) { scaleValue = 0; }
            else { scaleValue = Mathf.Clamp01((timer - subValue) / (startTime + childSub * i - subValue)); }

            texts[i].transform.localScale = Vector3.Lerp(startScale, endScale, scaleValue);
            texts[i].color = Color.white * scaleValue;
        }
    }
}
