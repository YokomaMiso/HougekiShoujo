using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwardName : MonoBehaviour
{
    float timer;
    const float shrinkStart = 4.5f;
    const float childSub = 0.5f;

    float limit;

    readonly Vector3 startScale = Vector3.one * 5;
    readonly Vector3 endScale = Vector3.one;

    Image[] names = new Image[(int)AWARD_ID.MAX_NUM];

    void Start()
    {
        limit = shrinkStart + childSub * (int)AWARD_ID.MAX_NUM;
        for (int i = 0; i < (int)AWARD_ID.MAX_NUM; i++) { names[i] = transform.GetChild(i).GetComponent<Image>(); }
    }

    void Update()
    {
        if (timer >= limit) { return; }

        timer += Time.deltaTime;
        if (timer >= limit) { timer = limit; }

        for (int i = 0; i < (int)AWARD_ID.MAX_NUM; i++)
        {
            //float scaleValue = Mathf.Clamp01(timer - alphaStart - (childSub * i));
            //illusts[i].color = Color.white * colorValue;
        }
    }
}
