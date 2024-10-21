using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerNameInAward : MonoBehaviour
{
    float timer;
    const float childSub = 0.5f;
    const float alphaStart = 3.0f;

    float limit;

    Text[] texts = new Text[(int)AWARD_ID.MAX_NUM];

    public void SetText(AWARD_ID _id, ResultScoreBoard.KDFData _kdf)
    {
        texts[(int)_id] = transform.GetChild((int)_id).GetComponent<Text>();

        texts[(int)_id].text = _kdf.playerName;
    }
    void Start()
    {
        limit = alphaStart + (childSub * (int)AWARD_ID.MAX_NUM + 1);
    }
    void Update()
    {
        if (timer >= limit) { return; }

        timer += Time.deltaTime;
        if (timer >= limit) { timer = limit; }

        for (int i = 0; i < (int)AWARD_ID.MAX_NUM; i++)
        {
            float colorValue = Mathf.Clamp01(timer - alphaStart - (childSub * i));
            texts[i].color = Color.black * colorValue;
        }
    }
}
